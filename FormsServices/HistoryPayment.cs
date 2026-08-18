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
            try
            {
                InitializeComponent();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                FontHelper.ApplyFontSettings(this, null);

                Logger.Info("Форма HistoryPayment инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации HistoryPayment", ex);
                throw;
            }
        }

        private void HistoryPayment_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка HistoryPayment_Load");

                Width = (int)(Screen.PrimaryScreen.Bounds.Width * FormWidthRatio);
                Height = (int)(Screen.PrimaryScreen.Bounds.Height * FormHeightRatio);

                var _currentDataTable = GeneralContext.GetDataFromDatabase(BaseQuery,
                    HistoryPaymentContext.ConnectionStringPayment());
                dataGridViewHistory.DataSource = _currentDataTable;

                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2,
                    Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);

                radioForMonth.Location = new Point(this.Width - 200, 10);
                radioForWeek.Location = new Point(this.Width - 200, 30);
                radioForDay.Location = new Point(this.Width - 200, 50);
                radioOtherPeriod.Location = new Point(this.Width - 200, 70);

                Logger.Info($"Загружено {_currentDataTable?.Rows?.Count ?? 0} записей истории платежей");
                Logger.Info($"Размеры формы настроены: {Width}x{Height}");
                Logger.Info("HistoryPayment_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в HistoryPayment_Load", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки формы", "Ошибка");
            }
        }

        private void jeanModernButtonShow_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Нажата кнопка 'Показать'");
                LoadHistoryData();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonShow_Click", ex);
                MessageHelper.MessageWindowOk($"Ошибка при загрузке данных: {ex.Message}", "Ошибка");
            }
        }

        private void jeanModernButtonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Нажата кнопка 'Обновить'");
                var _currentDataTable = GeneralContext.GetDataFromDatabase(BaseQuery,
                    HistoryPaymentContext.ConnectionStringPayment());
                radioOtherPeriod.Checked = true;

                dataGridViewHistory.DataSource = _currentDataTable;

                Logger.Info($"Данные обновлены, загружено {_currentDataTable?.Rows?.Count ?? 0} записей");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonRefresh_Click", ex);
                MessageHelper.MessageWindowOk($"Ошибка при обновлении данных: {ex.Message}", "Ошибка");
            }
        }

        private void LoadHistoryData()
        {
            try
            {
                var now = DateTime.Now;
                string filter = "";
                string countFilter = "";
                string periodDescription = "";

                if (radioForMonth.Checked)
                {
                    var beginMonth = new DateTime(now.Year, now.Month, 1);
                    filter = $"WHERE Дата_платежа BETWEEN '{beginMonth:yyyy-MM-dd HH:mm:ss}' AND '{now:yyyy-MM-dd HH:mm:ss}' ORDER BY Дата_платежа";
                    countFilter = $"WHERE Дата_платежа BETWEEN '{beginMonth:yyyy-MM-dd HH:mm:ss}' AND '{now:yyyy-MM-dd HH:mm:ss}'";
                    periodDescription = $"за месяц ({beginMonth:yyyy-MM-dd} - {now:yyyy-MM-dd})";
                }
                else if (radioForWeek.Checked)
                {
                    var startLastWeek = now.AddDays(-(int)now.DayOfWeek + 1);
                    filter = $"WHERE Дата_платежа BETWEEN '{startLastWeek:yyyy-MM-dd HH:mm:ss}' AND '{now:yyyy-MM-dd HH:mm:ss}' ORDER BY Дата_платежа";
                    countFilter = $"WHERE Дата_платежа BETWEEN '{startLastWeek:yyyy-MM-dd HH:mm:ss}' AND '{now:yyyy-MM-dd HH:mm:ss}'";
                    periodDescription = $"за неделю ({startLastWeek:yyyy-MM-dd} - {now:yyyy-MM-dd})";
                }
                else if (radioForDay.Checked)
                {
                    var startDay = new DateTime(now.Year, now.Month, now.Day);
                    filter = $"WHERE Дата_платежа BETWEEN '{startDay:yyyy-MM-dd HH:mm:ss}' AND '{now:yyyy-MM-dd HH:mm:ss}' ORDER BY Дата_платежа";
                    countFilter = $"WHERE Дата_платежа BETWEEN '{startDay:yyyy-MM-dd HH:mm:ss}' AND '{now:yyyy-MM-dd HH:mm:ss}'";
                    periodDescription = $"за день ({startDay:yyyy-MM-dd})";
                }
                else if (radioOtherPeriod.Checked)
                {
                    var beginDate = jeanDateTimePickerBegin.Value;
                    var endDate = jeanDateTimePickerEnd.Value;
                    filter = $"WHERE Дата_платежа BETWEEN '{beginDate:yyyy-MM-dd}' AND '{endDate:yyyy-MM-dd}' ORDER BY Дата_платежа";
                    countFilter = $"WHERE Дата_платежа BETWEEN '{beginDate:yyyy-MM-dd}' AND '{endDate:yyyy-MM-dd}'";
                    periodDescription = $"за период ({beginDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd})";
                }

                Logger.Info($"Загрузка истории платежей {periodDescription}");

                var dataQuery = string.IsNullOrEmpty(filter) ? BaseQuery : $"{BaseQuery} {filter}";
                var countQuery = string.IsNullOrEmpty(countFilter) ? CountQuery : $"{CountQuery} {countFilter}";

                var _currentDataTable = GeneralContext.GetDataFromDatabase(dataQuery,
                    HistoryPaymentContext.ConnectionStringPayment());
                dataGridViewHistory.DataSource = _currentDataTable;

                var count = GeneralContext.GetElementFromDatabase(countQuery, HistoryPaymentContext.ConnectionStringPayment());
                labelPayments.Text = $"Платежей{(string.IsNullOrEmpty(countFilter) ? " за все время" : " за период")}: {count}";

                Logger.Info($"Загружено {_currentDataTable?.Rows?.Count ?? 0} платежей, всего: {count}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в LoadHistoryData", ex);
                throw;
            }
        }

        private void jeanSoftTextBoxSearch__TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(jeanSoftTextBoxSearch.Texts))
                {
                    jeanModernButtonErase.Visible = false;
                    var _currentDataTableBase = GeneralContext.GetDataFromDatabase(BaseQuery,
                        HistoryPaymentContext.ConnectionStringPayment());
                    dataGridViewHistory.DataSource = _currentDataTableBase;
                    Logger.Info("Поиск очищен, загружены все записи");
                    return;
                }

                jeanModernButtonErase.Visible = true;
                var searchQuery = BuildSearchQuery(jeanSoftTextBoxSearch.Texts);
                var _currentDataTable = GeneralContext.GetDataFromDatabase(searchQuery,
                    HistoryPaymentContext.ConnectionStringPayment());
                dataGridViewHistory.DataSource = _currentDataTable;

                Logger.Info($"Поиск платежей: '{jeanSoftTextBoxSearch.Texts}', найдено {_currentDataTable?.Rows?.Count ?? 0} записей");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при поиске: {jeanSoftTextBoxSearch.Texts}", ex);
            }
        }

        private string BuildSearchQuery(string searchText)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в BuildSearchQuery для '{searchText}'", ex);
                return BuildSimpleSearchQuery(searchText);
            }
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
            try
            {
                jeanSoftTextBoxSearch.Texts = "";
                Logger.Info("Очищен поисковый запрос в истории платежей");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при очистке поиска", ex);
            }
        }
    }
}