# Документация контрактов User Service

В данном документе описаны структуры запросов, ответов и событий (Events) для интеграции с `User Service`. 
Все идентификаторы (`Id`) используют формат Guid. Даты передаются в формате ISO 8601.

---

## MVP

### 1. Регистрация пользователя (Registration)

#### Запрос (Command / Request)
```json
{
  "Login": "user_login",
  "Password": "RawPassword123!", 
  "Name": "Иван Иванов"
}
```

#### Ответ (Response)
```json
{
  "UserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "IsSuccess": false,
  "ErrorMessage": "Логин уже занят"
}
```

#### Событие в очередь (UserRegisteredEvent)
*Предназначено для: Chat Service*
```json
{
  "EventId": "8f3b2a1c-4b5d-6e7f-8a9b-0c1d2e3f4a5b",
  "Timestamp": "2026-09-25T21:42:00Z",
  "UserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "Login": "user_login",
  "Name": "Иван Иванов"
}
```

---

### 2. Вход / Авторизация (Login / Authentication)

#### Запрос (Command / Request)
```json
{
  "Login": "user_login",
  "Password": "RawPassword123!"
}
```

#### Ответ (Response)
```json
{
  "IsSuccess": true,
  "ErrorMessage": null,
  "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzZmE4NWY2NC01NzE3LTQ1NjItYjNmYy0yYzk2M2Y2NmFmYTYiLCJleHAiOjE3OTA0NDYwMDB9..."
}
```

---

## Остальные функции

### 3. Просмотр профиля (Get Profile)

#### Запрос (Query / Request)
```json
{
  "TargetUserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "RequestedByUserId": "c6b89155-2d46-4b8c-8f11-9a7e8e50bc9b"
}
```

#### Ответ (Response)
```json
{
  "UserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "Login": "user_login",
  "Status": "Online",
  "RegistrationDate": "2026-09-25T18:30:00Z",
  "BirthDate": "2000-01-15",
  "Bio": "Разработчик на C#. Учусь чистой архитектуре."
}
```

---

### 4. Редактирование профиля (Update Profile)

#### Запрос (Command / Request)
```json
{
  "UserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "NewName": "Иван Петров",
  "NewBio": "Новое описание профиля",
  "NewBirthDate": "2000-01-20"
}
```

#### Ответ (Response)
```json
{
  "IsSuccess": true,
  "ErrorMessage": null
}
```

#### Событие в очередь (UserProfileUpdatedEvent)
*Предназначено для: Chat Service*
```json
{
  "EventId": "9a1b2c3d-4e5f-6a7b-8c9d-0e1f2a3b4c5d",
  "Timestamp": "2026-09-25T21:45:00Z",
  "UserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "NewName": "Иван Петров"
}
```

---

### 5. Выход (Logout)

#### Запрос (Command / Request)
```json
{
  "UserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

#### Ответ (Response)
```json
{
  "IsSuccess": true
}
```

#### Событие в очередь (UserLoggedOutEvent)
*Предназначено для: Chat Service*
```json
{
  "EventId": "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d",
  "Timestamp": "2026-09-25T21:47:00Z",
  "UserId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```