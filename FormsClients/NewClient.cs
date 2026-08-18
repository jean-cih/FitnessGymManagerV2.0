using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class NewClient : Form
    {
        private string lefts = string.Empty;
        private string price = string.Empty;
        private string termMembership = string.Empty;

        private bool space;
        private bool isDigit;

        private FadeAnimation _fadeAnimation;

        public NewClient()
        {
            try
            {
                InitializeComponent();

                this.KeyPreview = true;
                this.KeyDown += jeanTextBoxBirthday_KeyDown;

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                Logger.Info("Форма NewClient инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации NewClient", ex);
                throw;
            }
        }

        private void NewClient_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка NewClient_Load");
                LoadServicesData();
                FontHelper.ApplyFontSettings(this, null);
                Logger.Info("NewClient_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в NewClient_Load", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки формы", "Ошибка");
            }
        }

        private void LoadServicesData()
        {
            try
            {
                dataGridViewServices.DataSource = GeneralContext.GetDataFromDatabase(
                    "SELECT Абонемент, Цена, Срок_действия, Посещений FROM Descriptions",
                    ServicesContext.ConnectionStringServices());
                Logger.Info("Данные услуг загружены");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке данных услуг", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки услуг", "Ошибка");
            }
        }

        private void dataGridViewServices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                jeanTextBoxPurchase.Text = dataGridViewServices.Rows[e.RowIndex].Cells[0].Value.ToString();
                lefts = dataGridViewServices.Rows[e.RowIndex].Cells[3].Value.ToString();
                price = dataGridViewServices.Rows[e.RowIndex].Cells[1].Value.ToString();
                termMembership = dataGridViewServices.Rows[e.RowIndex].Cells[2].Value.ToString();

                Logger.Info($"Выбрана услуга: {jeanTextBoxPurchase.Text}, цена: {price}, срок: {termMembership}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при выборе услуги", ex);
            }
        }

        private void jeanTextBoxBirthday_KeyDown(object sender, KeyEventArgs e)
        {
            space = e.KeyCode == Keys.Space ? true : false;
        }

        private void jeanTextBoxBirthday_KeyPress(object sender, KeyPressEventArgs e)
        {
            isDigit = char.IsDigit(e.KeyChar) ? true : false;
        }

        private void jeanTextBoxBirthday_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FormatBirthdayInput();
                ValidateBirthday();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при форматировании даты рождения", ex);
            }
        }

        private void FormatBirthdayInput()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error("Ошибка в FormatBirthdayInput", ex);
            }
        }

        private void ValidateBirthday()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ValidateBirthday", ex);
            }
        }

        private void jeanTextBoxNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    jeanTextBoxNumber.BackColor = Color.FromArgb(255, 150, 150);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanTextBoxNumber_KeyPress", ex);
            }
        }

        private void jeanTextBoxNumber_TextChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanTextBoxNumber_TextChanged", ex);
            }
        }

        private void ValidateTextInput(JeanTextBox jeanTextBox, int maxLength)
        {
            try
            {
                jeanTextBox.BackColor = jeanTextBox.Text.All(c => !char.IsDigit(c)) && jeanTextBox.Text.Length <= maxLength
                    ? Color.White
                    : Color.FromArgb(255, 150, 150);
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ValidateTextInput для {jeanTextBox.Name}", ex);
            }
        }

        private void jeanTextBoxName_TextChanged(object sender, EventArgs e) => ValidateTextInput(jeanTextBoxName, 20);
        private void jeanTextBoxSurname_TextChanged(object sender, EventArgs e) => ValidateTextInput(jeanTextBoxSurname, 20);
        private void jeanTextBoxFather_TextChanged(object sender, EventArgs e) => ValidateTextInput(jeanTextBoxFather, 20);

        private void jeanTextBoxNumberCard_TextChanged(object sender, EventArgs e)
        {
            try
            {
                jeanTextBoxNumberCard.BackColor =
                    (Regex.IsMatch(jeanTextBoxNumberCard.Text, @"^\d*$") && jeanTextBoxNumberCard.Text.Length <= 13)
                    ? Color.White
                    : Color.FromArgb(255, 150, 150);
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanTextBoxNumberCard_TextChanged", ex);
            }
        }

        private void jeanTextBoxPurchase_TextChanged(object sender, EventArgs e)
        {
            try
            {
                jeanTextBoxPurchase.BackColor = jeanTextBoxPurchase.Text.Length <= 100
                    ? Color.White
                    : Color.FromArgb(255, 150, 150);
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanTextBoxPurchase_TextChanged", ex);
            }
        }

        private void comboBoxFormDiscount_TextChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error("Ошибка в comboBoxFormDiscount_TextChanged", ex);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Начало сохранения нового клиента");

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
                Logger.Info($"Клиент успешно добавлен: {clientData.Surname} {clientData.Name}, карта: {clientData.CardNumber}");
                MessageHelper.MessageWindowOk("Клиент добавлен", "Сообщение");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при сохранении клиента", ex);
                MessageHelper.MessageWindowOk($"Ошибка при сохранении: {ex.Message}", "Ошибка");
            }
        }

        private bool ValidateInputs()
        {
            try
            {
                if (jeanTextBoxNumber.BackColor != Color.White || jeanTextBoxName.BackColor != Color.White ||
                    jeanTextBoxSurname.BackColor != Color.White || jeanTextBoxFather.BackColor != Color.White ||
                    jeanTextBoxPurchase.BackColor != Color.White || jeanTextBoxBirthday.BackColor != Color.White ||
                    jeanTextBoxNumberCard.BackColor != Color.White)
                {
                    Logger.Warning("Валидация не пройдена: неправильные данные");
                    MessageHelper.MessageWindowOk("Неправильные данные", "Предупреждение");
                    return false;
                }

                if (string.IsNullOrEmpty(jeanTextBoxNumber.Text) ||
                    string.IsNullOrEmpty(jeanTextBoxName.Text) ||
                    string.IsNullOrEmpty(jeanTextBoxSurname.Text))
                {
                    Logger.Warning("Валидация не пройдена: незаполненные обязательные поля");
                    MessageHelper.MessageWindowOk("Незаполненные данные", "Предупреждение");
                    return false;
                }

                if (!string.IsNullOrEmpty(jeanTextBoxPurchase.Text) && string.IsNullOrEmpty(jeanTextBoxNumberCard.Text) && jeanTextBoxPurchase.Text != "Разовый")
                {
                    Logger.Warning("Валидация не пройдена: для абонемента нужен номер карты");
                    MessageHelper.MessageWindowOk("Для абонемента нужен номер карты", "Предупреждение");
                    return false;
                }

                if (string.IsNullOrEmpty(jeanTextBoxPurchase.Text) && jeanTextBoxNumberCard.Text.Length == 13)
                {
                    Logger.Warning("Валидация не пройдена: выбрана карта, но не выбрана услуга");
                    MessageHelper.MessageWindowOk("Выберете услугу", "Предупреждение");
                    return false;
                }

                var discountParts = comboBoxFormDiscount.Text.Split(' ');
                if (discountParts[0] != "Скидка" &&
                    (!int.TryParse(discountParts[0], out var discount) || discount < 0 || discount > 100))
                {
                    Logger.Warning($"Валидация не пройдена: неверный формат скидки '{comboBoxFormDiscount.Text}'");
                    MessageHelper.MessageWindowOk("Не правильный формат скидки", "Предупреждение");
                    return false;
                }

                if (jeanTextBoxNumberCard.Text != "")
                {
                    var cardExist = GeneralContext.GetElementFromDatabase($"SELECT Id FROM Contacts WHERE №Карты = '{jeanTextBoxNumberCard.Text}'",
                        ClientsContext.ConnectionStringClients());
                    if (cardExist != null)
                    {
                        Logger.Warning($"Валидация не пройдена: карта {jeanTextBoxNumberCard.Text} уже существует");
                        MessageHelper.MessageWindowOk("Клиент с такой картой уже существует", "Предупреждение");
                        return false;
                    }
                }

                var phoneExist = GeneralContext.GetElementFromDatabase($"SELECT Телефон FROM Contacts WHERE Телефон = '{jeanTextBoxNumber.Text}'",
                    ClientsContext.ConnectionStringClients());
                if (phoneExist != null)
                {
                    Logger.Warning($"Валидация не пройдена: телефон {jeanTextBoxNumber.Text} уже существует");
                    MessageHelper.MessageWindowOk("Клиент с таким номером уже существует", "Предупреждение");
                    return false;
                }

                Logger.Info("Валидация пройдена успешно");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ValidateInputs", ex);
                MessageHelper.MessageWindowOk($"Ошибка валидации: {ex.Message}", "Ошибка");
                return false;
            }
        }

        private DataClient PrepareClientData()
        {
            try
            {
                var discountParts = comboBoxFormDiscount.Text.Split(' ');
                var finalPrice = price;

                if (decimal.TryParse(discountParts[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var discount) &&
                    !string.IsNullOrEmpty(price))
                {
                    finalPrice = (Convert.ToDecimal(price) * (1 - discount / 100)).ToString("0");
                }

                string termDate = "";
                if (!string.IsNullOrEmpty(jeanTextBoxPurchase.Text))
                {
                    termDate = jeanDateTimePickerSell.Value.AddMonths(Convert.ToInt32(termMembership))
                        .ToString("yyyy-MM-dd"); // ISO формат
                }

                var clientData = new DataClient
                {
                    Surname = jeanTextBoxSurname.Text,
                    Name = jeanTextBoxName.Text,
                    FatherName = jeanTextBoxFather.Text,
                    Gender = radioButtonMan.Checked ? "Мужской" : radioButtonWoman.Checked ? "Женский" : "",
                    Phone = jeanTextBoxNumber.Text,
                    CardNumber = jeanTextBoxNumberCard.Text,
                    Service = jeanTextBoxPurchase.Text,
                    FinalPrice = finalPrice,
                    VisitDate = checkBoxVisited.Checked ? DateTime.Now.ToString("yyyy-MM-dd") : "", // ISO формат
                    TermDate = termDate,
                    VisitsLeft = lefts,
                    Birthday = FormatBirthdayForDatabase(jeanTextBoxBirthday.Text), // Убедитесь, что этот метод тоже возвращает ISO
                    Discount = discountParts[0] != "Скидка" && !string.IsNullOrEmpty(discountParts[0]) ? TryParseInt(discountParts[0]).ToString() : "",
                    Saved = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") // ISO формат с временем
                };

                Logger.Info($"Подготовлены данные клиента: {clientData.Surname} {clientData.Name}");
                return clientData;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в PrepareClientData", ex);
                throw;
            }
        }

        private int TryParseInt(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return 0;

                string cleanValue = new string(value.Where(c => char.IsDigit(c) || c == '-').ToArray());

                if (string.IsNullOrEmpty(cleanValue))
                    return 0;

                if (int.TryParse(cleanValue, out int result))
                    return result;

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при парсинге числа '{value}'", ex);
                return 0;
            }
        }

        private string FormatBirthdayForDatabase(string birthdayText)
        {
            try
            {
                if (string.IsNullOrEmpty(birthdayText) || birthdayText.Length != 10)
                    return string.Empty;

                var parts = birthdayText.Split('.');
                if (parts.Length == 3)
                {
                    return $"{parts[2]}-{parts[1]}-{parts[0]}"; // yyyy-MM-dd
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при форматировании даты рождения '{birthdayText}'", ex);
                return string.Empty;
            }

            return string.Empty;
        }

        private void SaveClientToDatabase(DataClient data)
        {
            try
            {
                using (var conn = new SQLiteConnection(ClientsContext.ConnectionStringClients()))
                using (var cmd = new SQLiteCommand(
                    "INSERT INTO Contacts ([Фамилия],[Имя],[Пол],[Телефон],[№Карты],[Покупки],[Отчество],[Дата_рождения]," +
                    "[Скидка],[Сохранено]) VALUES (@Фамилия,@Имя,@Пол,@Телефон,@№Карты,@Покупки,@Отчество,@Дата_рождения,@Скидка,@Сохранено)", conn))
                {
                    conn.Open();

                    string visitedDate = null;
                    if (!string.IsNullOrEmpty(data.VisitDate) && DateTime.TryParse(data.VisitDate, out DateTime tempVisited))
                        visitedDate = tempVisited.ToString("yyyy-MM-dd");

                    string termDate = null;
                    if (!string.IsNullOrEmpty(data.TermDate) && DateTime.TryParse(data.TermDate, out DateTime tempTerm))
                        termDate = tempTerm.ToString("yyyy-MM-dd");

                    string birthday = null;
                    if (!string.IsNullOrEmpty(data.Birthday) && DateTime.TryParse(data.Birthday, out DateTime tempBirthday))
                        birthday = tempBirthday.ToString("yyyy-MM-dd");

                    cmd.Parameters.AddWithValue("@Фамилия", data.Surname);
                    cmd.Parameters.AddWithValue("@Имя", data.Name);
                    cmd.Parameters.AddWithValue("@Пол", data.Gender ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Телефон", data.Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@№Карты", data.CardNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Покупки", string.IsNullOrEmpty(data.FinalPrice) ? (object)DBNull.Value : Convert.ToInt32(data.FinalPrice));
                    cmd.Parameters.AddWithValue("@Отчество", data.FatherName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Дата_рождения", birthday ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Скидка", data.Discount);
                    cmd.Parameters.AddWithValue("@Сохранено", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    cmd.ExecuteNonQuery();
                    Logger.Info($"Клиент сохранен в БД: {data.Surname} {data.Name}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при сохранении клиента {data.Surname} {data.Name} в БД", ex);
                throw;
            }
        }

        private void UpdateServiceStatistics()
        {
            try
            {
                var quantity = GeneralContext.GetElementFromDatabase(
                    $"SELECT Проданных_за_месяц FROM Descriptions WHERE Абонемент = '{jeanTextBoxPurchase.Text}';",
                    ServicesContext.ConnectionStringServices());

                GeneralContext.CommandDataFromDatabase(
                    $"UPDATE Descriptions SET Проданных_за_месяц = '{Convert.ToInt32(quantity) + 1}' " +
                    $"WHERE Абонемент = '{jeanTextBoxPurchase.Text}';",
                    ServicesContext.ConnectionStringServices());

                Logger.Info($"Обновлена статистика услуги {jeanTextBoxPurchase.Text}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при обновлении статистики для услуги {jeanTextBoxPurchase.Text}", ex);
                throw;
            }
        }

        private void SavePaymentHistory(DataClient data)
        {
            try
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
                    cmd.Parameters.AddWithValue("@Дата_начала", jeanDateTimePickerSell.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Дата_окончания", data.TermDate);
                    cmd.Parameters.AddWithValue("@Цена", data.FinalPrice);
                    cmd.Parameters.AddWithValue("@Дата_платежа", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    cmd.ExecuteNonQuery();
                    Logger.Info($"Сохранена история платежа для {data.Surname} {data.Name}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при сохранении истории платежа для {data.Surname} {data.Name}", ex);
                throw;
            }
        }

        private void SaveIssuedMembership(DataClient data)
        {
            try
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
                    cmd.Parameters.AddWithValue("@Дата_оформления", jeanDateTimePickerSell.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Абонемент", data.Service);
                    cmd.Parameters.AddWithValue("@Посетил", data.VisitDate);
                    cmd.Parameters.AddWithValue("@Оплата", data.FinalPrice);
                    cmd.Parameters.AddWithValue("@Статус", "активирован");
                    cmd.Parameters.AddWithValue("@Посещений_осталось", data.VisitsLeft);
                    cmd.Parameters.AddWithValue("@Окончание_заморозки", "");

                    cmd.ExecuteNonQuery();
                    Logger.Info($"Сохранен абонемент для {data.Surname} {data.Name}, карта: {data.CardNumber}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при сохранении абонемента для {data.Surname} {data.Name}", ex);
                throw;
            }
        }

        private void ClearForm()
        {
            try
            {
                jeanTextBoxSurname.Text = string.Empty;
                jeanTextBoxName.Text = string.Empty;
                jeanTextBoxNumber.Text = string.Empty;
                jeanTextBoxNumberCard.Text = string.Empty;
                jeanTextBoxPurchase.Text = string.Empty;
                jeanTextBoxFather.Text = string.Empty;
                jeanTextBoxBirthday.Text = string.Empty;
                comboBoxFormDiscount.Text = "Скидка (%)";
                Logger.Info("Форма очищена");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при очистке формы", ex);
            }
        }
    }
}