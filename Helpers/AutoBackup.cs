using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0.Helpers
{
    using global::GymApplicationV2._0.Helpers.GymApplicationV2._0.Helpers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace GymApplicationV2._0.Helpers
    {
        public class DatabaseBackupManager
        {
            private readonly string _sourceDirectory;
            private readonly string _backupDirectory;
            private readonly int _maxBackupFiles;
            private readonly string _backupPrefix = "backup_";

            public DatabaseBackupManager(string sourceDirectory, string backupDirectory = null, int maxBackupFiles = 7)
            {
                _sourceDirectory = sourceDirectory;
                _backupDirectory = backupDirectory ?? Path.Combine(sourceDirectory, "Backups");
                _maxBackupFiles = maxBackupFiles;

                if (!Directory.Exists(_backupDirectory))
                {
                    Directory.CreateDirectory(_backupDirectory);
                }
            }

            public async Task<bool> CreateBackupAsync(IProgress<string> progress = null)
            {
                try
                {
                    if (!Directory.Exists(_sourceDirectory))
                    {
                        Logger.Error($"Папка с базами данных не найдена: {_sourceDirectory}");
                        return false;
                    }

                    var dbFiles = Directory.GetFiles(_sourceDirectory, "*.db");
                    if (dbFiles.Length == 0)
                    {
                        Logger.Warning("Не найдено файлов баз данных для резервного копирования");
                        return false;
                    }

                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string backupFolder = Path.Combine(_backupDirectory, timestamp);
                    Directory.CreateDirectory(backupFolder);

                    progress?.Report($"Создание бэкапа в {backupFolder}");

                    int totalFiles = dbFiles.Length;
                    int processedFiles = 0;

                    foreach (var dbFile in dbFiles)
                    {
                        string fileName = Path.GetFileName(dbFile);
                        string backupPath = Path.Combine(backupFolder, fileName);

                        progress?.Report($"Копирование {fileName}... ({processedFiles + 1}/{totalFiles})");

                        await Task.Run(() => File.Copy(dbFile, backupPath, true));

                        processedFiles++;
                        Logger.Info($"Создан бэкап: {fileName} -> {backupPath}");
                    }

                    DeleteOldBackups();

                    progress?.Report($"✅ Бэкап создан успешно!");
                    Logger.Info($"Резервное копирование завершено. Создано {processedFiles} копий");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error("Ошибка при создании резервной копии", ex);
                    progress?.Report($"❌ Ошибка: {ex.Message}");
                    return false;
                }
            }
            public async Task<bool> CreateBackupAsync(string databaseName, IProgress<string> progress = null)
            {
                try
                {
                    string sourcePath = Path.Combine(_sourceDirectory, databaseName);
                    if (!File.Exists(sourcePath))
                    {
                        Logger.Error($"База данных не найдена: {databaseName}");
                        return false;
                    }

                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string backupFolder = Path.Combine(_backupDirectory, timestamp);
                    Directory.CreateDirectory(backupFolder);

                    string backupPath = Path.Combine(backupFolder, databaseName);

                    progress?.Report($"Копирование {databaseName}...");
                    await Task.Run(() => File.Copy(sourcePath, backupPath, true));

                    DeleteOldBackups();

                    Logger.Info($"Создан бэкап для {databaseName} -> {backupPath}");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error($"Ошибка при создании бэкапа для {databaseName}", ex);
                    return false;
                }
            }

            private void DeleteOldBackups()
            {
                try
                {
                    var backupFolders = Directory.GetDirectories(_backupDirectory)
                        .Select(d => new DirectoryInfo(d))
                        .OrderByDescending(d => d.CreationTime)
                        .ToList();

                    if (backupFolders.Count > _maxBackupFiles)
                    {
                        var foldersToDelete = backupFolders.Skip(_maxBackupFiles);
                        foreach (var folder in foldersToDelete)
                        {
                            Directory.Delete(folder.FullName, true);
                            Logger.Info($"Удален старый бэкап: {folder.Name}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Ошибка при удалении старых бэкапов", ex);
                }
            }

            public bool RestoreFromBackup(string backupFolderPath, bool overwrite = true)
            {
                try
                {
                    if (!Directory.Exists(backupFolderPath))
                    {
                        Logger.Error($"Папка с бэкапом не найдена: {backupFolderPath}");
                        return false;
                    }

                    var backupFiles = Directory.GetFiles(backupFolderPath, "*.db");
                    foreach (var backupFile in backupFiles)
                    {
                        string fileName = Path.GetFileName(backupFile);
                        string destinationPath = Path.Combine(_sourceDirectory, fileName);

                        // Закрываем все соединения с БД
                        System.Data.SQLite.SQLiteConnection.ClearAllPools();
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();

                        File.Copy(backupFile, destinationPath, overwrite);
                        Logger.Info($"Восстановлен файл: {fileName} из {backupFolderPath}");
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error($"Ошибка при восстановлении из бэкапа {backupFolderPath}", ex);
                    return false;
                }
            }

            public List<BackupInfo> GetBackups()
            {
                var backups = new List<BackupInfo>();

                try
                {
                    if (!Directory.Exists(_backupDirectory))
                        return backups;

                    var backupFolders = Directory.GetDirectories(_backupDirectory);
                    foreach (var folder in backupFolders)
                    {
                        var dirInfo = new DirectoryInfo(folder);
                        var files = Directory.GetFiles(folder, "*.db");

                        backups.Add(new BackupInfo
                        {
                            FolderPath = folder,
                            FolderName = dirInfo.Name,
                            CreationDate = dirInfo.CreationTime,
                            FileCount = files.Length,
                            TotalSize = files.Sum(f => new FileInfo(f).Length),
                            Files = files.Select(f => Path.GetFileName(f)).ToList()
                        });
                    }

                    return backups.OrderByDescending(b => b.CreationDate).ToList();
                }
                catch (Exception ex)
                {
                    Logger.Error("Ошибка при получении списка бэкапов", ex);
                    return backups;
                }
            }

            public BackupInfo GetLatestBackup()
            {
                return GetBackups().FirstOrDefault();
            }
        }

        public class BackupInfo
        {
            public string FolderPath { get; set; }
            public string FolderName { get; set; }
            public DateTime CreationDate { get; set; }
            public int FileCount { get; set; }
            public long TotalSize { get; set; }
            public List<string> Files { get; set; } = new List<string>();

            public string SizeFormatted => FormatSize(TotalSize);
            public string CreationDateFormatted => CreationDate.ToString("dd.MM.yyyy HH:mm:ss");

            private string FormatSize(long bytes)
            {
                string[] sizes = { "Б", "КБ", "МБ", "ГБ" };
                double len = bytes;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }
                return $"{len:0.##} {sizes[order]}";
            }
        }
    }


    public static class AutoBackup
    {
        private static DatabaseBackupManager _backupManager;
        private static Timer _autoBackupTimer;
        private static bool _isBackupInProgress;


        public static void Initialize(string databasesPath, int autoBackupIntervalMinutes = 24)
        {
            try
            {
                _backupManager = new DatabaseBackupManager(databasesPath);

                // Настройка автоматического бэкапа каждые N часов
                _autoBackupTimer = new Timer
                {
                    Interval = autoBackupIntervalMinutes * 60 * 1000 // в миллисекундах
                };
                _autoBackupTimer.Tick += async (s, e) => await AutoBackupTimer_Tick();
                _autoBackupTimer.Start();

                Logger.Info($"Автоматическое резервное копирование инициализировано (интервал: {autoBackupIntervalMinutes} мин.)");

                Task.Run(async () => await CreateBackupAsync());
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации AutoBackup", ex);
            }
        }

        private static async Task AutoBackupTimer_Tick()
        {
            if (_isBackupInProgress) return;
            await CreateBackupAsync();
        }

        public static async Task<bool> CreateBackupAsync(IProgress<string> progress = null)
        {
            if (_backupManager == null)
            {
                Logger.Error("BackupManager не инициализирован");
                return false;
            }

            if (_isBackupInProgress)
            {
                Logger.Warning("Бэкап уже создается");
                return false;
            }

            _isBackupInProgress = true;
            try
            {
                return await _backupManager.CreateBackupAsync(progress);
            }
            finally
            {
                _isBackupInProgress = false;
            }
        }

        public static List<BackupInfo> GetBackups()
        {
            return _backupManager?.GetBackups() ?? new List<BackupInfo>();
        }

        public static bool RestoreFromBackup(string backupFolderPath)
        {
            if (_backupManager == null) return false;
            return _backupManager.RestoreFromBackup(backupFolderPath);
        }

        public static void Stop()
        {
            _autoBackupTimer?.Stop();
            _autoBackupTimer?.Dispose();
            Logger.Info("Автоматическое резервное копирование остановлено");
        }
    }
}