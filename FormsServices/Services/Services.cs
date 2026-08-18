using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class Services : Form
    {
        private string _id = string.Empty;
        private string _termMembership = string.Empty;
        private string _servicesQuantity = string.Empty;
        private string _servicesCost = string.Empty;
        private string _labelMembership = string.Empty;
        private string _typeMembership = string.Empty;
        public string NumberCard = string.Empty;

        private FadeAnimation _fadeAnimation;

        public Services()
        {
            try
            {
                InitializeComponent();
                InitializeCustomDesign();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                FontHelper.ApplyFontSettings(this, null);

                Logger.Info("Форма Services инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации Services", ex);
                throw;
            }
        }

        private void Services_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка Services_Load");
                RefreshServicesData();
                Logger.Info("Services_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в Services_Load", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки формы", "Ошибка");
            }
        }

        private void InitializeCustomDesign()
        {
            try
            {
                dateActivation.CreateStyledDateTimePicker(new Size(140, 15), new Point(checkBoxVisited.Location.X, checkBoxVisited.Location.Y + 30));
                Logger.Info("Дизайн формы Services инициализирован");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeCustomDesign", ex);
            }
        }

        private void RefreshServicesData()
        {
            try
            {
                dataGridViewServices.DataSource = GeneralContext.GetDataFromDatabase("SELECT" +
                    " Id," +
                    " Абонемент," +
                    " Цена," +
                    " Срок_действия AS 'Срок действия'," +
                    " Посещений," +
                    " Тип" +
                    " FROM Descriptions",
                    ServicesContext.ConnectionStringServices()
                );

                if (dataGridViewServices.Columns["Id"] != null)
                {
                    dataGridViewServices.Columns["Id"].Visible = false;
                }

                Logger.Info($"Данные услуг загружены, строк: {dataGridViewServices.Rows.Count}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке данных услуг", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки данных услуг", "Ошибка");
            }
        }

        private void dataGridViewServices_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridViewServices.SelectedRows.Count == 0) return;

                var selectedRow = dataGridViewServices.SelectedRows[0];
                _id = selectedRow.Cells[0].Value?.ToString();
                _labelMembership = selectedRow.Cells[1].Value?.ToString();
                _servicesCost = selectedRow.Cells[2].Value.ToString();
                _termMembership = selectedRow.Cells[3].Value?.ToString();
                _servicesQuantity = selectedRow.Cells[4].Value.ToString();
                _typeMembership = selectedRow.Cells[5].Value.ToString();

                Logger.Info($"Выбрана услуга: {_labelMembership}, ID: {_id}, цена: {_servicesCost}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при выборе услуги", ex);
            }
        }

        private void buttonAddService_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Нажата кнопка 'Добавить услугу'");
                using (var serviceForm = new FieldForService())
                {
                    serviceForm.UpdateData();
                    serviceForm.ShowDialog();
                    RefreshServicesData();
                    Logger.Info("Диалог добавления услуги закрыт");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при добавлении услуги", ex);
                MessageHelper.MessageWindowOk($"Ошибка при добавлении услуги: {ex.Message}", "Ошибка");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_labelMembership))
                {
                    Logger.Warning("Попытка удаления без выбранной услуги");
                    return;
                }

                Logger.Info($"Нажата кнопка 'Удалить' для услуги: {_labelMembership}, ID: {_id}");

                if (MessageHelper.MessageWindowYesNo("Вы действительно хотите удалить услугу?") != DialogResult.Yes)
                {
                    Logger.Info($"Удаление услуги отменено пользователем: {_labelMembership}");
                    return;
                }

                GeneralContext.CommandDataFromDatabase(
                    $"DELETE FROM Descriptions WHERE Id = '{_id}'",
                    ServicesContext.ConnectionStringServices());

                Logger.Info($"Услуга удалена: {_labelMembership}, ID: {_id}");
                MessageHelper.MessageWindowOk("Услуга удалена", "Сообщение");
                RefreshServicesData();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при удалении услуги {_labelMembership}, ID: {_id}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при удалении: {ex.Message}", "Ошибка");
            }
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Нажата кнопка 'Продать'");

                if (string.IsNullOrWhiteSpace(_labelMembership))
                {
                    Logger.Warning("Попытка продажи без выбранной услуги");
                    MessageHelper.MessageWindowOk("Нужно сначала выбрать услугу", "Предупреждение");
                    return;
                }

                if (string.IsNullOrWhiteSpace(NumberCard))
                {
                    Logger.Warning("Попытка продажи без выбранного клиента");
                    MessageHelper.MessageWindowOk("Клиент не выбран", "Предупреждение");
                    return;
                }

                Logger.Info($"Продажа услуги '{_labelMembership}' для клиента с картой: {NumberCard}");

                object _existInIssued = GeneralContext.GetElementFromDatabase($"SELECT Клиент FROM Issued WHERE №Карты LIKE '%{NumberCard}%'",
                    IssuedMembershipContext.ConnectionStringIssued());

                if (_existInIssued != null)
                {
                    DialogResult result = MessageHelper.MessageWindowYesNo("У клиента уже есть абонемент\nПродать еще один?");
                    if (result != DialogResult.Yes)
                    {
                        Logger.Info($"Продажа отменена пользователем для клиента с картой: {NumberCard}");
                        return;
                    }
                    Logger.Info($"Пользователь подтвердил продажу второго абонемента для клиента с картой: {NumberCard}");
                }

                UpdateClientDataCard(NumberCard);
                ProcessServiceSale();
                Logger.Info($"Услуга успешно продана для клиента с картой: {NumberCard}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при продаже услуги для клиента с картой: {NumberCard}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при продаже: {ex.Message}", "Ошибка");
            }
        }

        private void UpdateClientDataCard(string numberCard)
        {
            try
            {
                string[] names = labelName.Text.Split(' ');

                var updateQuery = $@"UPDATE Contacts SET №Карты = @CardNumber 
                  WHERE Имя LIKE '%{names[0]}%' 
                    AND Фамилия LIKE '%{names[1]}%'";

                GeneralContext.CommandDataFromDatabase(updateQuery,
                    ClientsContext.ConnectionStringClients(), new SQLiteParameter("@CardNumber", numberCard));

                Logger.Info($"Обновлен номер карты для клиента: {labelName.Text}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при обновлении карты клиента: {labelName.Text}", ex);
                throw;
            }
        }

        private void UpdateClientData()
        {
            try
            {
                int clientPurchases = GetClientPurchases();

                GeneralContext.CommandDataFromDatabase($@"
                    UPDATE Contacts SET 
                    Покупки = '{clientPurchases + Convert.ToInt32(_servicesCost)}'
                    WHERE №Карты = '{NumberCard}'",
                    ClientsContext.ConnectionStringClients());

                Logger.Info($"Обновлены покупки клиента с картой: {NumberCard}, сумма: {clientPurchases + Convert.ToInt32(_servicesCost)}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при обновлении данных клиента с картой: {NumberCard}", ex);
                throw;
            }
        }

        private void ProcessServiceSale()
        {
            try
            {
                UpdateClientData();
                UpdateServiceStatistics();
                AddIssuedMembership();
                AddPaymentHistory();

                Logger.Info($"Продажа услуги обработана для клиента с картой: {NumberCard}");
                MessageHelper.MessageWindowOk("Данные клиента обновлены", "Сообщение");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при обработке продажи для клиента с картой: {NumberCard}", ex);
                throw;
            }
        }

        private int? GetServiceQuantityLeft()
        {
            try
            {
                var left = GeneralContext.GetElementFromDatabase(
                    $"SELECT Посещений FROM Descriptions WHERE Id = '{_id}'",
                    ServicesContext.ConnectionStringServices());

                if (string.IsNullOrEmpty(left?.ToString())) return null;

                var quantity = Convert.ToInt32(left);
                return checkBoxVisited.Checked ? quantity - 1 : quantity;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при получении количества посещений для услуги ID: {_id}", ex);
                return null;
            }
        }

        private int GetClientPurchases()
        {
            try
            {
                var purchase = GeneralContext.GetElementFromDatabase(
                    $"SELECT Покупки FROM Contacts WHERE №Карты = '{NumberCard}'",
                    ClientsContext.ConnectionStringClients());

                return purchase == null ? 0 : Convert.ToInt32(purchase);
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при получении покупок клиента с картой: {NumberCard}", ex);
                return 0;
            }
        }

        private void UpdateServiceStatistics()
        {
            try
            {
                var quantity = GeneralContext.GetElementFromDatabase(
                    $"SELECT Проданных_за_месяц FROM Descriptions WHERE Id = '{_id}'",
                    ServicesContext.ConnectionStringServices());

                int numbers = (quantity != DBNull.Value && quantity != null) ? Convert.ToInt32(quantity) : 0;

                GeneralContext.CommandDataFromDatabase($@"
                    UPDATE Descriptions SET 
                    Проданных_за_месяц = '{numbers + 1}' 
                    WHERE Id = '{_id}'",
                    ServicesContext.ConnectionStringServices());

                Logger.Info($"Обновлена статистика услуги {_labelMembership}, ID: {_id}, продано: {numbers + 1}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при обновлении статистики услуги {_labelMembership}, ID: {_id}", ex);
                throw;
            }
        }

        private void AddPaymentHistory()
        {
            try
            {
                var fatherName = GeneralContext.GetElementFromDatabase(
                    $"SELECT Отчество FROM Contacts WHERE №Карты = '{NumberCard}'",
                    ClientsContext.ConnectionStringClients())?.ToString() ?? string.Empty;

                var endDate = dateActivation.Value.AddMonths(Convert.ToInt32(_termMembership));
                var clientName = $"{labelName.Text} {fatherName}";

                using (var conn = new SQLiteConnection(HistoryPaymentContext.ConnectionStringPayment()))
                using (var cmd = new SQLiteCommand(
                    @"INSERT INTO History (
                        [Клиент],[Абонемент],[Дата_начала],[Дата_окончания],[Цена],[Дата_платежа]
                    ) VALUES (
                        @Клиент,@Абонемент,@Дата_начала,@Дата_окончания,@Цена,@Дата_платежа
                    )", conn))
                {
                    cmd.Parameters.AddWithValue("@Клиент", clientName);
                    cmd.Parameters.AddWithValue("@Абонемент", _labelMembership);
                    cmd.Parameters.AddWithValue("@Дата_начала", dateActivation.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Дата_окончания", endDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Цена", _servicesCost);
                    cmd.Parameters.AddWithValue("@Дата_платежа", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Logger.Info($"Добавлена история платежа для клиента: {clientName}, услуга: {_labelMembership}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении истории платежа для клиента с картой: {NumberCard}", ex);
                throw;
            }
        }

        private void AddIssuedMembership()
        {
            try
            {
                var fatherName = GeneralContext.GetElementFromDatabase(
                    $"SELECT Отчество FROM Contacts WHERE №Карты = '{NumberCard}'",
                    ClientsContext.ConnectionStringClients())?.ToString() ?? string.Empty;

                var clientName = $"{labelName.Text} {fatherName}";
                var quantityLeft = GetServiceQuantityLeft();
                var termMonths = Convert.ToInt32(_termMembership);

                DateTime endDate;

                using (var conn = new SQLiteConnection(IssuedMembershipContext.ConnectionStringIssued()))
                {
                    conn.Open();

                    // Проверяем последний абонемент
                    using (var checkCmd = new SQLiteCommand(
                        @"SELECT Дата_окончания FROM Issued 
                  WHERE №Карты = @№Карты 
                  ORDER BY Id DESC LIMIT 1", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@№Карты", NumberCard);

                        var lastEndDateStr = checkCmd.ExecuteScalar()?.ToString();

                        if (!string.IsNullOrEmpty(lastEndDateStr) && DateTime.TryParse(lastEndDateStr, out DateTime lastEndDate))
                        {
                            if (lastEndDate.Date >= dateActivation.Value.Date)
                            {
                                endDate = lastEndDate.Date.AddDays(1).AddMonths(termMonths);
                                Logger.Info($"Продление абонемента с учетом предыдущего: новая дата окончания {endDate:yyyy-MM-dd}");
                            }
                            else
                            {
                                endDate = dateActivation.Value.Date.AddMonths(termMonths);
                                Logger.Info($"Новый абонемент с датой окончания {endDate:yyyy-MM-dd}");
                            }
                        }
                        else
                        {
                            endDate = dateActivation.Value.Date.AddMonths(termMonths);
                            Logger.Info($"Первый абонемент с датой окончания {endDate:yyyy-MM-dd}");
                        }
                    }

                    var visitDate = checkBoxVisited.Checked ? DateTime.Now.ToString() : string.Empty;

                    // Вставляем новую запись
                    using (var cmd = new SQLiteCommand(
                        @"INSERT INTO Issued (
                    [Клиент],[№Карты],[Дата_окончания],[Дата_оформления],
                    [Абонемент],[Посетил],[Оплата],[Статус],[Посещений_осталось],[Окончание_заморозки]
                ) VALUES (
                    @Клиент,@№Карты,@Дата_окончания,@Дата_оформления,
                    @Абонемент,@Посетил,@Оплата,@Статус,@Посещений_осталось,@Окончание_заморозки
                )", conn))
                    {
                        cmd.Parameters.AddWithValue("@Клиент", clientName);
                        cmd.Parameters.AddWithValue("@№Карты", NumberCard);
                        cmd.Parameters.AddWithValue("@Дата_окончания", endDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Дата_оформления", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Абонемент", _labelMembership);
                        cmd.Parameters.AddWithValue("@Посетил", visitDate);
                        cmd.Parameters.AddWithValue("@Оплата", _servicesCost);
                        cmd.Parameters.AddWithValue("@Статус", "активирован");
                        cmd.Parameters.AddWithValue("@Посещений_осталось", quantityLeft?.ToString() ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Окончание_заморозки", string.Empty);

                        cmd.ExecuteNonQuery();
                        Logger.Info($"Добавлен абонемент для клиента: {clientName}, услуга: {_labelMembership}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении абонемента для клиента с картой: {NumberCard}", ex);
                throw;
            }
        }

        private void jeanModernButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info($"Нажата кнопка 'Изменить' для услуги: {_labelMembership}, ID: {_id}");

                ShowFormWithData(new ChangeService(), form => {
                    var f = (ChangeService)form;
                    f.jeanTextBoxPrice.Text = _servicesCost;
                    f.jeanTextBoxTerm.Text = _termMembership;
                    f.jeanTextBoxVisited.Text = _servicesQuantity;
                    f.jeanTextBoxName.Text = _labelMembership;
                    f._typeMembership = _typeMembership;
                    f._id = _id;
                    f.UpdateData();
                });

                RefreshServicesData();
                Logger.Info($"Диалог изменения услуги закрыт, данные обновлены");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при изменении услуги {_labelMembership}, ID: {_id}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при изменении: {ex.Message}", "Ошибка");
            }
        }

        private void ShowFormWithData(Form form, Action<Form> setData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_labelMembership))
                {
                    Logger.Warning("Попытка изменения без выбранной услуги");
                    MessageHelper.MessageWindowOk("Выберите услугу из таблицы", "Сообщение");
                    return;
                }

                setData(form);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ShowFormWithData для формы {form?.GetType().Name}", ex);
                throw;
            }
        }

        private void jeanSoftTextBoxPurchase__TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(jeanSoftTextBoxPurchase.Texts))
                {
                    jeanModernButtonErase.Visible = false;
                    RefreshServicesData();
                    return;
                }

                jeanModernButtonErase.Visible = true;
                var searchQuery = BuildSearchQuery(jeanSoftTextBoxPurchase.Texts);
                dataGridViewServices.DataSource = GeneralContext.GetDataFromDatabase(searchQuery,
                    ServicesContext.ConnectionStringServices());

                Logger.Info($"Поиск услуг: '{jeanSoftTextBoxPurchase.Texts}'");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при поиске услуг: {jeanSoftTextBoxPurchase.Texts}", ex);
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
                Абонемент,
                Цена,
                Срок_действия,
                Посещений,
                Проданных_за_месяц
                FROM Descriptions 
                WHERE Абонемент LIKE '%{names[0]}%' 
                AND Абонемент LIKE '%{names[1]}%'";
        }

        private string BuildSimpleSearchQuery(string name)
        {
            return $@"SELECT 
                Абонемент,
                Цена,
                Срок_действия,
                Посещений,
                Проданных_за_месяц
                FROM Descriptions 
                WHERE Абонемент LIKE '%{name}%'";
        }

        private void jeanModernButtonErase_Click(object sender, EventArgs e)
        {
            try
            {
                jeanSoftTextBoxPurchase.Texts = "";
                Logger.Info("Очищен поисковый запрос");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при очистке поиска", ex);
            }
        }
    }
}