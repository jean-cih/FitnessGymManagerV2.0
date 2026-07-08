using GymApplicationV2._0.FormsServices;
using GymApplicationV2._0.FormsSettings;
using GymApplicationV2._0.FormsSplashScreens;
using System;
using System.Collections.Generic;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //LoadingScreen splash = new LoadingScreen();
            //splash.Show();

            //string messageDB = string.Empty;
            //for (int i = 0; i <= 100; i += 10)
            //{
            //    if (i < 20)
            //    {
            //        messageDB = "Инициализация Баз Данных...";
            //    }
            //    else if (i > 20 && i < 40)
            //    {
            //        messageDB = "Загрузка Клиентов...";
            //    }
            //    else if (i > 40 && i < 60)
            //    {
            //        messageDB = "Загрузка Услуг...";
            //    }
            //    else
            //    {
            //        messageDB = "Загрузка Архива...";
            //    }
            //    splash.UpdateProgress($"Загрузка... {i}%", messageDB, i);
            //    Thread.Sleep(100);
            //}

            //splash.Close();


            Application.Run(new Form1());
        }
    }
}
