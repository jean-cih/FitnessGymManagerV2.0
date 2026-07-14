using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class HistoryPayment : Form
    {
        private const double FormWidthRatio = 0.75;
        private const double FormHeightRatio = 0.75;
        private const string BaseQuery = "SELECT" +
            " Клиент," +
            " Абонемент," +
            " Дата_начала AS 'Дата начала'," +
            " Дата_окончания AS 'Дата окончания'," +
            " Цена," +
            " Дата_платежа AS 'Дата платежа'" +
            " FROM History";

        private const string CountQuery = "SELECT COUNT(*) FROM History";

        private FadeAnimation _fadeAnimation;

        public HistoryPayment()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, null);
        }

        private void HistoryPayment_Load(object sender, EventArgs e)
        {
            Width = (int)(Screen.PrimaryScreen.Bounds.Width * FormWidthRatio);
            Height = (int)(Screen.PrimaryScreen.Bounds.Height * FormHeightRatio);

            dataGridViewHistory.DataSource = GeneralContext.GetDataFromDatabase(BaseQuery,
                HistoryPaymentContext.ConnectionStringPayment());

            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2,
                Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);

            radioForMonth.Location = new Point(this.Width - 200, 10);
            radioForWeek.Location = new Point(this.Width - 200, 30);
            radioForDay.Location = new Point(this.Width - 200, 50);
            radioOtherPeriod.Location = new Point(this.Width - 200, 70);
        }

        private void jeanModernButtonShow_Click(object sender, EventArgs e)
        {
            LoadHistoryData();
        }

        private void jeanModernButtonRefresh_Click(object sender, EventArgs e)
        {
            dataGridViewHistory.DataSource = GeneralContext.GetDataFromDatabase(BaseQuery,
                HistoryPaymentContext.ConnectionStringPayment());
            radioOtherPeriod.Checked = true;
        }

        private void LoadHistoryData()
        {
            var now = DateTime.Now;
            string filter = "";
            string countFilter = "";

            if (radioForMonth.Checked)
            {
                var beginMonth = new DateTime(now.Year, now.Month, 1);
                filter = $"WHERE Дата_платежа BETWEEN '{beginMonth}' AND '{now}' ORDER BY Дата_платежа";
                countFilter = $"WHERE Дата_платежа BETWEEN '{beginMonth}' AND '{now}'";
            }
            else if (radioForWeek.Checked)
            {
                var startLastWeek = now.AddDays(-(int)now.DayOfWeek + 1);
                filter = $"WHERE Дата_платежа BETWEEN '{startLastWeek}' AND '{now}' ORDER BY Дата_платежа";
                countFilter = $"WHERE Дата_платежа BETWEEN '{startLastWeek}' AND '{now}'";
            }
            else if (radioForDay.Checked)
            {
                var startDay = new DateTime(now.Year, now.Month, now.Day);
                filter = $"WHERE Дата_платежа BETWEEN '{startDay}' AND '{now}' ORDER BY Дата_платежа";
                countFilter = $"WHERE Дата_платежа BETWEEN '{startDay}' AND '{now}'";
            }
            else if (radioOtherPeriod.Checked)
            {
                filter = $"WHERE Дата_платежа BETWEEN '{jeanDateTimePickerBegin.Value.ToShortDateString()}' AND '{jeanDateTimePickerEnd.Value.ToShortDateString()}' ORDER BY Дата_платежа";
                countFilter = $"WHERE Дата_платежа BETWEEN '{jeanDateTimePickerBegin.Value.ToShortDateString()}' AND '{jeanDateTimePickerEnd.Value.ToShortDateString()}'";
            }

            var dataQuery = string.IsNullOrEmpty(filter) ? BaseQuery : $"{BaseQuery} {filter}";
            var countQuery = string.IsNullOrEmpty(countFilter) ? CountQuery : $"{CountQuery} {countFilter}";

            dataGridViewHistory.DataSource = GeneralContext.GetDataFromDatabase(dataQuery,
                HistoryPaymentContext.ConnectionStringPayment());
            labelPayments.Text = $"Платежей{(string.IsNullOrEmpty(countFilter) ? " за все время" : " за период")}: " + GeneralContext.GetElementFromDatabase(countQuery,
                HistoryPaymentContext.ConnectionStringPayment());
        }

        private void jeanSoftTextBoxSearch__TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(jeanSoftTextBoxSearch.Texts))
            {
                jeanModernButtonErase.Visible = false;
                dataGridViewHistory.DataSource = GeneralContext.GetDataFromDatabase(BaseQuery,
                HistoryPaymentContext.ConnectionStringPayment());
                return;
            }

            jeanModernButtonErase.Visible = true;
            var searchQuery = BuildSearchQuery(jeanSoftTextBoxSearch.Texts);
            dataGridViewHistory.DataSource = GeneralContext.GetDataFromDatabase(searchQuery,
                HistoryPaymentContext.ConnectionStringPayment());
        }

        private string BuildSearchQuery(string searchText)
        {
            string[] names = searchText.Split(' ');
            for (int i = 0; i < names.Length; i++)
            {
                if (!string.IsNullOrEmpty(names[i]))
                {
                    names[i] = char.ToUpper(names[i][0]) + names[i].Substring(1);
                }
            }

            return names.Length > 1
                ? BuildFullNameSearchQuery(names)
                : BuildSimpleSearchQuery(names[0]);
        }
    
        private string BuildFullNameSearchQuery(string[] names)
        {
            return $@"SELECT 
                    Клиент,
                    Абонемент,
                    Дата_начала AS 'Дата начала',
                    Дата_окончания AS 'Дата окончания',
                    Цена,
                    Дата_платежа AS 'Дата платежа'
                    FROM History 
                    WHERE Клиент LIKE '%{names[0]}%' 
                    AND Клиент LIKE '%{names[1]}%'";
        }

        private string BuildSimpleSearchQuery(string name)
        {
            return $@"SELECT 
                Клиент,
                Абонемент,
                Дата_начала AS 'Дата начала',
                Дата_окончания AS 'Дата окончания',
                Цена,
                Дата_платежа AS 'Дата платежа'
                FROM History
                WHERE Клиент LIKE '%{name}%'";
        }

        private void jeanModernButtonErase_Click(object sender, EventArgs e)
        {
            jeanSoftTextBoxSearch.Texts = "";
        }
    }
}