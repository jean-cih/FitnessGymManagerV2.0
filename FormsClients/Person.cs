using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using Shadow;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GymApplicationV2._0.FormsClients
{
    public partial class Person : ShadowedForm
    {
        private string path = "\\Photos\\";

        // Элементы управления
        private PictureBox profilePicture;
        private Label userName;
        private Label userStatus;
        private JeanPanel headerPanel;
        private JeanPanel infoPanel;
        private JeanPanel membershipPanel;
        private JeanPanel statsPanel;
        private JeanModernButton closeButton;

        private Timer _fadeTimer;
        private float _opacity = 0;

        public Person(ClientData data, Panel panelPerson)
        {
            InitializeComponent();

            CreateControls(data, panelPerson);
            InitializeCustomDesign(data, panelPerson);

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

        private void CreateControls(ClientData data, Panel personPanel)
        {
            // Header Panel
            headerPanel = new JeanPanel
            {
                Size = new Size(410, 140),
                Location = new Point(20, 20),
                BackColor = Color.White,
                GradientBottomColor = Color.White,
                GradientTapColor = Color.White,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10),
                BorderRadius = 20,
            };

            // Profile Picture
            profilePicture = new PictureBox
            {
                Size = new Size(100, 100),
                Location = new Point(20, 20),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.None
            };

            // User Name
            userName = new Label
            {
                Text = data.FullName,
                AutoSize = false,
                Size = new Size(300, 30),
                Location = new Point(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Montserrat", 15, FontStyle.Bold),
                ForeColor = Color.Black
            };

            // User Status
            userStatus = new Label
            {
                Text = "● Активный клиент",
                AutoSize = false,
                Size = new Size(150, 20),
                Location = new Point(130, 60),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Montserrat", 9, FontStyle.Regular),
                ForeColor = Color.LimeGreen
            };

            // Info Panel
            infoPanel = new JeanPanel
            {
                Size = new Size(410, 200),
                Location = new Point(20, 170),
                BackColor = Color.White,
                GradientBottomColor = Color.White,
                GradientTapColor = Color.White,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10),
                BorderRadius = 20,
            };

            // Membership Panel
            membershipPanel = new JeanPanel
            {
                Size = new Size(410, 200),
                Location = new Point(20, 380),
                BackColor = Color.White,
                GradientBottomColor = Color.White,
                GradientTapColor = Color.White,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10),
                BorderRadius = 20,
            };

            // Stats Panel
            statsPanel = new JeanPanel
            {
                Size = new Size(410, 80),
                Location = new Point(20, 590),
                BackColor = Color.White,
                GradientBottomColor = Color.White,
                GradientTapColor = Color.White,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10),
                BorderRadius = 20,
            };

            // Close Button
            closeButton = new JeanModernButton
            {
                Size = new Size(50, 30),
                Location = new Point(380, 680),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Montserrat", 10, FontStyle.Bold),
            };

            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 80, 100);
            closeButton.Click += (s, e) => CloseWithAnimation(personPanel);

            // Добавляем элементы на форму
            headerPanel.Controls.Add(profilePicture);
            headerPanel.Controls.Add(userName);
            headerPanel.Controls.Add(userStatus);

            this.Controls.Add(headerPanel);
            this.Controls.Add(infoPanel);
            this.Controls.Add(membershipPanel);
            this.Controls.Add(statsPanel);
            this.Controls.Add(closeButton);
        }

        private void InitializeCustomDesign(ClientData data, Panel personPanel)
        {
            // Настройка формы
            this.Size = new Size(460, 840);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(20);
            this.DoubleBuffered = true;

            // Градиентный фон
            this.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(113, 96, 232),
                    Color.DodgerBlue,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }

                // Неоновая рамка
                using (var pen = new Pen(Color.FromArgb(80, 120, 200), 2))
                {
                    pen.DashStyle = DashStyle.Solid;
                    e.Graphics.DrawRectangle(pen, new Rectangle(1, 1, Width - 3, Height - 3));
                }
            };

            // Заполняем информационные панели
            FillInfoPanel(data);
            FillMembershipPanel(data);
            FillStatsPanel(data);

            StyleButton(closeButton, "➡", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0));

            // Обработчики событий
            this.MouseDown += Person_MouseDown;
            this.MouseMove += Person_MouseMove;
            this.MouseUp += Person_MouseUp;
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) CloseWithAnimation(personPanel); };
        }

        private void StyleButton(JeanModernButton button, string text, Color baseColor, int radius, int radiusSize, Color radiusColor)
        {
            button.Text = text;
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

        private void FillInfoPanel(ClientData data)
        {
            var titleLabel = new Label
            {
                Text = "Личные данные",
                Font = new Font("Montserrat", 13, FontStyle.Bold),
                Size = new Size(200, 25),
                Location = new Point(15, 10),
                ForeColor = Color.FromArgb(113, 96, 232)
            };

            object idClient = GeneralContext.GetElementFromDatabase("SELECT id FROM Contacts WHERE №Карты = '" + data.CardNumber + "';",
                ClientsContext.ConnectionStringClients());
            if (idClient == null)
            {
                idClient = "None";
            }

            var infoItems = new[]
            {
                new { Icon = "🆔", Label = "ID:", Value = idClient.ToString(), Y = 40 },
                new { Icon = "📞", Label = "Телефон:", Value = data.Phone, Y = 70 },
                new { Icon = "📧", Label = "Почта:", Value = data.Email, Y = 100 },
                new { Icon = "🎂", Label = "День рождения:", Value = data.Birthday, Y = 130 },
                new { Icon = "⭐", Label = "Сохранен:", Value = data.Saved, Y = 160 }
            };

            infoPanel.Controls.Add(titleLabel);

            foreach (var item in infoItems)
            {
                var iconLabel = new Label
                {
                    Text = item.Icon,
                    Font = new Font("Segoe UI Emoji", 12),
                    Size = new Size(30, 20),
                    Location = new Point(15, item.Y),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Black
                };

                var labelLabel = new Label
                {
                    Text = item.Label,
                    Font = new Font("Montserrat", 9, FontStyle.Bold),
                    Size = new Size(120, 20),
                    Location = new Point(50, item.Y),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.Black
                };

                var valueLabel = new Label
                {
                    Text = item.Value,
                    Font = new Font("Montserrat", 9, FontStyle.Regular),
                    Size = new Size(200, 20),
                    Location = new Point(180, item.Y),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.Black
                };

                infoPanel.Controls.Add(iconLabel);
                infoPanel.Controls.Add(labelLabel);
                infoPanel.Controls.Add(valueLabel);
            }
        }

        private void FillMembershipPanel(ClientData data)
        {
            var titleLabel = new Label
            {
                Text = "Абонемент",
                Font = new Font("Montserrat", 13, FontStyle.Bold),
                Size = new Size(200, 25),
                Location = new Point(15, 10),
                ForeColor = Color.FromArgb(113, 96, 232)
            };

            var membershipItems = new[]
            {
                new { Icon = "🎯", Label = "Тип:", Value = data.Membership, Y = 40 },
                new { Icon = "🔢", Label = "Номер:", Value = data.CardNumber, Y = 70 },
                new { Icon = "📅", Label = "Окончание:", Value = data.Visits, Y = 100 },
                new { Icon = "👣", Label = "Осталось посещений:", Value = data.VisitsLeft, Y = 130 },
                new { Icon = "🎫", Label = "Скидка:", Value = data.Discount, Y = 160 }
            };

            membershipPanel.Controls.Add(titleLabel);

            foreach (var item in membershipItems)
            {
                var iconLabel = new Label
                {
                    Text = item.Icon,
                    Font = new Font("Segoe UI Emoji", 12),
                    Size = new Size(30, 20),
                    Location = new Point(15, item.Y),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Black
                };

                var labelLabel = new Label
                {
                    Text = item.Label,
                    Font = new Font("Montserrat", 9, FontStyle.Bold),
                    Size = new Size(120, 20),
                    Location = new Point(50, item.Y),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.Black
                };

                var valueLabel = new Label
                {
                    Text = item.Value,
                    Font = new Font("Montserrat", 9, FontStyle.Regular),
                    Size = new Size(200, 20),
                    Location = new Point(180, item.Y),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.Black
                };

                membershipPanel.Controls.Add(iconLabel);
                membershipPanel.Controls.Add(labelLabel);
                membershipPanel.Controls.Add(valueLabel);
            }
        }

        private void FillStatsPanel(ClientData data)
        {
            var statsItems = new[]
            {
                new { Icon = "🔄", Label = "Последнее посещение:", Value = data.VisitDate, X = 15 },
                new { Icon = "📊", Label = "Статус:", Value = "● Активен", X = 200 }
            };

            foreach (var item in statsItems)
            {
                var iconLabel = new Label
                {
                    Text = item.Icon,
                    Font = new Font("Segoe UI Emoji", 12),
                    Size = new Size(30, 20),
                    Location = new Point(item.X, 15),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Black
                };

                var labelLabel = new Label
                {
                    Text = item.Label,
                    Font = new Font("Montserrat", 8, FontStyle.Bold),
                    Size = new Size(120, 15),
                    Location = new Point(item.X + 35, 10),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.Black
                };

                var valueLabel = new Label
                {
                    Text = item.Value,
                    Font = new Font("Montserrat", 10, FontStyle.Bold),
                    Size = new Size(100, 20),
                    Location = new Point(item.X + 35, 30),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.LimeGreen
                };

                statsPanel.Controls.Add(iconLabel);
                statsPanel.Controls.Add(labelLabel);
                statsPanel.Controls.Add(valueLabel);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string pathToPhotos = Environment.CurrentDirectory;
            profilePicture.Image = Image.FromFile(FindPhoto(userName.Text, pathToPhotos + path, "Мужской"));
            MakePictureRound(profilePicture, Color.White, 2);
        }

        public void MakePictureRound(PictureBox pictureBox, Color borderColor, int borderThickness)
        {
            if (pictureBox.Image == null)
            {
                // Если изображения нет, можно очистить регион или просто выйти.
                // Присваиваем регион по умолчанию, чтобы избежать проблем, если он был ранее установлен.
                pictureBox.Region = new Region(new Rectangle(0, 0, pictureBox.Width, pictureBox.Height));
                return;
            }

            // Сохраняем исходное изображение
            Image originalImage = pictureBox.Image;

            // Создаем новый Bitmap с размерами PictureBox
            // (Предполагается, что PictureBox имеет квадратные размеры для идеального круга)
            // Если PictureBox не квадратный, круг будет вписан в наименьшую сторону.
            int diameter = Math.Min(pictureBox.Width, pictureBox.Height);
            Bitmap roundedImage = new Bitmap(diameter, diameter);

            using (Graphics g = Graphics.FromImage(roundedImage))
            {
                // Включаем сглаживание для качественного круга
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Создаем Path для круга
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, diameter, diameter);

                // Обрезаем Graphics контекст по кругу
                g.SetClip(path);

                // Вычисляем размеры и позицию для отрисовки исходного изображения
                // так, чтобы оно было центрировано и максимально заполняло круг.
                Rectangle sourceRect; // Часть исходного изображения, которую будем использовать
                Rectangle destRect = new Rectangle(0, 0, diameter, diameter); // Куда отрисовывать на новом Bitmap

                // Рассчитываем, как масштабировать и обрезать исходное изображение,
                // чтобы оно выглядело хорошо в круге.
                // Аналогично PictureBox.SizeMode.Zoom или PictureBox.SizeMode.CenterImage
                float imageAspectRatio = (float)originalImage.Width / originalImage.Height;
                float pictureBoxAspectRatio = (float)diameter / diameter; // Всегда 1.0 для квадрата

                if (imageAspectRatio > pictureBoxAspectRatio) // Изображение шире, чем "круг"
                {
                    int scaledHeight = diameter;
                    int scaledWidth = (int)(imageAspectRatio * scaledHeight);
                    int xOffset = (scaledWidth - diameter) / 2;
                    sourceRect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);
                    g.DrawImage(originalImage, new Rectangle(-xOffset, 0, scaledWidth, scaledHeight), sourceRect, GraphicsUnit.Pixel);
                }
                else // Изображение выше, чем "круг" (или равно)
                {
                    int scaledWidth = diameter;
                    int scaledHeight = (int)(scaledWidth / imageAspectRatio);
                    int yOffset = (scaledHeight - diameter) / 2;
                    sourceRect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);
                    g.DrawImage(originalImage, new Rectangle(0, -yOffset, scaledWidth, scaledHeight), sourceRect, GraphicsUnit.Pixel);
                }

                // Сбрасываем Clip, чтобы рамка не обрезалась
                g.ResetClip();

                // Отрисовываем рамку вокруг круга
                using (var pen = new Pen(borderColor, borderThickness))
                {
                    g.DrawEllipse(pen, borderThickness / 2, borderThickness / 2,
                                  diameter - borderThickness, diameter - borderThickness);
                }
            }

            // Устанавливаем новое обрезанное изображение в PictureBox
            pictureBox.Image = roundedImage;

            // Удаляем старый регион (если был), чтобы не было конфликтов
            if (pictureBox.Region != null)
            {
                pictureBox.Region.Dispose();
                pictureBox.Region = null; // Сбрасываем регион, так как теперь само изображение круглое
            }

            // Если вы хотите, чтобы PictureBox сам был кругом, но это не обязательно,
            // так как изображение уже круглое. Но это может помочь с кликами и т.п.
            GraphicsPath pictureboxPath = new GraphicsPath();
            pictureboxPath.AddEllipse(0, 0, diameter, diameter);
            pictureBox.Region = new Region(pictureboxPath);

            // Отписываемся от Paint события, так как мы уже отрисовали рамку на Bitmap
            // Если вы хотите, чтобы рамка отрисовывалась всегда поверх,
            // то событие Paint может быть полезным, но тогда убедитесь, что
            // оно не конфликтует с отрисовкой на Bitmap.
            // Обычно, когда само изображение круглое, Region и Paint для рамки не нужны.
        }


        private string FindPhoto(string clientName, string folderPath, string sex)
        {
            string[] allowedExt = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".tif", ".tiff" };

            string baseFolder = Path.IsPathRooted(folderPath) ? folderPath : Path.Combine(Environment.CurrentDirectory, folderPath);

            try
            {
                if (!Directory.Exists(baseFolder))
                    return string.Empty;

                if (Path.HasExtension(clientName))
                {
                    string full = Path.Combine(baseFolder, clientName);
                    if (File.Exists(full))
                        return full;

                    var filesWithSameName = Directory.EnumerateFiles(baseFolder, Path.GetFileNameWithoutExtension(clientName) + ".*");
                    var match = filesWithSameName
                        .FirstOrDefault(f =>
                            string.Equals(Path.GetFileNameWithoutExtension(f), Path.GetFileNameWithoutExtension(clientName), StringComparison.OrdinalIgnoreCase)
                            && allowedExt.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase));
                    return match ?? string.Empty;
                }

                foreach (var ext in allowedExt)
                {
                    string full = Path.Combine(baseFolder, clientName + ext);
                    if (File.Exists(full))
                        return full;
                }

                var candidates = Directory.EnumerateFiles(baseFolder, clientName + ".*")
                    .Where(f => allowedExt.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase));
                var first = candidates.FirstOrDefault();
                if (first != null)
                    return first;

                return Environment.CurrentDirectory + path + "userMale.png";
            }
            catch (Exception)
            {
                return Environment.CurrentDirectory + path + "userMale.png";
            }
        }

        private void CloseWithAnimation(Panel personPanel)
        {
            var closeTimer = new Timer();
            closeTimer.Interval = 10;
            float closeOpacity = 1;
            closeTimer.Tick += (s, args) =>
            {
                closeOpacity -= 0.06f;
                this.Opacity = closeOpacity;

                if (closeOpacity <= 0)
                {
                    closeTimer.Stop();
                    this.Close();
                }
            };
            closeTimer.Start();

            personPanel.Visible = false;
        }

        private bool _isMousePressed;
        private Point _clickPoint;

        private void Person_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y < 40)
            {
                _isMousePressed = true;
                _clickPoint = e.Location;
            }
        }

        private void Person_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMousePressed)
            {
                this.Location = new Point(
                    this.Location.X + e.X - _clickPoint.X,
                    this.Location.Y + e.Y - _clickPoint.Y
                );
            }
        }

        private void Person_MouseUp(object sender, MouseEventArgs e)
        {
            _isMousePressed = false;
        }
    }
}