using System;
using LogerService.Domain.Enums;

namespace LogerService.Domain.Entities;

public class LogItem
{
    //id записи лога
    public int Id { get; }

    //идентификатор для связи событий в цепочку
    public Guid TraceId { get; }

    //уникальный идентификатор события
    public Guid EventId { get; }

    //время возникновения события
    public DateTime OccurredAt { get; }

    //название сервиса, в котором произошло событие
    public string ServiceName { get; }

    //уровень логирования
    public LogLevel Level { get; }

    //сообщение лога
    public string Message { get; }

    public string UserId { get; }

    //дополнительные данные лога
    public string? Metadata { get; }

    public DateTime CreatedAt { get; }

}


