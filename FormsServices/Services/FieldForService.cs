using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
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

        Panel titlePanel;

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
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(1);
            this.DoubleBuffered = true;

            titlePanel = new Panel
            {
                Size = new Size(this.Width, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0),
            };

            titleLabel.Location = new Point((this.Width - titleLabel.Width) / 2, (titlePanel.Height - titleLabel.Height) / 2);
            titlePanel.Controls.Add(titleLabel);

            label1.ForeColor = Color.MediumSlateBlue;

            // Стилизация текстовых полей
            StyleTextBox(jeanTextBoxName, "Введите название услуги");
            StyleTextBox(jeanTextBoxPrice, "Цена (руб)");
            StyleTextBox(jeanTextBoxTerm, "Срок действия (месяцев)");
            StyleTextBox(jeanTextBoxVisited, "Количество посещений (опционально)");

            // Стилизация кнопок
            StyleButton(jeanModernButtonAdd, "Добавить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point((this.Width - jeanModernButtonAdd.Width) / 2, this.Height - jeanModernButtonAdd.Height - 60));

            // Добавление подсказки
            hintLabel.Location = new Point((this.Width - hintLabel.Width) / 2, this.Height - hintLabel.Height - 10);

            var btnClose = new JeanModernButton
            {
                Font = new Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 28),
                Cursor = Cursors.Hand
            };

            StyleButton(btnClose, "X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, (titlePanel.Height - btnClose.Height) / 2));

            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);
        }

        private void StyleTextBox(JeanTextBox textBox, string placeholder)
        {
            textBox.BorderColor = Color.FromArgb(80, 80, 120);
            textBox.BackColor = Color.White;
            textBox.ForeColor = Color.Black;
            textBox.Font = new Font("Montserrat", 9);

            // События для подсветки
            textBox.Enter += (s, e) =>
            {
                textBox.BorderColor = Color.FromArgb(113, 96, 232);
            };

            textBox.Leave += (s, e) =>
            {
                textBox.BorderColor = Color.FromArgb(113, 96, 232);
            };
        }

        private void StyleButton(JeanModernButton button, string text, Color baseColor, int radius, int radiusSize, Color radiusColor, Point location)
        {
            button.Text = text;
            button.Font = new Font("Montserrat", 10, FontStyle.Bold);
            button.BackColor = baseColor;
            button.BorderColor = radiusColor;
            button.BackgroundColor = baseColor;
            button.TextColor = Color.White;
            button.BorderRadius = radius;
            button.BorderSize = radiusSize;
            button.Location = location;

            // Эффекты при наведении
            button.MouseEnter += (s, e) =>
            {
                button.BackColor = Color.FromArgb(
                    Math.Min(baseColor.R + 30, 255),
                    Math.Min(baseColor.G + 30, 255),
                    Math.Min(baseColor.B + 30, 255));
                button.BackgroundColor = button.BackColor;
            };

            button.MouseLeave += (s, e) =>
            {
                button.BackColor = baseColor;
                button.BackgroundColor = baseColor;
            };

            button.MouseDown += (s, e) =>
            {
                button.BackColor = Color.FromArgb(
                    Math.Max(baseColor.R - 30, 0),
                    Math.Max(baseColor.G - 30, 0),
                    Math.Max(baseColor.B - 30, 0));
                button.BackgroundColor = button.BackColor;
            };

            button.MouseUp += (s, e) =>
            {
                button.BackColor = baseColor;
                button.BackgroundColor = baseColor;
            };
        }

        private void FieldForService_Load(object sender, EventArgs e)
        {
            jeanModernButtonAdd.Font = new Font("Montserrat", DataConfig.sizeFontButtons, FontStyle.Bold);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            if (Message.MessageWindowYesNo("Вы уверены что хотите добавить новую услугу?") != DialogResult.Yes)
                return;

            InsertService();

            // Анимация успешного добавления
            jeanModernButtonAdd.BackColor = Color.FromArgb(50, 200, 100);
            jeanModernButtonAdd.Text = "✓ Готово";

            var timer = new Timer();
            timer.Interval = 500;
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                Message.MessageWindowOk("Услуга успешно добавлена");
                _fadeAnimation.CloseWithAnimation();
            };
            timer.Start();
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

            if (jeanTextBoxVisited.Text != "inf" &&
                (!int.TryParse(jeanTextBoxVisited.Text, out int quantity) || quantity < 0))
            {
                jeanTextBoxVisited.BorderColor = Color.FromArgb(255, 100, 100);
                isValid = false;
            }

            if (!isValid)
            {
                Message.MessageWindowOk("Проверьте правильность заполнения полей");
                return false;
            }

            if (jeanTextBoxName.Text.Length > 100 ||
                jeanTextBoxPrice.Text.Length > 20 ||
                (!string.IsNullOrEmpty(jeanTextBoxVisited.Text) && jeanTextBoxVisited.Text.Length > 20))
            {
                Message.MessageWindowOk("Превышен лимит количества символов");
                return false;
            }

            return true;
        }

        private void InsertService()
        {
            using (var conn = new SQLiteConnection(ServicesContext.ConnectionStringServices()))
            using (var cmd = new SQLiteCommand(
                "INSERT INTO Descriptions ([Абонемент],[Цена],[Срок_действия],[Посещений],[Проданных_за_месяц]) " +
                "VALUES (@Абонемент,@Цена,@Срок_действия,@Посещений,@Проданных_за_месяц)", conn))
            {
                cmd.Parameters.AddWithValue("@Абонемент", jeanTextBoxName.Text.Trim());
                cmd.Parameters.AddWithValue("@Цена", jeanTextBoxPrice.Text.Trim());
                cmd.Parameters.AddWithValue("@Срок_действия", jeanTextBoxTerm.Text.Trim());
                cmd.Parameters.AddWithValue("@Посещений", string.IsNullOrEmpty(jeanTextBoxVisited.Text) ? DBNull.Value : (object)jeanTextBoxVisited.Text.Trim());
                cmd.Parameters.AddWithValue("@Проданных_за_месяц", 0);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void jeanModernButton1_Click(object sender, EventArgs e)
        {
            _fadeAnimation.CloseWithAnimation();
        }

        private void jeanTextBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void jeanTextBoxTerm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void jeanTextBoxVisited_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Сброс цвета при вводе
        private void jeanTextBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is JeanTextBox textBox)
            {
                textBox.BorderColor = Color.FromArgb(80, 80, 120);
            }
        }
    }
}