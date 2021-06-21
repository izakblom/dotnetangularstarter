using Google.Cloud.Logging.Type;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Logging
{
    public class LogMessage
    {
        public LoggerSeverity Severity { get; set; }
        public string Message { get; set; }
        public string Location { get; set; }
        public IDictionary<string, string> Labels { get; set; }

        public LogMessage() { }

        public LogMessage(LoggerSeverity severity, string message, string location, IDictionary<string, string> labels = null)
        {
            Severity = severity;
            Message = message;
            Location = location;
            Labels = labels;
        }
    }
}
