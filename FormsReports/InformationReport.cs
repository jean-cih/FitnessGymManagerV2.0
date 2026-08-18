using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class InformationReport : Form
    {
        public bool periodForMonth;
        public bool periodForWeek;
        public bool periodForDay;
        public bool otherPeriod;

        public DateTime dateBegin;
        public DateTime dateEnd;

        public bool sellServices;
        public bool forPeriod;

        private FadeAnimation _fadeAnimation;

        public InformationReport()
        {
            try
            {
                InitializeComponent();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                Logger.Info("Форма InformationReport инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации InformationReport", ex);
                throw;
            }
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка Attendance_Load");
                dataGridViewShowReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (forPeriod)
                {
                    Logger.Info("Загрузка отчета по посещениям за период");
                    LoadPeriodClientsReport();
                }
                else if (sellServices)
                {
                    Logger.Info("Загрузка отчета по проданным услугам");
                    LoadServicesReport();
                }

                FontHelper.ApplyFontSettings(this, null);
                Logger.Info("Attendance_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в Attendance_Load", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки отчета", "Ошибка");
            }
        }

        private void LoadPeriodClientsReport()
        {
            try
            {
                DateTime startDate = DateTime.Now;
                string query = string.Empty;

                if (periodForMonth)
                {
                    DateTime today = DateTime.Now.Date;
                    startDate = new DateTime(today.Year, today.Month, 1);
                    DateTime endDate = today.AddDays(1).AddSeconds(-1);

                    query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{startDate}' AND '{endDate}' ORDER BY Посетил";
                    labelShowPeriod.Text = $"Посещения с {startDate.ToShortDateString()} по {endDate.ToShortDateString()}";
                    Logger.Info($"Загружен отчет за месяц: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}");
                }
                else if (periodForWeek)
                {
                    DateTime today = DateTime.Now.Date;
                    int daysOffset = (int)today.DayOfWeek - 1;
                    if (daysOffset < 0) daysOffset = 6;

                    startDate = today.AddDays(-daysOffset);
                    DateTime endDate = today.AddDays(1).AddSeconds(-1);

                    query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{startDate}' AND '{endDate}' ORDER BY Посетил";
                    labelShowPeriod.Text = $"Посещения с {startDate.ToShortDateString()} по {endDate.ToShortDateString()}";
                    Logger.Info($"Загружен отчет за неделю: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}");
                }
                else if (periodForDay)
                {
                    startDate = DateTime.Now.Date;
                    DateTime endDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{startDate}' AND '{endDate}' ORDER BY Посетил";
                    labelShowPeriod.Text = $"Посещения за {startDate.ToShortDateString()}";
                    Logger.Info($"Загружен отчет за день: {startDate.ToShortDateString()}");
                }
                else if (otherPeriod)
                {
                    // Для произвольного периода
                    DateTime beginDate = dateBegin.Date;
                    DateTime endDate = dateEnd.Date.AddDays(1).AddSeconds(-1); // Конец дня
                    query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{beginDate}' AND '{endDate}' ORDER BY Посетил";
                    labelShowPeriod.Text = $"Посещения с {dateBegin.ToShortDateString()} по {dateEnd.ToShortDateString()}";
                    Logger.Info($"Загружен отчет за произвольный период: {dateBegin.ToShortDateString()} - {dateEnd.ToShortDateString()}");
                }

                DataTable dataTable = GeneralContext.GetDataFromDatabase(query,
                    IssuedMembershipContext.ConnectionStringIssued());

                dataGridViewShowReport.DataSource = dataTable;

                labelQuantity.Text = dataGridViewShowReport.Rows.Count.ToString();

                Logger.Info($"Загружено {dataGridViewShowReport.Rows.Count} записей посещений");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке отчета по посещениям", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки данных посещений", "Ошибка");
            }
        }

        private void LoadServicesReport()
        {
            try
            {
                DateTime startDate = DateTime.Now;
                string query = "SELECT * FROM Descriptions";

                labelShowPeriod.Text = $"Абонементы";
                labelAllClients.Text = "Всего продано:";

                var totalSold = GeneralContext.GetElementFromDatabase($"SELECT SUM(Проданных_за_месяц) FROM Descriptions",
                    ServicesContext.ConnectionStringServices());

                labelQuantity.Text = totalSold?.ToString() ?? "0";

                dataGridViewShowReport.DataSource = GeneralContext.GetDataFromDatabase(
                    "SELECT Абонемент, Цена, Проданных_за_месяц AS 'Проданных за месяц' FROM Descriptions WHERE Проданных_за_месяц != 0",
                    ServicesContext.ConnectionStringServices());

                Logger.Info($"Загружен отчет по услугам. Всего продано: {labelQuantity.Text}");
                Logger.Info($"Загружено {dataGridViewShowReport.Rows.Count} видов услуг");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке отчета по услугам", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки данных услуг", "Ошибка");
            }
        }
    }
}