using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class ChooseClient : Form
    {
        private string _name = string.Empty;
        private string _surname = string.Empty;
        private string _numberCard = string.Empty;

        private FadeAnimation _fadeAnimation;

        private DataTable _currentDataTable;

        public ChooseClient()
        {
            try
            {
                InitializeComponent();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                Logger.Info("Форма ChooseClient инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации ChooseClient", ex);
                throw;
            }
        }

        private void ChooseClient_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка ChooseClient_Load");
                ConfigureFormSize();
                PositionControls();
                LoadClientData();

                FontHelper.ApplyFontSettings(this, null);
                Logger.Info("ChooseClient_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ChooseClient_Load", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки формы", "Ошибка");
            }
        }

        private void ConfigureFormSize()
        {
            try
            {
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.65);
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.7);
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ConfigureFormSize", ex);
            }
        }

        private void PositionControls()
        {
            try
            {
                jeanPanel1.Size = new Size(this.Width - 40, this.Height - 200);
                jeanSoftTextBoxSearch.Location = new Point(this.Width / 2 - 150, 10);
                pictureBoxSearch.Location = new Point(this.Width / 2 - 140, 15);
                jeanModernButtonErase.Location = new Point(this.Width / 2 - 140 + 253, 15);
                jeanModernButtonChoose.Location = new Point(this.Width / 2 - 60, this.Height - 130);
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

                Logger.Info($"Загружено {_currentDataTable?.Rows?.Count ?? 0} клиентов");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке данных клиентов", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки данных клиентов", "Ошибка");
            }
        }

        private void jeanSoftTextBoxSearch__TextChanged(object sender, EventArgs e)
        {
            try
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

                Logger.Info($"Поиск клиента: '{jeanSoftTextBoxSearch.Texts}'");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при поиске: {jeanSoftTextBoxSearch.Texts}", ex);
            }
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

        private void dataGridViewClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridViewClients.SelectedRows.Count == 0) return;

                var selectedRow = dataGridViewClients.SelectedRows[0];
                _surname = selectedRow.Cells[0].Value.ToString();
                _name = selectedRow.Cells[1].Value.ToString();
                _numberCard = selectedRow.Cells[4].Value.ToString();

                labelName.Text = $"{_surname} {_name} : {_numberCard}";

                if (_numberCard == "")
                {
                    jeanTextBoxNumberCard.Visible = true;
                }

                Logger.Info($"Выбран клиент: {_surname} {_name}, карта: {_numberCard}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при выборе клиента из таблицы", ex);
            }
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_name) ||
                    string.IsNullOrWhiteSpace(_surname))
                {
                    Logger.Warning("Попытка выбора без выбранного клиента");
                    MessageHelper.MessageWindowOk("Выберите клиента", "Предупреждение");
                    return;
                }

                Logger.Info($"Выбран клиент для обслуживания: {_name} {_surname}, карта: {_numberCard}");
                OpenServicesForm();
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при выборе клиента", ex);
                MessageHelper.MessageWindowOk("Ошибка при выборе клиента", "Ошибка");
            }
        }

        private void OpenServicesForm()
        {
            try
            {
                var services = new Services
                {
                    jeanModernButtonAdd = { Visible = false },
                    jeanModernButtonDelete = { Visible = false },
                    jeanModernButtonSell = { Visible = true },
                    jeanModernButtonChange = { Visible = false },
                    labelName = { Text = $"{_name} {_surname}", Visible = true },
                    jeanSoftTextBoxPurchase = { Visible = true },
                    NumberCard = _numberCard,
                    checkBoxVisited = { Visible = true },
                    dateActivation = { Visible = true }
                };

                services.Show();
                Logger.Info($"Открыта форма Services для клиента: {_name} {_surname}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при открытии Services для клиента {_name} {_surname}", ex);
                throw;
            }
        }

        private void jeanTextBoxNumberCard_TextChanged(object sender, EventArgs e)
        {
            try 
            {
                if (Regex.IsMatch(jeanTextBoxNumberCard.Text, @"^-?\d+(\d+)?$") || jeanTextBoxNumberCard.Text.Length == 0)
                {
                    jeanTextBoxNumberCard.BackColor = Color.White;
                }
                else
                {
                    jeanTextBoxNumberCard.BackColor = Color.FromArgb(255, 150, 150);
                }

                if (jeanTextBoxNumberCard.Text.Length != 13)
                    return;

                _numberCard = jeanTextBoxNumberCard.Text.Trim();

                Logger.Info($"Номер карты изменен: {_numberCard}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при вводе номера карты", ex);
            }
        }
    }
}