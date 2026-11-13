-- =====================================================================
--  ChatService — схема данных (PostgreSQL 13+)
--  Файл: docs/ChatService.fixed.sql
--  Исправленная версия: docs/ChatService.sql
--
--  ВЛАДЕЛЕЦ СХЕМЫ: ChatService (схема chat).
--  Таблицы пользователей здесь НЕТ: данные пользователя запрашиваются
--  у UserService по HTTP (architecture.md, п. 7), поэтому в этой БД
--  хранится только user_id — внешний идентификатор без FK.
--
--  Отличия от исходного скрипта:
--    1) удалена таблица "User" (двойное владение данными; хеши паролей
--       не должны жить в чат-сервисе);
--    2) все обязательные колонки — NOT NULL, добавлены CHECK и UNIQUE;
--    3) TIMESTAMP -> TIMESTAMPTZ + DEFAULT now();
--    4) добавлены индексы под реальные запросы (история сообщений,
--       список чатов пользователя, состав участников);
--    5) исправлены имена: ChatMemmber -> chat_members, AutorId -> author_id;
--    6) добавлены поля под заявленные функции: soft delete, edited_at,
--       last_read_message_id (C12), left_at (C3), client_message_id (ретраи);
--    7) явные ON DELETE для дочерних таблиц;
--    8) скрипт обёрнут в транзакцию.
--
--  Идентификаторы (id) генерирует приложение. Рекомендуется UUIDv7
--  (Guid.CreateVersion7() в .NET 9+): ключ упорядочен по времени,
--  поэтому (sent_at, id) — корректный и стабильный порядок сообщений.
-- =====================================================================

BEGIN;

CREATE SCHEMA chat;

-- ---------------------------------------------------------------------
-- 1. Чаты
-- ---------------------------------------------------------------------
CREATE TABLE chat.chats
(
    id         uuid        PRIMARY KEY,
    name       varchar(100),

    -- Владелец чата: идентификатор пользователя из UserService, без FK.
    -- Целостность проверяется на уровне приложения (HTTP-вызов при создании
    -- чата); отсутствие FK сознательное — см. блок «ВАРИАНТ» в конце файла.
    owner_id   uuid        NOT NULL,

    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NOT NULL DEFAULT now(),

    -- Мягкое удаление: чат удаляет владелец (C1), но историю не рвём —
    -- иначе теряются ссылки в сообщениях и у участников.
    deleted_at timestamptz,

    CONSTRAINT ck_chats_name_not_blank CHECK (name IS NULL OR btrim(name) <> '')
);

-- Запрос «чаты, где я владелец».
CREATE INDEX ix_chats_owner ON chat.chats (owner_id);

-- ---------------------------------------------------------------------
-- 2. Участники чата (M:N Chat <-> User + роль участника)
-- ---------------------------------------------------------------------
CREATE TABLE chat.chat_members
(
    id         uuid        PRIMARY KEY,

    chat_id    uuid        NOT NULL
        REFERENCES chat.chats (id) ON DELETE CASCADE,

    -- Идентификатор пользователя из UserService, без FK.
    user_id    uuid        NOT NULL,

    -- MemberRole: Owner = 0, Admin = 1, Member = 2.
    -- EF Core мапит enum с int-основанием в integer без доп. конфигурации.
    role       integer     NOT NULL DEFAULT 2,

    joined_at  timestamptz NOT NULL DEFAULT now(),

    -- Выход из чата (C3): строку не удаляем, чтобы сохранить историю участия.
    left_at    timestamptz,

    -- Индикатор «прочитано» (C12): последнее прочитанное сообщение участника.
    -- Счётчик непрочитанных = COUNT(*) сообщений новее этой отметки.
    last_read_message_id uuid,

    CONSTRAINT ck_chat_members_role CHECK (role IN (0, 1, 2)),
    CONSTRAINT ck_chat_members_left_after_join CHECK (left_at IS NULL OR left_at >= joined_at)
);

-- Ровно один действующий владелец на чат — инвариант агрегата Chat,
-- выраженный средствами БД (без триггеров).
CREATE UNIQUE INDEX ux_chat_members_single_owner
    ON chat.chat_members (chat_id)
    WHERE role = 0 AND left_at IS NULL;

-- Один действующий участник = одна строка на пару (чат, пользователь).
-- Индекс частичный, поэтому выход и повторное вступление сохраняют историю.
-- Повторное вступление (не нарушая индекс):
--   INSERT INTO chat.chat_members (id, chat_id, user_id, role)
--   VALUES ($1, $2, $3, $4)
--   ON CONFLICT (chat_id, user_id) WHERE left_at IS NULL
--   DO UPDATE SET left_at = NULL, joined_at = now(), role = EXCLUDED.role;
CREATE UNIQUE INDEX ux_chat_members_active
    ON chat.chat_members (chat_id, user_id)
    WHERE left_at IS NULL;

