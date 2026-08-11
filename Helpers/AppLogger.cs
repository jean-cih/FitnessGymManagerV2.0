using GymApplicationV2._0.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplicationV2._0.Helpers
{
    public class AppLogger
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();

        public AppLogger(string appFilesPath)
        {
            string logFileName = $"app_log_{DateTime.Now:yyyyMMdd}.log";
            _logFilePath = Path.Combine(appFilesPath, logFileName);

            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
            }
        }

        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        public void LogError(string message, Exception ex = null)
        {
            string errorMsg = ex != null ? $"{message}: {ex.Message}" : message;
            Log("ERROR", errorMsg);
        }

        private void Log(string level, string message)
        {
            lock (_lockObject)
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}";
                File.AppendAllText(_logFilePath, logEntry);
            }
        }
    }

    public static class Logger
    {
        private static AppLogger _instance;
        private static readonly object _lock = new object();

        public static void Initialize(string appFilesPath)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppLogger(appFilesPath);
                    }
                }
            }
        }

        public static void Info(string message)
        {
            _instance?.LogInfo(message);
        }

        public static void Warning(string message)
        {
            _instance?.LogWarning(message);
        }

        public static void Error(string message, Exception ex = null)
        {
            _instance?.LogError(message, ex);
        }
    }
}