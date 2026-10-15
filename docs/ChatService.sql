-- Пользователь.
CREATE TABLE "User"
(
	"Id" UUID NOT NULL PRIMARY KEY,
	"Login" VARCHAR(1024),
	"PasswordHash" VARCHAR(1024),
	"Status" VARCHAR(1024),
	"Role" SMALLINT
);

-- Чат.
CREATE TABLE "Chat"
(
	"Id" UUID NOT NULL PRIMARY KEY,
	"Name" VARCHAR(1024),
	"OwnerId" UUID,
	"CreatedAt" TIMESTAMP,
	CONSTRAINT "fk_OwnerId" FOREIGN KEY ("OwnerId")
		REFERENCES "User" ("Id")
);

-- Участник чата.
CREATE TABLE "ChatMemmber"
(
	"Id" UUID NOT NULL PRIMARY KEY,
	"ChatId" UUID,
	"UserId" UUID,
	"Role" SMALLINT,
	"JoinedAt" TIMESTAMP,
	CONSTRAINT "fk_ChatId" FOREIGN KEY ("ChatId")
		REFERENCES "Chat" ("Id"),
	CONSTRAINT "fk_UserId" FOREIGN KEY ("UserId")
		REFERENCES "User" ("Id")
);

-- Сообщение.
CREATE TABLE "Message"
(
	"Id" UUID NOT NULL PRIMARY KEY,
	"ChatId" UUID,
	"AutorId" UUID,
	"Content" VARCHAR(1024),
	"SentAt" TIMESTAMP,
	CONSTRAINT "fk_ChatId" FOREIGN KEY ("ChatId")
		REFERENCES "Chat" ("Id"),
	CONSTRAINT "fk_UserId" FOREIGN KEY ("AutorId")
		REFERENCES "User" ("Id")
);

