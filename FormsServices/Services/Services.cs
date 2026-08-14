using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Helpers;
using GymApplicationV2._0.Data;
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
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, null);
        }

        private void Services_Load(object sender, EventArgs e)
        {
            RefreshServicesData();
        }

        private void RefreshServicesData()
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
        }

        private void dataGridViewServices_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewServices.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewServices.SelectedRows[0];
            _id = selectedRow.Cells[0].Value?.ToString();
            _labelMembership = selectedRow.Cells[1].Value?.ToString();
            _servicesCost = selectedRow.Cells[2].Value.ToString();
            _termMembership = selectedRow.Cells[3].Value?.ToString();
            _servicesQuantity = selectedRow.Cells[4].Value.ToString();
            _typeMembership = selectedRow.Cells[5].Value.ToString();
        }

        private void buttonAddService_Click(object sender, EventArgs e)
        {
            using (var serviceForm = new FieldForService())
            {
                serviceForm.UpdateData();

                serviceForm.ShowDialog();
                RefreshServicesData();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_labelMembership)) return;

            if (MessageHelper.MessageWindowYesNo("Вы действительно хотите удалить услугу?") != DialogResult.Yes)
                return;

            GeneralContext.CommandDataFromDatabase(
                $"DELETE FROM Descriptions WHERE Id = '{_id}'",
                ServicesContext.ConnectionStringServices());

            MessageHelper.MessageWindowOk("Услуга удалена", "Сообщение");
            RefreshServicesData();
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_labelMembership))
            {
                MessageHelper.MessageWindowOk("Нужно сначала выбрать услугу", "Предупреждение");
                return;
            }

            if (string.IsNullOrWhiteSpace(NumberCard))
            {
                MessageHelper.MessageWindowOk("Клиент не выбран", "Предупреждение");
                return;
            }

            object _existInIssued = GeneralContext.GetElementFromDatabase($"SELECT Клиент FROM Issued WHERE №Карты LIKE '%{NumberCard}%'",
                IssuedMembershipContext.ConnectionStringIssued());

            if (_existInIssued != null)
            {
                DialogResult result = MessageHelper.MessageWindowYesNo("У клиента уже есть абонемент\nПродать еще один?");
                if(result != DialogResult.Yes) return;
            }

            UpdateClientDataCard(NumberCard);
            ProcessServiceSale();
        }

        private void UpdateClientDataCard(string numberCard)
        {
            string[] names = labelName.Text.Split(' ');

            var updateQuery = $@"UPDATE Contacts SET №Карты = @CardNumber 
              WHERE Имя LIKE '%{names[0]}%' 
                AND Фамилия LIKE '%{names[1]}%'";

            GeneralContext.CommandDataFromDatabase(updateQuery,
                ClientsContext.ConnectionStringClients(), new SQLiteParameter("@CardNumber", numberCard));
        }

        private void UpdateClientData()
        {
            int clientPurchases = GetClientPurchases();

            GeneralContext.CommandDataFromDatabase($@"
                UPDATE Contacts SET 
                Покупки = '{clientPurchases + _servicesCost}'
                WHERE №Карты = '{NumberCard}'",
                ClientsContext.ConnectionStringClients());
        }

        private void ProcessServiceSale()
        {
            UpdateClientData();
            UpdateServiceStatistics();
            AddIssuedMembership();
            AddPaymentHistory();

            MessageHelper.MessageWindowOk("Данные клиента обновлены", "Сообщение");
        }

        private int? GetServiceQuantityLeft()
        {
            var left = GeneralContext.GetElementFromDatabase(
                $"SELECT Посещений FROM Descriptions WHERE Id = '{_id}'",
                ServicesContext.ConnectionStringServices());

            if (string.IsNullOrEmpty(left?.ToString())) return null;

            var quantity = Convert.ToInt32(left);
            return checkBoxVisited.Checked ? quantity - 1 : quantity;
        }

        private int GetClientPurchases()
        {
            var purchase = GeneralContext.GetElementFromDatabase(
                $"SELECT Покупки FROM Contacts WHERE №Карты = '{NumberCard}'",
                ClientsContext.ConnectionStringClients());

            return purchase == null ? Convert.ToInt32(purchase) : 0;
        }

        private void UpdateServiceStatistics()
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
        }

        private void AddPaymentHistory()
        {
            var fatherName = GeneralContext.GetElementFromDatabase(
                $"SELECT Отчество FROM Contacts WHERE №Карты = '{NumberCard}'",
                ClientsContext.ConnectionStringClients())?.ToString() ?? string.Empty;

            var now = DateTime.Now;
            var endDate = now.AddMonths(Convert.ToInt32(_termMembership));
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
                cmd.Parameters.AddWithValue("@Дата_начала", now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Дата_окончания", endDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Цена", _servicesCost);
                cmd.Parameters.AddWithValue("@Дата_платежа", now.ToString("yyyy-MM-dd HH:mm:ss"));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void AddIssuedMembership()
        {
            var fatherName = GeneralContext.GetElementFromDatabase(
                $"SELECT Отчество FROM Contacts WHERE №Карты = '{NumberCard}'",
                ClientsContext.ConnectionStringClients())?.ToString() ?? string.Empty;

            var now = DateTime.Now;
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
                        if (lastEndDate.Date >= now.Date)
                        {
                            endDate = lastEndDate.Date.AddDays(1).AddMonths(termMonths);
                        }
                        else
                        {
                            endDate = now.Date.AddMonths(termMonths);
                        }
                    }
                    else
                    {
                        endDate = now.Date.AddMonths(termMonths);
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
                    cmd.Parameters.AddWithValue("@Дата_оформления", now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Абонемент", _labelMembership);
                    cmd.Parameters.AddWithValue("@Посетил", visitDate);
                    cmd.Parameters.AddWithValue("@Оплата", _servicesCost);
                    cmd.Parameters.AddWithValue("@Статус", "активирован");
                    cmd.Parameters.AddWithValue("@Посещений_осталось", quantityLeft?.ToString() ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Окончание_заморозки", string.Empty);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void jeanModernButton1_Click(object sender, EventArgs e)
        {
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
        }

        private void ShowFormWithData(Form form, Action<Form> setData)
        {
            if (string.IsNullOrWhiteSpace(_labelMembership))
            {
                MessageHelper.MessageWindowOk("Выберите услугу из таблицы", "Сообщение");
                return;
            }

            setData(form);
            form.ShowDialog();
        }

        private void jeanSoftTextBoxPurchase__TextChanged(object sender, EventArgs e)
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
            jeanSoftTextBoxPurchase.Texts = "";
        }
    }
}