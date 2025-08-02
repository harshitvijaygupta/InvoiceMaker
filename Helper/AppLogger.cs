using InvoiceMaker.Helper.Interface;

namespace InvoiceMaker.Helper;

public class AppLogger : IAppLogger
{
    private readonly ILogger _logger;

    public AppLogger(ILoggerFactory loggerFactory)
    {
        // You can change the category name if needed
        _logger = loggerFactory.CreateLogger("AppLogger");
    }

    public void LogInformation(string message)
    {
        _logger.LogInformation(message);
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning(message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        _logger.LogError(ex, message);
    }
}