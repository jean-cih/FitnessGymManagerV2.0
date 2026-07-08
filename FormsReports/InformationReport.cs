using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class InformationReport : Form
    {
        public bool periodForMonth;
        public bool periodForWeek;
        public bool periodForDay;
        public bool otherPeriond;

        public bool sellServices;
        public bool historyPayment;

        public DateTime dateBegin;
        public DateTime dateEnd;

        public bool allClient;
        public bool forPeriod;

        private Timer _fadeTimer;
        private float _opacity = 0;

        public InformationReport()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;
            SetupAnimation();
        }

        private void SetupAnimation()
        {
            _fadeTimer = new Timer();
            _fadeTimer.Interval = 10;
            _fadeTimer.Tick += (s, e) =>
            {
                _opacity += 0.05f;
                this.Opacity = _opacity;

                if (_opacity >= 1)
                {
                    _fadeTimer.Stop();
                    _fadeTimer.Dispose();
                }
            };
            _fadeTimer.Start();
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadReportData();
        }

        private void SetupDataGridView()
        {
            dataGridViewShowReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewShowReport.DefaultCellStyle.Font = new Font("ShowTables", DataClass.sizeFontTables);
            dataGridViewShowReport.ColumnHeadersDefaultCellStyle.Font = new Font("ShowTables", DataClass.sizeFontTables);
        }

        private void LoadReportData()
        {
            if (allClient)
            {
                LoadAllClientsReport();
            }
            else if (forPeriod)
            {
                LoadPeriodClientsReport();
            }
            else
            {
                LoadServicesReport();
            }
        }

        private void LoadAllClientsReport()
        {
            dataGridViewShowReport.DataSource = GeneralContext.GetDataFromDatabase("SELECT * FROM Contacts",
                ClientsContext.ConnectionStringClients());
            labelQuantity.Text = GeneralContext.GetElementFromDatabase("SELECT COUNT(*) FROM Contacts",
                ClientsContext.ConnectionStringClients()).ToString();
            labelShowPeriod.Text = "Клиенты за все время";
        }

        private void LoadPeriodClientsReport()
        {
            DateTime startDate = DateTime.Now;
            string query = string.Empty;

            if (periodForMonth)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{startDate}' AND '{DateTime.Now}' ORDER BY Посетил";
                labelShowPeriod.Text = $"Посещения с {startDate.ToShortDateString()} по {DateTime.Now.ToShortDateString()}";
            }
            else if (periodForWeek)
            {
                startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
                query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{startDate}' AND '{DateTime.Now}' ORDER BY Посетил";
                labelShowPeriod.Text = $"Посещения с {startDate.ToShortDateString()} по {DateTime.Now.ToShortDateString()}";
            }
            else if (periodForDay)
            {
                startDate = DateTime.Now.Date;
                query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{startDate}' AND '{DateTime.Now}' ORDER BY Посетил";
                labelShowPeriod.Text = $"Посещения с {startDate.ToShortDateString()} по {startDate.AddDays(1).ToShortDateString()}";
            }
            else if (otherPeriond)
            {
                query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{dateBegin}' AND '{dateEnd}' ORDER BY Посетил";
                labelShowPeriod.Text = $"Посещения с {dateBegin.ToShortDateString()} по {dateEnd.ToShortDateString()}";
            }

            dataGridViewShowReport.DataSource = GeneralContext.GetDataFromDatabase(query,
                IssuedMembershipContext.ConnectionStringIssued());
            labelQuantity.Text = dataGridViewShowReport.Rows.Count.ToString();
        }

        private void LoadServicesReport()
        {
            DateTime startDate = DateTime.Now;
            string query = string.Empty;

            if (sellServices)
            {
                labelShowPeriod.Text = $"Абонементы";
                labelAllClients.Text = "Всего продано:";
                labelQuantity.Text = GeneralContext.GetElementFromDatabase($"SELECT SUM(Проданных_за_месяц) FROM Descriptions",
                ServicesContext.ConnectionStringServices()).ToString();
                dataGridViewShowReport.DataSource = GeneralContext.GetElementFromDatabase("SELECT * FROM Descriptions",
                ServicesContext.ConnectionStringServices());
            }
            else if (historyPayment)
            {
                startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
                //query = $"SELECT Посетил, Фамилия, Имя, Телефон, №Карты, Абонемент FROM Contacts WHERE Посетил BETWEEN '{startDate}' AND '{DateTime.Now}' ORDER BY Посетил";
                labelShowPeriod.Text = $"Платежи с {startDate.ToShortDateString()} по {DateTime.Now.ToShortDateString()}";
                dataGridViewShowReport.DataSource = GeneralContext.GetDataFromDatabase("SELECT * FROM History",
                HistoryPaymentContext.ConnectionStringPayment());
                //labelQuantity.Text = ClientsContext.GetElementClient($"SELECT COUNT(*) FROM History WHERE Посетил BETWEEN '{startDate}' AND '{DateTime.Now}'").ToString();
                labelQuantity.Visible = false;
                labelAllClients.Visible = false;
            }
        }
    }
}