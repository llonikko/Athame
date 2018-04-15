using System;
using System.Collections.Generic;
using System.IO;
using CenterCLR;

namespace Athame.Core.Logging
{
    public class FileLogger : ILogger, IDisposable
    {
        private readonly string logDirectory;
        private string logFileName = String.Empty;
        private FileStream logFile;
        private StreamWriter writer;
        private string formattedName;

        public FileLogger(string logDirectory)
        {
            this.logDirectory = logDirectory;
            FilenameFormat = "log{FileDate}.log";
            LineFormat = "[{Date}] [{Level}] {Tag}: {Message}";
        }

        public string FilenameFormat { get; set; }

        public string LineFormat { get; set; }

        private void EnsureLog()
        {
            var temp = new Dictionary<string, object> { { "FileDate", DateTime.Now.ToString("yyyyMMdd") } };
            formattedName = Named.Format(FilenameFormat, temp);
            if (logFileName != formattedName)
            {
                logFile?.Dispose();
                writer?.Dispose();
                logFile = File.Open(Path.Combine(logDirectory, formattedName), FileMode.Append, FileAccess.Write);
                logFileName = formattedName;
                writer = new StreamWriter(logFile) { AutoFlush = true };
            }
        }

        public void Write(Level level, string moduleTag, string message)
        {
            EnsureLog();
            var vars = new Dictionary<string, object>
            {
                { "Date", DateTime.Now.ToString("O")},
                { "Level", level},
                { "Tag", moduleTag ?? "Global"},
                { "Message", message }
            };
            var formattedMessage = Named.Format(LineFormat, vars);
            writer.WriteLine(formattedMessage);


        }

        public void Dispose()
        {
            logFile?.Dispose();
            writer?.Dispose();
        }
    }
}
