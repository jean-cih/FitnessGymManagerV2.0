using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class ChangeService : ShadowedForm
    {
        private FadeAnimation _fadeAnimation;

        Panel titlePanel;

        string[] notChangeableTexts = new string[]
            {
                "РЕДАКТИРОВАНИЕ УСЛУГИ"
            };

        public ChangeService()
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
                Size = new Size(874, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0),
            };

            titleLabel.Location = new Point((this.Width - titleLabel.Width) / 2, (titlePanel.Height - titleLabel.Height) / 2);
            titlePanel.Controls.Add(titleLabel);

            label1.ForeColor = Color.MediumSlateBlue;

            // Стилизация текстовых полей
            StyleTextBox(jeanTextBoxName, "Название услуги");
            StyleTextBox(jeanTextBoxPrice, "Цена");
            StyleTextBox(jeanTextBoxTerm, "Срок действия (мес)");
            StyleTextBox(jeanTextBoxVisited, "Количество посещений");

            // Стилизация кнопок
            StyleButton(jeanModernButtonSave, "Сохранить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point((this.Width - jeanModernButtonSave.Width) / 2, this.Height - jeanModernButtonSave.Height - 50));

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
            //textBox.BorderFocusColor = Color.FromArgb(120, 180, 255);
            textBox.BackColor = Color.White;
            textBox.ForeColor = Color.Black;
            textBox.Font = new Font("Montserrat", 9);
            //textBox.PlaceholderColor = Color.FromArgb(120, 120, 150);
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

        private void jeanModernButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(jeanTextBoxName.Text) ||
                string.IsNullOrWhiteSpace(jeanTextBoxPrice.Text) ||
                string.IsNullOrWhiteSpace(jeanTextBoxTerm.Text) ||
                string.IsNullOrWhiteSpace(jeanTextBoxVisited.Text))
            {
                Message.MessageWindowOk("Заполните все поля");
                return;
            }

            if (Message.MessageWindowYesNo("Вы уверены что хотите изменить услугу?") != DialogResult.Yes)
                return;

            var updateQuery = $@"UPDATE Descriptions SET 
                              Абонемент = '{jeanTextBoxName.Text.Trim()}',
                              Цена = '{jeanTextBoxPrice.Text.Trim()}',
                              Срок_действия = '{jeanTextBoxTerm.Text.Trim()}',
                              Посещений = '{jeanTextBoxVisited.Text.Trim()}'
                              WHERE Id = '{DataConfig.membershipId}'";

            GeneralContext.CommandDataFromDatabase(updateQuery,
                ServicesContext.ConnectionStringServices());

            // Анимация успешного сохранения
            jeanModernButtonSave.BackColor = Color.FromArgb(50, 200, 100);
            jeanModernButtonSave.Text = "✓ Готово";

            var timer = new Timer();
            timer.Interval = 500;
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                Message.MessageWindowOk("Услуга изменена");
                _fadeAnimation.CloseWithAnimation();
            };
            timer.Start();
        }

        private void jeanModernButton1_Click(object sender, EventArgs e)
        {
            _fadeAnimation.CloseWithAnimation();
        }

        private void JeanSoftTextBox_Enter(object sender, EventArgs e)
        {
            if (sender is jeanSoftTextBox textBox)
            {
                textBox.BorderColor = Color.FromArgb(100, 180, 255);
            }
        }

        private void JeanSoftTextBox_Leave(object sender, EventArgs e)
        {
            if (sender is jeanSoftTextBox textBox)
            {
                textBox.BorderColor = Color.FromArgb(80, 80, 120);
            }
        }

        // Валидация числовых полей
        private void jeanSoftTextBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void jeanSoftTextBoxTerm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void jeanSoftTextBoxQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}