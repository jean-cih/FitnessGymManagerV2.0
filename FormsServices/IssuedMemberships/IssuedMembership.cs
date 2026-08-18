using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.FormsServices;
using GymApplicationV2._0.Helpers;
using GymApplicationV2._0.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class IssuedMembership : Form
    {
        private ToolStripDropDownMenu _menu;
        private string _dateOver = string.Empty;
        private string _client = string.Empty;
        private string _membership = string.Empty;
        private string _cost = string.Empty;
        private string _status = string.Empty;
        private string _visits = string.Empty;
        private string _id = string.Empty;
        private string _freezeDate = string.Empty;
        private string _numberCard = string.Empty;

        private FadeAnimation _fadeAnimation;

        public IssuedMembership()
        {
            try
            {
                InitializeComponent();
                InitializeMenu();
                SetupFormLayout();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                FontHelper.ApplyFontSettings(this, null);

                Logger.Info("Форма IssuedMembership инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации IssuedMembership", ex);
                throw;
            }
        }

        private void InitializeMenu()
        {
            try
            {
                _menu = new ToolStripDropDownMenu
                {
                    Font = new Font("Arial", 12)
                };

                var freezeItem = new ToolStripMenuItem("Заморозить", Properties.Resources.freeze)
                {
                    Tag = "freeze"
                };

                var changeItem = new ToolStripMenuItem("Изменить параметры", Properties.Resources.change)
                {
                    Tag = "change"
                };

                _menu.Items.Add(freezeItem);
                _menu.Items.Add(changeItem);

                _menu.ItemClicked += MenuItemClicked;
                jeanModernButtonChangeData.Click += (s, e) =>
                    _menu.Show(jeanModernButtonChangeData, new Point(0, jeanModernButtonChangeData.Height));

                Logger.Info("Меню IssuedMembership инициализировано");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeMenu", ex);
            }
        }

        private void SetupFormLayout()
        {
            try
            {
                Width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.75);
                Height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.75);

                jeanPanel.Size = new Size(Width - 40, Height - 180);
                jeanSoftTextBoxSearch.Location = new Point(Width / 2 - 150, 55);
                jeanModernButtonErase.Location = new Point(Width / 2 + 110, 60);
                pictureBoxSearch.Location = new Point(Width / 2 - 140, 60);
                jeanModernButtonChangeData.Location = new Point(Width - 150, 55);

                dataGridViewIssued.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewIssued.DefaultCellStyle.Font = new Font("Issued", DataConfig.sizeFontTables);
                dataGridViewIssued.ColumnHeadersDefaultCellStyle.Font = new Font("Issued", DataConfig.sizeFontTables);

                Logger.Info($"Размеры формы настроены: {Width}x{Height}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в SetupFormLayout", ex);
            }
        }

        private void IssuedMembership_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка IssuedMembership_Load");
                RefreshDataGrid();
                Logger.Info("IssuedMembership_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в IssuedMembership_Load", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки формы", "Ошибка");
            }
        }

        private void MenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (e.ClickedItem.Tag.ToString() == "freeze")
                {
                    Logger.Info("Выбран пункт меню 'Заморозить'");
                    ShowFreezeDialog();
                }
                else
                {
                    Logger.Info("Выбран пункт меню 'Изменить параметры'");
                    ShowChangeDialog();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в MenuItemClicked", ex);
            }
        }

        private DataTable _currentDataTable;

        private void RefreshDataGrid()
        {
            try
            {
                string query = @"
            SELECT Id, 
                Клиент,
                №Карты,
                Дата_окончания AS 'Дата окончания',
                Дата_оформления AS 'Дата оформления',
                Абонемент,
                Посетил,
                Оплата,
                Статус,
                Посещений_осталось AS 'Посещений осталось',
                Окончание_заморозки AS 'Окончание заморозки'
            FROM Issued ORDER BY Id DESC";

                _currentDataTable = GeneralContext.GetDataFromDatabase(query,
                    IssuedMembershipContext.ConnectionStringIssued());

                dataGridViewIssued.DataSource = _currentDataTable;

                if (dataGridViewIssued.Columns["Id"] != null)
                {
                    dataGridViewIssued.Columns["Id"].Visible = false;
                }

                Logger.Info($"Загружено {_currentDataTable?.Rows?.Count ?? 0} записей абонементов");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке данных абонементов", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки данных", "Ошибка");
            }
        }

        private void jeanModernButtonErase_Click(object sender, EventArgs e)
        {
            try
            {
                jeanSoftTextBoxSearch.Texts = string.Empty;
                Logger.Info("Очищен поисковый запрос");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при очистке поиска", ex);
            }
        }

        private void jeanSoftTextBoxSearch__TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(jeanSoftTextBoxSearch.Texts))
                {
                    jeanModernButtonErase.Visible = false;
                    RefreshDataGrid();
                    return;
                }

                jeanModernButtonErase.Visible = true;
                var searchQuery = BuildSearchQuery(jeanSoftTextBoxSearch.Texts);
                dataGridViewIssued.DataSource = GeneralContext.GetDataFromDatabase(searchQuery,
                    IssuedMembershipContext.ConnectionStringIssued());

                if (dataGridViewIssued.Columns["Id"] != null)
                {
                    dataGridViewIssued.Columns["Id"].Visible = false;
                }

                Logger.Info($"Поиск абонементов: '{jeanSoftTextBoxSearch.Texts}'");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при поиске: {jeanSoftTextBoxSearch.Texts}", ex);
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
                    Id,
                    Клиент,
                    №Карты,
                    Дата_окончания AS 'Дата окончания',
                    Дата_оформления AS 'Дата оформления',
                    Абонемент,
                    Посетил,
                    Оплата,
                    Статус,
                    Посещений_осталось AS 'Посещений осталось',
                    Окончание_заморозки AS 'Окончание заморозки'
                    FROM Issued 
                    WHERE №Карты LIKE '%{names[0]}%' 
                    OR Клиент LIKE '%{names[0]}%' 
                    AND Клиент LIKE '%{names[1]}%'";
        }

        private string BuildSimpleSearchQuery(string name)
        {
            return $@"SELECT 
                    Id,
                    Клиент,
                    №Карты,
                    Дата_окончания AS 'Дата окончания',
                    Дата_оформления AS 'Дата оформления',
                    Абонемент,
                    Посетил,
                    Оплата,
                    Статус,
                    Посещений_осталось AS 'Посещений осталось',
                    Окончание_заморозки AS 'Окончание заморозки'
                    FROM Issued  
                WHERE №Карты LIKE '%{name}%' 
                OR Клиент LIKE '%{name}%'";
        }

        private void dataGridViewIssued_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridViewIssued.SelectedRows.Count == 0) return;

                var row = dataGridViewIssued.SelectedRows[0];
                _id = row.Cells[0].Value.ToString();
                _client = row.Cells[1].Value.ToString();
                _numberCard = row.Cells[2].Value.ToString();
                _dateOver = row.Cells[3].Value.ToString();
                _membership = row.Cells[5].Value.ToString();
                _cost = row.Cells[7].Value.ToString();
                _status = row.Cells[8].Value.ToString();
                _visits = row.Cells[9].Value.ToString();
                _freezeDate = row.Cells[10].Value.ToString();

                nameClient.Text = _client;

                Logger.Info($"Выбран клиент: {_client}, карта: {_numberCard}, ID: {_id}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при выборе клиента", ex);
            }
        }

        private void ShowFreezeDialog()
        {
            try
            {
                if (_id == "")
                {
                    Logger.Warning("Попытка заморозки без выбранного клиента");
                    MessageHelper.MessageWindowOk("Выберите номер клиента из таблицы", "Предупреждение");
                    return;
                }

                Logger.Info($"Открытие диалога заморозки для клиента: {_client}, ID: {_id}");

                using (var freezeDialog = new FreezeMembership())
                {
                    freezeDialog._id = _id;
                    freezeDialog._client = _client;
                    freezeDialog._numberCard = _numberCard;

                    freezeDialog.UpdateData();

                    freezeDialog.ShowDialog();
                    Logger.Info($"Диалог заморозки закрыт для клиента: {_client}");
                }

                RefreshDataGrid();
                Logger.Info($"Данные обновлены после заморозки для клиента: {_client}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при открытии диалога заморозки для клиента {_client}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при открытии заморозки: {ex.Message}", "Ошибка");
            }
        }

        private void ShowChangeDialog()
        {
            try
            {
                if (_id == "")
                {
                    Logger.Warning("Попытка изменения без выбранного клиента");
                    MessageHelper.MessageWindowOk("Выберите номер клиента из таблицы", "Предупреждение");
                    return;
                }

                Logger.Info($"Открытие диалога изменения для клиента: {_client}, ID: {_id}");

                using (var changeDialog = new ChangeIssuedMembership())
                {
                    changeDialog.jeanTextBoxClient.Text = _client;
                    changeDialog.jeanTextBoxStatus.Text = _status;
                    changeDialog.jeanTextBoxMembership.Text = _membership;
                    changeDialog.jeanTextBoxTerm.Text = _dateOver;
                    changeDialog.jeanTextBoxCost.Text = _cost;
                    changeDialog.jeanTextBoxVisits.Text = _visits;
                    changeDialog.jeanTextBoxFreezeDate.Text = _freezeDate;
                    changeDialog._id = _id;

                    changeDialog.UpdateData();

                    changeDialog.ShowDialog();
                    Logger.Info($"Диалог изменения закрыт для клиента: {_client}");
                }

                RefreshDataGrid();
                Logger.Info($"Данные обновлены после изменения для клиента: {_client}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при открытии диалога изменения для клиента {_client}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при открытии изменения: {ex.Message}", "Ошибка");
            }
        }
    }
}