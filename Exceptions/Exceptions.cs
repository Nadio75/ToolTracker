namespace ToolTracker.Exceptions;

public abstract class DomainException(string message) : Exception(message);

public sealed class NotFoundException(string entity, Guid id)
    : DomainException($"{entity} '{id}' was not found.");

public sealed class ConflictException(string message) : DomainException(message);

public sealed class IdempotencyKeyReuseException(string key)
    : DomainException($"Idempotency-Key '{key}' was already used with a different request payload.");