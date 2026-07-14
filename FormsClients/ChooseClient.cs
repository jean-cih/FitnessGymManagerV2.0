using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class ChooseClient : Form
    {
        private string _name = "";
        private string _surname = "";
        private string _numberCard = "";

        private FadeAnimation _fadeAnimation;

        public ChooseClient()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();
        }
        
        private void ChooseClient_Load(object sender, EventArgs e)
        {
            ConfigureFormSize();
            PositionControls();
            LoadClientData();

            FontHelper.ApplyFontSettings(this, null);
        }

        private void ConfigureFormSize()
        {
            this.Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.65);
            this.Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.7);
        }

        private void PositionControls()
        {
            jeanPanel1.Size = new Size(this.Width - 40, this.Height - 200);
            jeanSoftTextBoxSearch.Location = new Point(this.Width / 2 - 150, 10);
            pictureBoxSearch.Location = new Point(this.Width / 2 - 140, 15);
            jeanModernButtonErase.Location = new Point(this.Width / 2 - 140 + 253, 15);
            jeanModernButtonChoose.Location = new Point(this.Width / 2 - 60, this.Height - 130);
        }

        private DataTable _currentDataTable;

        private void LoadClientData()
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

            dataGridViewClients.ColumnHeaderMouseClick += DataGridViewClients_ColumnHeaderMouseClick;
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

        private void jeanSoftTextBoxSearch__TextChanged(object sender, EventArgs e)
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

        private void jeanModernButtonErase_Click(object sender, EventArgs e)
        {
            jeanSoftTextBoxSearch.Texts = "";
        }

        private void dataGridViewClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewClients.SelectedRows[0];
            _surname = selectedRow.Cells[0].Value.ToString();
            _name = selectedRow.Cells[1].Value.ToString();
            _numberCard = selectedRow.Cells[4].Value.ToString();

            labelName.Text = $"{_surname} {_name} : {_numberCard}";
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_name) ||
                string.IsNullOrWhiteSpace(_surname))
            {
                Message.MessageWindowOk("Выберите клиента");
                return;
            }

            OpenServicesForm();
            this.Close();
        }

        private void OpenServicesForm()
        {
            var services = new Services
            {
                jeanModernButtonAdd = { Visible = false },
                jeanModernButtonDelete = { Visible = false },
                jeanModernButtonSell = { Visible = true },
                jeanModernButtonChange = { Visible = false },
                labelName = { Text = $"{_name} {_surname}", Visible = true },
                jeanSoftTextBoxPurchase = { Visible = true },
                labelNumberCard = { Text = _numberCard, Visible = true },
                checkBoxVisited = { Visible = true }
            };

            services.Show();
        }
    }
}