using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

        public DateTime dateBegin;
        public DateTime dateEnd;

        public bool forPeriod;

        private FadeAnimation _fadeAnimation;

        private DataTable _currentDataTable;

        public Dictionary<string, string> userStatus = new Dictionary<string, string>();

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

        private void DataGridViewClients_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentDataTable == null) return;

            string columnName = dataGridViewShowReport.Columns[e.ColumnIndex].Name;
            bool ascending = dataGridViewShowReport.Tag == null || !((bool)dataGridViewShowReport.Tag);

            DataTable sortedTable = SortDataTable(_currentDataTable, columnName, ascending);
            dataGridViewShowReport.DataSource = sortedTable;

            dataGridViewShowReport.Tag = ascending;
        }

        private DataTable SortDataTable(DataTable table, string columnName, bool ascending)
        {
            DataTable sortedTable = table.Clone();

            bool isDateColumn = columnName.Contains("Дата") || columnName.Contains("рождения") ||
                                columnName.Contains("Сохранено");

            IEnumerable<DataRow> sortedRows;

            if (isDateColumn)
            {
                if (ascending)
                {
                    sortedRows = table.AsEnumerable()
                        .OrderBy(row => DateTime.TryParse(row[columnName].ToString(), out DateTime d) ? d : DateTime.MinValue);
                }
                else
                {
                    sortedRows = table.AsEnumerable()
                        .OrderByDescending(row => DateTime.TryParse(row[columnName].ToString(), out DateTime d) ? d : DateTime.MinValue);
                }
            }
            else if (columnName == "Покупки")
            {
                if (ascending)
                {
                    sortedRows = table.AsEnumerable()
                        .OrderBy(row => int.TryParse(row[columnName].ToString(), out int n) ? n : 0);
                }
                else
                {
                    sortedRows = table.AsEnumerable()
                        .OrderByDescending(row => int.TryParse(row[columnName].ToString(), out int n) ? n : 0);
                }
            }
            else
            {
                if (ascending)
                {
                    sortedRows = table.AsEnumerable().OrderBy(row => row[columnName].ToString());
                }
                else
                {
                    sortedRows = table.AsEnumerable().OrderByDescending(row => row[columnName].ToString());
                }
            }

            foreach (DataRow row in sortedRows)
            {
                sortedTable.ImportRow(row);
            }

            return sortedTable;
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
            else if (otherPeriond)
            {
                query = $"SELECT Посетил, Клиент, №Карты, Абонемент FROM Issued WHERE Посетил BETWEEN '{dateBegin}' AND '{dateEnd}' ORDER BY Посетил";
                labelShowPeriod.Text = $"Посещения с {dateBegin.ToShortDateString()} по {dateEnd.ToShortDateString()}";
            }

            DataTable dataTable = GeneralContext.GetDataFromDatabase(query,
                IssuedMembershipContext.ConnectionStringIssued());

            dataTable.Columns.Add("Статус", typeof(string));

            foreach (DataRow row in dataTable.Rows)
            {
                string cardNumber = row["№Карты"].ToString();

                if (userStatus.TryGetValue(cardNumber, out string status))
                {
                    row["Статус"] = status;
                }
                else
                {
                    row["Статус"] = "Неизвестно";
                }
            }

            dataGridViewShowReport.DataSource = dataTable;

            dataGridViewShowReport.Columns["Статус"].DefaultCellStyle.Font = new Font(dataGridViewShowReport.Font, FontStyle.Bold);

            foreach (DataGridViewRow row in dataGridViewShowReport.Rows)
            {
                if (row.Cells["Статус"].Value != null)
                {
                    string status = row.Cells["Статус"].Value.ToString();

                    switch (status)
                    {
                        case "Активен":
                            row.Cells["Статус"].Style.ForeColor = Color.Green;
                            row.Cells["Статус"].Style.Font = new Font(dataGridViewShowReport.Font, FontStyle.Bold);
                            break;
                        case "Неизвестно":
                            row.Cells["Статус"].Style.ForeColor = Color.Black;
                            row.Cells["Статус"].Style.Font = new Font(dataGridViewShowReport.Font, FontStyle.Bold);
                            break;
                        default:
                            row.Cells["Статус"].Style.ForeColor = Color.Red;
                            row.Cells["Статус"].Style.Font = new Font(dataGridViewShowReport.Font, FontStyle.Bold);
                            break;
                    }
                }
            }

            labelQuantity.Text = dataGridViewShowReport.Rows.Count.ToString();
        }

        private void LoadServicesReport()
        {
            DateTime startDate = DateTime.Now;
            string query = string.Empty;

            labelShowPeriod.Text = $"Абонементы";
            labelAllClients.Text = "Всего продано:";
            labelQuantity.Text = GeneralContext.GetElementFromDatabase($"SELECT SUM(Проданных_за_месяц) FROM Descriptions",
            ServicesContext.ConnectionStringServices()).ToString();
            dataGridViewShowReport.DataSource = GeneralContext.GetElementFromDatabase("SELECT * FROM Descriptions",
            ServicesContext.ConnectionStringServices());
        }
    }
}