using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.Entity.Infrastructure;
using GymApplicationV2._0.Components;
using GymApplicationV2._0.Connections;
using System.Runtime.Remoting.Contexts;

namespace GymApplicationV2._0
{
    public partial class NewClient : Form
    {
        private Timer _fadeTimer;
        private float _opacity = 0;

        public NewClient()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += jeanTextBoxBirthday_KeyDown;

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

        private void NewClient_Load(object sender, EventArgs e)
        {
            LoadServicesData();
            SetFonts();
        }

        private void LoadServicesData()
        {
            dataGridViewServices.DataSource = GeneralContext.GetDataFromDatabase(
                "SELECT Абонемент, Цена, Срок_действия, Посещений FROM Descriptions",
                ServicesContext.ConnectionStringServices());
        }

        private void SetFonts()
        {
            jeanModernButtonAdd.Font = new Font("Добавить", DataConfig.sizeFontButtons);

            dataGridViewServices.DefaultCellStyle.Font =
            dataGridViewServices.ColumnHeadersDefaultCellStyle.Font =
                new Font("Contacts", DataConfig.sizeFontTables);

            radioButtonMan.Font = new Font("муж", DataConfig.sizeFontCaptions - 2);
            radioButtonWoman.Font = new Font("жен", DataConfig.sizeFontCaptions - 2);
            checkBoxVisited.Font = new Font("Отметить посещение сразу", DataConfig.sizeFontCaptions - 2);
        }

        string lefts = "", price = "", termMembership = "";

        private void dataGridViewServices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            jeanTextBoxPurchase.Text = dataGridViewServices.Rows[e.RowIndex].Cells[0].Value.ToString();
            lefts = dataGridViewServices.Rows[e.RowIndex].Cells[3].Value.ToString();
            price = dataGridViewServices.Rows[e.RowIndex].Cells[1].Value.ToString();
            termMembership = dataGridViewServices.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        bool space;
        private void jeanTextBoxBirthday_KeyDown(object sender, KeyEventArgs e)
        {
            space = e.KeyCode == Keys.Space ? true : false;
        }

        bool isDigit;
        private void jeanTextBoxBirthday_KeyPress(object sender, KeyPressEventArgs e)
        {
            isDigit = char.IsDigit(e.KeyChar) ? true : false;
        }

        private void jeanTextBoxBirthday_TextChanged(object sender, EventArgs e)
        {
            FormatBirthdayInput();
            ValidateBirthday();
        }

        private void FormatBirthdayInput()
        {
            if (space)
            {
                if (jeanTextBoxBirthday.Text.Length == 2)
                {
                    jeanTextBoxBirthday.Text = $"0{jeanTextBoxBirthday.Text[0]}.";
                    jeanTextBoxBirthday.SelectionStart = jeanTextBoxBirthday.Text.Length;
                }
                else if (jeanTextBoxBirthday.Text.Length == 5)
                {
                    jeanTextBoxBirthday.Text = $"{jeanTextBoxBirthday.Text.Substring(0, 3)}0{jeanTextBoxBirthday.Text[3]}.";
                    jeanTextBoxBirthday.SelectionStart = jeanTextBoxBirthday.Text.Length;
                }
            }

            if ((jeanTextBoxBirthday.Text.Length == 2 || jeanTextBoxBirthday.Text.Length == 5) && isDigit)
            {
                jeanTextBoxBirthday.Text += ".";
                jeanTextBoxBirthday.SelectionStart = jeanTextBoxBirthday.Text.Length;
            }
        }

        private void ValidateBirthday()
        {
            if (jeanTextBoxBirthday.Text.Length == 10)
            {
                var day = int.Parse(jeanTextBoxBirthday.Text.Substring(0, 2));
                var month = int.Parse(jeanTextBoxBirthday.Text.Substring(3, 2));
                var year = int.Parse(jeanTextBoxBirthday.Text.Substring(6, 4));

                jeanTextBoxBirthday.BackColor = (day > 31 || month > 12 || year > DateTime.Now.Year)
                    ? Color.FromArgb(255, 150, 150)
                    : Color.White;
            }
            else
            {
                jeanTextBoxBirthday.BackColor = Color.White;
            }
        }

