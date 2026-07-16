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
    public partial class IssuedMembership : Form
    {
        private ToolStripDropDownMenu _menu;
        private string _numberCard = string.Empty;
        private string _dateOver = string.Empty;
        private string _client = string.Empty;
        private string _membership = string.Empty;
        private string _cost = string.Empty;
        private string _status = string.Empty;
        private string _visits = string.Empty;

        private FadeAnimation _fadeAnimation;

        public IssuedMembership()
        {
            InitializeComponent();
            InitializeMenu();
            SetupFormLayout();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, null);
        }

        private void InitializeMenu()
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
        }

        private void SetupFormLayout()
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
        }

        private void IssuedMembership_Load(object sender, EventArgs e) =>
            RefreshDataGrid();

        private void MenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag.ToString() == "freeze")
                ShowFreezeDialog();
            else
                ShowChangeDialog();
        }

        private DataTable _currentDataTable;

        private void RefreshDataGrid()
        {
            string query = @"
        SELECT 
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
        FROM Issued";

            _currentDataTable = GeneralContext.GetDataFromDatabase(query,
                IssuedMembershipContext.ConnectionStringIssued());

            GeneralContext.FormatDateColumns(_currentDataTable);

            dataGridViewIssued.DataSource = _currentDataTable;

            GeneralContext.FormatData(dataGridViewIssued);
        }
        private void jeanModernButtonRefresh_Click(object sender, EventArgs e) =>
            RefreshDataGrid();

        private void jeanModernButtonErase_Click(object sender, EventArgs e) =>
            jeanSoftTextBoxSearch.Texts = string.Empty;

        private void jeanSoftTextBoxSearch__TextChanged(object sender, EventArgs e)
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
            if (dataGridViewIssued.SelectedRows.Count == 0) return;

            var row = dataGridViewIssued.SelectedRows[0];
            _client = row.Cells[0].Value.ToString();
            _numberCard = row.Cells[1].Value.ToString();
            _dateOver = row.Cells[2].Value.ToString();
            _membership = row.Cells[4].Value.ToString();
            _cost = row.Cells[5].Value.ToString();
            _status = row.Cells[6].Value.ToString();
            _visits = row.Cells[7].Value.ToString();

            card.Text = _numberCard;
            nameClient.Text = _client;
        }

        private void ShowFreezeDialog()
        {
            if (!ValidateCardNumber()) return;

            using (var freezeDialog = new FreezeMembership())
            {
                freezeDialog.txtClientName.Text = _client;
                freezeDialog.txtCardNumber.Text = _numberCard;
                freezeDialog.txtFreezeDate.Text = DateTime.Now.ToString("dd.MM.yy");
                freezeDialog._dateOver = _dateOver;

                if (freezeDialog.ShowDialog() == DialogResult.OK)
                {
                    RefreshDataGrid();
                }
            }
        }

        private void ShowChangeDialog()
        {
            if (!ValidateCardNumber()) return;

            using (var changeDialog = new ChangeIssuedMembership())
            {
                changeDialog.jeanTextBoxClient.Text = _client;
                changeDialog.jeanTextBoxStatus.Text = _status;
                changeDialog.jeanTextBoxMembership.Text = _membership;
                changeDialog.jeanTextBoxTerm.Text = _dateOver;
                changeDialog.jeanTextBoxCost.Text = _cost;
                changeDialog.jeanTextBoxVisits.Text = _visits;
                changeDialog._numberCard = _numberCard;

                if (changeDialog.ShowDialog() == DialogResult.OK)
                {
                    RefreshDataGrid();
                }
            }
        }

        private bool ValidateCardNumber()
        {
            var isValid = new Regex(@"^\d{13}$").IsMatch(_numberCard);

            if (!isValid)

                Message.MessageWindowOk("Выберите номер клиента из таблицы");

            return isValid;
        }
    }
}