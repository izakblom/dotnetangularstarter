using Google.Api;
using Google.Cloud.Logging.Type;
using Google.Cloud.Logging.V2;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Common.Logging
{
    public class CustomLoggerGCP : ICustomLogger, IDisposable
    {
        private readonly IConfiguration _configuration;
        protected string _logName { get; set; }
        protected List<LogMessage> _logMessages = new List<LogMessage>();
        protected string _route { get; set; }
        protected IDictionary<string, string> _labels = new Dictionary<string, string>();
        private bool _logDevelopment { get; set; }

        public CustomLoggerGCP(IConfiguration configuration, bool logDevelopment = false)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logName = _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:GCSettings:LogName"];
            _logDevelopment = logDevelopment;
        }

        public void LogMessage(LoggerSeverity severity, string message, string location, IDictionary<string, string> labels = null)
        {
            if (!_logDevelopment && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
                return;


            LogSeverity eSeverity = (LogSeverity)((int)severity * 100);
            // Your Google Cloud Platform project ID.
            string projectId = _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:GCSettings:project_id"];

            // Instantiates a client.
            var client = LoggingServiceV2Client.Create();

            // Prepare new log entry.
            LogEntry logEntry = new LogEntry();
            LogName logName = new LogName(projectId, _logName);
            LogNameOneof logNameToWrite = LogNameOneof.From(logName);
            logEntry.LogName = logName.ToString();
            logEntry.Severity = eSeverity;

            // Create log entry message.
            string messageId = DateTime.Now.Millisecond.ToString();
            //Type myType = typeof(QuickStart);
            string entrySeverity = logEntry.Severity.ToString().ToUpper();
            logEntry.TextPayload = $"{messageId} {entrySeverity} {location} - {message}";

            // Set the resource type to control which GCP resource the log entry belongs to.
            // See the list of resource types at:
            // https://cloud.google.com/logging/docs/api/v2/resource-list
            // This sample uses resource type 'global' causing log entries to appear in the 
            // "Global" resource list of the Developers Console Logs Viewer:
            //  https://console.cloud.google.com/logs/viewer
            MonitoredResource resource = new MonitoredResource();
            resource.Type = "global";

            if (labels == null)
                labels = new Dictionary<string, string>();

            //add env details to logging
            labels.Add("build", Environment.GetEnvironmentVariable("BUILD_NUMBER") ?? "unknown");
            labels.Add("env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown");

            // Add log entry to collection for writing. Multiple log entries can be added.
            IEnumerable<LogEntry> logEntries = new LogEntry[] { logEntry };

            // Write new log entry.
            client.WriteLogEntries(logNameToWrite, resource, labels, logEntries);

        }

        public void LogException(Exception exception, string message = "", IDictionary<string, string> labels = null)
        {
            if (!_logDevelopment && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
                return;


            LogSeverity eSeverity = LogSeverity.Error;
            // Your Google Cloud Platform project ID.
            string projectId = _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:GCSettings:project_id"];

            // Instantiates a client.
            var client = LoggingServiceV2Client.Create();

            // Prepare new log entry.
            LogEntry logEntry = new LogEntry();
            LogName logName = new LogName(projectId, _logName);
            LogNameOneof logNameToWrite = LogNameOneof.From(logName);
            logEntry.LogName = logName.ToString();
            logEntry.Severity = eSeverity;

            // Create log entry message.
            string messageId = DateTime.Now.Millisecond.ToString();
            //Type myType = typeof(QuickStart);
            string entrySeverity = logEntry.Severity.ToString().ToUpper();
            logEntry.TextPayload = $"{messageId} {entrySeverity}  {message} {exception.StackTrace}";

            // Set the resource type to control which GCP resource the log entry belongs to.
            // See the list of resource types at:
            // https://cloud.google.com/logging/docs/api/v2/resource-list
            // This sample uses resource type 'global' causing log entries to appear in the 
            // "Global" resource list of the Developers Console Logs Viewer:
            //  https://console.cloud.google.com/logs/viewer
            MonitoredResource resource = new MonitoredResource();
            resource.Type = "global";

            if (labels == null)
                labels = new Dictionary<string, string>();

            //add env details to logging
            labels.Add("build", Environment.GetEnvironmentVariable("BUILD_NUMBER") ?? "unknown");
            labels.Add("env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown");

            // Add log entry to collection for writing. Multiple log entries can be added.
            IEnumerable<LogEntry> logEntries = new LogEntry[] { logEntry };

            // Write new log entry.
            client.WriteLogEntries(logNameToWrite, resource, labels, logEntries);

        }

        public void LogMessages(List<LogMessage> messages, IDictionary<string, string> labels = null)
        {
            if (!_logDevelopment && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
                return;

            // Your Google Cloud Platform project ID.
            string projectId = _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:GCSettings:project_id"];

            if (labels == null)
                labels = new Dictionary<string, string>();

            //add env details to logging
            labels.Add("build", Environment.GetEnvironmentVariable("BUILD_NUMBER") ?? "unknown");
            labels.Add("env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown");

            MonitoredResource resource = new MonitoredResource();
            resource.Type = "global";

            // Instantiates a client.
            var client = LoggingServiceV2Client.Create();

            List<LogEntry> logEntries = new List<LogEntry>();

            LogName logName = new LogName(projectId, _logName);
            LogNameOneof logNameToWrite = LogNameOneof.From(logName);


            messages.ForEach(m =>
            {

                // Prepare new log entry.
                LogEntry logEntry = new LogEntry();
                logEntry.LogName = logName.ToString();
                LogSeverity eSeverity = (LogSeverity)((int)m.Severity * 100);
                logEntry.Severity = eSeverity;

                string messageId = DateTime.Now.Millisecond.ToString();
                string entrySeverity = logEntry.Severity.ToString().ToUpper();
                logEntry.TextPayload = $"{messageId} {entrySeverity} {m.Location} - {m.Message}";


                if (m.Labels != null) logEntry.Labels.Add(m.Labels);

                logEntries.Add(logEntry);
            });

            // Write new log entry.
            client.WriteLogEntries(logNameToWrite, resource, labels, logEntries);

        }


        public void Dispose()
        {
        }
    }

}