        private void jeanTextBoxNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                jeanTextBoxNumber.BackColor = Color.FromArgb(255, 150, 150);
            }
        }

        private void jeanTextBoxNumber_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(jeanTextBoxNumber.Text))
            {
                if (jeanTextBoxNumber.Text.Length == 1)
                {
                    jeanTextBoxNumber.Text += "(";
                }
                else if (jeanTextBoxNumber.Text.Length == 5)
                {
                    jeanTextBoxNumber.Text += ")";
                }
                else if (jeanTextBoxNumber.Text.Length == 9 || jeanTextBoxNumber.Text.Length == 12)
                {
                    jeanTextBoxNumber.Text += "-";
                }

                jeanTextBoxNumber.SelectionStart = jeanTextBoxNumber.Text.Length;
            }

            var phoneNumber = jeanTextBoxNumber.Text;
            jeanTextBoxNumber.BackColor = jeanTextBoxNumber.Text.Length <= 15
                ? Color.White
                : Color.FromArgb(255, 150, 150);
        }

        private void ValidateTextInput(JeanTextBox jeanTextBox, int maxLength)
        {
            jeanTextBox.BackColor = jeanTextBox.Text.All(c => !char.IsDigit(c)) && jeanTextBox.Text.Length <= maxLength
                ? Color.White
                : Color.FromArgb(255, 150, 150);
        }

        private void jeanTextBoxName_TextChanged(object sender, EventArgs e) => ValidateTextInput(jeanTextBoxName, 20);
        private void jeanTextBoxSurname_TextChanged(object sender, EventArgs e) => ValidateTextInput(jeanTextBoxSurname, 20);
        private void jeanTextBoxFather_TextChanged(object sender, EventArgs e) => ValidateTextInput(jeanTextBoxFather, 20);

        private void jeanTextBoxNumberCard_TextChanged(object sender, EventArgs e)
        {
            jeanTextBoxNumberCard.BackColor =
                (Regex.IsMatch(jeanTextBoxNumberCard.Text, @"^\d*$") && jeanTextBoxNumberCard.Text.Length <= 13)
                ? Color.White
                : Color.FromArgb(255, 150, 150);
        }

        private void jeanTextBoxPurchase_TextChanged(object sender, EventArgs e)
        {
            jeanTextBoxPurchase.BackColor = jeanTextBoxPurchase.Text.Length <= 100
                ? Color.White
                : Color.FromArgb(255, 150, 150);
        }

        private void comboBoxFormDiscount_TextChanged(object sender, EventArgs e)
        {
            var parts = comboBoxFormDiscount.Text.Split(' ');

            if (parts[0] == "Скидка" ||
                (int.TryParse(parts[0], out var discount) && discount >= 0 && discount <= 100))
            {
                comboBoxFormDiscount.BackColor = Color.White;
            }
            else
            {
                comboBoxFormDiscount.BackColor = Color.FromArgb(255, 150, 150);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var clientData = PrepareClientData();

            SaveClientToDatabase(clientData);

            if (!string.IsNullOrEmpty(jeanTextBoxPurchase.Text))
            {
                UpdateServiceStatistics();
                SavePaymentHistory(clientData);
                SaveIssuedMembership(clientData);
            }

            ClearForm();
            Message.MessageWindowOk("Клиент добавлен");
        }

        private bool ValidateInputs()
        {
            if (jeanTextBoxNumber.BackColor != Color.White || jeanTextBoxName.BackColor != Color.White ||
                jeanTextBoxSurname.BackColor != Color.White || jeanTextBoxFather.BackColor != Color.White ||
                jeanTextBoxPurchase.BackColor != Color.White || jeanTextBoxBirthday.BackColor != Color.White || 
                jeanTextBoxNumberCard.BackColor != Color.White)
            {
                Message.MessageWindowOk("Неправильные данные");
                return false;
            }

            if (string.IsNullOrEmpty(jeanTextBoxNumber.Text) ||
                string.IsNullOrEmpty(jeanTextBoxName.Text) ||
                string.IsNullOrEmpty(jeanTextBoxSurname.Text))
            {
                Message.MessageWindowOk("Незаполненные данные");
                return false;
            }

            if (!string.IsNullOrEmpty(jeanTextBoxPurchase.Text) && string.IsNullOrEmpty(jeanTextBoxNumberCard.Text) && jeanTextBoxPurchase.Text != "Разовый")
            {
                Message.MessageWindowOk("Для абонемента нужен номер карты");
                return false;
            }

            if (string.IsNullOrEmpty(jeanTextBoxPurchase.Text) && jeanTextBoxNumberCard.Text.Length == 13)
            {
                Message.MessageWindowOk("Выберете услугу");
                return false;
            }

            var discountParts = comboBoxFormDiscount.Text.Split(' ');
            if (discountParts[0] != "Скидка" &&
                (!int.TryParse(discountParts[0], out var discount) || discount < 0 || discount > 100))
            {
                Message.MessageWindowOk("Не правильный формат скидки");
                return false;
            }

            var cardExist = GeneralContext.GetElementFromDatabase($"SELECT Id FROM Contacts WHERE №Карты = '{jeanTextBoxNumberCard.Text}'",
                ClientsContext.ConnectionStringClients());
            if (cardExist != null)
            {
                Message.MessageWindowOk("Клиент с такой картой уже существует");
                return false;
            }

            var phoneExist = GeneralContext.GetElementFromDatabase($"SELECT Телефон FROM Contacts WHERE Телефон = '{jeanTextBoxNumber.Text}'",
                ClientsContext.ConnectionStringClients());
            if (phoneExist != null)
            {
                Message.MessageWindowOk("Клиент с таким номером уже существует");
                return false;
            }

            return true;
        }

        private NewClientData PrepareClientData()
        {
            var discountParts = comboBoxFormDiscount.Text.Split(' ');
            var finalPrice = price;

            if (decimal.TryParse(discountParts[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var discount) &&
                !string.IsNullOrEmpty(price))
            {
                finalPrice = (Convert.ToDecimal(price) * (1 - discount / 100)).ToString("0");
            }

            // Форматируем даты правильно
            string termDate = "";
            if (!string.IsNullOrEmpty(jeanTextBoxPurchase.Text))
            {
                termDate = jeanDateTimePickerSell.Value.AddMonths(Convert.ToInt32(termMembership))
                    .ToShortDateString();
            }

            return new NewClientData
            {
                Surname = jeanTextBoxSurname.Text,
                Name = jeanTextBoxName.Text,
                FatherName = jeanTextBoxFather.Text,
                Gender = radioButtonMan.Checked ? "Мужской" : radioButtonWoman.Checked ? "Женский" : "",
                Phone = jeanTextBoxNumber.Text,
                CardNumber = jeanTextBoxNumberCard.Text,
                Service = jeanTextBoxPurchase.Text,
                FinalPrice = finalPrice,
                VisitedDate = checkBoxVisited.Checked ? DateTime.Now.ToShortDateString() : "",
                TermDate = termDate,
                VisitsLeft = lefts,
                Birthday = FormatBirthdayForDatabase(jeanTextBoxBirthday.Text),
                Discount = discountParts[0] != "Скидка" && !string.IsNullOrEmpty(discountParts[0]) ? Convert.ToInt32(discountParts[0]) : 0
            };
        }

        private string FormatBirthdayForDatabase(string birthdayText)
        {
            if (string.IsNullOrEmpty(birthdayText) || birthdayText.Length != 10)
                return "";

            try
            {
                var parts = birthdayText.Split('.');
                if (parts.Length == 3)
                {
                    return $"{parts[2]}-{parts[1]}-{parts[0]}"; // yyyy-MM-dd
                }
            }
            catch
            {
                return "";
            }

            return "";
        }

        private void SaveClientToDatabase(NewClientData data)
        {
            using (var conn = new SQLiteConnection(ClientsContext.ConnectionStringClients()))
            using (var cmd = new SQLiteCommand(
                "INSERT INTO Contacts ([Фамилия],[Имя],[Пол],[Телефон],[№Карты],[Покупки],[Отчество],[Дата_рождения]," +
                "[Скидка],[Сохранено]) VALUES (@Фамилия,@Имя,@Пол,@Телефон,@№Карты,@Покупки,@Отчество,@Дата_рождения,@Скидка,@Сохранено)", conn))
            {
                conn.Open();

                string visitedDate = null;
                if (!string.IsNullOrEmpty(data.VisitedDate) && DateTime.TryParse(data.VisitedDate, out DateTime tempVisited))
                    visitedDate = tempVisited.ToShortDateString();

                string termDate = null;
                if (!string.IsNullOrEmpty(data.TermDate) && DateTime.TryParse(data.TermDate, out DateTime tempTerm))
                    termDate = tempTerm.ToShortDateString();

                string birthday = null;
                if (!string.IsNullOrEmpty(data.Birthday) && DateTime.TryParse(data.Birthday, out DateTime tempBirthday))
                    birthday = tempBirthday.ToShortDateString();

                cmd.Parameters.AddWithValue("@Фамилия", data.Surname);
                cmd.Parameters.AddWithValue("@Имя", data.Name);
                cmd.Parameters.AddWithValue("@Пол", data.Gender ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Телефон", data.Phone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@№Карты", data.CardNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Покупки", string.IsNullOrEmpty(data.FinalPrice) ? (object)DBNull.Value : Convert.ToInt32(data.FinalPrice));
                cmd.Parameters.AddWithValue("@Отчество", data.FatherName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Дата_рождения", birthday ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Скидка", data.Discount);
                cmd.Parameters.AddWithValue("@Сохранено", DateTime.Now.ToShortDateString());

                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateServiceStatistics()
        {
            var quantity = GeneralContext.GetElementFromDatabase(
                $"SELECT Проданных_за_месяц FROM Descriptions WHERE Абонемент = '{jeanTextBoxPurchase.Text}';",
                ServicesContext.ConnectionStringServices());

            GeneralContext.CommandDataFromDatabase(
                $"UPDATE Descriptions SET Проданных_за_месяц = '{Convert.ToInt32(quantity) + 1}' " +
                $"WHERE Абонемент = '{jeanTextBoxPurchase.Text}';",
                ServicesContext.ConnectionStringServices());
        }

        private void SavePaymentHistory(NewClientData data)
        {
            using (var conn = new SQLiteConnection(HistoryPaymentContext.ConnectionStringPayment()))
            using (var cmd = new SQLiteCommand(
                "INSERT INTO History ([Клиент],[Абонемент],[Дата_начала],[Дата_окончания]," +
                "[Цена],[Дата_платежа]) VALUES (@Клиент,@Абонемент,@Дата_начала,@Дата_окончания," +
                "@Цена,@Дата_платежа)", conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@Клиент", $"{data.Surname} {data.Name} {data.FatherName}");
                cmd.Parameters.AddWithValue("@Абонемент", data.Service);
                cmd.Parameters.AddWithValue("@Дата_начала", jeanDateTimePickerSell.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@Дата_окончания", data.TermDate);
                cmd.Parameters.AddWithValue("@Цена", data.FinalPrice);
                cmd.Parameters.AddWithValue("@Дата_платежа", DateTime.Now.ToShortDateString());

                cmd.ExecuteNonQuery();
            }
        }

        private void SaveIssuedMembership(NewClientData data)
        {
            using (var conn = new SQLiteConnection(IssuedMembershipContext.ConnectionStringIssued()))
            using (var cmd = new SQLiteCommand(
                "INSERT INTO Issued ([Клиент],[№Карты],[Дата_окончания],[Дата_оформления]," +
                "[Абонемент],[Посетил],[Оплата],[Статус],[Посещений_осталось],[Окончание_заморозки]) " +
                "VALUES (@Клиент,@№Карты,@Дата_окончания,@Дата_оформления,@Абонемент,@Посетил, @Оплата," +
                "@Статус,@Посещений_осталось,@Окончание_заморозки)", conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@Клиент", $"{data.Surname} {data.Name} {data.FatherName}");
                cmd.Parameters.AddWithValue("@№Карты", data.CardNumber);
                cmd.Parameters.AddWithValue("@Дата_окончания", data.TermDate);
                cmd.Parameters.AddWithValue("@Дата_оформления", jeanDateTimePickerSell.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@Абонемент", data.Service);
                cmd.Parameters.AddWithValue("@Посетил", data.VisitedDate);
                cmd.Parameters.AddWithValue("@Оплата", data.FinalPrice);
                cmd.Parameters.AddWithValue("@Статус", "активирован");
                cmd.Parameters.AddWithValue("@Посещений_осталось", data.VisitsLeft);
                cmd.Parameters.AddWithValue("@Окончание_заморозки", "");

                cmd.ExecuteNonQuery();
            }
        }

        private void ClearForm()
        {
            jeanTextBoxSurname.Text = "";
            jeanTextBoxName.Text = "";
            jeanTextBoxNumber.Text = "";
            jeanTextBoxNumberCard.Text = "";
            jeanTextBoxPurchase.Text = "";
            jeanTextBoxFather.Text = "";
            jeanTextBoxBirthday.Text = "";
            comboBoxFormDiscount.Text = "Скидка (%)";
        }
    }

    public class NewClientData
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string CardNumber { get; set; }
        public string Service { get; set; }
        public string FinalPrice { get; set; }
        public string VisitedDate { get; set; }
        public string TermDate { get; set; }
        public string VisitsLeft { get; set; }
        public string Birthday { get; set; }
        public int Discount { get; set; }
    }
}