using System;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using NLog;

namespace EarthApi.Servicies;

public class LoggerService : ILoggerService
{
    private readonly Logger _logger;
    public LoggerService()
    {
        _logger = LogManager.GetCurrentClassLogger();
    }
    public void Debug(string message)
    {
        _logger.Debug(message);
    }

    public void Error(string message)
    {
        _logger.Error(message);
    }

    public void Info(string message)
    {
        _logger.Info(message);
    }
}
