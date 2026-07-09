using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.FormsClients;
using Microsoft.Office.Interop.Excel;
using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class Clients : Form
    {
        private const string PhotosPath = "\\Photos\\";
        private System.Windows.Forms.Button _cellButton;
        int location;

        private Timer _fadeTimer;
        private float _opacity = 0;

        public Clients()
        {
            InitializeComponent();

            dataGridViewClients.CellMouseEnter += dataGridViewClients_CellMouseEnter;

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

        private void Clients_Load(object sender, EventArgs e)
        {
            ConfigureFormSize();
            PositionControls();
            LoadClientData();
            SetFonts();
        }

        private void ConfigureFormSize()
        {
            this.Width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.85);
            this.Height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.85);
        }

        private void PositionControls()
        {
            jeanPanel.Size = new Size(this.Width - 40, this.Height - 150);
            jeanSoftTextBoxSearch.Location = new System.Drawing.Point(this.Width / 2 - 150, 30);
            jeanModernButtonErase.Location = new System.Drawing.Point(this.Width / 2 - 150 + 260, 35);
            pictureBoxSearch.Location = new System.Drawing.Point(this.Width / 2 - 140, 35);
            jeanModernButtonChangeData.Location = new System.Drawing.Point(this.Width - 160, 13);
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
            jeanModernButtonChangeData.Font = new System.Drawing.Font("Изменить данные клиента", DataConfig.sizeFontButtons);

            dataGridViewClients.DefaultCellStyle.Font =
            dataGridViewClients.ColumnHeadersDefaultCellStyle.Font =
                new System.Drawing.Font("Contacts", DataConfig.sizeFontTables);
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (dataGridViewClients.CurrentRow == null) return;

            int rowIndex = location / dataGridViewClients.Rows[0].Height;
            var row = dataGridViewClients.CurrentRow;

            var clientData = new ClientData
            {
                 FullName = $"{dataGridViewClients.Rows[rowIndex].Cells[0].Value} {dataGridViewClients.Rows[rowIndex].Cells[1].Value}",
                 Phone = dataGridViewClients.Rows[rowIndex].Cells[3].Value.ToString(),
                 CardNumber = dataGridViewClients.Rows[rowIndex].Cells[4].Value.ToString(),
                 Birthday = dataGridViewClients.Rows[rowIndex].Cells[8].Value.ToString(),
                 Saved = dataGridViewClients.Rows[rowIndex].Cells[10].Value.ToString(),
                 Discount = dataGridViewClients.Rows[rowIndex].Cells[9].Value.ToString(),
                 Email = dataGridViewClients.Rows[rowIndex].Cells[7].Value.ToString(),
                 Gender = dataGridViewClients.Rows[rowIndex].Cells[2].Value.ToString(),
            };

            string query = @"
                  SELECT Абонемент, 
                         Дата_окончания AS 'Дата окончания', 
                         Посещений_осталось AS 'Посещений осталось',
                         Посетил
                  FROM Issued 
                  WHERE №Карты = @cardNumber";

            System.Data.DataTable table = new System.Data.DataTable();
            table = GeneralContext.GetDataFromDatabase(query,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", clientData.CardNumber));

            if (table != null && table.Rows.Count > 0)
            {
                clientData.VisitDate = table.Rows[0]["Посетил"] != DBNull.Value ? table.Rows[0]["Посетил"].ToString() : "";
                clientData.VisitsLeft = table.Rows[0]["Посещений осталось"] != DBNull.Value ? table.Rows[0]["Посещений осталось"].ToString() : "";
                clientData.Membership = table.Rows[0]["Абонемент"] != DBNull.Value ? table.Rows[0]["Абонемент"].ToString() : "";
                clientData.Visits = table.Rows[0]["Дата окончания"] != DBNull.Value ? table.Rows[0]["Дата окончания"].ToString() : "";

            }
            else
            {
                clientData.Visits = "";
                clientData.VisitsLeft = "";
                clientData.Membership = "";
                clientData.VisitDate = "";
            }

            if (!checkBoxPerson.Checked)
            {
                ImportPersonFormToPanel(clientData);
            }
            else
            {
                var personForm = new Person(clientData, panelPerson);
                personForm.Show();
            }
        }

        private void ImportPersonFormToPanel(ClientData data)
        {
            panelPerson.Visible = true;

            panelPerson.Controls.Clear();
            var personForm = new Person(data, panelPerson);

            personForm.TopLevel = false;
            personForm.AutoScroll = true;
            personForm.FormBorderStyle = FormBorderStyle.None;
            personForm.Dock = DockStyle.Fill;

            panelPerson.Controls.Add(personForm);

            personForm.Show();

            panelPerson.Refresh();
        }

        private void dataGridViewClients_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                e.RowIndex >= dataGridViewClients.Rows.Count ||
                e.ColumnIndex >= dataGridViewClients.Columns.Count)
                return;

            if (e.ColumnIndex != 0)
            {
                if (_cellButton != null)
                {
                    dataGridViewClients.Controls.Remove(_cellButton);
                    _cellButton.Dispose();
                    _cellButton = null;
                }
                return;
            }

            if (_cellButton != null)
            {
                dataGridViewClients.Controls.Remove(_cellButton);
                _cellButton.Dispose();
            }

            _cellButton = new System.Windows.Forms.Button
            {
                Text = "Открыть",
                Size = new Size(80, 30),
                Location = GetButtonLocation(e)
            };

            _cellButton.Click += button_Click;
            dataGridViewClients.Controls.Add(_cellButton);

            location = e.RowIndex * dataGridViewClients.Rows[e.RowIndex].Height;
        }

        private System.Drawing.Point GetButtonLocation(DataGridViewCellEventArgs e)
        {
            var cellRect = dataGridViewClients.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            return new System.Drawing.Point(cellRect.X + dataGridViewClients.Columns[e.ColumnIndex].Width, cellRect.Y);
        }

        private void jeanModernButtonChangeData_Click(object sender, EventArgs e)
        {
            new ChangeData().Show();
            this.Close();
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
    }

    public class ClientData
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string CardNumber { get; set; }
        public string Membership { get; set; }
        public string Birthday { get; set; }
        public string Visits { get; set; }
        public string VisitsLeft { get; set; }
        public string Saved { get; set; }
        public string Discount { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string VisitDate { get; set; }
    }
}