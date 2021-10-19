using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TodoList.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        private readonly string _categoryName;

        private static readonly object Lock = new object();

        public FileLogger(string path, string categoryName)
        {
            _filePath = path;
            _categoryName = categoryName ?? "";
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                StringBuilder log = new StringBuilder();
                log.Append(DateTime.Now + "   [" + logLevel.ToString().ToUpperInvariant() + "] [" + _categoryName + "]  ");
                log.Append(formatter(state, exception) + Environment.NewLine);
                if (exception != null)
                {
                    log.Append(exception.GetType() + "    " + exception.Message + Environment.NewLine);
                    log.Append(exception.StackTrace + Environment.NewLine);
                }

                string fullLog = log.ToString();
                lock (Lock)
                {
                    File.AppendAllText(_filePath, fullLog);
                }
            }
        }
    }
}
