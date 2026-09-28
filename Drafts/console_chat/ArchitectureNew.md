```mermaid
graph TD
    classDef client fill:#b3e5fc,stroke:#01579b,stroke-width:2px;
    classDef gateway fill:#c8e6c9,stroke:#1b5e20,stroke-width:2px;
    classDef service fill:#fff9c4,stroke:#f57f17,stroke-width:2px;
    classDef broker fill:#ffe0b2,stroke:#e65100,stroke-width:2px;
    classDef db fill:#ffcdd2,stroke:#b71c1c,stroke-width:2px;

    Client["Консольный клиент (Console App)"]:::client
    Gateway["API Gateway"]:::gateway
    
    subgraph Microservices ["Микросервисы (Console Apps)"]
        UserSvc["UserService (Clean + DDD)"]:::service
        ChatSvc["ChatService (Clean + DDD)"]:::service
        
        subgraph LogSystem ["LoggerService"]
            LogSvc["Логика логгера"]:::service
            LogHttp["[HTTP API Endpoint]"]:::gateway
        end
    end

    Rabbit["RabbitMQ (Брокер сообщений)"]:::broker

    subgraph Storage ["Базы данных"]
        UserDB[("User DB (PostgreSQL)")]:::db
        ChatDB[("Chat DB (PostgreSQL)")]:::db
    end

    Client -- "1. Запросы / Команды" --> Gateway
    Client -. "4. Чтение логов (HTTP GET)" .-> LogHttp
    
    Gateway -- "2. Публикация событий" --> Rabbit
    
    Rabbit <--> "3. Стриминг / RPC" <--> UserSvc
    Rabbit <--> "3. Стриминг / RPC" <--> ChatSvc
    Rabbit -- "3. Асинхронные логи" --> LogSvc

    UserSvc --> UserDB
    ChatSvc --> ChatDB
```
