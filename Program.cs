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

        private static bool isNewInstance = true;

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
            }
        }

        private static void InitializeApplication(LoadingScreen splash)
        {
            splash.UpdateProgress("Создание структуры папок...", "Инициализация", 5);
            Thread.Sleep(50);

            splash.UpdateProgress("Создание ресурсов", "Ресурсы", 10);
            EnsureRequiredDirectoriesExist();

            CopyPhotosToOutput();

            CheckIfConfigExists(splash);

            CheckIfDataExistsClients(splash);
            CheckIfDataExistsServices(splash);
            CheckIfDataExistsPayment(splash);
            CheckIfDataExistsArchive(splash);
            CheckIfDataExistsIssued(splash);
            CheckIfDataExistsProducts(splash);

            splash.UpdateProgress("Миграция данных...", "Обновление", 85);
            DatabaseMigration.MigrateDatesToIsoFormat();
            Thread.Sleep(100);

            LoadSettings();

            splash.UpdateProgress("Готово!", "Запуск приложения", 100);
            Thread.Sleep(300);
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
            }
            catch (Exception ex)
            {
                MessageHelper.MessageWindowOk($"Ошибка загрузки настроек: {ex.Message}", "Ошибка");
            }
        }

        public static void CopyPhotosToOutput()
        {
            string repoRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"));
            string sourcePath = Path.Combine(repoRoot, "Photos");
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Photos");

            if (Directory.Exists(sourcePath))
            {
                CopyDirectory(sourcePath, targetPath);
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
            string databasesPath = Path.Combine(appDirectory, "Databases");
            if (!Directory.Exists(databasesPath))
            {
                Directory.CreateDirectory(databasesPath);
            }

            // Создаем папку AppFiles
            string appFilesPath = Path.Combine(appDirectory, "AppFiles");
            if (!Directory.Exists(appFilesPath))
            {
                Directory.CreateDirectory(appFilesPath);
            }

            Logger.Initialize(appFilesPath);
            Logger.Info("\n=== ПРИЛОЖЕНИЕ ЗАПУЩЕНО: " + DateTime.Now + " ===\n");
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
            }
            Thread.Sleep(100);
        }


        private static void CheckIfDataExistsServices(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД услуг...", "Базы данных", 40);

            if (!File.Exists(GetDatabasePath("Databases", "Services.db")))
            {
                ServicesContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsPayment(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД платежей...", "Базы данных", 60);

            if (!File.Exists(GetDatabasePath("Databases", "Payments.db")))
            {
                HistoryPaymentContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsArchive(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД архива...", "Базы данных", 70);

            if (!File.Exists(GetDatabasePath("Databases", "Archive.db")))
            {
                ArchiveServicesContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsIssued(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД абонементов...", "Базы данных", 75);

            if (!File.Exists(GetDatabasePath("Databases", "IssuedMembership.db")))
            {
                IssuedMembershipContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsProducts(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД товаров...", "Базы данных", 80);

            if (!File.Exists(GetDatabasePath("Databases", "Products.db")))
            {
                ProductsContext.CreatingDatabase();
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
