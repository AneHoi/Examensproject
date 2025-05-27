namespace application.Events;

public record ProductEvent(Guid ProductId, string EventType, DateTime Timestamp);
