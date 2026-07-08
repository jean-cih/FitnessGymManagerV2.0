using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using Shadow;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsServices
{
    public partial class BackToLife : ShadowedForm
    {
        private bool _isMousePress;
        private Point _clickPoint;
        private Point _formStartPoint;
        private Timer _fadeTimer;
        private float _opacity = 0;

        public BackToLife()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;
            InitializeCustomDesign();
            SetupAnimation();
        }

        private void InitializeCustomDesign()
        {
            // Настройка стиля формы
            this.BackColor = Color.White;
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

            // Настройка заголовка
            titleLabel.Font = new Font("Montserrat", 14, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(220, 220, 255);
            titleLabel.BackColor = Color.Transparent;
            titleLabel.AutoSize = true;

            // Добавление иконок
            var iconLabel = new Label
            {
                Text = "⚡",
                Font = new Font("Segoe UI Emoji", 34),
                AutoSize = true,
                Location = new Point(20, 10),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(100, 150, 255)
            };
            this.Controls.Add(iconLabel);

            // Стилизация меток
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
            StyleTextBox(jeanSoftTextBoxMembership);
            StyleTextBox(jeanSoftTextBoxTerm);
            StyleTextBox(jeanSoftTextBoxVisits);

            // Стилизация кнопок
            StyleButton(jeanModernButtonBackToLife, "Вернуть", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0));
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
        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Добавление свечения по краям
            using (var pen = new Pen(Color.FromArgb(40, 80, 160), 2))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(2, 2, Width - 5, Height - 5));
            }
        }
        */
        #region Form Movement Handlers
        private void BackToLife_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y < 50) // Перетаскивание только за верхнюю часть
            {
                _isMousePress = true;
                _clickPoint = Cursor.Position;
                _formStartPoint = Location;
            }
        }

        private void BackToLife_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMousePress) return;

            var cursorOffset = new Point(
                Cursor.Position.X - _clickPoint.X,
                Cursor.Position.Y - _clickPoint.Y);

            Location = new Point(
                _formStartPoint.X + cursorOffset.X,
                _formStartPoint.Y + cursorOffset.Y);
        }

        private void BackToLife_MouseUp(object sender, MouseEventArgs e)
        {
            _isMousePress = false;
            _clickPoint = Point.Empty;
        }
        #endregion

        private void jeanModernButtonBackToLife_Click(object sender, EventArgs e)
        {
            if (Message.MessageWindowYesNo("Вы уверены что хотите восстановить абонемент?") != DialogResult.Yes)
                return;

            string updateQuery = @"
                UPDATE Contacts SET 
                    Абонемент = @membership, 
                    Срок_абонемента = @term, 
                    Посещений_осталось = @visits 
                WHERE №Карты = @cardNumber;";

            GeneralContext.CommandDataFromDatabase(updateQuery,
                ArchiveServicesContext.ConnectionStringArchive(),
                new SQLiteParameter("@membership", jeanSoftTextBoxMembership.Texts),
                new SQLiteParameter("@term", jeanSoftTextBoxTerm.Texts),
                new SQLiteParameter("@visits", jeanSoftTextBoxVisits.Texts),
                new SQLiteParameter("@cardNumber", labelNubmerCard.Text));

            string deleteQuery = "DELETE FROM Archive WHERE №Карты = @cardNumber;";

            GeneralContext.CommandDataFromDatabase(deleteQuery,
                ArchiveServicesContext.ConnectionStringArchive(),
                new SQLiteParameter("@cardNumber", labelNubmerCard.Text));

            Message.MessageWindowOk("Абонемент восстановлен");

            // Анимация закрытия
            var closeTimer = new Timer();
            closeTimer.Interval = 10;
            float closeOpacity = 1;
            closeTimer.Tick += (s, args) =>
            {
                closeOpacity -= 0.05f;
                this.Opacity = closeOpacity;

                if (closeOpacity <= 0)
                {
                    closeTimer.Stop();
                    this.Close();
                }
            };
            closeTimer.Start();
        }

        private void jeanModernButton1_Click(object sender, EventArgs e)
        {
            // Анимация закрытия
            var closeTimer = new Timer();
            closeTimer.Interval = 10;
            float closeOpacity = 1;
            closeTimer.Tick += (s, args) =>
            {
                closeOpacity -= 0.05f;
                this.Opacity = closeOpacity;

                if (closeOpacity <= 0)
                {
                    closeTimer.Stop();
                    this.Close();
                }
            };
            closeTimer.Start();
        }
    }
}