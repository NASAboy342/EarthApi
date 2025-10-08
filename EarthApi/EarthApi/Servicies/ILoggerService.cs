using System;

namespace EarthApi.Servicies;

public interface ILoggerService
{
    void Info(string message);
    void Error(string message);
    void Debug(string message);
}
