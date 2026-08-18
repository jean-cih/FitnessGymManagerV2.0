using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.FormsServices;
using GymApplicationV2._0.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class ArchiveServices : Form
    {
        private ToolStripDropDownMenu _menu;
        private string _id = string.Empty;
        private string _client = string.Empty;
        private string _membership = string.Empty;
        private string _term = string.Empty;
        private string _cost = string.Empty;
        private string _numberCard = string.Empty;
        private string _visits = string.Empty;

        private FadeAnimation _fadeAnimation;

        public ArchiveServices()
        {
            try
            {
                InitializeComponent();

                InitializeMenu();
                jeanModernButtonChangeData.Click += Button_Click;
                Controls.Add(jeanModernButtonChangeData);

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                Logger.Info("Форма ArchiveServices инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации ArchiveServices", ex);
                throw;
            }
        }

        private void InitializeMenu()
        {
            try
            {
                _menu = new ToolStripDropDownMenu();
                _menu.Font = new Font("Arial", 12, FontStyle.Regular);
                ToolStripMenuItem item1 = new ToolStripMenuItem("Вернуть из архива", Properties.Resources.backToLife);
                ToolStripMenuItem item2 = new ToolStripMenuItem("Изменить параметры", Properties.Resources.change);
                _menu.Items.Add(item1);
                _menu.Items.Add(item2);

                _menu.Items[0].Click += jeanModernButtonBackLife_Click;
                _menu.Items[1].Click += jeanModernButtonChange_Click;

                Logger.Info("Меню архива инициализировано");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeMenu", ex);
            }
        }

        private void ArchiveServices_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка ArchiveServices_Load");
                ConfigureFormSize();
                LoadArchiveData();

                FontHelper.ApplyFontSettings(this, null);
                Logger.Info("ArchiveServices_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ArchiveServices_Load", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки формы", "Ошибка");
            }
        }

        private void ConfigureFormSize()
        {
            try
            {
                Width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.75);
                Height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.75);

                jeanPanel.Size = new Size(Width - 40, Height - 180);
                jeanSoftTextBoxSearch.Location = new Point(Width / 2 - 150, 30);
                jeanModernButtonErase.Location = new Point(Width / 2 - 150 + 260, 35);
                pictureBoxSearch.Location = new Point(Width / 2 - 140, 35);
                jeanModernButtonChangeData.Location = new Point(Width - 150, 30);

                dataGridViewArchive.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2,
                    Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);

                Logger.Info($"Размеры формы настроены: {Width}x{Height}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ConfigureFormSize", ex);
            }
        }

        private DataTable _currentDataTable;

        private void LoadArchiveData()
        {
            try
            {
                string query = "SELECT " +
                "Id, " +
                "Клиент, " +
                "№Карты, " +
                "Дата_окончания AS 'Дата окончания', " +
                "Абонемент, " +
                "Оплата, " +
                "Посещений_осталось AS 'Посещений осталось' " +
                "FROM Archive";

                _currentDataTable = GeneralContext.GetDataFromDatabase(query,
                ArchiveServicesContext.ConnectionStringArchive());

                dataGridViewArchive.DataSource = _currentDataTable;

                if (dataGridViewArchive.Columns["Id"] != null)
                {
                    dataGridViewArchive.Columns["Id"].Visible = false;
                }

                Logger.Info($"Загружено {_currentDataTable?.Rows?.Count ?? 0} записей из архива");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при загрузке данных архива", ex);
                MessageHelper.MessageWindowOk("Ошибка загрузки данных архива", "Ошибка");
            }
        }

        private void jeanSoftTextBoxSearch__TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(jeanSoftTextBoxSearch.Texts))
                {
                    jeanModernButtonErase.Visible = false;
                    LoadArchiveData();
                    return;
                }

                jeanModernButtonErase.Visible = true;
                var searchQuery = BuildSearchQuery(jeanSoftTextBoxSearch.Texts);
                dataGridViewArchive.DataSource = GeneralContext.GetDataFromDatabase(searchQuery,
                    ArchiveServicesContext.ConnectionStringArchive());

                if (dataGridViewArchive.Columns["Id"] != null)
                {
                    dataGridViewArchive.Columns["Id"].Visible = false;
                }

                Logger.Info($"Поиск в архиве: '{jeanSoftTextBoxSearch.Texts}'");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при поиске в архиве: {jeanSoftTextBoxSearch.Texts}", ex);
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
                    №Карты AS 'Карта',
                    Дата_окончания AS 'Дата окончания',
                    Абонемент,
                    Оплата,
                    Посещений_осталось AS 'Посещений осталось'
                    FROM Archive 
                    WHERE №Карты LIKE '%{names[0]}%' 
                    OR Клиент LIKE '%{names[0]}%' 
                    AND Клиент LIKE '%{names[1]}%'";
        }

        private string BuildSimpleSearchQuery(string name)
        {
            return $@"SELECT 
                Id,
                Клиент,
                №Карты AS 'Карта',
                Дата_окончания AS 'Дата окончания',
                Абонемент,
                Оплата,
                Посещений_осталось AS 'Посещений осталось'
                FROM Archive 
                WHERE №Карты LIKE '%{name}%' 
                OR Клиент LIKE '%{name}%'";
        }

        private void jeanModernButtonErase_Click(object sender, EventArgs e)
        {
            try
            {
                jeanSoftTextBoxSearch.Texts = "";
                Logger.Info("Очищен поисковый запрос в архиве");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при очистке поиска в архиве", ex);
            }
        }

        private void jeanModernButtonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Обновление данных архива");
                LoadArchiveData();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при обновлении данных архива", ex);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            try
            {
                _menu.Show(jeanModernButtonChangeData, new Point(0, jeanModernButtonChangeData.Height));
                Logger.Info("Открыто меню действий с архивом");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при открытии меню", ex);
            }
        }

        private void dataGridViewArchive_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                var row = dataGridViewArchive.Rows[e.RowIndex];
                _id = row.Cells[0].Value?.ToString() ?? "";
                _client = row.Cells[1].Value?.ToString() ?? "";
                _numberCard = row.Cells[2].Value?.ToString() ?? "";
                _term = row.Cells[3].Value?.ToString() ?? "";
                _membership = row.Cells[4].Value?.ToString() ?? "";
                _cost = row.Cells[5].Value?.ToString() ?? "";
                _visits = row.Cells[6].Value?.ToString() ?? "";

                nameClient.Text = _client;

                Logger.Info($"Выбран клиент из архива: {_client}, карта: {_numberCard}, ID: {_id}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при выборе клиента из архива", ex);
            }
        }

        private void ShowFormWithData(Form form, Action<Form> setData)
        {
            try
            {
                if (_id == string.Empty)
                {
                    Logger.Warning("Попытка действия без выбранного клиента");
                    MessageHelper.MessageWindowOk("Выберите номер клиента из таблицы", "Предупреждение");
                    return;
                }

                setData(form);
                Logger.Info($"Открытие формы {form.GetType().Name} для клиента: {_client}");
                form.ShowDialog();
                LoadArchiveData();
                Logger.Info($"Форма {form.GetType().Name} закрыта, данные обновлены");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ShowFormWithData для формы {form?.GetType().Name}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при открытии формы: {ex.Message}", "Ошибка");
            }
        }

        private void jeanModernButtonBackLife_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info($"Нажата кнопка 'Вернуть из архива' для клиента: {_client}");
                ShowFormWithData(new BackToLife(), form => {
                    var f = (BackToLife)form;
                    f._client = _client;
                    f._numberCard = _numberCard;
                    f.jeanTextBoxMembership.Text = _membership;
                    f.jeanTextBoxTerm.Text = _term;
                    f.jeanTextBoxVisits.Text = _visits;
                    f._id = _id;

                    f.UpdateData();
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при возврате клиента {_client} из архива", ex);
                MessageHelper.MessageWindowOk($"Ошибка при возврате из архива: {ex.Message}", "Ошибка");
            }
        }

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info($"Нажата кнопка 'Изменить параметры' для клиента: {_client}");
                ShowFormWithData(new ChangeArhiveService(), form => {
                    var f = (ChangeArhiveService)form;
                    f.jeanTextBoxClient.Text = _client;
                    f.jeanTextBoxCard.Text = _numberCard;
                    f.jeanTextBoxMembership.Text = _membership;
                    f.jeanTextBoxTerm.Text = _term;
                    f.jeanTextBoxCost.Text = _cost;
                    f.jeanTextBoxVisits.Text = _visits;
                    f._id = _id;

                    f.UpdateData();
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при изменении параметров клиента {_client} в архиве", ex);
                MessageHelper.MessageWindowOk($"Ошибка при изменении параметров: {ex.Message}", "Ошибка");
            }
        }
    }
}