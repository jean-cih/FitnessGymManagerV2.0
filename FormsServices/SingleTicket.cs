using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Helpers;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class SingleTicket : Form
    {
        private const double FormWidthRatio = 0.65;
        private const double FormHeightRatio = 0.65;
        private const int SingleTicketPrice = 250;
        private const string SingleTicketType = "Разовое";

        private string _cardNumber = "";
        private string _surname = "";
        private string _name = "";
        private string _fatherName = "";
        private string _phone = "";
        private string _clientId = "";

        private FadeAnimation _fadeAnimation;

        public SingleTicket()
        {
            try
            {
                InitializeComponent();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                Logger.Info("Форма SingleTicket инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации SingleTicket", ex);
                throw;
            }
        }

        private void SingleTicket_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка SingleTicket_Load");

                Width = (int)(Screen.PrimaryScreen.Bounds.Width * FormWidthRatio);
                Height = (int)(Screen.PrimaryScreen.Bounds.Height * FormHeightRatio);

                InitializeData();

                FontHelper.ApplyFontSettings(this, null);

                Logger.Info($"Размеры формы настроены: {Width}x{Height}");
                Logger.Info("SingleTicket_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в SingleTicket_Load", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки формы", "Ошибка");
            }
        }

        private void InitializeData()
        {
            try
            {
                if (!File.Exists("Databases\\Clients.db"))
                {
                    Logger.Info("База данных Clients.db не найдена, создается новая");
                    ClientsContext.CreatingDatabase();
                    Logger.Info("База данных Clients.db создана");
                }
                else
                {
                    Logger.Info("Загрузка данных клиентов");
                    LoadClientData();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeData", ex);
                MessageHelper.MessageWindowOk("Ошибка инициализации данных", "Ошибка");
            }
        }

        private void LoadClientData()
        {
            try
            {
                var _currentDataTable = GeneralContext.GetDataFromDatabase("SELECT " +
                    "Фамилия," +
                    "Имя," +
                    "Телефон," +
                    "№Карты AS 'Карта'," +
                    "Покупки," +
                    "Отчество," +
                    "Email," +
                    "Дата_рождения AS 'Дата рождения'," +
                    "Сохранено" +
                    " FROM Contacts",
                    ClientsContext.ConnectionStringClients());

                dataGridViewClients.DataSource = _currentDataTable;

                Logger.Info($"Загружено {_currentDataTable?.Rows?.Count ?? 0} клиентов");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке данных клиентов", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки данных клиентов", "Ошибка");
            }
        }

        private void dataGridViewClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridViewClients.SelectedRows.Count == 0) return;

                var row = dataGridViewClients.SelectedRows[0];
                _surname = row.Cells[0].Value.ToString();
                _name = row.Cells[1].Value.ToString();
                _fatherName = row.Cells[6].Value.ToString();
                _cardNumber = row.Cells[4].Value.ToString();
                _phone = row.Cells[3].Value.ToString();

                labelName.Text = $"{_surname} {_name} : {_cardNumber}";

                Logger.Info($"Выбран клиент: {_surname} {_name}, карта: {_cardNumber}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при выборе клиента", ex);
            }
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Нажата кнопка 'Продать разовое посещение'");

                var clientIdObj = GeneralContext.GetElementFromDatabase(
                    $"SELECT Id FROM Contacts WHERE Фамилия = '{_surname}' AND Имя = '{_name}' AND Отчество = '{_fatherName}' AND №Карты = '{_cardNumber}' AND Телефон = '{_phone}'",
                    ClientsContext.ConnectionStringClients());

                _clientId = clientIdObj?.ToString() ?? "";

                if (string.IsNullOrEmpty(_clientId))
                {
                    Logger.Warning("Попытка продажи без выбранного клиента");
                    MessageHelper.MessageWindowOk("Выберите клиента", "Предупреждение");
                    return;
                }

                Logger.Info($"Продажа разового билета для клиента ID: {_clientId}, {_surname} {_name}");
                ProcessSingleTicketSale();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при продаже разового билета для клиента {_surname} {_name}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при продаже: {ex.Message}", "Ошибка");
            }
        }

        private void ProcessSingleTicketSale()
        {
            try
            {
                if (ExistMembership())
                {
                    UpdateClientRecord();
                    AddPaymentHistory();
                    MessageHelper.MessageWindowOk("Разовое посещение продано", "Сообщение");
                    Logger.Info($"Разовое посещение успешно продано для клиента {_surname} {_name}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при обработке продажи разового билета для клиента {_surname} {_name}", ex);
                throw;
            }
        }

        private bool ExistMembership()
        {
            try
            {
                var _membershipId = GeneralContext.GetElementFromDatabase(
                    $"SELECT Дата_окончания FROM Issued WHERE Клиент = '{_surname} {_name} {_fatherName}' OR №Карты = '{_cardNumber}'",
                    IssuedMembershipContext.ConnectionStringIssued());

                if (_membershipId != DBNull.Value && _membershipId != null)
                {
                    if (DateTime.TryParse(_membershipId.ToString(), out DateTime endDate))
                    {
                        if (DateTime.Now < endDate)
                        {
                            Logger.Warning($"У клиента {_surname} {_name} еще действителен абонемент до {endDate:yyyy-MM-dd}");
                            MessageHelper.MessageWindowOk("У клиента еще действителен абонемент", "Предупреждение");
                            return false;
                        }
                    }
                }

                Logger.Info($"У клиента {_surname} {_name} нет активного абонемента");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при проверке абонемента для клиента {_surname} {_name}", ex);
                return false;
            }
        }

        private void UpdateClientRecord()
        {
            try
            {
                var purchases = GeneralContext.GetElementFromDatabase(
                    $"SELECT Покупки FROM Contacts WHERE Id = '{_clientId}'",
                    ClientsContext.ConnectionStringClients());

                var currentCosts = (purchases != DBNull.Value && purchases != null) ? Convert.ToInt32(purchases) : 0;
                var newTotal = currentCosts + SingleTicketPrice;

                GeneralContext.CommandDataFromDatabase($@"
                    UPDATE Contacts SET 
                    Покупки = '{newTotal}'
                    WHERE Id = '{_clientId}'",
                    ClientsContext.ConnectionStringClients());

                Logger.Info($"Обновлены покупки клиента ID: {_clientId}, сумма: {newTotal} (было: {currentCosts}, добавлено: {SingleTicketPrice})");
                LoadClientData();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при обновлении записи клиента ID: {_clientId}", ex);
                throw;
            }
        }

        private void AddPaymentHistory()
        {
            try
            {
                var fullName = $"{_surname} {_name} {_fatherName}";

                using (var conn = new SQLiteConnection(HistoryPaymentContext.ConnectionStringPayment()))
                using (var cmd = new SQLiteCommand(
                    @"INSERT INTO History (
                        [Клиент],[Абонемент],[Дата_начала],[Дата_окончания],[Цена],[Дата_платежа]
                    ) VALUES (
                        @Клиент,@Абонемент,@Дата_начала,@Дата_окончания,@Цена,@Дата_платежа
                    )", conn))
                {
                    cmd.Parameters.AddWithValue("@Клиент", fullName);
                    cmd.Parameters.AddWithValue("@Абонемент", SingleTicketType);
                    cmd.Parameters.AddWithValue("@Дата_начала", string.Empty);
                    cmd.Parameters.AddWithValue("@Дата_окончания", string.Empty);
                    cmd.Parameters.AddWithValue("@Цена", SingleTicketPrice);
                    cmd.Parameters.AddWithValue("@Дата_платежа", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Logger.Info($"Добавлена история платежа для клиента {fullName}, сумма: {SingleTicketPrice}, тип: {SingleTicketType}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении истории платежа для клиента {_surname} {_name}", ex);
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
                    LoadClientData();
                    return;
                }

                jeanModernButtonErase.Visible = true;
                var searchQuery = BuildSearchQuery(jeanSoftTextBoxSearch.Texts);
                dataGridViewClients.DataSource = GeneralContext.GetDataFromDatabase(searchQuery,
                    ClientsContext.ConnectionStringClients());

                Logger.Info($"Поиск клиентов: '{jeanSoftTextBoxSearch.Texts}'");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при поиске клиентов: {jeanSoftTextBoxSearch.Texts}", ex);
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
                Фамилия,
                Имя,
                Телефон,
                №Карты AS 'Карта',
                Покупки,
                Отчество,
                Email,
                Дата_рождения AS 'Дата рождения',
                Сохранено
                FROM Contacts 
                WHERE №Карты LIKE '%{names[0]}%'
                OR Фамилия LIKE '%{names[0]}%'
                AND Имя LIKE '%{names[1]}%'
                OR Имя LIKE '%{names[0]}%'
                AND Фамилия LIKE '%{names[1]}%'";
        }

        private string BuildSimpleSearchQuery(string term)
        {
            return $@"SELECT 
                Фамилия,
                Имя,
                Телефон,
                №Карты AS 'Карта',
                Покупки,
                Отчество,
                Email,
                Дата_рождения AS 'Дата рождения',
                Сохранено
                FROM Contacts  
                WHERE №Карты LIKE '%{term}%' 
                OR Фамилия LIKE '%{term}%' 
                OR Имя LIKE '%{term}%'";
        }

        private void jeanModernButtonErase_Click(object sender, EventArgs e)
        {
            try
            {
                jeanSoftTextBoxSearch.Texts = string.Empty;
                Logger.Info("Очищен поисковый запрос");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при очистке поиска", ex);
            }
        }
    }
}