-- «Список чатов пользователя» (C11).
CREATE INDEX ix_chat_members_user
    ON chat.chat_members (user_id)
    WHERE left_at IS NULL;

-- «Состав участников чата» (C7).
CREATE INDEX ix_chat_members_chat
    ON chat.chat_members (chat_id)
    WHERE left_at IS NULL;

-- ---------------------------------------------------------------------
-- 3. Сообщения
-- ---------------------------------------------------------------------
CREATE TABLE chat.messages
(
    id                uuid        PRIMARY KEY,

    chat_id           uuid        NOT NULL
        REFERENCES chat.chats (id) ON DELETE CASCADE,

    -- Автор: идентификатор пользователя из UserService, без FK.
    author_id         uuid        NOT NULL,

    -- text, а не varchar(1024): ограничение совпадает с доменом
    -- (Message.MaxContentLength = 2000) и выражено через CHECK.
    content           text        NOT NULL,

    sent_at           timestamptz NOT NULL DEFAULT now(),

    -- Редактирование и удаление сообщения (C10).
    edited_at         timestamptz,
    deleted_at        timestamptz,

    -- Идемпотентность: при таймауте и повторной отправке дубля не будет.
    client_message_id uuid,

    CONSTRAINT ck_messages_content_len
        CHECK (char_length(content) BETWEEN 1 AND 2000),
    CONSTRAINT ck_messages_edited_after_sent
        CHECK (edited_at IS NULL OR edited_at >= sent_at),
    CONSTRAINT ck_messages_deleted_after_sent
        CHECK (deleted_at IS NULL OR deleted_at >= sent_at)
);

-- C5: история сообщений чата с курсорной (keyset) пагинацией.
-- Такой индекс отдаёт и «последние N сообщений», и «сообщения до курсора»:
--   SELECT id, author_id, content, sent_at
--   FROM chat.messages
--   WHERE chat_id = $1
--     AND deleted_at IS NULL
--     AND (sent_at, id) < ($2, $3)      -- курсор; для первой страницы условие убирается
--   ORDER BY sent_at DESC, id DESC
--   LIMIT $4;
CREATE INDEX ix_messages_chat_sent
    ON chat.messages (chat_id, sent_at DESC, id DESC);

-- Защита от дублей при повторной отправке (client_message_id не обязателен).
CREATE UNIQUE INDEX ux_messages_client_id
    ON chat.messages (chat_id, client_message_id)
    WHERE client_message_id IS NOT NULL;

-- ---------------------------------------------------------------------
-- 4. Отложенный FK: отметка прочтения ссылается на сообщение
-- ---------------------------------------------------------------------
-- Отдельным ALTER, чтобы не было циклической зависимости при создании таблиц.
-- ON DELETE SET NULL: при физическом удалении сообщения (каскад от чата)
-- отметка прочтения просто обнуляется.
ALTER TABLE chat.chat_members
    ADD CONSTRAINT fk_chat_members_last_read_message
    FOREIGN KEY (last_read_message_id)
    REFERENCES chat.messages (id) ON DELETE SET NULL;

-- =====================================================================
--  ВАРИАНТ с ссылочной целостностью.
--  Если UserService и ChatService используют один физический PostgreSQL
--  (как в architecture.md, п. 1) и вы готовы принять связанность миграций,
--  можно добавить FK в схему users. Цена: миграции ChatService зависят от
--  порядка применения миграций UserService, а пользователей нельзя удалять
--  физически (только soft delete), иначе чат перестанет консистентно читаться.
--
--    ALTER TABLE chat.chats ADD CONSTRAINT fk_chats_owner
--        FOREIGN KEY (owner_id)  REFERENCES users.users (id) ON DELETE RESTRICT;
--    ALTER TABLE chat.chat_members ADD CONSTRAINT fk_chat_members_user
--        FOREIGN KEY (user_id)   REFERENCES users.users (id) ON DELETE RESTRICT;
--    ALTER TABLE chat.messages ADD CONSTRAINT fk_messages_author
--        FOREIGN KEY (author_id) REFERENCES users.users (id) ON DELETE RESTRICT;
--
--  Промежуточный вариант — локальный кэш профилей (в architecture.md, п. 7
--  он упомянут как защита от временной связанности сервисов): таблица
--  chat.user_cache (user_id PK, display_name, avatar_url, synced_at) с TTL
--  и обновлением при HTTP-вызове. Тогда FK ставится на неё, а не в users.
-- =====================================================================

COMMIT;
