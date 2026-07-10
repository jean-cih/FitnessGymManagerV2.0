using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class FieldForService : ShadowedForm
    {
        private bool _isMousePressed;
        private Point _clickPoint;
        private Point _formStartPoint;

        private FadeAnimation _fadeAnimation;

        public FieldForService()
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

            // Иконка добавления
            var iconLabel = new Label
            {
                Text = "➕",
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
            StyleTextBox(jeanTextBoxName, "Введите название услуги");
            StyleTextBox(jeanTextBoxPrice, "Цена (руб)");
            StyleTextBox(jeanTextBoxTerm, "Срок действия (месяцев)");
            StyleTextBox(jeanTextBoxVisited, "Количество посещений (опционально)");

            // Стилизация кнопок
            StyleButton(jeanModernButtonAdd, "Добавить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0));
            StyleButton(jeanModernButton1, "X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0));

            // Добавление подсказки
            hintLabel.Font = new Font("Montserrat", 8, FontStyle.Italic);
            hintLabel.ForeColor = Color.FromArgb(140, 140, 180);
            hintLabel.BackColor = Color.Transparent;
            hintLabel.AutoSize = true;
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
        private void FieldForService_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y < 40) // Перетаскивание только за верхнюю панель
            {
                _isMousePressed = true;
                _clickPoint = Cursor.Position;
                _formStartPoint = Location;
            }
        }

        private void FieldForService_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMousePressed) return;

            var cursorOffset = new Point(
                Cursor.Position.X - _clickPoint.X,
                Cursor.Position.Y - _clickPoint.Y);

            Location = new Point(
                _formStartPoint.X + cursorOffset.X,
                _formStartPoint.Y + cursorOffset.Y);
        }

        private void FieldForService_MouseUp(object sender, MouseEventArgs e)
        {
            _isMousePressed = false;
            _clickPoint = Point.Empty;
        }
        #endregion

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