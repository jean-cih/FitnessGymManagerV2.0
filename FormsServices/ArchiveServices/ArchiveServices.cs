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
        private string client = "", membership = "", term = "", cost = "", numberCard = "", visits = "";

        private FadeAnimation _fadeAnimation;

        public ArchiveServices()
        {
            InitializeComponent();

            InitializeMenu();
            jeanModernButtonChangeData.Click += Button_Click;
            Controls.Add(jeanModernButtonChangeData);

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();
        }

        private void InitializeMenu()
        {
            _menu = new ToolStripDropDownMenu();
            _menu.Font = new Font("Arial", 12, FontStyle.Regular);
            ToolStripMenuItem item1 = new ToolStripMenuItem("Вернуть из архива", Properties.Resources.backToLife);
            ToolStripMenuItem item2 = new ToolStripMenuItem("Изменить параметры", Properties.Resources.change);
            _menu.Items.Add(item1);
            _menu.Items.Add(item2);

            _menu.Items[0].Click += jeanModernButtonBackLife_Click;
            _menu.Items[1].Click += jeanModernButtonChange_Click;
        }

        private void ArchiveServices_Load(object sender, EventArgs e)
        {
            ConfigureFormSize();
            LoadArchiveData();

            FontHelper.ApplyFontSettings(this, null);
        }

        private void ConfigureFormSize()
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
        }

        private DataTable _currentDataTable;

        private void LoadArchiveData()
        {
            string query = "SELECT " +
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

            dataGridViewArchive.ColumnHeaderMouseClick += DataGridViewArchive_ColumnHeaderMouseClick;
        }

        private void DataGridViewArchive_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentDataTable == null) return;

            string columnName = dataGridViewArchive.Columns[e.ColumnIndex].Name;
            bool ascending = dataGridViewArchive.Tag == null || !((bool)dataGridViewArchive.Tag);

            DataTable sortedTable = SortDataTable(_currentDataTable, columnName, ascending);
            dataGridViewArchive.DataSource = sortedTable;

            dataGridViewArchive.Tag = ascending;
        }

        private DataTable SortDataTable(DataTable table, string columnName, bool ascending)
        {
            DataTable sortedTable = table.Clone();

            bool isDateColumn = columnName.Contains("Дата") || columnName.Contains("окончания");

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
            else if (columnName == "Оплата")
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

        private void jeanSoftTextBoxSearch__TextChanged(object sender, EventArgs e)
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
            jeanSoftTextBoxSearch.Texts = "";
        }

        private void jeanModernButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadArchiveData();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            _menu.Show(jeanModernButtonChangeData, new Point(0, jeanModernButtonChangeData.Height));
        }

        private void dataGridViewArchive_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridViewArchive.Rows[e.RowIndex];
            client = row.Cells[0].Value?.ToString() ?? "";
            numberCard = row.Cells[1].Value?.ToString() ?? "";
            term = row.Cells[2].Value?.ToString() ?? "";
            membership = row.Cells[3].Value?.ToString() ?? "";
            cost = row.Cells[4].Value?.ToString() ?? "";
            visits = row.Cells[5].Value?.ToString() ?? "";

            nameClient.Text = client;
            card.Text = numberCard;
        }

        private void ShowFormWithData(Form form, Action<Form> setData)
        {
            if (!new Regex(@"^\d{13}$").IsMatch(numberCard))
            {
                Message.MessageWindowOk("Выберите номер клиента из таблицы");
                return;
            }

            setData(form);
            form.ShowDialog();
            LoadArchiveData();
        }

        private void jeanModernButtonBackLife_Click(object sender, EventArgs e)
        {
            ShowFormWithData(new BackToLife(), form => {
                var f = (BackToLife)form;
                f.labelNameClient.Text = client;
                f.labelNubmerCard.Text = numberCard;
                f.jeanTextBoxMembership.Text = membership;
                f.jeanTextBoxTerm.Text = term;
                f.jeanTextBoxVisits.Text = visits;
            });
        }

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            ShowFormWithData(new ChangeArhiveService(), form => {
                var f = (ChangeArhiveService)form;
                f.jeanTextBoxClient.Text = client;
                f.jeanTextBoxCard.Text = numberCard;
                f.jeanTextBoxMembership.Text = membership;
                f.jeanTextBoxTerm.Text = term;
                f.jeanTextBoxCost.Text = cost;
                f.jeanTextBoxVisits.Text = visits;
            });
        }
    }
}