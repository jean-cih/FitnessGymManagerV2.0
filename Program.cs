using GymApplicationV2._0.Connections;
using GymApplicationV2._0.FormsServices;
using GymApplicationV2._0.FormsSettings;
using GymApplicationV2._0.FormsSplashScreens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
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

                Application.Run(new Form1());
                //Application.Run(new Design());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка при запуске");
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

            splash.UpdateProgress("Готово!", "Запуск приложения", 100);
            Thread.Sleep(300);
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
        }

        private static void CheckIfConfigExists(LoadingScreen splash)
        {
            splash.UpdateProgress("Загрузка конфигурации...", "Инициализация", 20);

            string fontFilePath = Path.Combine("AppFiles", "Font.json");
            ConfigManager.Initialize(fontFilePath);

            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsClients(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД клиентов...", "Базы данных", 30);

            if (!File.Exists("Databases\\Clients.db"))
            {
                ClientsContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }


        private static void CheckIfDataExistsServices(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД услуг...", "Базы данных", 40);

            if (!File.Exists("Databases\\Services.db"))
            {
                ServicesContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsPayment(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД платежей...", "Базы данных", 60);

            if (!File.Exists("Databases\\Payments.db"))
            {
                HistoryPaymentContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsArchive(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД архива...", "Базы данных", 70);

            if (!File.Exists("Databases\\Archive.db"))
            {
                ArchiveServicesContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsIssued(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД абонементов...", "Базы данных", 80);

            if (!File.Exists("Databases\\IssuedMembership.db"))
            {
                IssuedMembershipContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsProducts(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД товаров...", "Базы данных", 90);

            if (!File.Exists("Databases\\Products.db"))
            {
                ProductsContext.CreatingDatabase();
            }
            Thread.Sleep(150);
        }
    }
}
