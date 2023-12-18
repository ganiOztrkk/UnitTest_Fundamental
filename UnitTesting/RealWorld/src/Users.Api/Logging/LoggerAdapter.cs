namespace Users.Api.Logging;

public sealed class LoggerAdapter<TType>(ILogger<TType> logger) : ILoggerAdapter<TType>
{
    private readonly ILogger<TType> _logger = logger;

    public void LogInformation(string? message, params object?[] args)
    {
        _logger.LogInformation(message,args);
    }

    public void LogError(Exception? exception, string? message, params object?[] args)
    {
        _logger.LogError(exception, message, args);
    }
}