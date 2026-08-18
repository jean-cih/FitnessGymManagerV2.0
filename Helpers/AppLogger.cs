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
        private readonly string _logDirectory;
        private readonly object _lockObject = new object();
        private const int MAX_LOG_DAYS = 7;

        public AppLogger(string appFilesPath)
        {
            _logDirectory = appFilesPath;
            string logFileName = $"app_log_{DateTime.Now:yyyyMMdd}.log";
            _logFilePath = Path.Combine(appFilesPath, logFileName);

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
            }

            DeleteOldLogs();
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

        private void DeleteOldLogs()
        {
            try
            {
                if (!Directory.Exists(_logDirectory))
                    return;

                // Получаем все файлы логов в директории
                var logFiles = Directory.GetFiles(_logDirectory, "app_log_*.log");

                // Вычисляем дату, до которой нужно удалять (MAX_LOG_DAYS дней назад)
                DateTime cutoffDate = DateTime.Now.AddDays(-MAX_LOG_DAYS);

                foreach (var filePath in logFiles)
                {
                    try
                    {
                        // Извлекаем дату из имени файла
                        string fileName = Path.GetFileName(filePath);
                        string datePart = fileName.Replace("app_log_", "").Replace(".log", "");

                        if (DateTime.TryParseExact(datePart, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime fileDate))
                        {
                            // Если файл старше cutoffDate - удаляем
                            if (fileDate < cutoffDate)
                            {
                                File.Delete(filePath);
                                System.Diagnostics.Debug.WriteLine($"Удален старый лог-файл: {fileName}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Логируем ошибку удаления в Debug (нельзя использовать Logger, так как он еще может быть не инициализирован)
                        System.Diagnostics.Debug.WriteLine($"Ошибка при удалении лог-файла {filePath}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при удалении старых логов: {ex.Message}");
            }
        }
    }

    public static class Logger
    {
        private static AppLogger _instance;
        private static readonly object _lock = new object();
        private static string _logDirectory;

        public static void Initialize(string appFilesPath)
        {
            _logDirectory = appFilesPath;

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