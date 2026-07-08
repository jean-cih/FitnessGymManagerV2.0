using GymApplicationV2._0.Connections;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class ChangeData : Form
    {
        private Timer _fadeTimer;
        private float _opacity = 0;

        public ChangeData()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;
            SetupAnimation();
        }

        private void ChangeData_Load(object sender, EventArgs e)
        {
            ConfigureFormSize();
            PositionControls();
            LoadClientData();
            SetFonts();
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

        private void ConfigureFormSize()
        {
            this.Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.85);
            this.Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.85);
        }

        private void PositionControls()
        {
            jeanPanel1.Size = new Size(this.Width - 250, this.Height - 250);
            jeanSoftTextBoxSearch.Location = new Point(this.Width / 2 - 40, 40);
            jeanModernButtonErase.Location = new Point(this.Width / 2 - 40 + 260, 45);
            pictureBoxSearch.Location = new Point(this.Width / 2 - 30, 45);
            jeanModernButtonChange.Location = new Point(this.Width / 2 - 20, this.Height - 150);
            jeanModernButtonDelete.Location = new Point(this.Width / 2 + 110, this.Height - 150);
        }

        private void LoadClientData()
        {
            dataGridViewClients.DataSource = GeneralContext.GetDataFromDatabase("SELECT " +
                "Фамилия," +
                "Имя," +
                "Пол," +
                "Телефон," +
                "№Карты AS 'Карта'," +
                "Покупки," +
                "Отчество," +
                "Email," +
                "Дата_рождения AS 'Дата рождения'," +
                "Скидка," +
                "Сохранено" +
                " FROM Contacts",
                ClientsContext.ConnectionStringClients());
        }

        private void SetFonts()
        {
            jeanModernButtonDelete.Font = new Font("Удалить", DataClass.sizeFontButtons);
            jeanModernButtonChange.Font = new Font("Изменить", DataClass.sizeFontButtons);

            dataGridViewClients.DefaultCellStyle.Font =
            dataGridViewClients.ColumnHeadersDefaultCellStyle.Font =
                new Font("Contacts", DataClass.sizeFontTables);
        }

        private void dataGridViewClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewClients.SelectedRows[0];

            jeanTextBoxClient.Text = $"{selectedRow.Cells[0].Value} {selectedRow.Cells[1].Value} {selectedRow.Cells[6].Value}";
            jeanTextBoxGender.Text = selectedRow.Cells[2].Value.ToString();
            jeanTextBoxPhone.Text = selectedRow.Cells[3].Value.ToString();
            jeanTextBoxNumberCard.Text = selectedRow.Cells[4].Value.ToString();
            jeanTextBoxPurchase.Text = selectedRow.Cells[5].Value.ToString();
            jeanTextBoxEmail.Text = selectedRow.Cells[7].Value.ToString();
            jeanTextBoxBirthday.Text = selectedRow.Cells[8].Value.ToString();
            jeanTextBoxDiscount.Text = selectedRow.Cells[9].Value.ToString();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(jeanTextBoxClient.Text))
            {
                Message.MessageWindowOk("Клиент не выбран");
                return;
            }

            if (Message.MessageWindowYesNo("Вы действительно хотите изменить данные клиента?") != DialogResult.Yes)
                return;

            string[] fullName = jeanTextBoxClient.Text.Split(' ');

            if (fullName.Length < 2)
            {
                MessageBox.Show("Введите имя и фамилию");
                return;
            }

            string middleName = fullName.Length > 2 ? fullName[2].Trim() : "";

            var updateQuery = @"UPDATE Contacts SET 
                Фамилия = @LastName,
                Имя = @FirstName,
                Пол = @Gender,
                Телефон = @Phone,
                №Карты = @CardNumber,
                Покупки = @Purchases,
                Отчество = @MiddleName,
                Email = @Email,
                Дата_рождения = @BirthDate,
                Скидка = @Discount
                WHERE (Фамилия = @LastName AND Имя = @FirstName) 
                   OR (Телефон = @Phone AND NULLIF(@Phone, '') IS NOT NULL) 
                   OR (№Карты = @CardNumber AND NULLIF(@CardNumber, '') IS NOT NULL)";

            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@LastName", fullName[0].Trim()),
                new SQLiteParameter("@FirstName", fullName[1].Trim()),
                new SQLiteParameter("@Gender", jeanTextBoxGender.Text.Trim()),
                new SQLiteParameter("@Phone", jeanTextBoxPhone.Text.Trim()),
                new SQLiteParameter("@CardNumber", jeanTextBoxNumberCard.Text.Trim()),
                new SQLiteParameter("@Purchases", jeanTextBoxPurchase.Text.Trim()),
                new SQLiteParameter("@MiddleName", middleName),
                new SQLiteParameter("@Email", jeanTextBoxEmail.Text.Trim()),
                new SQLiteParameter("@BirthDate", jeanTextBoxBirthday.Text.Trim()),
                new SQLiteParameter("@Discount", jeanTextBoxDiscount.Text.Trim()),
                new SQLiteParameter("@WhereCardNumber", jeanTextBoxNumberCard.Text.Trim())
            };

            GeneralContext.CommandDataFromDatabase(updateQuery,
                ClientsContext.ConnectionStringClients(), parameters);

            Message.MessageWindowOk("Данные клиента обновлены");
            RefreshDataAndClearFields();
            jeanTextBoxClient.Text = "";
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(jeanTextBoxClient.Text))
            {
                Message.MessageWindowOk("Клиент не выбран");
                return;
            }

            if (Message.MessageWindowYesNo("Вы действительно хотите удалить клиента?") != DialogResult.Yes)
                return;

            string[] fullName = jeanTextBoxClient.Text.Split(' ');

            string lastName = fullName.Length > 0 ? fullName[0].Trim() : "";
            string firstName = fullName.Length > 1 ? fullName[1].Trim() : "";

            var deleteQuery = @"DELETE FROM Contacts 
                WHERE (Фамилия = @LastName AND Имя = @FirstName) 
                   OR (@Phone != '' AND Телефон = @Phone) 
                   OR (@CardNumber != '' AND №Карты = @CardNumber)";

            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@LastName", lastName),
                new SQLiteParameter("@FirstName", firstName),
                new SQLiteParameter("@Phone", jeanTextBoxPhone.Text.Trim()),
                new SQLiteParameter("@CardNumber", jeanTextBoxNumberCard.Text.Trim())
            };

            GeneralContext.CommandDataFromDatabase(deleteQuery,
                ClientsContext.ConnectionStringClients(), parameters);
            Message.MessageWindowOk("Клиент удален");
            RefreshDataAndClearFields();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            jeanSoftTextBoxSearch.Texts = "";
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(jeanSoftTextBoxSearch.Texts))
            {
                jeanModernButtonErase.Visible = false;
                LoadClientData();
                return;
            }

            jeanModernButtonErase.Visible = true;
            string[] fullName = jeanSoftTextBoxSearch.Texts.Split(' ');

            for (int i = 0; i < fullName.Length; i++)
            {
                if (!string.IsNullOrEmpty(fullName[i]))
                {
                    fullName[i] = char.ToUpper(fullName[i][0]) + fullName[i].Substring(1);
                }
            }

            string query = fullName.Length > 1
                ? BuildSearchQueryWithFullName(fullName)
                : BuildSimpleSearchQuery(fullName[0]);

            dataGridViewClients.DataSource = GeneralContext.GetDataFromDatabase(query,
                ClientsContext.ConnectionStringClients());
        }

        private string BuildSearchQueryWithFullName(string[] fullName)
        {
            return $@"SELECT 
                Фамилия,
                Имя,
                Пол,
                Телефон,
                №Карты AS 'Карта',
                Покупки,
                Отчество,
                Email,
                Дата_рождения AS 'Дата рождения',
                Скидка,
                Сохранено
                FROM Contacts  
                WHERE №Карты LIKE '%{fullName[0]}%' 
                OR Фамилия LIKE '%{fullName[0]}%' 
                AND Имя LIKE '%{fullName[1]}%' 
                OR Имя LIKE '%{fullName[0]}%' 
                AND Фамилия LIKE '%{fullName[1]}%'";
        }

        private string BuildSimpleSearchQuery(string searchTerm)
        {
            return $@"SELECT 
                Фамилия,
                Имя,
                Пол,
                Телефон,
                №Карты AS 'Карта',
                Покупки,
                Отчество,
                Email,
                Дата_рождения AS 'Дата рождения',
                Скидка,
                Сохранено
                FROM Contacts  
                WHERE №Карты LIKE '%{searchTerm}%' 
                OR Фамилия LIKE '%{searchTerm}%' 
                OR Имя LIKE '%{searchTerm}%'";
        }

        private void RefreshDataAndClearFields()
        {
            LoadClientData();
            ClearAllFields();
        }

        private void ClearAllFields()
        {
            jeanTextBoxClient.Text =
            jeanTextBoxGender.Text =
            jeanTextBoxPhone.Text =
            jeanTextBoxNumberCard.Text =
            jeanTextBoxEmail.Text =
            jeanTextBoxBirthday.Text =
            jeanTextBoxDiscount.Text =
            jeanTextBoxPurchase.Text = "";
        }
    }
}