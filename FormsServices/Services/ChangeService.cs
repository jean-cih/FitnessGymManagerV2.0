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
        private bool _isMousePressed;
        private Point _clickPoint;
        private Point _formStartPoint;

        private FadeAnimation _fadeAnimation;

        public ChangeService()
        {
            InitializeComponent();
            InitializeCustomDesign();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, null);
        }

        private void InitializeCustomDesign()
        {
            // Настройка стиля формы
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(1);
            this.DoubleBuffered = true;

            // Градиентный фон
            this.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(113, 96, 232),
                    Color.FromArgb(255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }

                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(80, 120, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
                }

                // Разделительная линия
                using (var pen = new Pen(Color.FromArgb(60, 60, 100), 1))
                {
                    e.Graphics.DrawLine(pen, 0, 40, Width, 40);
                }
            };

            // Кастомный заголовок
            titleLabel.Font = new Font("Montserrat", 12, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(220, 220, 255);
            titleLabel.BackColor = Color.Transparent;
            titleLabel.AutoSize = true;

            // Иконка услуги
            var iconLabel = new Label
            {
                Text = "🎯",
                Font = new Font("Segoe UI Emoji", 16),
                AutoSize = true,
                Location = new Point(10, 10),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(100, 180, 255)
            };
            this.Controls.Add(iconLabel);

            // Стилизация всех меток
            foreach (Control control in this.Controls)
            {
                if (control is Label label && label != titleLabel && label != iconLabel)
                {
                    label.Font = new Font("Montserrat", 9, FontStyle.Regular);
                    label.ForeColor = Color.FromArgb(180, 180, 220);
                    label.BackColor = Color.Transparent;
                }
            }

            // Стилизация текстовых полей
            StyleTextBox(jeanSoftTextBoxName, "Название услуги");
            StyleTextBox(jeanSoftTextBoxPrice, "Цена");
            StyleTextBox(jeanSoftTextBoxTerm, "Срок действия (мес)");
            StyleTextBox(jeanSoftTextBoxQuantity, "Количество посещений");

            // Стилизация кнопок
            StyleButton(jeanModernButtonSave, "Сохранить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0));
            StyleButton(jeanModernButton1, "X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0));

            // Добавление подсказки
            hintLabel.Font = new Font("Montserrat", 8, FontStyle.Italic);
            hintLabel.ForeColor = Color.FromArgb(140, 140, 180);
            hintLabel.BackColor = Color.Transparent;
            hintLabel.AutoSize = true;
        }

        private void StyleTextBox(jeanSoftTextBox textBox, string placeholder)
        {
            textBox.BorderColor = Color.FromArgb(80, 80, 120);
            textBox.BorderFocusColor = Color.FromArgb(120, 180, 255);
            textBox.BackColor = Color.White;
            textBox.ForeColor = Color.Black;
            textBox.Font = new Font("Montserrat", 9);
            textBox.PlaceholderColor = Color.FromArgb(120, 120, 150);
        }

        private void StyleButton(JeanModernButton button, string text, Color baseColor, int radius, int radiusSize, Color radiusColor)
        {
            button.Text = text;
            button.Font = new Font("Montserrat", 10, FontStyle.Bold);
            button.BackColor = baseColor;
            button.BorderColor = radiusColor;
            button.BackgroundColor = baseColor;
            button.TextColor = Color.White;
            button.BorderRadius = radius;
            button.BorderSize = radiusSize;

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

        #region Form Movement Handlers
        private void ChangeService_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y < 40) // Перетаскивание только за верхнюю панель
            {
                _isMousePressed = true;
                _clickPoint = Cursor.Position;
                _formStartPoint = Location;
            }
        }

        private void ChangeService_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMousePressed) return;

            var cursorOffset = new Point(
                Cursor.Position.X - _clickPoint.X,
                Cursor.Position.Y - _clickPoint.Y);

            Location = new Point(
                _formStartPoint.X + cursorOffset.X,
                _formStartPoint.Y + cursorOffset.Y);
        }

        private void ChangeService_MouseUp(object sender, MouseEventArgs e)
        {
            _isMousePressed = false;
            _clickPoint = Point.Empty;
        }
        #endregion

        private void jeanModernButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(jeanSoftTextBoxName.Texts) ||
                string.IsNullOrWhiteSpace(jeanSoftTextBoxPrice.Texts) ||
                string.IsNullOrWhiteSpace(jeanSoftTextBoxTerm.Texts) ||
                string.IsNullOrWhiteSpace(jeanSoftTextBoxQuantity.Texts))
            {
                Message.MessageWindowOk("Заполните все поля");
                return;
            }

            if (Message.MessageWindowYesNo("Вы уверены что хотите изменить услугу?") != DialogResult.Yes)
                return;

            var updateQuery = $@"UPDATE Descriptions SET 
                              Абонемент = '{jeanSoftTextBoxName.Texts.Trim()}',
                              Цена = '{jeanSoftTextBoxPrice.Texts.Trim()}',
                              Срок_действия = '{jeanSoftTextBoxTerm.Texts.Trim()}',
                              Посещений = '{jeanSoftTextBoxQuantity.Texts.Trim()}'
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