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
            try
            {
                InitializeComponent();

                SubscribeEvents();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                Logger.Info("Форма Clients инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации Clients", ex);
                throw;
            }
        }

        private void SubscribeEvents()
        {
            dataGridViewClients.ColumnHeaderMouseClick += DataGridViewClients_ColumnHeaderMouseClick;
        }

        private void Clients_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка Clients_Load");
                PositionControls();
                LoadClientData();

                FontHelper.ApplyFontSettings(this, null);
                Logger.Info("Clients_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в Clients_Load", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки формы", "Ошибка");
            }
        }

        private void PositionControls()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error("Ошибка в PositionControls", ex);
            }
        }


        private void LoadClientData()
        {
            try
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

                dataGridViewClients.DataSource = _currentDataTable;

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

                Logger.Info($"Загружено {_currentDataTable?.Rows?.Count ?? 0} клиентов");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке данных клиентов", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки данных клиентов", "Ошибка");
            }
        }

        private void DataGridViewClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                if (e.ColumnIndex == 1)
                {
                    OpenClient(e.RowIndex);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в DataGridViewClients_CellClick", ex);
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

                Logger.Info($"Открыт клиент: {clientData.Surname} {clientData.Name}, карта: {clientData.CardNumber}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при открытии клиента (rowIndex: {rowIndex})", ex);
                MessageHelper.MessageWindowOk($"Ошибка при открытии клиента: {ex.Message}", "Ошибка");
            }
        }

        private void OpenOrActivatePersonForm(DataClient clientData)
        {
            try
            {
                var existingForm = Application.OpenForms
                    .OfType<Person>()
                    .FirstOrDefault(p => p.CardNumber == clientData.CardNumber);

                if (existingForm != null && !existingForm.IsDisposed)
                {
                    existingForm.WindowState = FormWindowState.Normal;
                    existingForm.BringToFront();
                    existingForm.Focus();
                    Logger.Info($"Активирована существующая форма Person для клиента {clientData.CardNumber}");
                }
                else
                {
                    var personForm = new Person(clientData, panelPerson);
                    personForm.Show(this);
                    Logger.Info($"Создана новая форма Person для клиента {clientData.CardNumber}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при открытии Person для клиента {clientData.CardNumber}", ex);
                throw;
            }
        }

        private DataClient LoadClientData(DataGridViewRow row)
        {
            try
            {
                var clientData = new DataClient
                {
                    Id = row.Cells[0].Value.ToString() ?? "",
                    Surname = row.Cells[1].Value.ToString() ?? "",
                    Name = row.Cells[2].Value.ToString() ?? "",
                    Gender = row.Cells[3].Value?.ToString() ?? "",
                    Phone = row.Cells[4].Value?.ToString() ?? "",
                    CardNumber = row.Cells[5].Value?.ToString() ?? "",
                    Purchase = row.Cells[6].Value?.ToString() ?? "",
                    Email = row.Cells[8].Value?.ToString() ?? "",
                    Birthday = row.Cells[9].Value?.ToString() ?? "",
                    Discount = row.Cells[10].Value?.ToString() ?? "",
                    Saved = row.Cells[11].Value?.ToString() ?? "",
                };

                DataTable table = GeneralContext.GetDataFromDatabase(@"
                      SELECT Абонемент, 
                             Дата_окончания AS 'Дата окончания', 
                             Посещений_осталось AS 'Посещений осталось',
                             Посетил
                      FROM Issued 
                      WHERE №Карты = @cardNumber 
                            ORDER BY Id ASC
                            LIMIT 1",
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", clientData.CardNumber));

                if (table != null && table.Rows.Count > 0)
                {
                    clientData.Service = table.Rows[0]["Абонемент"] != DBNull.Value ? table.Rows[0]["Абонемент"].ToString() : "";
                    clientData.VisitDate = table.Rows[0]["Посетил"] != DBNull.Value ? table.Rows[0]["Посетил"].ToString() : "";
                    clientData.TermDate = table.Rows[0]["Дата окончания"] != DBNull.Value ? table.Rows[0]["Дата окончания"].ToString() : "";
                    clientData.VisitsLeft = table.Rows[0]["Посещений осталось"] != DBNull.Value ? table.Rows[0]["Посещений осталось"].ToString() : "";
                }

                return clientData;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке данных клиента из строки", ex);
                throw;
            }
        }

        private void DataGridViewClients_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (_currentDataTable == null) return;

                string columnName = dataGridViewClients.Columns[e.ColumnIndex].Name;
                bool ascending = dataGridViewClients.Tag == null || !((bool)dataGridViewClients.Tag);

                DataTable sortedTable = SortDataTable(_currentDataTable, columnName, ascending);
                dataGridViewClients.DataSource = sortedTable;

                dataGridViewClients.Tag = ascending;

                Logger.Info($"Сортировка по колонке '{columnName}', порядок: {(ascending ? "возрастание" : "убывание")}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при сортировке", ex);
            }
        }

        private DataTable SortDataTable(DataTable table, string columnName, bool ascending)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при сортировке таблицы по колонке '{columnName}'", ex);
                throw;
            }
        }

        private void ImportPersonFormToPanel(DataClient data)
        {
            try
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

                Logger.Info($"Импорт Person в панель для клиента: {data.Surname} {data.Name}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при импорте Person в панель для клиента {data.CardNumber}", ex);
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

                if (dataGridViewClients.Columns["Id"] != null)
                {
                    dataGridViewClients.Columns["Id"].Visible = false;
                }

                Logger.Info($"Поиск клиентов: '{jeanSoftTextBoxSearch.Texts}'");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при поиске: {jeanSoftTextBoxSearch.Texts}", ex);
            }
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
                Id,
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
                Id,
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
            try
            {
                jeanSoftTextBoxSearch.Texts = "";
                Logger.Info("Очищен поисковый запрос");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при очистке поиска", ex);
            }
        }

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jeanTextBoxClient.Text))
                {
                    Logger.Warning("Попытка изменения без выбранного клиента");
                    MessageHelper.MessageWindowOk("Клиент не выбран", "Предупреждение");
                    return;
                }

                if (MessageHelper.MessageWindowYesNo("Вы действительно хотите изменить данные клиента?") != DialogResult.Yes)
                {
                    Logger.Info("Изменение клиента отменено пользователем");
                    return;
                }

                string[] fullName = jeanTextBoxClient.Text.Split(' ');

                if (fullName.Length < 2)
                {
                    Logger.Warning("Некорректное имя клиента");
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

                Logger.Info($"Обновлены данные клиента ID: {_id}, {fullName[0]} {fullName[1]}");
                MessageHelper.MessageWindowOk("Данные клиента обновлены", "Сообщение");
                RefreshDataAndClearFields();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при изменении клиента ID: {_id}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при изменении клиента: {ex.Message}", "Ошибка");
            }
        }

        private void jeanModernButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jeanTextBoxClient.Text))
                {
                    Logger.Warning("Попытка удаления без выбранного клиента");
                    MessageHelper.MessageWindowOk("Клиент не выбран", "Предупреждение");
                    return;
                }

                if (MessageHelper.MessageWindowYesNo("Вы действительно хотите удалить клиента?") != DialogResult.Yes)
                {
                    Logger.Info("Удаление клиента отменено пользователем");
                    return;
                }

                var deleteQuery = @"DELETE FROM Contacts WHERE Id = @id";

                GeneralContext.CommandDataFromDatabase(deleteQuery,
                    ClientsContext.ConnectionStringClients(),
                    new SQLiteParameter("@id", _id));

                Logger.Info($"Удален клиент ID: {_id}, {jeanTextBoxClient.Text}");
                MessageHelper.MessageWindowOk("Клиент удален", "Сообщение");
                RefreshDataAndClearFields();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при удалении клиента ID: {_id}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при удалении клиента: {ex.Message}", "Ошибка");
            }
        }

        private void RefreshDataAndClearFields()
        {
            try
            {
                LoadClientData();
                ClearAllFields();
                Logger.Info("Данные обновлены и поля очищены");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при обновлении данных", ex);
            }
        }

        private void ClearAllFields()
        {
            try
            {
                jeanSoftTextBoxSearch.Texts = "";
                jeanTextBoxClient.Text = string.Empty;
                jeanTextBoxGender.Text = string.Empty;
                jeanTextBoxPhone.Text = string.Empty;
                jeanTextBoxNumberCard.Text = string.Empty;
                jeanTextBoxEmail.Text = string.Empty;
                jeanTextBoxBirthday.Text = string.Empty;
                jeanTextBoxDiscount.Text = string.Empty;
                jeanTextBoxPurchase.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при очистке полей", ex);
            }
        }

        private void dataGridViewClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
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

                Logger.Info($"Выбран клиент для редактирования: {jeanTextBoxClient.Text}, ID: {_id}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при выборе клиента в таблице", ex);
            }
        }
    }
}