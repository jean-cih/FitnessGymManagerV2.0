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
using System.Runtime.InteropServices; // Нужен для работы с Win32 API
using System.Diagnostics;             // Нужен для работы с процессами

namespace GymApplicationV2._0
{
    internal static class Program
    {
        /*
        // ---- Импорт функций Windows API для гарантированного управления окнами ----
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        // Делегат для обхода окон системы
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        // Константы Win32 API
        private const int SW_RESTORE = 9;
        private const int SW_SHOW = 5;

        // Уникальный идентификатор приложения (Mutex)
        private static readonly string MutexName = "Local\\GymApplicationV2.0_Unique_Mutex_ID";
        private static Mutex _appMutex;
        */

        private static bool isNewInstance = true;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Проверяем, запущено ли уже приложение
            //_appMutex = new Mutex(true, MutexName, out bool isNewInstance);

            if (!isNewInstance)
            {
                // Если приложение УЖЕ запущено, находим его окно и разворачиваем его
                //ActivateExistingInstance();
                // Завершаем работу текущего (нового) процесса
            }
            else
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
                //finally
                //{
                //    if (_appMutex != null)
                //    {
                //        _appMutex.Close();
                //   }
                //}
            }
        }
        /*
        /// <summary>
        /// Находит уже запущенный процесс программы, восстанавливает его окно и выводит на передний план.
        /// </summary>
        private static void ActivateExistingInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();

            // Ищем процесс с точно таким же именем (например, "GYM MASTER")
            foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (process.Id != currentProcess.Id)
                {
                    // Находим настоящее окно формы по ID процесса
                    IntPtr windowHandle = FindMainWindowWithTitle(process.Id);

                    if (windowHandle != IntPtr.Zero)
                    {
                        // Насильно выводим окно на передний план, обходя блокировки Windows
                        ForceForegroundWindow(windowHandle);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Сканирует все окна процесса и находит то, у которого есть текстовый заголовок (это наша Form1)
        /// </summary>
        private static IntPtr FindMainWindowWithTitle(int processId)
        {
            IntPtr foundWindow = IntPtr.Zero;
            IntPtr fallbackWindow = IntPtr.Zero;

            EnumWindows((hWnd, lParam) =>
            {
                GetWindowThreadProcessId(hWnd, out uint windowProcessId);
                if (windowProcessId == processId)
                {
                    fallbackWindow = hWnd; // Запоминаем хоть какое-то окно на крайний случай

                    // Проверяем длину заголовка окна. У служебных окон WinForms длина заголовка всегда 0.
                    int length = GetWindowTextLength(hWnd);
                    if (length > 0)
                    {
                        foundWindow = hWnd;
                        return false; // Нашли настоящее окно приложения, прекращаем поиск
                    }
                }
                return true; // Продолжаем обход окон
            }, IntPtr.Zero);

            return foundWindow != IntPtr.Zero ? foundWindow : fallbackWindow;
        }

        /// <summary>
        /// Принудительно восстанавливает окно и выводит его поверх всех остальных окон в ОС
        /// </summary>
        private static void ForceForegroundWindow(IntPtr hWnd)
        {
            // Если окно свернуто в панель задач — восстанавливаем его
            if (IsIconic(hWnd))
            {
                ShowWindow(hWnd, SW_RESTORE);
            }
            else
            {
                ShowWindow(hWnd, SW_SHOW);
            }

            // Трюк для обхода защиты Windows от кражи фокуса (AttachThreadInput)
            IntPtr foregroundWindow = GetForegroundWindow();
            uint foregroundThreadId = GetWindowThreadProcessId(foregroundWindow, out _);
            uint currentThreadId = GetWindowThreadProcessId(hWnd, out _);

            if (foregroundThreadId != currentThreadId)
            {
                AttachThreadInput(foregroundThreadId, currentThreadId, true);
                SetForegroundWindow(hWnd);
                AttachThreadInput(foregroundThreadId, currentThreadId, false);
            }
            else
            {
                SetForegroundWindow(hWnd);
            }
        }
        */

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
                MessageBox.Show($"Ошибка загрузки настроек: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            splash.UpdateProgress("Проверка БД абонементов...", "Базы данных", 80);

            if (!File.Exists(GetDatabasePath("Databases", "IssuedMembership.db")))
            {
                IssuedMembershipContext.CreatingDatabase();
            }
            Thread.Sleep(100);
        }

        private static void CheckIfDataExistsProducts(LoadingScreen splash)
        {
            splash.UpdateProgress("Проверка БД товаров...", "Базы данных", 90);

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
