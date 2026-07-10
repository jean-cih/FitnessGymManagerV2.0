using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsServices
{
    public partial class ChangeArhiveService : ShadowedForm
    {
        private bool _isMousePressed;
        private Point _clickPoint;
        private Point _formStartPoint;

        private FadeAnimation _fadeAnimation;

        public ChangeArhiveService()
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

            // Иконка редактирования
            var iconLabel = new Label
            {
                Text = "✏️",
                Font = new Font("Segoe UI Emoji", 34),
                AutoSize = true,
                Location = new Point(20, 10),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(100, 180, 255)
            };
            this.Controls.Add(iconLabel);

            // Стилизация всех меток
            foreach (Control control in this.Controls)
            {
                if (control is Label label && label != titleLabel)
                {
                    label.Font = new Font("Montserrat", 9, FontStyle.Regular);
                    label.ForeColor = Color.FromArgb(180, 180, 220);
                    label.BackColor = Color.Transparent;
                }
            }

            // Стилизация текстовых полей
            StyleTextBox(jeanSoftTextBoxClient);
            StyleTextBox(jeanSoftTextBoxTerm);
            StyleTextBox(jeanSoftTextBoxMembership);
            StyleTextBox(jeanSoftTextBoxCost);
            StyleTextBox(jeanSoftTextBoxVisits);
            StyleTextBox(jeanSoftTextBoxCard);

            // Стилизация кнопок
            StyleButton(jeanModernButtonChange, "Сохранить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0));
            StyleButton(jeanModernButton1, "X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0));

            // Добавление подсказки внизу формы
            hintLabel.Font = new Font("Montserrat", 8, FontStyle.Italic);
            hintLabel.ForeColor = Color.FromArgb(140, 140, 180);
            hintLabel.BackColor = Color.Transparent;
            hintLabel.AutoSize = true;
        }

        private void StyleTextBox(jeanSoftTextBox textBox)
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
        private void ChangeArchiveService_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y < 40)
            {
                _isMousePressed = true;
                _clickPoint = Cursor.Position;
                _formStartPoint = Location;
            }
        }

        private void ChangeArchiveService_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMousePressed) return;

            var cursorOffset = new Point(
                Cursor.Position.X - _clickPoint.X,
                Cursor.Position.Y - _clickPoint.Y);

            Location = new Point(
                _formStartPoint.X + cursorOffset.X,
                _formStartPoint.Y + cursorOffset.Y);
        }

        private void ChangeArchiveService_MouseUp(object sender, MouseEventArgs e)
        {
            _isMousePressed = false;
            _clickPoint = Point.Empty;
        }
        #endregion

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            if (Message.MessageWindowYesNo("Вы уверены что хотите изменить данные?") != DialogResult.Yes)
                return;

            var query = $@"UPDATE Archive SET 
                        Клиент = '{jeanSoftTextBoxClient.Texts}',
                        Дата_окончания = '{jeanSoftTextBoxTerm.Texts}',
                        Абонемент = '{jeanSoftTextBoxMembership.Texts}',
                        Оплата = '{jeanSoftTextBoxCost.Texts}',
                        Посещений_осталось = '{jeanSoftTextBoxVisits.Texts}'
                        WHERE №Карты = '{jeanSoftTextBoxCard.Texts}';";

            GeneralContext.CommandDataFromDatabase(query,
                ArchiveServicesContext.ConnectionStringArchive());

            // Анимация успешного сохранения
            jeanModernButtonChange.BackColor = Color.FromArgb(50, 200, 100);
            jeanModernButtonChange.Text = "✓ Готово";

            var timer = new Timer();
            timer.Interval = 500;
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                Message.MessageWindowOk("Данные в архиве обновлены");
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
    }
}