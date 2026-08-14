using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class FieldForService : ShadowedForm
    {
        private FadeAnimation _fadeAnimation;

        private ComboBox typeMembership;
        private Panel titlePanel;

        string[] notChangeableTexts = new string[]
            {
                "ДОБАВЛЕНИЕ УСЛУГИ"
            };

        public FieldForService()
        {
            InitializeComponent();
            InitializeCustomDesign();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, notChangeableTexts);

            titlePanel.EnableDrag(this);
        }

        private void InitializeCustomDesign()
        {
            // Настройка стиля формы
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            titlePanel = new Panel
            {
                Size = new Size(this.Width, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0),
            };
            titlePanel.Controls.Add(titleLabel);

            // Стилизация текстовых полей
            UIStyler.StyleTextBox(this, jeanTextBoxName);
            UIStyler.StyleTextBox(this, jeanTextBoxPrice);
            UIStyler.StyleTextBox(this, jeanTextBoxTerm);
            UIStyler.StyleTextBox(this, jeanTextBoxVisited);

            typeMembership = new ComboBox
            {
                Size = new Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            // Заполняем причины заморозки
            typeMembership.Items.AddRange(new object[]
            {
                "обычный",
                "безлимитный"
            });
            typeMembership.SelectedIndex = 0;

            // Стилизация кнопок
            var jeanModernButtonAdd = UIStyler.CreateStyledButton("Добавить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point(this.Width / 2 - 60, this.Height - 80), new Size(120, 40));
            var btnClose = UIStyler.CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, 10), new Size(30, 28));

            jeanModernButtonAdd.Click += buttonSave_Click;
            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);
            this.Controls.Add(typeMembership);
            this.Controls.Add(jeanModernButtonAdd);
        }

        public void UpdateData()
        {
            labelService.Location = new Point((this.Width - labelService.Width) / 2, labelService.Location.Y);
            typeMembership.Location = new Point((this.Width - typeMembership.Width) / 2, jeanTextBoxVisited.Location.Y + 50);
            titleLabel.Location = new Point((this.Width - titleLabel.Width) / 2, (titlePanel.Height - titleLabel.Height) / 2);
            hintLabel.Location = new Point((this.Width - hintLabel.Width) / 2, this.Height - hintLabel.Height - 10);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            if (MessageHelper.MessageWindowYesNo("Вы уверены что хотите добавить новую услугу?") != DialogResult.Yes)
                return;

            InsertService();

            MessageHelper.ShowNotification(this, "✅ Услуга успешно добавлена", 1500);
            _fadeAnimation.CloseWithAnimation();
        }

        private bool ValidateInput()
        {
            // Визуальная валидация
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(jeanTextBoxName.Text))
            {
                jeanTextBoxName.BorderColor = Color.FromArgb(255, 100, 100);
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(jeanTextBoxPrice.Text) || !int.TryParse(jeanTextBoxPrice.Text, out int price) || price < 0)
            {
                jeanTextBoxPrice.BorderColor = Color.FromArgb(255, 100, 100);
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(jeanTextBoxTerm.Text) || !int.TryParse(jeanTextBoxTerm.Text, out int term) || term < 0)
            {
                jeanTextBoxTerm.BorderColor = Color.FromArgb(255, 100, 100);
                isValid = false;
            }

            if (!isValid)
            {
                MessageHelper.MessageWindowOk("Проверьте правильность заполнения полей", "Предупреждение");
                return false;
            }

            if (jeanTextBoxName.Text.Length > 100 ||
                jeanTextBoxPrice.Text.Length > 20 ||
                (!string.IsNullOrEmpty(jeanTextBoxVisited.Text) && jeanTextBoxVisited.Text.Length > 20))
            {
                MessageHelper.MessageWindowOk("Превышен лимит количества символов", "Предупреждение");
                return false;
            }

            return true;
        }

        private void InsertService()
        {
            using (var conn = new SQLiteConnection(ServicesContext.ConnectionStringServices()))
            using (var cmd = new SQLiteCommand(
                "INSERT INTO Descriptions ([Абонемент],[Цена],[Срок_действия],[Посещений],[Тип],[Проданных_за_месяц]) " +
                "VALUES (@Абонемент,@Цена,@Срок_действия,@Посещений,@Тип,@Проданных_за_месяц)", conn))
            {
                cmd.Parameters.AddWithValue("@Абонемент", jeanTextBoxName.Text.Trim());
                cmd.Parameters.AddWithValue("@Цена", jeanTextBoxPrice.Text.Trim());
                cmd.Parameters.AddWithValue("@Срок_действия", jeanTextBoxTerm.Text.Trim());
                cmd.Parameters.AddWithValue("@Посещений", string.IsNullOrWhiteSpace(jeanTextBoxVisited.Text) || jeanTextBoxVisited.Text.Trim() == "0"
                    ? DBNull.Value
                    : (object)jeanTextBoxVisited.Text.Trim());
                cmd.Parameters.AddWithValue("@Тип", typeMembership.Text);
                cmd.Parameters.AddWithValue("@Проданных_за_месяц", 0);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}