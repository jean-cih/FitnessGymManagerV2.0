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
    public partial class Services : Form
    {
        private string _termMembership = string.Empty;
        private string _servicesQuantity = string.Empty;
        private string _servicesCost = string.Empty;

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
                " Абонемент," +
                " Цена," +
                " Срок_действия AS 'Срок действия'," +
                " Посещений" +
                " FROM Descriptions",
                ServicesContext.ConnectionStringServices()
            );
        }

        private void dataGridViewServices_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewServices.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewServices.SelectedRows[0];
            labelMembership.Text = selectedRow.Cells[0].Value?.ToString();
            _termMembership = selectedRow.Cells[2].Value?.ToString();
            _servicesCost = selectedRow.Cells[1].Value.ToString();
            _servicesQuantity = selectedRow.Cells[3].Value.ToString();
        }

        private void buttonAddService_Click(object sender, EventArgs e)
        {
            using (var serviceForm = new FieldForService())
            {
                serviceForm.ShowDialog();
                RefreshServicesData();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(labelMembership.Text)) return;

            if (Message.MessageWindowYesNo("Вы действительно хотите удалить услугу?") != DialogResult.Yes)
                return;

            GeneralContext.CommandDataFromDatabase(
                $"DELETE FROM Descriptions WHERE Абонемент = '{labelMembership.Text}'",
                ServicesContext.ConnectionStringServices());

            Message.MessageWindowOk("Услуга удалена");
            RefreshServicesData();
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(labelMembership.Text))
            {
                Message.MessageWindowOk("Нужно сначала выбрать услугу");
                return;
            }

            if (string.IsNullOrWhiteSpace(labelNumberCard.Text))
            {
                Message.MessageWindowOk("Клиент не выбран");
                return;
            }

            ProcessServiceSale();
        }

        private void ProcessServiceSale()
        {
            var (quantityLeft, clientPurchases) = GetClientAndServiceData();
            var visitDate = checkBoxVisited.Checked ? DateTime.Now.ToString() : string.Empty;

            UpdateClientData(clientPurchases);
            UpdateServiceStatistics();
            AddPaymentHistory();
            AddIssuedMembership();

            Message.MessageWindowOk("Данные клиента обновлены");
        }

        private (int? quantityLeft, int clientPurchases) GetClientAndServiceData()
        {
            var quantityLeft = GetServiceQuantityLeft();
            var clientPurchases = GetClientPurchases();
            return (quantityLeft, clientPurchases);
        }

        private int? GetServiceQuantityLeft()
        {
            var left = GeneralContext.GetElementFromDatabase(
                $"SELECT Посещений FROM Descriptions WHERE Абонемент = '{labelMembership.Text}'",
                ServicesContext.ConnectionStringServices());

            if (string.IsNullOrEmpty(left?.ToString())) return null;

            var quantity = Convert.ToInt32(left);
            return checkBoxVisited.Checked ? quantity - 1 : quantity;
        }

        private int GetClientPurchases()
        {
            var purchase = GeneralContext.GetElementFromDatabase(
                $"SELECT Покупки FROM Contacts WHERE №Карты = '{labelNumberCard.Text}'",
                ClientsContext.ConnectionStringClients());

            return purchase == null ? Convert.ToInt32(purchase) : 0;
        }

        private void UpdateClientData(int clientPurchases)
        {
            var now = DateTime.Now;
            var endDate = now.AddMonths(Convert.ToInt32(_termMembership));

            GeneralContext.CommandDataFromDatabase($@"
                UPDATE Contacts SET 
                Покупки = '{clientPurchases + _servicesCost}'
                WHERE №Карты = '{labelNumberCard.Text}'",
                ClientsContext.ConnectionStringClients());
        }

        private void UpdateServiceStatistics()
        {
            var quantity = GeneralContext.GetElementFromDatabase(
                $"SELECT Проданных_за_месяц FROM Descriptions WHERE Абонемент = '{labelMembership.Text}'",
                ServicesContext.ConnectionStringServices());

            int numbers = (quantity != DBNull.Value && quantity != null) ? Convert.ToInt32(quantity) : 0;

            GeneralContext.CommandDataFromDatabase($@"
                UPDATE Descriptions SET 
                Проданных_за_месяц = '{numbers + 1}' 
                WHERE Абонемент = '{labelMembership.Text}'",
                ServicesContext.ConnectionStringServices());
        }

        private void AddPaymentHistory()
        {
            var fatherName = GeneralContext.GetElementFromDatabase(
                $"SELECT Отчество FROM Contacts WHERE №Карты = '{labelNumberCard.Text}'",
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
                cmd.Parameters.AddWithValue("@Абонемент", labelMembership.Text);
                cmd.Parameters.AddWithValue("@Дата_начала", now.ToShortDateString());
                cmd.Parameters.AddWithValue("@Дата_окончания", endDate.ToShortDateString());
                cmd.Parameters.AddWithValue("@Цена", _servicesCost);
                cmd.Parameters.AddWithValue("@Дата_платежа", now.ToShortDateString());

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void AddIssuedMembership()
        {
            var fatherName = GeneralContext.GetElementFromDatabase(
                $"SELECT Отчество FROM Contacts WHERE №Карты = '{labelNumberCard.Text}'",
                ClientsContext.ConnectionStringClients())?.ToString() ?? string.Empty;

            var now = DateTime.Now;
            var endDate = now.AddMonths(Convert.ToInt32(_termMembership));
            var clientName = $"{labelName.Text} {fatherName}";
            var quantityLeft = GetServiceQuantityLeft();

            using (var conn = new SQLiteConnection(IssuedMembershipContext.ConnectionStringIssued()))
            using (var cmd = new SQLiteCommand(
                @"INSERT INTO Issued (
                    [Клиент],[№Карты],[Дата_окончания],[Дата_оформления],
                    [Абонемент],[Оплата],[Статус],[Посещений_осталось],[Окончание_заморозки]
                ) VALUES (
                    @Клиент,@№Карты,@Дата_окончания,@Дата_оформления,
                    @Абонемент,@Оплата,@Статус,@Посещений_осталось,@Окончание_заморозки
                )", conn))
            {
                cmd.Parameters.AddWithValue("@Клиент", clientName);
                cmd.Parameters.AddWithValue("@№Карты", labelNumberCard.Text);
                cmd.Parameters.AddWithValue("@Дата_окончания", endDate.ToShortDateString());
                cmd.Parameters.AddWithValue("@Дата_оформления", now.ToShortDateString());
                cmd.Parameters.AddWithValue("@Абонемент", labelMembership.Text);
                cmd.Parameters.AddWithValue("@Оплата", _servicesCost);
                cmd.Parameters.AddWithValue("@Статус", "активирован");
                cmd.Parameters.AddWithValue("@Посещений_осталось", quantityLeft?.ToString() ?? string.Empty);
                cmd.Parameters.AddWithValue("@Окончание_заморозки", string.Empty);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        private void jeanModernButton1_Click(object sender, EventArgs e)
        {
            DataConfig.membershipId = GeneralContext.GetElementFromDatabase("SELECT Id FROM Descriptions",
                ServicesContext.ConnectionStringServices()).ToString();

            ShowFormWithData(new ChangeService(), form => {
                var f = (ChangeService)form;
                f.jeanTextBoxPrice.Text = _servicesCost;
                f.jeanTextBoxTerm.Text = _termMembership;
                f.jeanTextBoxVisited.Text = _servicesQuantity;
                f.jeanTextBoxName.Text = labelMembership.Text;
            });

            RefreshServicesData();
        }

        private void ShowFormWithData(Form form, Action<Form> setData)
        {
            if (string.IsNullOrWhiteSpace(labelMembership.Text))
            {
                Message.MessageWindowOk("Выберите услугу из таблицы");
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