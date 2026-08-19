using GymApplicationV2._0.Connections;
using GymApplicationV2._0.FormsSplashScreens;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    internal static class Program
    {
        private static string _databasesPath;
        private static string _appFilesPath;

        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                LoadingScreen splash = new LoadingScreen();
                splash.Show();

                InitializeApplication(splash);

                splash.Close();

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageHelper.MessageWindowOk(ex.ToString() + " Ошибка при запуске", "Ошибка");
                Logger.Error($"Критическая ошибка при запуске: {ex.Message}", ex);
            }
            finally
            {

                CreateShutdownBackup();
            }
        }

        private static void InitializeApplication(LoadingScreen splash)
        {
            splash.UpdateProgress("Создание структуры папок...", "Инициализация", 5);
            Thread.Sleep(50);

            splash.UpdateProgress("Создание ресурсов", "Ресурсы", 10);
            EnsureRequiredDirectoriesExist();

            Logger.Initialize(_appFilesPath);
            Logger.Info("\n=== ПРИЛОЖЕНИЕ ЗАПУЩЕНО: " + DateTime.Now + " ===\n");

            CopyPhotosToOutput();

            CheckIfConfigExists(splash);

            CheckIfDataExistsClients(splash);
            CheckIfDataExistsServices(splash);
            CheckIfDataExistsPayment(splash);
            CheckIfDataExistsArchive(splash);
            CheckIfDataExistsIssued(splash);
            CheckIfDataExistsProducts(splash);

            Thread.Sleep(100);

            LoadSettings();

            // Инициализация системы резервного копирования
            InitializeBackupSystem(splash);

            splash.UpdateProgress("Готово!", "Запуск приложения", 100);
            Thread.Sleep(300);
        }

        private static void InitializeBackupSystem(LoadingScreen splash)
        {
            splash.UpdateProgress("Инициализация системы бэкапов...", "Резервное копирование", 85);

            try
            {
                _databasesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases");
                AutoBackup.Initialize(_databasesPath, autoBackupIntervalMinutes: 180);

                splash.UpdateProgress("Создание резервной копии...", "Резервное копирование", 90);

                IProgress<string> progress = new Progress<string>(msg =>
                    splash.UpdateProgress(msg, "Резервное копирование", 92));

                var task = AutoBackup.CreateBackupAsync(progress);
                task.Wait(5000);

                Logger.Info("✅ Система резервного копирования инициализирована");
                Logger.Info($"📁 Папка бэкапов: {Path.Combine(_databasesPath, "Backups")}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации системы бэкапов", ex);
            }

            Thread.Sleep(100);
        }

        private static void CreateShutdownBackup()
        {
            try
            {
                if (!string.IsNullOrEmpty(_databasesPath))
                {
                    Logger.Info("=== ПРИЛОЖЕНИЕ ЗАКРЫВАЕТСЯ: " + DateTime.Now + " ===");
                    Logger.Info("Создание финального бэкапа...");

                    var task = AutoBackup.CreateBackupAsync();
                    task.Wait(3000);

                    if (task.Result)
                    {
                        Logger.Info("✅ Финальный бэкап создан успешно");
                    }
                    else
                    {
                        Logger.Warning("⚠️ Финальный бэкап не создан");
                    }

                    AutoBackup.Stop();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при создании финального бэкапа: {ex.Message}");
            }
        }

        private static void LoadSettings()
        {
            try
            {
                DataConfig.sizeFontCaptions = ConfigManager.GetSetting<int>("headlineSize");
                DataConfig.sizeFontButtons = ConfigManager.GetSetting<int>("sizeKeyName");
                DataConfig.sizeFontTables = ConfigManager.GetSetting<int>("sizeTableTitle");
                DataConfig.sizeFontText = ConfigManager.GetSetting<int>("textSize");
                DataConfig.styleForm = ConfigManager.GetSetting<string>("designForm");
                DataConfig.styleBackground = ConfigManager.GetSetting<string>("designBackground");

                Logger.Info("Настройки загружены успешно");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка загрузки настроек: {ex.Message}", ex);
                MessageHelper.MessageWindowOk($"Ошибка загрузки настроек: {ex.Message}", "Ошибка");
            }
        }

        public static void CopyPhotosToOutput()
        {
            try
            {
                string repoRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"));
                string sourcePath = Path.Combine(repoRoot, "Photos");
                string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Photos");

                if (Directory.Exists(sourcePath))
                {
                    CopyDirectory(sourcePath, targetPath);
                    Logger.Info("Фото скопированы успешно");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при копировании фото: {ex.Message}", ex);
            }
        }

        private static void CopyDirectory(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);
            }

            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                CopyDirectory(subDir, Path.Combine(targetDir, Path.GetFileName(subDir)));
            }
        }

        public static void EnsureRequiredDirectoriesExist()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Создаем папку Databases
            _databasesPath = Path.Combine(appDirectory, "Databases");
            if (!Directory.Exists(_databasesPath))
            {
                Directory.CreateDirectory(_databasesPath);
            }

            // Создаем папку AppFiles
            _appFilesPath = Path.Combine(appDirectory, "AppFiles");
            if (!Directory.Exists(_appFilesPath))
            {
                Directory.CreateDirectory(_appFilesPath);
            }

            // Создаем папку для бэкапов
            string backupsPath = Path.Combine(_databasesPath, "Backups");
            if (!Directory.Exists(backupsPath))
            {
                Directory.CreateDirectory(backupsPath);
            }
        }

        private static void CheckIfConfigExists(LoadingScreen splash)
        {
            splash.UpdateProgress("Загрузка конфигурации...", "Инициализация", 20);
            ConfigManager.Initialize();
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsClients(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД клиентов...", "Базы данных", 30);

            if (!File.Exists(GetDatabasePath("Databases", "Clients.db")))
            {
                ClientsContext.CreatingDatabase();
                Logger.Info("Создана база данных Clients.db");
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsServices(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД услуг...", "Базы данных", 40);

            if (!File.Exists(GetDatabasePath("Databases", "Services.db")))
            {
                ServicesContext.CreatingDatabase();
                Logger.Info("Создана база данных Services.db");
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsPayment(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД платежей...", "Базы данных", 60);

            if (!File.Exists(GetDatabasePath("Databases", "Payments.db")))
            {
                HistoryPaymentContext.CreatingDatabase();
                Logger.Info("Создана база данных Payments.db");
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsArchive(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД архива...", "Базы данных", 70);

            if (!File.Exists(GetDatabasePath("Databases", "Archive.db")))
            {
                ArchiveServicesContext.CreatingDatabase();
                Logger.Info("Создана база данных Archive.db");
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsIssued(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД абонементов...", "Базы данных", 75);

            if (!File.Exists(GetDatabasePath("Databases", "IssuedMembership.db")))
            {
                IssuedMembershipContext.CreatingDatabase();
                Logger.Info("Создана база данных IssuedMembership.db");
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsProducts(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД товаров...", "Базы данных", 80);

            if (!File.Exists(GetDatabasePath("Databases", "Products.db")))
            {
                ProductsContext.CreatingDatabase();
                Logger.Info("Создана база данных Products.db");
            }
            Thread.Sleep(150);
        }

        private static string GetDatabasePath(string dir_db, string db)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dbFolder = Path.Combine(appDirectory, dir_db);
            string dbPath = Path.Combine(dbFolder, db);

            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            return dbPath;
        }
    }
}