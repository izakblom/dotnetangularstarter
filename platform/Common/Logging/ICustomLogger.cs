using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Logging
{
    public interface ICustomLogger
    {
        void LogMessage(LoggerSeverity severity, string message, string location, IDictionary<string, string> labels = null);
        void LogMessages(List<LogMessage> messages, IDictionary<string, string> labels = null);
    }

    public enum LoggerSeverity
    {
        Default = 0,
        Debug = 1,
        Info = 2,
        Notice = 3,
        Warning = 4,
        Error = 5,
        Critical = 6,
        Alert = 7,
        Emergency = 8
    }

}
