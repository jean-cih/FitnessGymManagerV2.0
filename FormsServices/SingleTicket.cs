using GymApplicationV2._0.Connections;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private Timer _fadeTimer;
        private float _opacity = 0;

        public SingleTicket()
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

        private void SingleTicket_Load(object sender, EventArgs e)
        {
            Width = (int)(Screen.PrimaryScreen.Bounds.Width * FormWidthRatio);
            Height = (int)(Screen.PrimaryScreen.Bounds.Height * FormHeightRatio);

            SetFonts();
            InitializeData();
        }

        private void SetFonts()
        {
            jeanModernButtonSell.Font = new Font("Продать", DataClass.sizeFontButtons);
            dataGridViewClients.DefaultCellStyle.Font = new Font("Contacts", DataClass.sizeFontTables);
            dataGridViewClients.ColumnHeadersDefaultCellStyle.Font = new Font("Contacts", DataClass.sizeFontTables);
            checkBoxVisited.Font = new Font("Отметить посещение сразу", DataClass.sizeFontCaptions - 2);
        }

        private void InitializeData()
        {
            if (!File.Exists("Databases\\Clients.db"))
            {
                ClientsContext.CreatingDatabase();
            }
            else
            {
                LoadClientData();
            }
        }

        private void LoadClientData()
        {
            dataGridViewClients.DataSource = GeneralContext.GetDataFromDatabase("SELECT " +
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
        }

        private void dataGridViewClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count == 0) return;

            var row = dataGridViewClients.SelectedRows[0];
            _surname = row.Cells[0].Value.ToString();
            _name = row.Cells[1].Value.ToString();
            _fatherName = row.Cells[6].Value.ToString();
            _cardNumber = row.Cells[4].Value.ToString();
            _phone = row.Cells[3].Value.ToString();

            labelName.Text = $"{_surname} {_name} : {_cardNumber}";
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            _clientId = GeneralContext.GetElementFromDatabase($"SELECT Id FROM Contacts WHERE Фамилия = '{_surname}' AND Имя = '{_name}' AND Отчество = '{_fatherName}' AND №Карты = '{_cardNumber}' AND Телефон = '{_phone}'",
                ClientsContext.ConnectionStringClients()).ToString();

            if (_clientId == "")
            {
                Message.MessageWindowOk("Выберите клиента");
                return;
            }

            ProcessSingleTicketSale();
        }

        private void ProcessSingleTicketSale()
        {
            if (ExistMembership())
            {
                UpdateClientRecord();
                AddPaymentHistory();
                Message.MessageWindowOk("Разовое посещение продано");
            }
        }

        private bool ExistMembership()
        {
            var _membershipId = GeneralContext.GetElementFromDatabase($"SELECT Дата_окончания FROM Issued WHERE Клиент = '{_surname} {_name} {_fatherName}' OR №Карты = '{_cardNumber}'",
                IssuedMembershipContext.ConnectionStringIssued());

            if (_membershipId != DBNull.Value && DateTime.Now < Convert.ToDateTime(_membershipId))
            {
                Message.MessageWindowOk("У клиента еще действителен абонемент");
                return false;
            }

            return true;
        }

        private void UpdateClientRecord()
        {
            var purchases = GeneralContext.GetElementFromDatabase(
                $"SELECT Покупки FROM Contacts WHERE Id = '{_clientId}'",
                ClientsContext.ConnectionStringClients());


            var currentCosts = purchases != DBNull.Value ? Convert.ToInt32(purchases) : 0;
            var visitsLeft = checkBoxVisited.Checked ? 0 : 1;

            GeneralContext.CommandDataFromDatabase($@"
                UPDATE Contacts SET 
                Покупки = '{currentCosts + SingleTicketPrice}'
                WHERE Id = '{_clientId}'",
                ClientsContext.ConnectionStringClients());

            LoadClientData();
        }

        private void AddPaymentHistory()
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
                cmd.Parameters.AddWithValue("@Дата_платежа", DateTime.Now.ToShortDateString());

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void jeanSoftTextBoxSearch__TextChanged(object sender, EventArgs e)
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
            jeanSoftTextBoxSearch.Texts = string.Empty;
        }
    }
}