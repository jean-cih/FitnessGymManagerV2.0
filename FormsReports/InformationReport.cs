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
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            dataGridViewShowReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (forPeriod)
            {
                LoadPeriodClientsReport();
            }
            else if (sellServices)
            {
                LoadServicesReport();
            }

            FontHelper.ApplyFontSettings(this, null);
        }
        private void LoadPeriodClientsReport()
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
            }
            else if (periodForDay)
            {
                startDate = DateTime.Now.Date;
                DateTime endDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{startDate}' AND '{endDate}' ORDER BY Посетил";
                labelShowPeriod.Text = $"Посещения за {startDate.ToShortDateString()}";
            }
            else if (otherPeriod)
            {
                // Для произвольного периода
                DateTime beginDate = dateBegin.Date;
                DateTime endDate = dateEnd.Date.AddDays(1).AddSeconds(-1); // Конец дня
                query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{beginDate}' AND '{endDate}' ORDER BY Посетил";
                labelShowPeriod.Text = $"Посещения с {dateBegin.ToShortDateString()} по {dateEnd.ToShortDateString()}";
            }

            DataTable dataTable = GeneralContext.GetDataFromDatabase(query,
                IssuedMembershipContext.ConnectionStringIssued());

            dataGridViewShowReport.DataSource = dataTable;

            labelQuantity.Text = dataGridViewShowReport.Rows.Count.ToString();
        }

        private void LoadServicesReport()
        {
            DateTime startDate = DateTime.Now;
            string query = "SELECT * FROM Descriptions";

            labelShowPeriod.Text = $"Абонементы";
            labelAllClients.Text = "Всего продано:";

            labelQuantity.Text = GeneralContext.GetElementFromDatabase($"SELECT SUM(Проданных_за_месяц) FROM Descriptions",
            ServicesContext.ConnectionStringServices()).ToString();

            dataGridViewShowReport.DataSource = GeneralContext.GetDataFromDatabase("SELECT Абонемент, Цена, Проданных_за_месяц AS 'Проданных за месяц'  FROM Descriptions WHERE Проданных_за_месяц != 0",
            ServicesContext.ConnectionStringServices());
        }
    }
}