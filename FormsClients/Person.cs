using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsClients
{
    internal partial class Person : ShadowedForm
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

        private FadeAnimation _fadeAnimation;

        public string CardNumber { get; private set; }

        public Person(DataClient data, Panel panelPerson)
        {
            InitializeComponent();

            CardNumber = data.CardNumber;

            CreateControls(data, panelPerson);
            InitializeCustomDesign(data, panelPerson);

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            this.EnableDrag(this);
        }

        private void CreateControls(DataClient data, Panel personPanel)
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
                Text = data.Surname + " " + data.Name + " " + data.FatherName,
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

            // Добавляем элементы на форму
            headerPanel.Controls.Add(profilePicture);
            headerPanel.Controls.Add(userName);
            headerPanel.Controls.Add(userStatus);

            this.Controls.Add(headerPanel);
            this.Controls.Add(infoPanel);
            this.Controls.Add(membershipPanel);
            this.Controls.Add(statsPanel);
        }

        private void InitializeCustomDesign(DataClient data, Panel personPanel)
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

            var closeButton = UIStyler.CreateStyledButton("➡", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(380, 680), new Size(50, 30));
            closeButton.Click += (s, e) => CloseWithAnimation(personPanel);

            personPanel.Controls.Add(closeButton);
        }

        private void FillInfoPanel(DataClient data)
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

        private void FillMembershipPanel(DataClient data)
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
                new { Icon = "🎯", Label = "Тип:", Value = data.Service, Y = 40 },
                new { Icon = "🔢", Label = "Номер:", Value = data.CardNumber, Y = 70 },
                new { Icon = "📅", Label = "Окончание:", Value = data.TermDate, Y = 100 },
                new { Icon = "👣", Label = "Осталось посещений:", Value = data.VisitsLeft, Y = 130 },
                new { Icon = "🎫", Label = "Скидка:", Value = data.Discount.ToString(), Y = 160 }
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

        private void FillStatsPanel(DataClient data)
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
                pictureBox.Region = new Region(new Rectangle(0, 0, pictureBox.Width, pictureBox.Height));
                return;
            }

            Image originalImage = pictureBox.Image;

            int diameter = Math.Min(pictureBox.Width, pictureBox.Height);
            Bitmap roundedImage = new Bitmap(diameter, diameter);

            using (Graphics g = Graphics.FromImage(roundedImage))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, diameter, diameter);

                g.SetClip(path);

                Rectangle sourceRect; 
                Rectangle destRect = new Rectangle(0, 0, diameter, diameter);

                float imageAspectRatio = (float)originalImage.Width / originalImage.Height;
                float pictureBoxAspectRatio = (float)diameter / diameter;

                if (imageAspectRatio > pictureBoxAspectRatio)
                {
                    int scaledHeight = diameter;
                    int scaledWidth = (int)(imageAspectRatio * scaledHeight);
                    int xOffset = (scaledWidth - diameter) / 2;
                    sourceRect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);
                    g.DrawImage(originalImage, new Rectangle(-xOffset, 0, scaledWidth, scaledHeight), sourceRect, GraphicsUnit.Pixel);
                }
                else
                {
                    int scaledWidth = diameter;
                    int scaledHeight = (int)(scaledWidth / imageAspectRatio);
                    int yOffset = (scaledHeight - diameter) / 2;
                    sourceRect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);
                    g.DrawImage(originalImage, new Rectangle(0, -yOffset, scaledWidth, scaledHeight), sourceRect, GraphicsUnit.Pixel);
                }

                g.ResetClip();

                using (var pen = new Pen(borderColor, borderThickness))
                {
                    g.DrawEllipse(pen, borderThickness / 2, borderThickness / 2,
                                  diameter - borderThickness, diameter - borderThickness);
                }
            }

            pictureBox.Image = roundedImage;

            if (pictureBox.Region != null)
            {
                pictureBox.Region.Dispose();
                pictureBox.Region = null;
            }

            GraphicsPath pictureboxPath = new GraphicsPath();
            pictureboxPath.AddEllipse(0, 0, diameter, diameter);
            pictureBox.Region = new Region(pictureboxPath);
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
            _fadeAnimation.CloseWithAnimation();
            personPanel.Visible = false;
        }
    }
}