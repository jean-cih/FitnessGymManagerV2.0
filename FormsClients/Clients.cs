using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.FormsClients;
using GymApplicationV2._0.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class Clients : Form
    {
        private string _id = string.Empty;

        private FadeAnimation _fadeAnimation;

        private DataTable _currentDataTable;

        public Clients()
        {
            InitializeComponent();

            SubscribeEvents();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();
        }

        private void SubscribeEvents()
        {
            dataGridViewClients.ColumnHeaderMouseClick += DataGridViewClients_ColumnHeaderMouseClick;
        }

        private void Clients_Load(object sender, EventArgs e)
        {
            PositionControls();
            LoadClientData();

            FontHelper.ApplyFontSettings(this, null);
        }

        private void PositionControls()
        {
            this.Width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.85);
            this.Height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.85);

            jeanSoftTextBoxSearch.Location = new Point(this.Width / 2 - 150, 30);
            jeanModernButtonErase.Location = new Point(this.Width / 2 - 150 + 260, 35);
            pictureBoxSearch.Location = new Point(this.Width / 2 - 140, 35);

            jeanModernButtonDelete.Location = new Point(this.Width / 2 + jeanModernButtonDelete.Width / 2 + 20, this.Height - 3 * jeanModernButtonDelete.Height);
            jeanModernButtonChange.Location = new Point(this.Width / 2 - jeanModernButtonChange.Width / 2 - 20, this.Height - 3 * jeanModernButtonChange.Height);

            checkBoxPerson.Location = new Point(this.Width / 2 + 2 * jeanModernButtonDelete.Width, this.Height - 3 * jeanModernButtonChange.Height + 10);

            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2,
                Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
            
            panelPerson.Location = new Point(dataGridViewClients.Location.X + dataGridViewClients.Width - 440, panelPerson.Location.Y);
            panelPerson.Width = 460;
            panelPerson.Height = jeanPanel.Height;
        }


        private void LoadClientData()
        {
            string query = "SELECT " +
                "Id," +
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
                " FROM Contacts";

            _currentDataTable = GeneralContext.GetDataFromDatabase(query,
                ClientsContext.ConnectionStringClients());

            //GeneralContext.FormatDateColumns(_currentDataTable);

            dataGridViewClients.DataSource = _currentDataTable;

            //GeneralContext.FormatData(dataGridViewClients);


            if (dataGridViewClients.Columns.Count > 0)
            {
                dataGridViewClients.Columns[0].DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
                dataGridViewClients.Columns[0].DefaultCellStyle.Font = new Font(
            dataGridViewClients.DefaultCellStyle.Font, FontStyle.Bold);
                dataGridViewClients.CellClick += DataGridViewClients_CellClick;
            }

            if (dataGridViewClients.Columns["Id"] != null)
            {
                dataGridViewClients.Columns["Id"].Visible = false;
            }
        }

        private void DataGridViewClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (e.ColumnIndex == 0)
            {
                OpenClient(e.RowIndex);
            }
        }

        private void OpenClient(int rowIndex)
        {
            try
            {
                if (rowIndex < 0 || rowIndex >= dataGridViewClients.Rows.Count) return;

                var row = dataGridViewClients.Rows[rowIndex];

                var clientData = LoadClientData(row);

                if (!checkBoxPerson.Checked)
                {
                    ImportPersonFormToPanel(clientData);
                }
                else
                {
                    OpenOrActivatePersonForm(clientData);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MessageWindowOk($"Ошибка при открытии клиента: {ex.Message}", "Ошибка");
            }
        }

        private void OpenOrActivatePersonForm(DataClient clientData)
        {
            var existingForm = Application.OpenForms
                .OfType<Person>()
                .FirstOrDefault(p => p.CardNumber == clientData.CardNumber);

            if (existingForm != null && !existingForm.IsDisposed)
            {
                existingForm.WindowState = FormWindowState.Normal;
                existingForm.BringToFront();
                existingForm.Focus();
            }
            else
            {
                var personForm = new Person(clientData, panelPerson);
                personForm.Show(this);
            }
        }

        private DataClient LoadClientData(DataGridViewRow row)
        {
            var clientData = new DataClient
            {
                Surname = row.Cells[0].Value.ToString() ?? "",
                Name = row.Cells[1].Value.ToString() ?? "",
                Gender = row.Cells[2].Value?.ToString() ?? "",
                Phone = row.Cells[3].Value?.ToString() ?? "",
                CardNumber = row.Cells[4].Value?.ToString() ?? "",
                Discount = Convert.ToInt32(row.Cells[8].Value),
                Email = row.Cells[7].Value?.ToString() ?? "",
                Birthday = row.Cells[10].Value?.ToString() ?? "",
            };

            string query = @"
          SELECT Абонемент, 
                 Дата_окончания AS 'Дата окончания', 
                 Посещений_осталось AS 'Посещений осталось',
                 Посетил
          FROM Issued 
          WHERE №Карты = @cardNumber 
                ORDER BY Id ASC
                LIMIT 1";

            DataTable table = GeneralContext.GetDataFromDatabase(query,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", clientData.CardNumber));

            if (table != null && table.Rows.Count > 0)
            {
                clientData.VisitDate = table.Rows[0]["Посетил"] != DBNull.Value ? table.Rows[0]["Посетил"].ToString() : "";
                clientData.VisitsLeft = table.Rows[0]["Посещений осталось"] != DBNull.Value ? table.Rows[0]["Посещений осталось"].ToString() : "";
                clientData.Service = table.Rows[0]["Абонемент"] != DBNull.Value ? table.Rows[0]["Абонемент"].ToString() : "";
                clientData.TermDate = table.Rows[0]["Дата окончания"] != DBNull.Value ? table.Rows[0]["Дата окончания"].ToString() : "";
            }

            return clientData;
        }

        private void DataGridViewClients_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentDataTable == null) return;

            string columnName = dataGridViewClients.Columns[e.ColumnIndex].Name;
            bool ascending = dataGridViewClients.Tag == null || !((bool)dataGridViewClients.Tag);

            DataTable sortedTable = SortDataTable(_currentDataTable, columnName, ascending);
            dataGridViewClients.DataSource = sortedTable;

            dataGridViewClients.Tag = ascending;
        }

        private DataTable SortDataTable(DataTable table, string columnName, bool ascending)
        {
            DataTable sortedTable = table.Clone();

            bool isDateColumn = columnName.Contains("Дата") || columnName.Contains("рождения") ||
                                columnName.Contains("Сохранено");

            IEnumerable<DataRow> sortedRows;

            if (isDateColumn)
            {
                if (ascending)
                {
                    sortedRows = table.AsEnumerable()
                        .OrderBy(row => DateTime.TryParse(row[columnName].ToString(), out DateTime d) ? d : DateTime.MinValue);
                }
                else
                {
                    sortedRows = table.AsEnumerable()
                        .OrderByDescending(row => DateTime.TryParse(row[columnName].ToString(), out DateTime d) ? d : DateTime.MinValue);
                }
            }
            else if (columnName == "Покупки")
            {
                if (ascending)
                {
                    sortedRows = table.AsEnumerable()
                        .OrderBy(row => int.TryParse(row[columnName].ToString(), out int n) ? n : 0);
                }
                else
                {
                    sortedRows = table.AsEnumerable()
                        .OrderByDescending(row => int.TryParse(row[columnName].ToString(), out int n) ? n : 0);
                }
            }
            else
            {
                if (ascending)
                {
                    sortedRows = table.AsEnumerable().OrderBy(row => row[columnName].ToString());
                }
                else
                {
                    sortedRows = table.AsEnumerable().OrderByDescending(row => row[columnName].ToString());
                }
            }

            foreach (DataRow row in sortedRows)
            {
                sortedTable.ImportRow(row);
            }

            return sortedTable;
        }

        private void ImportPersonFormToPanel(DataClient data)
        {
            panelPerson.Visible = false;
            panelPerson.Controls.Clear();

            var personForm = new Person(data, panelPerson);
            personForm.Visible = false;
            personForm.TopLevel = false;
            personForm.AutoScroll = true;
            personForm.FormBorderStyle = FormBorderStyle.None;
            personForm.Dock = DockStyle.Fill;

            panelPerson.Controls.Add(personForm);

            panelPerson.Visible = true;
            personForm.Visible = true;
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
                WHERE №Карты LIKE '%{term}%' 
                OR Фамилия LIKE '%{term}%' 
                OR Имя LIKE '%{term}%'";
        }

        private void jeanModernButtonErase_Click(object sender, EventArgs e)
        {
            jeanSoftTextBoxSearch.Texts = "";
        }

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(jeanTextBoxClient.Text))
            {
                MessageHelper.MessageWindowOk("Клиент не выбран", "Предупреждение");
                return;
            }

            if (MessageHelper.MessageWindowYesNo("Вы действительно хотите изменить данные клиента?") != DialogResult.Yes)
                return;

            string[] fullName = jeanTextBoxClient.Text.Split(' ');

            if (fullName.Length < 2)
            {
                MessageHelper.MessageWindowOk("Введите имя и фамилию", "Предупреждение");
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
              WHERE Id = @id";

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
              new SQLiteParameter("@id", _id)
            };

            GeneralContext.CommandDataFromDatabase(updateQuery,
                ClientsContext.ConnectionStringClients(), parameters);

            MessageHelper.MessageWindowOk("Данные клиента обновлены", "Сообщение");
            RefreshDataAndClearFields();
            jeanTextBoxClient.Text = "";
        }

        private void jeanModernButtonDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(jeanTextBoxClient.Text))
            {
                MessageHelper.MessageWindowOk("Клиент не выбран", "Предупреждение");
                return;
            }

            if (MessageHelper.MessageWindowYesNo("Вы действительно хотите удалить клиента?") != DialogResult.Yes)
                return;

            var deleteQuery = @"DELETE FROM Contacts WHERE Id = @id";

            GeneralContext.CommandDataFromDatabase(deleteQuery,
                ClientsContext.ConnectionStringClients(),
                new SQLiteParameter("@id", _id));
            MessageHelper.MessageWindowOk("Клиент удален", "Сообщение");
            RefreshDataAndClearFields();
        }

        private void RefreshDataAndClearFields()
        {
            LoadClientData();
            ClearAllFields();
        }

        private void ClearAllFields()
        {
            jeanTextBoxClient.Text = string.Empty;
            jeanTextBoxGender.Text = string.Empty;
            jeanTextBoxPhone.Text = string.Empty;
            jeanTextBoxNumberCard.Text = string.Empty;
            jeanTextBoxEmail.Text = string.Empty;
            jeanTextBoxBirthday.Text = string.Empty;
            jeanTextBoxDiscount.Text = string.Empty;
            jeanTextBoxPurchase.Text = string.Empty;
        }

        private void dataGridViewClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewClients.SelectedRows[0];

            _id = selectedRow.Cells[0].Value.ToString();
            jeanTextBoxClient.Text = $"{selectedRow.Cells[1].Value} {selectedRow.Cells[2].Value} {selectedRow.Cells[7].Value}";
            jeanTextBoxGender.Text = selectedRow.Cells[3].Value.ToString();
            jeanTextBoxPhone.Text = selectedRow.Cells[4].Value.ToString();
            jeanTextBoxNumberCard.Text = selectedRow.Cells[5].Value.ToString();
            jeanTextBoxPurchase.Text = selectedRow.Cells[6].Value.ToString();
            jeanTextBoxEmail.Text = selectedRow.Cells[8].Value.ToString();
            jeanTextBoxBirthday.Text = selectedRow.Cells[11].Value.ToString();
            jeanTextBoxDiscount.Text = selectedRow.Cells[9].Value.ToString();
        }
    }
}