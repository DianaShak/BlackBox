Что User Service будет принимать в запросе и отдавать в ответе по функциям:

MVP:
1. Регистрация
В запросе:
Login, Password, Name

В ответе:
UserId (Guid), Status (Успешно или ошибка «логин уже занят»)

Что отправляется в очередь как Event (для API Gateway):
UserRegisteredEvent (UserId, Login, Name)

Что отправляется в очередь как Event (для чат сервиса):
UserRegisteredEvent (UserId, Login, Name)

2. Вход / Авторизация
В запросе:
Login, Password

В ответе:
Status (успешно / ошибка «неверный пароль/логин»), JWT-токен

Остальные функции:
3. Просмотр профиля
В запросе:
TargetUserId, RequestedByUserId

В ответе:
UserId, Login, Status (онлайн / оффлайн), RegistrationDate, BirthDate, Bio (описание/description)

4. Редактирование профиля
В запросе:
UserId, NewName, NewBio, NewBirthDate

В ответе:
Status (успешно, ошибка)

Что отправляется в очередь как Event (для чат сервиса):
UserProfileUpdatedEvent (UserId, NewName)

5. Выход
В запросе:
UserId

В ответе:
Status (успешно)

Что отправляется в очеред как Event (для чат сервиса):
UserLoggedOutEvent (UserId)