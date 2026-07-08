using GymApplicationV2._0.Components;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.FormsSettings;
using Microsoft.Office.Interop.Excel;
using Shadow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace GymApplicationV2._0
{
    public partial class Form1 : ShadowedForm
    {
        private string nameClient = "";
        private string numberCard;
        private string filePath = Path.Combine("AppFiles", "Font.txt");
        private int numberLeft;

        private ToolStripDropDownMenu _menu_service;
        private ToolStripDropDownMenu _menu_settings;

        private System.Windows.Forms.Timer _fadeTimer;
        private float _opacity = 0;

        private readonly Color PrimaryOrange = Color.FromArgb(255, 140, 0);  
        private readonly Color PrimaryBlue = Color.MediumSlateBlue;          

        private readonly Color SoftOrange = Color.FromArgb(255, 200, 150);  

        private readonly Color SoftSlateBlue = Color.FromArgb(200, 190, 255); 

        private readonly Color DarkGray = Color.FromArgb(50, 50, 60);        
        private readonly Color MediumGray = Color.FromArgb(120, 120, 130);   
        private readonly Color LightGray = Color.FromArgb(240, 240, 245);     
        private readonly Color White = Color.White;

        private readonly Color BackgroundLight = Color.FromArgb(250, 248, 252); 
        private readonly Color BackgroundDark = Color.FromArgb(245, 242, 248); 

        private readonly Color CardBackground = Color.White;

        private readonly Color HoverBlue = Color.FromArgb(130, 110, 255);    
        private readonly Color PressedBlue = Color.FromArgb(110, 90, 230);    

        private readonly Color DarkBlue = Color.FromArgb(83, 69, 190);     
        private readonly Color LightBlue = Color.FromArgb(147, 132, 255);

        private readonly Color TextColor = Color.FromArgb(33, 33, 33);
        private readonly Color PrimaryColor = Color.FromArgb(63, 81, 181);

        private System.Windows.Forms.PictureBox backgroundPicture;
        private System.Windows.Forms.Timer _backgroundAnimationTimer;
        private List<FloatingElement> _floatingElements = new List<FloatingElement>();
        private List<System.Drawing.Point> _animatedPoints = new List<System.Drawing.Point>();
        private Random _random = new Random();

        private int baseButtonWidth = 150;
        private int baseSpacing = 10;
        private int baseStartX;
        private int baseButtonHeight = 40;

        JeanModernButton btnClose;

        public Form1()
        {
            InitializeComponent();

            EnsureRequiredDirectoriesExist();
            CopyPhotosToOutput();

            InitializeCustomDesign();
            UpdateButtonLayout();

            jeanModernButtonServices.Click += Button_Click_Services;

            _menu_service = new ToolStripDropDownMenu();
            _menu_service.Font = new System.Drawing.Font("Segoe UI", 11, FontStyle.Regular);

            ToolStripMenuItem item1 = new ToolStripMenuItem("Абонементы", Properties.Resources.membership);
            ToolStripMenuItem item2 = new ToolStripMenuItem("Выданные абонементы", Properties.Resources.issuedMembership);
            ToolStripMenuItem item3 = new ToolStripMenuItem("Абонементы в архиве", Properties.Resources.archive);
            ToolStripMenuItem item4 = new ToolStripMenuItem("История платежей", Properties.Resources.payments);

            _menu_service.Items.Add(item1);
            _menu_service.Items.Add(item2);
            _menu_service.Items.Add(item3);
            _menu_service.Items.Add(item4);

            _menu_service.Items[0].Click += jeanModernButtonService_Click;
            _menu_service.Items[1].Click += jeanModernButtonChange_Click;
            _menu_service.Items[2].Click += jeanModernButtonArchive_Click;
            _menu_service.Items[3].Click += jeanModernButtonHistoryPayment_Click;

            jeanModernButtonSettings.Click += Button_Click_Settings;

            _menu_settings = new ToolStripDropDownMenu();
            _menu_settings.Font = new System.Drawing.Font("Segoe UI", 11, FontStyle.Regular);

            ToolStripMenuItem item5 = new ToolStripMenuItem("Дизайн", Properties.Resources.membership);
            ToolStripMenuItem item6 = new ToolStripMenuItem("Загрузка данных", Properties.Resources.issuedMembership);
            ToolStripMenuItem item7 = new ToolStripMenuItem("Документация", Properties.Resources.archive);

            _menu_settings.Items.Add(item5);
            _menu_settings.Items.Add(item6);
            _menu_settings.Items.Add(item7);

            _menu_settings.Items[0].Click += jeanModernButtonDesign_Click;
            _menu_settings.Items[1].Click += jeanModernButtonImport_Click;
            _menu_settings.Items[2].Click += jeanModernButtonDocumentation_Click;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            
            SetupAnimation();
        }

        public static void CopyPhotosToOutput()
        {
            string repoRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"));
            string sourcePath = Path.Combine(repoRoot, "Photos");
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Photos");

            if (Directory.Exists(sourcePath))
            {
                CopyDirectory(sourcePath, targetPath);
            }
        }

        private static void CopyDirectory(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);
            }

            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                CopyDirectory(subDir, Path.Combine(targetDir, Path.GetFileName(subDir)));
            }
        }

        private void UpdateButtonLayout()
        {
            this.Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.70);
            this.Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.70);

            System.Drawing.Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            int screenWidth = screenBounds.Width;

            baseStartX = this.Width;


            float scaleX = (float)screenWidth / baseStartX;

            int buttonWidth = (int)(baseButtonWidth);

            int spacing = (int)(baseSpacing * scaleX);

            int startX = this.Width - (buttonWidth * 6);
            if ( screenWidth <= 1400)
            {
                startX += (int)(1.5 * buttonWidth) - 20;
                buttonWidth -= 40;
            }
            else if ( screenWidth <= 1600)
            {
                startX += buttonWidth;
                buttonWidth -= 30;
            }

            int buttonHeight = baseButtonHeight;

            jeanModernButtonSettings.Width = buttonWidth;
            jeanModernButtonSettings.Height = buttonHeight;
            jeanModernButtonSettings.Location = new System.Drawing.Point(startX, 15);

            jeanModernButtonServices.Width = buttonWidth;
            jeanModernButtonServices.Height = buttonHeight;
            jeanModernButtonServices.Location = new System.Drawing.Point(startX + buttonWidth + spacing, 15);

            jeanModernButtonPurchase.Width = buttonWidth;
            jeanModernButtonPurchase.Height = buttonHeight;
            jeanModernButtonPurchase.Location = new System.Drawing.Point(startX + (buttonWidth + spacing) * 2, 15);

            jeanModernButtonClients.Width = buttonWidth;
            jeanModernButtonClients.Height = buttonHeight;
            jeanModernButtonClients.Location = new System.Drawing.Point(startX + (buttonWidth + spacing) * 3, 15);

            jeanModernButtonReport.Width = buttonWidth;
            jeanModernButtonReport.Height = buttonHeight;

            jeanModernButtonReport.Location = new System.Drawing.Point(startX + (buttonWidth + spacing) * 4, 15);

            btnClose.Location = new System.Drawing.Point(startX + (buttonWidth + spacing) * 5 + 10, 20);
        }

        private void InitializeCustomDesign()
        {
            CheckIfDataExistsFont();

            btnClose = new JeanModernButton
            {
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 28),
                Cursor = Cursors.Hand,
            };

            btnClose = CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, PrimaryOrange, new System.Drawing.Point(jeanModernButtonReport.Location.X + jeanModernButtonReport.Width + 20, 20), new Size(30, 28));
            btnClose.Click += (s, e) => CloseWithAnimation();
            this.Controls.Add(btnClose);
            btnClose.Visible = false;

            // Основные настройки формы
            this.Text = "GYM MASTER";
            this.ForeColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(20);

            // Градиентный фон с новой цветовой схемой
            this.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    BackgroundLight,
                    BackgroundDark,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            };
            CreateMainCard();

            CreateNavigationPanel();

            CreateVisitedCard();

            CreateClientPanel();

            if (DataClass.styleBackground == "Dynamic")
            {
                CreateDynamicBackground();
            }
            else if (DataClass.styleBackground == "Casual")
            {
                CreateBackground();
            }
            else if (DataClass.styleBackground == "Minimal")
            {
                CreateMinimalBackground();
            }
            else if (DataClass.styleBackground == "Static")
            {
                CreateStaticBackground();
            }
        }

        public static void EnsureRequiredDirectoriesExist()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Создаем папку Databases
            string databasesPath = Path.Combine(appDirectory, "Databases");
            if (!Directory.Exists(databasesPath))
            {
                Directory.CreateDirectory(databasesPath);
            }

            // Создаем папку AppFiles
            string appFilesPath = Path.Combine(appDirectory, "AppFiles");
            if (!Directory.Exists(appFilesPath))
            {
                Directory.CreateDirectory(appFilesPath);
            }
        }

        private void CreateNavigationPanel()
        {
            var navPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = PrimaryBlue
            };

            var titleLabel = new System.Windows.Forms.Label
            {
                Text = "🏋️ СИБИРЯК",
                Font = new System.Drawing.Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                AutoSize = true
            };

            // Стилизуем кнопки навигации в синей гамме
            StyleButton(jeanModernButtonSettings, "⚙️ Настройки", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 2);
            StyleButton(jeanModernButtonServices, "🎫 Услуги", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 2);
            StyleButton(jeanModernButtonPurchase, "🛒 Товары", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 2);
            StyleButton(jeanModernButtonClients, "👥 Клиенты", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 2);
            StyleButton(jeanModernButtonReport, "📊 Отчет", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 2);

            navPanel.Controls.Add(titleLabel);
            navPanel.Controls.AddRange(new Control[] { jeanModernButtonSettings, jeanModernButtonServices, jeanModernButtonPurchase, jeanModernButtonClients, jeanModernButtonReport });

            this.Controls.Add(navPanel);
        }

        private void StyleButton(JeanModernButton button,
                         string text,
                         Color backColor,
                         Color foreColor,
                         Color mouseOverBackColor,
                         Color mouseDownBackColor,
                         Size size,
                         System.Drawing.Font font,
                         FlatStyle flatStyle,
                         int borderSize)
        {
            button.Text = text;
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Size = size;
            button.Font = font;
            button.FlatStyle = flatStyle;
            button.FlatAppearance.BorderSize = borderSize;
            button.FlatAppearance.MouseOverBackColor = mouseOverBackColor;
            button.FlatAppearance.MouseDownBackColor = mouseDownBackColor;
            button.BorderSize = borderSize;
        }

        private void CreateMainCard()
        {
            var mainCard = new JeanPanel
            {
                Size = new Size((int)(2.9 * jeanModernButtonSettings.Width), 300),
                Location = new System.Drawing.Point(80, 100),
                BackColor = White,
                GradientBottomColor = White,
                GradientTapColor = White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15),
                BorderRadius = 20,
            };

            var titleLabel = new System.Windows.Forms.Label
            {
                Text = "🎯 Продажи",
                Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true,
                BackColor = White,
            };

            // Стилизуем кнопки продаж в оранжевой гамме
            StyleButton(jeanModernButtonNewMember, "🆕 Новый", PrimaryOrange, White, SoftOrange, Color.FromArgb(220, 120, 0), new Size(130, 40), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 0);
            jeanModernButtonNewMember.Location = new System.Drawing.Point(40, 65);

            StyleButton(jeanModernButtonSingleTicket, "🎫 Разовый", PrimaryOrange, White, SoftOrange, Color.FromArgb(220, 120, 0), new Size(120, 40), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 0);
            jeanModernButtonSingleTicket.Location = new System.Drawing.Point(180, 65);

            StyleButton(jeanModernButtonChooseClient, "👤 Выбрать клиента", PrimaryBlue, White, LightBlue, DarkBlue, new Size(140, 50), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 0);
            jeanModernButtonChooseClient.Location = new System.Drawing.Point(100, 210);

            StyleButton(jeanModernButtonSell, "💰 Продать", Color.FromArgb(220, 80, 60), White, Color.FromArgb(240, 100, 80), Color.FromArgb(200, 60, 40), new Size(160, 45), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 0);
            jeanModernButtonSell.Location = new System.Drawing.Point(90, 150);

            mainCard.Controls.AddRange(new Control[] { titleLabel, jeanModernButtonNewMember, jeanModernButtonSingleTicket,
            jeanModernButtonChooseClient, jeanModernButtonSell });

            this.Controls.Add(mainCard);
        }

        private void CreateVisitedCard()
        {
            var visitedCard = new JeanPanel
            {
                Size = new Size(420, 150),
                Location = new System.Drawing.Point(450, 100),
                BackColor = CardBackground,
                GradientBottomColor = CardBackground,
                GradientTapColor = CardBackground,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15),
                BorderRadius = 20,
            };

            var titleLabel = new System.Windows.Forms.Label
            {
                Text = "👣 Посещение",
                Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = DarkGray,
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };

            //var tooltip = new System.Windows.Forms.ToolTip();
            //tooltip.SetToolTip(handleMode, "Кликните 'Ручной режим' и введите нужные данные,\nпосле нажмите Enter");

            jeanTextBoxNumberCard.Location = new System.Drawing.Point(40, 60);
            jeanTextBoxNumberCard.Size = new Size(300, 40);
            jeanTextBoxNumberCard.Font = new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold);
            jeanTextBoxNumberCard.BackColor = White;
            jeanTextBoxNumberCard.BorderColor = MediumGray;

            StyleButton(jeanModernButtonReturn, "↩️ Возврат", Color.FromArgb(220, 53, 69), White, Color.FromArgb(220, 220, 225), MediumGray, new Size(150, 35), new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold), FlatStyle.Flat, 0);
            jeanModernButtonReturn.Location = new System.Drawing.Point(45, 105);

            // Стилизуем кнопку очистки
            var eraseButton = new JeanModernButton
            {
                Location = new System.Drawing.Point(345, 65),
                Size = new Size(35, 35),
                Text = "✕",
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 0,
                BorderColor = White,
                BorderSize = 2
            };

            eraseButton.Click += (s, e) => ClickErase();

            eraseButton.FlatAppearance.BorderSize = 1;
            eraseButton.FlatAppearance.BorderColor = MediumGray;

            visitedCard.Controls.AddRange(new Control[] { titleLabel, jeanTextBoxNumberCard, jeanModernButtonReturn, eraseButton });

            this.Controls.Add(visitedCard);
        }

        private void ClickErase()
        {
            jeanTextBoxNumberCard.Text = "";
        }

        private void CreateClientPanel()
        {
            var clientPanel = new JeanPanel
            {
                Size = new Size(790, 2 * jeanModernButtonSettings.Width),
                Location = new System.Drawing.Point(450, 270),
                BackColor = CardBackground,
                GradientBottomColor = CardBackground,
                GradientTapColor = CardBackground,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15),
                BorderRadius = 20,
            };

            System.Drawing.Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            int screenWidth = screenBounds.Width;

            if (screenWidth <= 1600)
            {
                clientPanel.Location = new System.Drawing.Point(80, 420);
            }

            var titleLabel = new System.Windows.Forms.Label
            {
                Text = "👥 Информация о клиенте",
                Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = DarkGray,
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };

            // Стилизуем DataGridView с новой цветовой схемой
            dataGridViewClient.Location = new System.Drawing.Point(20, 60);
            dataGridViewClient.Size = new Size(750, 2 * jeanModernButtonSettings.Width);
            dataGridViewClient.BackgroundColor = White;
            dataGridViewClient.BorderStyle = BorderStyle.None;
            dataGridViewClient.EnableHeadersVisualStyles = false;

            // Заголовки столбцов - MediumSlateBlue
            dataGridViewClient.ColumnHeadersDefaultCellStyle.BackColor = PrimaryBlue;
            dataGridViewClient.ColumnHeadersDefaultCellStyle.ForeColor = White;
            dataGridViewClient.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            dataGridViewClient.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Основные ячейки
            dataGridViewClient.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9);
            dataGridViewClient.DefaultCellStyle.BackColor = White;
            dataGridViewClient.DefaultCellStyle.ForeColor = DarkGray;
            dataGridViewClient.DefaultCellStyle.SelectionBackColor = SoftSlateBlue;
            dataGridViewClient.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Чередующиеся строки
            dataGridViewClient.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 252);

            clientPanel.Controls.Add(titleLabel);
            clientPanel.Controls.Add(dataGridViewClient);

            this.Controls.Add(clientPanel);
        }

        private void Button_Click_Services(object sender, EventArgs e)
        {
            _menu_service.Show(jeanModernButtonServices, new System.Drawing.Point(0, jeanModernButtonServices.Height));
        }

        private void Button_Click_Settings(object sender, EventArgs e)
        {
            _menu_settings.Show(jeanModernButtonSettings, new System.Drawing.Point(0, jeanModernButtonSettings.Height));
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            
            JeanFormStyle.fStyle style;
            if (DataClass.styleForm == "UserStyle")
            {
                style = JeanFormStyle.fStyle.UserStyle;
            }
            else if (DataClass.styleForm == "SimpleDark")
            {
                style = JeanFormStyle.fStyle.SimpleDark;
            }
            else if (DataClass.styleForm == "TelegramStyle")
            {
                style = JeanFormStyle.fStyle.TelegramStyle;
            }
            else
            {
                style = JeanFormStyle.fStyle.None;
                btnClose.Visible = true;
            }

            jeanFormStyle.FormStyle = style;

            jeanModernButtonSettings.Font = new System.Drawing.Font("⚙️ Настройки", DataClass.sizeFontButtons);
            jeanModernButtonServices.Font = new System.Drawing.Font("🎫 Услуги", DataClass.sizeFontButtons);
            jeanModernButtonPurchase.Font = new System.Drawing.Font("🛒 Товары", DataClass.sizeFontButtons);
            jeanModernButtonClients.Font = new System.Drawing.Font("👥 Клиенты", DataClass.sizeFontButtons);
            jeanModernButtonReport.Font = new System.Drawing.Font("📊 Отчет", DataClass.sizeFontButtons);
            jeanModernButtonNewMember.Font = new System.Drawing.Font("🆕 Новый", DataClass.sizeFontButtons);
            jeanModernButtonSingleTicket.Font = new System.Drawing.Font("🎫 Разовый", DataClass.sizeFontButtons);
            jeanModernButtonChooseClient.Font = new System.Drawing.Font("👤 Выбрать клиента", DataClass.sizeFontButtons);
            jeanModernButtonSell.Font = new System.Drawing.Font("💰 Продать", DataClass.sizeFontButtons);


            dataGridViewClient.DefaultCellStyle.Font = new System.Drawing.Font("Issued", DataClass.sizeFontTables);
            dataGridViewClient.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Issued", DataClass.sizeFontTables);

            CheckIfDataExistsClients();
            CheckIfDataExistsServices();
            CheckIfDataExistsPayment();
            CheckIfDataExistsArchive();
            CheckIfDataExistsIssued();
            CheckIfDataExistsProducts();
        }

        private void CloseWithAnimation()
        {
            var closeTimer = new System.Windows.Forms.Timer();
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

        private JeanModernButton CreateStyledButton(string text, Color baseColor, int radius, int radiusSize, Color radiusColor, System.Drawing.Point location, Size size)
        {
            var button = new JeanModernButton
            {
                Text = text,
                Location = location,
                Size = size,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = baseColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(baseColor, 0.2f);
            button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(baseColor, 0.2f);

            button.Text = text;
            button.Font = new System.Drawing.Font("Montserrat", 10, FontStyle.Bold);
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

            return button;
        }

        private void SetupAnimation()
        {
            _fadeTimer = new System.Windows.Forms.Timer();
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

        private void CheckIfDataExistsFont()
        {
            if (!File.Exists(filePath))
            {
                CreateFile();
            }
            else
            {
                List<string> lines = new List<string>();
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
                try
                {
                    DataClass.sizeFontCaptions = Convert.ToInt32(lines[3]);
                    DataClass.sizeFontButtons = Convert.ToInt32(lines[5]);
                    DataClass.sizeFontTables = Convert.ToInt32(lines[7]);
                    DataClass.styleForm = lines[9];
                    DataClass.styleBackground = lines[11];
                }
                catch
                {
                    File.Delete(filePath);

                    CreateFile();
                }
            }
        }


        private void CheckIfDataExistsClients()
        {
            if (!File.Exists("Databases\\Clients.db"))
            {
                ClientsContext.CreatingDatabase();
            }
        }


        private void CheckIfDataExistsServices()
        {
            if (!File.Exists("Databases\\Services.db"))
            {
                ServicesContext.CreatingDatabase();
            }
        }

        private void CheckIfDataExistsPayment()
        {
            if (!File.Exists("Databases\\Payments.db"))
            {
                HistoryPaymentContext.CreatingDatabase();
            }
        }

        private void CheckIfDataExistsArchive()
        {
            if (!File.Exists("Databases\\Archive.db"))
            {
                ArchiveServicesContext.CreatingDatabase();
            }
        }

        private void CheckIfDataExistsIssued()
        {
            if (!File.Exists("Databases\\IssuedMembership.db"))
            {
                IssuedMembershipContext.CreatingDatabase();
            }
        }

        private void CheckIfDataExistsProducts()
        {
            if (!File.Exists("Databases\\Products.db"))
            {
                ProductsContext.CreatingDatabase();
            }
        }

        private void CreateFile()
        {
            using (FileStream fs = File.Create(filePath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(
                    "НИ В КОЕМ СЛУЧАЕ НЕ ИЗМЕНЯТЬ ЭТОТ ФАЙЛ\n\n" +
                    "Размер шрифта заголовков:\n10\n" +
                    "Размер шрифта названий кнопок:\n10\n" +
                    "Размер шрифта в таблице:\n10\n" +
                    "Дизайн оформления:\nNone\n" +
                    "Дизайн заднего фона:\nCasual");
                fs.Write(info, 0, info.Length);
            }
        }

        private bool isProgrammaticChange = false;
        private void jeanTextBoxNumberCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            isProgrammaticChange = true;

            if (e.KeyChar == (char)Keys.Enter)
            {

                if (Regex.IsMatch(jeanTextBoxNumberCard.Text, @"^-?\d+(\d+)?$") || jeanTextBoxNumberCard.Text.Length == 0)
                {
                    string cardNumber_trim = jeanTextBoxNumberCard.Text.Trim();

                    if (!ValidateIssuedExists(cardNumber_trim))
                        return;

                    if (TryHandleFrozenMembership(cardNumber_trim))
                        return;

                    if (!ValidateMembershipStatus(cardNumber_trim))
                        return;

                    ProcessClientVisit(cardNumber_trim);
                }
                else
                {
                    string[] names = jeanTextBoxNumberCard.Text.Split(' ');
                    if (names.Length < 2)
                    {
                        PlayErrorSound();
                        ShowMessage("Введите фамилию и имя через пробел");
                        ClearCardNumber();
                        return;
                    }

                    var searchQuery = BuildSearchQuery(names);
                    object existClient = GeneralContext.GetElementFromDatabase(searchQuery,
                IssuedMembershipContext.ConnectionStringIssued());

                    if (existClient == null)
                    {
                        Message.MessageWindowOk("Клиент без карты");
                        return;
                    }

                    string numberCard = existClient.ToString();
                    if (!ValidateIssuedExists(numberCard))
                        return;

                    if (TryHandleFrozenMembership(numberCard))
                        return;

                    if (!ValidateMembershipStatus(numberCard))
                        return;

                    
                    ProcessClientVisit(numberCard);
                }
            }
        }

        private string BuildSearchQuery(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (!string.IsNullOrEmpty(names[i]))
                {
                    names[i] = char.ToUpper(names[i][0]) + names[i].Substring(1);
                }
            }

            return BuildFullNameSearchQuery(names);

        }

        private string BuildFullNameSearchQuery(string[] names)
        {
            return $@"SELECT 
                    №Карты
                    FROM Issued 
                    WHERE Клиент LIKE '%{names[0]}%' 
                    AND Клиент LIKE '%{names[1]}%'";
        }

        // Вспомогательные методы
        private void PlayErrorSound()
        {
            using (SoundPlayer soundPlayerError = new SoundPlayer(Properties.Resources.error))
            {
                soundPlayerError.Play();
            }
        }

        private void ShowMessage(string message)
        {
            Message.MessageWindowOk(message);
        }

        private void ClearCardNumber()
        {
            jeanTextBoxNumberCard.Text = "";
        }

        private void UpdateSellButton(IssuedMembershipContext.IssuedInfo issuedInfo)
        {
            if (issuedInfo != null)
            {
                nameClient = issuedInfo.FullName;
                jeanModernButtonSell.Text = $"💰 Продать\n{nameClient}";
            }
        }

        private void textNumberClient_TextChanged(object sender, EventArgs e)
        {
            if (isProgrammaticChange)
            {
                isProgrammaticChange = false;
                return;
            }

            if (Regex.IsMatch(jeanTextBoxNumberCard.Text, @"^-?\d+(\d+)?$") || jeanTextBoxNumberCard.Text.Length == 0)
            {
                jeanTextBoxNumberCard.BackColor = Color.White;
            }
            else
            {
                jeanTextBoxNumberCard.BackColor = Color.FromArgb(255, 150, 150);
            }

            if (jeanTextBoxNumberCard.Text.Length != 13)
                return;

            string cardNumber = jeanTextBoxNumberCard.Text.Trim();

            if (!ValidateIssuedExists(cardNumber))
                return;

            if (TryHandleFrozenMembership(cardNumber))
                return;

            if (!ValidateMembershipStatus(cardNumber))
                return;

            ProcessClientVisit(cardNumber);
        }

        // Валидация существования клиента
        private bool ValidateIssuedExists(string cardNumber)
        {
            string query = "SELECT Id FROM Issued WHERE №Карты = @cardNumber";
            object existClient = GeneralContext.GetElementFromDatabase(query,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            if (existClient == null)
            {
                PlayErrorSound();
                ShowMessage("Этот номер не существует");
                ClearCardNumber();
                return false;
            }

            return true;
        }

        // Обработка замороженного абонемента
        private bool TryHandleFrozenMembership(string cardNumber)
        {
            string statusQuery = "SELECT Статус FROM Issued WHERE №Карты = @cardNumber";
            object status = GeneralContext.GetElementFromDatabase(statusQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            if (status?.ToString() != "заморожен")
                return false;

            string dateQuery = "SELECT Дата_окончания FROM Issued WHERE №Карты = @cardNumber";
            object endDate = GeneralContext.GetElementFromDatabase(dateQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            if (DateTime.Compare(Convert.ToDateTime(endDate), DateTime.Now) < 0)
            {
                ShowMessage("Заморозка закончилась");
                ClearCardNumber();
                return true;
            }

            string visitsQuery = "SELECT Посещений_осталось FROM Issued WHERE №Карты = @cardNumber";
            object visitsLeft = GeneralContext.GetElementFromDatabase(visitsQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            UnfreezeMembership(cardNumber, endDate, visitsLeft);
            ShowMessage("Заморозка снята");

            return true;
        }

        // Разморозка абонемента
        private void UnfreezeMembership(string cardNumber, object endDate, object visitsLeft)
        {
            string updateIssuedQuery = @"
                UPDATE Issued SET 
                    Статус = @status,
                    Посетил = @visitDate,
                    Дата_окончания = @endDate,
                    Посещений_осталось = @visitsLeft 
                WHERE №Карты = @cardNumber";

            GeneralContext.CommandDataFromDatabase(updateIssuedQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@status", "активирован"),
                new SQLiteParameter("@visitDate", DateTime.Now.ToShortDateString()),
                new SQLiteParameter("@endDate", endDate),
                new SQLiteParameter("@visitsLeft", visitsLeft),
                new SQLiteParameter("@cardNumber", cardNumber));
        }

        // Валидация статуса абонемента
        private bool ValidateMembershipStatus(string cardNumber)
        {
            string timeLeftQuery = "SELECT Дата_окончания FROM Issued WHERE №Карты = @cardNumber";
            object timeLeft = GeneralContext.GetElementFromDatabase(timeLeftQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            DateTime expiryDate = Convert.ToDateTime(timeLeft);
            if (expiryDate.Subtract(DateTime.Now).Days < 0)
            {
                HandleExpiredMembership(cardNumber);
                return false;
            }

            return true;
        }

        // Обработка просроченного абонемента
        private void HandleExpiredMembership(string cardNumber)
        {
            IssuedMembershipContext.IssuedInfo issuedInfo = GetIssuedInfo(cardNumber);
            UpdateSellButton(issuedInfo);
            DisplayClientData(cardNumber);

            ArchiveExpiredMembership(cardNumber, issuedInfo);
            ClearCardNumber();

            PlayErrorSound();
            ShowMessage("Абонемент закончился по времени");

            ResetClientMembership(cardNumber);
        }

        // Сброс данных абонемента клиента
        private void ResetClientMembership(string cardNumber)
        {
            string resetQuery = @"DELETE FROM Issued WHERE №Карты = @cardNumber";
            GeneralContext.CommandDataFromDatabase(resetQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));
        }

        // Архивация просроченного абонемента
        private void ArchiveExpiredMembership(string cardNumber, IssuedMembershipContext.IssuedInfo issuedInfo)
        {
            string archiveQuery = @"
                INSERT INTO Archive (
                    [Клиент], [№Карты], [Дата_окончания], [Абонемент], [Оплата], [Посещений_осталось]
                ) VALUES (@client, @cardNumber, @endDate, @membership, @price, @visitsLeft)";

            using (SQLiteConnection conn = new SQLiteConnection(ArchiveServicesContext.ConnectionStringArchive()))
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(archiveQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@client", $"{issuedInfo.FullName}");
                    cmd.Parameters.AddWithValue("@cardNumber", cardNumber);
                    cmd.Parameters.AddWithValue("@endDate", issuedInfo.EndDate);
                    cmd.Parameters.AddWithValue("@membership", issuedInfo.Membership);
                    cmd.Parameters.AddWithValue("@price", issuedInfo.Price);
                    cmd.Parameters.AddWithValue("@visitsLeft", issuedInfo.VisitsLeft);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Обработка посещения клиента
        private void ProcessClientVisit(string cardNumber)
        {
            string membershipQuery = "SELECT Абонемент FROM Issued WHERE №Карты = @cardNumber";
            object membership = GeneralContext.GetElementFromDatabase(membershipQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            if (membership.ToString() == "Безлимитный")
            {
                ProcessUnlimitedVisit(cardNumber);
                return;
            }

            string visitsQuery = "SELECT Посещений_осталось FROM Issued WHERE №Карты = @cardNumber";
            object visitsLeft = GeneralContext.GetElementFromDatabase(visitsQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            int remainingVisits = Convert.ToInt32(visitsLeft);
            if (remainingVisits <= 0)
            {
                HandleNoVisitsLeft(cardNumber);
                return;
            }

            numberLeft = remainingVisits;
            ProcessLimitedVisit(cardNumber, remainingVisits);
        }

        // Обработка безлимитного посещения
        private void ProcessUnlimitedVisit(string cardNumber)
        {
            GeneralContext.CommandDataFromDatabase(@"UPDATE Issued SET " +
                "Посетил = '" + DateTime.Now.ToShortDateString() + "' " +
                "WHERE №Карты = @cardNumber",
                IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber));

            ShowMessage("Клиент отмечен");
            DisplayClientData(cardNumber);
            ClearCardNumber();
        }

        // Обработка отсутствия посещений
        private void HandleNoVisitsLeft(string cardNumber)
        {
            IssuedMembershipContext.IssuedInfo issuedInfo = GetIssuedInfo(cardNumber);
            UpdateSellButton(issuedInfo);
            DisplayClientData(cardNumber);

            ArchiveExpiredMembership(cardNumber, issuedInfo);

            PlayErrorSound();
            ShowMessage("Абонемент закончился. Посещений осталось 0");
            ClearCardNumber();
        }

        // Обработка ограниченного посещения
        private void ProcessLimitedVisit(string cardNumber, int remainingVisits)
        {
            GeneralContext.CommandDataFromDatabase(@"UPDATE Issued SET " +
                "Посещений_осталось = '" + (remainingVisits - 1).ToString() + "', " +
                "Посетил = '" + DateTime.Now.ToShortDateString() + "' " +
                "WHERE №Карты = @cardNumber",
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            ShowMessage("Клиент отмечен");
            DisplayClientData(cardNumber);

            numberCard = cardNumber;
            jeanModernButtonReturn.Visible = true;
            ClearCardNumber();
        }

        private void DisplayClientData(string cardNumber)
        {
            string query = @"
                SELECT Клиент,
                       №Карты AS 'Карта', 
                       Абонемент, 
                       Дата_окончания AS 'Дата окончания', 
                       Посещений_осталось AS 'Посещений осталось' 
                FROM Issued 
                WHERE №Карты = @cardNumber";

            dataGridViewClient.DataSource = GeneralContext.GetDataFromDatabase(query,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));
        }

        private IssuedMembershipContext.IssuedInfo GetIssuedInfo(string cardNumber)
        {
            string clientQuery = @"
                SELECT Клиент, Абонемент, Дата_окончания, Посещений_осталось 
                FROM Issued 
                WHERE №Карты = @cardNumber";

            var result = IssuedMembershipContext.GetIssuedData(clientQuery,
                new SQLiteParameter("@cardNumber", cardNumber));

            string priceQuery = "SELECT Цена FROM Descriptions WHERE Абонемент = @membership";
            object price = GeneralContext.GetElementFromDatabase(priceQuery,
                ServicesContext.ConnectionStringServices(),
                new SQLiteParameter("@membership", result.Membership));

            return new IssuedMembershipContext.IssuedInfo
            {
                FullName = result.FullName,
                Membership = result.Membership,
                EndDate = result.EndDate,
                VisitsLeft = result.VisitsLeft,
                Price = price?.ToString() ?? "0"
            };
        }

        private void jeanModernButtonPurchase_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            products.Show();
        }

        private void jeanModernButtonClients_Click(object sender, EventArgs e)
        {
            Clients clients = new Clients();
            clients.Show();
        }

        private void jeanModernButtonReport_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.Show();
        }

        private void jeanModernButtonNewMember_Click(object sender, EventArgs e)
        {
            NewClient newClient = new NewClient();
            newClient.Show();
        }

        private void jeanModernButtonSingleTicket_Click(object sender, EventArgs e)
        {
            SingleTicket singleTicket = new SingleTicket();
            singleTicket.Show();
        }

        private void jeanModernButtonChooseClient_Click(object sender, EventArgs e)
        {
            ChooseClient chooseClient = new ChooseClient();
            chooseClient.Show();
        }

        private void jeanModernButton1_Click(object sender, EventArgs e)
        {
            if (nameClient == "")
            {
                Message.MessageWindowOk("Клиент не выбран");
                return;
            }

            Services services = new Services();
            services.Show();
            services.jeanModernButtonAdd.Visible = true;
            services.jeanModernButtonAdd.Visible = false;
            services.jeanModernButtonDelete.Visible = false;
            services.jeanModernButtonChange.Visible = false;
            services.jeanModernButtonSell.Visible = true;
            services.labelName.Visible = true;
            services.jeanSoftTextBoxPurchase.Visible = true;
            services.labelName.Text = nameClient;
            services.labelNumberCard.Text = numberCard;
            services.labelNumberCard.Visible = true;
            services.checkBoxVisited.Visible = true;
        }

        private void jeanModernButtonService_Click(object sender, EventArgs e)
        {
            Services services = new Services();
            services.Show();
        }

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            IssuedMembership issued = new IssuedMembership();
            issued.Show();
        }

        private void jeanModernButtonArchive_Click(object sender, EventArgs e)
        {
            ArchiveServices archiveServices = new ArchiveServices();
            archiveServices.Show();
        }

        private void jeanModernButtonHistoryPayment_Click(object sender, EventArgs e)
        {
            HistoryPayment historyPayment = new HistoryPayment();
            historyPayment.Show();
        }

        private void jeanModernButtonDesign_Click(object sender, EventArgs e)
        {
            Design design = new Design();
            design.Show();
        }

        private void jeanModernButtonImport_Click(object sender, EventArgs e)
        {
            Import import = new Import();
            import.Show();
        }

        private void jeanModernButtonDocumentation_Click(object sender, EventArgs e)
        {
            Documentation documentation = new Documentation();
            documentation.Show();
        }

        private void jeanModernButtonReturn_Click(object sender, EventArgs e)
        {
            if (Message.MessageWindowYesNo("Вы действительно хотите отменить посещение?") != DialogResult.Yes)
                return;

            string selectQuery = @"SELECT Клиент, №Карты, Абонемент, Дата_окончания AS 'Дата окончания', Посещений_осталось AS 'Посещений осталось' FROM Issued WHERE №Карты = @numberCard";
            if (Regex.IsMatch(numberCard, @"^-?\d+(\d+)?$") || numberCard.Length == 0)
            {
                GeneralContext.CommandDataFromDatabase("UPDATE Issued SET " +
                        "Посещений_осталось = '" + numberLeft.ToString() + "' " +
                        "WHERE №Карты = '" + numberCard + "';",
                IssuedMembershipContext.ConnectionStringIssued());

                MessageBox.Show("Посещения обновлены!");


                dataGridViewClient.DataSource = GeneralContext.GetDataFromDatabase(selectQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@numberCard", numberCard));
            }
            else
            {
                string[] nameParts = Regex.Split(numberCard, @"\s+");
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                for (int i = 0; i < nameParts.Length; i++)
                {
                    if (!string.IsNullOrEmpty(nameParts[i]))
                    {
                        nameParts[i] = textInfo.ToTitleCase(nameParts[i].ToLower());
                    }
                }

                if (nameParts.Length < 2)
                {
                    MessageBox.Show("Некорректный формат данных клиента.");
                    return;
                };

                GeneralContext.CommandDataFromDatabase(@"UPDATE Issued SET " +
                        "Посещений_осталось = '" + numberLeft.ToString() + "' " +
                        "WHERE LOWER(Клиент) LIKE LOWER(@surname) || '%' || LOWER(@name) OR LOWER(Клиент) LIKE LOWER(@name) || '%' || LOWER(@surname)",
                    IssuedMembershipContext.ConnectionStringIssued(),
                        new SQLiteParameter("@surname", nameParts[0]),
                        new SQLiteParameter("@name", nameParts[1]));

                selectQuery = @"SELECT Клиент, №Карты, Абонемент, Дата_окончания AS 'Дата окончания', Посещений_осталось AS 'Посещений осталось' FROM Issued WHERE LOWER(Клиент) LIKE LOWER(@surname) || '%' || LOWER(@name) OR LOWER(Клиент) LIKE LOWER(@name) || '%' || LOWER(@surname)";
                    

                dataGridViewClient.DataSource = GeneralContext.GetDataFromDatabase(selectQuery,
                    IssuedMembershipContext.ConnectionStringIssued(), 
                    new SQLiteParameter("@surname", nameParts[0]),
                    new SQLiteParameter("@name", nameParts[1]));

                MessageBox.Show("Посещения обновлены!");
            }

            jeanModernButtonReturn.Visible = false;
        }

        private void jeanModernButtonErase_Click(object sender, EventArgs e)
        {
            jeanTextBoxNumberCard.Text = "";
        }

        private void CreateDynamicBackground()
        {
            backgroundPicture = new System.Windows.Forms.PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            backgroundPicture.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(240, 240, 245),
                    Color.FromArgb(220, 220, 230),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }

                DrawBackgroundPattern(e.Graphics);
                DrawAnimatedElements(e.Graphics);
            };

            this.Controls.Add(backgroundPicture);
            backgroundPicture.SendToBack();

            StartBackgroundAnimation();
        }

        private void DrawBackgroundPattern(Graphics g)
        {
            string[] gymIcons = { "🏋️", "🤸", "🚴", "🏃", "🧘" };

            for (int i = 0; i < 20; i++)
            {
                int x = _random.Next(this.Width);
                int y = _random.Next(this.Height);
                float size = _random.Next(20, 40);
                float opacity = _random.Next(20, 60) / 100f;

                using (var brush = new SolidBrush(Color.FromArgb((int)(opacity * 255), Color.LightGray)))
                using (var font = new System.Drawing.Font("Segoe UI Emoji", size))
                {
                    string icon = gymIcons[_random.Next(gymIcons.Length)];
                    g.DrawString(icon, font, brush, x, y);
                }
            }

            using (var pen = new Pen(Color.FromArgb(30, PrimaryBlue), 1))
            {
                for (int x = 0; x < this.Width; x += 50)
                {
                    g.DrawLine(pen, x, 0, x, this.Height);
                }
                for (int y = 0; y < this.Height; y += 50)
                {
                    g.DrawLine(pen, 0, y, this.Width, y);
                }
            }
        }

        private void DrawAnimatedElements(Graphics g)
        {
            foreach (var point in _animatedPoints)
            {
                float size = 3 + (float)Math.Sin(DateTime.Now.Millisecond * 0.01) * 2;
                using (var brush = new SolidBrush(Color.FromArgb(150, PrimaryBlue)))
                {
                    g.FillEllipse(brush, point.X - size / 2, point.Y - size / 2, size, size);
                }
            }
        }

        private void StartBackgroundAnimation()
        {
            for (int i = 0; i < 30; i++)
            {
                _animatedPoints.Add(new System.Drawing.Point(
                    _random.Next(this.Width),
                    _random.Next(this.Height)
                ));
            }

            _backgroundAnimationTimer = new System.Windows.Forms.Timer();
            _backgroundAnimationTimer.Interval = 50;
            _backgroundAnimationTimer.Tick += (s, e) =>
            {
                for (int i = 0; i < _animatedPoints.Count; i++)
                {
                    var point = _animatedPoints[i];
                    _animatedPoints[i] = new System.Drawing.Point(
                        (point.X + _random.Next(-2, 3)) % this.Width,
                        (point.Y + _random.Next(-2, 3)) % this.Height
                    );
                }

                backgroundPicture.Invalidate();
            };
            _backgroundAnimationTimer.Start();
        }

        private void CreateStaticBackground()
        {
            try
            {
                var backgroundImage = new System.Windows.Forms.PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent
                };

                // Создаем программное изображение с спортивной тематикой
                var bitmap = new Bitmap(this.Width, this.Height);
                using (var g = Graphics.FromImage(bitmap))
                using (var brush = new LinearGradientBrush(
                    new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    Color.FromArgb(245, 248, 252), // Более светлый градиент
                    Color.FromArgb(235, 238, 248),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);

                    // Добавляем абстрактные спортивные элементы
                    DrawAbstractSportsElements(g, bitmap.Width, bitmap.Height);

                    // Добавляем геометрический паттерн
                    DrawGeometricPattern(g, bitmap.Width, bitmap.Height);

                    // Добавляем легкий branding
                    DrawSubtleBranding(g, bitmap.Width, bitmap.Height);
                }

                backgroundImage.Image = bitmap;
                this.Controls.Add(backgroundImage);
                backgroundImage.SendToBack();
            }
            catch
            {
                // Резервный вариант - простой градиент
                this.Paint += (s, e) =>
                {
                    using (var brush = new LinearGradientBrush(
                        this.ClientRectangle,
                        Color.FromArgb(240, 240, 245),
                        Color.FromArgb(220, 220, 230),
                        LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(brush, this.ClientRectangle);
                    }
                };
            }
        }

        private void DrawAbstractSportsElements(Graphics g, int width, int height)
        {
            // Цвета для спортивных элементов
            var sportColors = new[]
            {
        Color.FromArgb(25, 65, 150, 220),  // Синий
        Color.FromArgb(20, 80, 200, 180),  // Бирюзовый
        Color.FromArgb(18, 140, 100, 220), // Фиолетовый
        Color.FromArgb(15, 230, 100, 80)   // Оранжевый
    };

            var random = new Random(42); // Фиксированное seed для статичности

            // Абстрактные круги (гантели, мячи)
            for (int i = 0; i < 12; i++)
            {
                int size = random.Next(40, 120);
                int x = random.Next(-size / 3, width - size * 2 / 3);
                int y = random.Next(-size / 3, height - size * 2 / 3);

                using (var brush = new SolidBrush(sportColors[random.Next(sportColors.Length)]))
                {
                    g.FillEllipse(brush, x, y, size, size);

                    // Внутренний круг для объема
                    if (size > 60)
                    {
                        using (var innerBrush = new SolidBrush(Color.FromArgb(40, Color.White)))
                        {
                            g.FillEllipse(innerBrush, x + size / 4, y + size / 4, size / 2, size / 2);
                        }
                    }
                }
            }

            // Абстрактные линии (траектории движения)
            using (var linePen = new Pen(Color.FromArgb(30, 100, 140, 200), 2f))
            {
                linePen.EndCap = LineCap.Round;

                for (int i = 0; i < 8; i++)
                {
                    int startX = random.Next(width);
                    int startY = random.Next(height);
                    int endX = startX + random.Next(-100, 100);
                    int endY = startY + random.Next(-100, 100);

                    g.DrawLine(linePen, startX, startY, endX, endY);

                    // Стрелочка на конце
                    DrawArrowhead(g, linePen, startX, startY, endX, endY);
                }
            }
        }

        private void DrawArrowhead(Graphics g, Pen pen, int startX, int startY, int endX, int endY)
        {
            float angle = (float)Math.Atan2(endY - startY, endX - startX);
            int size = 8;

            PointF[] arrowPoints =
            {
        new PointF(endX, endY),
        new PointF(endX - size * (float)Math.Cos(angle - Math.PI/6),
                  endY - size * (float)Math.Sin(angle - Math.PI/6)),
        new PointF(endX - size * (float)Math.Cos(angle + Math.PI/6),
                  endY - size * (float)Math.Sin(angle + Math.PI/6))
    };

            g.FillPolygon(new SolidBrush(pen.Color), arrowPoints);
        }

        private void DrawGeometricPattern(Graphics g, int width, int height)
        {
            // Шестиугольный паттерн (соты)
            using (var hexagonPen = new Pen(Color.FromArgb(15, 80, 120, 180), 1f))
            {
                int hexSize = 60;
                for (int y = -hexSize; y < height + hexSize; y += (int)(hexSize * 1.5))
                {
                    for (int x = -hexSize; x < width + hexSize; x += (int)(hexSize * Math.Sqrt(3)))
                    {
                        DrawHexagon(g, hexagonPen, x + ((y / hexSize) % 2 == 0 ? (int)(hexSize * Math.Sqrt(3) / 2) : 0), y, hexSize);
                    }
                }
            }

            // Точки в узлах сетки
            using (var dotBrush = new SolidBrush(Color.FromArgb(20, 120, 160, 220)))
            {
                for (int x = 30; x < width; x += 50)
                {
                    for (int y = 30; y < height; y += 50)
                    {
                        g.FillEllipse(dotBrush, x - 1, y - 1, 3, 3);
                    }
                }
            }
        }

        private void DrawHexagon(Graphics g, Pen pen, int centerX, int centerY, int size)
        {
            PointF[] points = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                double angle = 2 * Math.PI / 6 * i;
                points[i] = new PointF(
                    centerX + size * (float)Math.Cos(angle),
                    centerY + size * (float)Math.Sin(angle)
                );
            }
            g.DrawPolygon(pen, points);
        }

        private void DrawSubtleBranding(Graphics g, int width, int height)
        {
            // Логотип в углу
            using (var font = new System.Drawing.Font("Segoe UI", 24, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(40, 60, 100, 160)))
            {
                g.DrawString("СИБИРЯК", font, brush, width - 170, 30);
            }

            // Спортивные иконки по углам
            string[] sportsIcons = { "🏋️", "🤸", "🚴", "🏃" };
            using (var iconFont = new System.Drawing.Font("Segoe UI Emoji", 32))
            using (var iconBrush = new SolidBrush(Color.FromArgb(25, 80, 140, 200)))
            {
                // Левый верхний
                g.DrawString(sportsIcons[0], iconFont, iconBrush, 30, 30);
                // Правый верхний
                g.DrawString(sportsIcons[1], iconFont, iconBrush, width - 70, 30);
                // Левый нижний
                g.DrawString(sportsIcons[2], iconFont, iconBrush, 30, height - 70);
                // Правый нижний
                g.DrawString(sportsIcons[3], iconFont, iconBrush, width - 70, height - 70);
            }

            // Тонкая надпись по центру
            using (var textFont = new System.Drawing.Font("Segoe UI", 36, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.FromArgb(12, 70, 130, 190)))
            {
                string text = "ФИТНЕС КЛУБ";
                var size = g.MeasureString(text, textFont);
                g.DrawString(text, textFont, textBrush,
                    (width - size.Width) / 2,
                    Convert.ToInt32((height - size.Height) / 1.5));
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _backgroundAnimationTimer?.Stop();
            _backgroundAnimationTimer?.Dispose();
        }

        private class FloatingElement
        {
            public PointF Position { get; set; }
            public float Size { get; set; }
            public float Speed { get; set; }
            public float Angle { get; set; }
            public Color Color { get; set; }
            public float Opacity { get; set; }
            public int Type { get; set; } // 0 - круг, 1 - линия, 2 - иконка
        }

        private void CreateBackground()
        {
            backgroundPicture = new System.Windows.Forms.PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            backgroundPicture.Paint += (s, e) =>
            {
                // Основной градиентный фон
                DrawMainGradient(e.Graphics);

                // Тонкий узор
                DrawSubtlePattern(e.Graphics);

                // Плавающие элементы
                DrawFloatingElements(e.Graphics);
            };

            this.Controls.Add(backgroundPicture);
            backgroundPicture.SendToBack();

            // Инициализируем плавающие элементы
            InitializeFloatingElements();

            // Запускаем плавную анимацию
            StartCalmAnimation();
        }

        private void DrawMainGradient(Graphics g)
        {
            using (var brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(248, 248, 252), // Очень светлый сиреневый
                Color.FromArgb(240, 240, 250), // Еще светлее
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void DrawSubtlePattern(Graphics g)
        {
            // Очень тонкая сетка
            using (var pen = new Pen(Color.FromArgb(15, PrimaryBlue), 0.5f))
            {
                for (int x = 0; x < this.Width; x += 80)
                {
                    g.DrawLine(pen, x, 0, x, this.Height);
                }
                for (int y = 0; y < this.Height; y += 80)
                {
                    g.DrawLine(pen, 0, y, this.Width, y);
                }
            }

            // Еле заметные точки в углах
            using (var brush = new SolidBrush(Color.FromArgb(10, PrimaryBlue)))
            {
                int cornerSize = 150;
                g.FillEllipse(brush, -cornerSize / 2, -cornerSize / 2, cornerSize, cornerSize);
                g.FillEllipse(brush, this.Width - cornerSize / 2, -cornerSize / 2, cornerSize, cornerSize);
                g.FillEllipse(brush, -cornerSize / 2, this.Height - cornerSize / 2, cornerSize, cornerSize);
                g.FillEllipse(brush, this.Width - cornerSize / 2, this.Height - cornerSize / 2, cornerSize, cornerSize);
            }
        }

        private void InitializeFloatingElements()
        {
            _floatingElements.Clear();

            // Создаем круги
            for (int i = 0; i < 12; i++)
            {
                _floatingElements.Add(new FloatingElement
                {
                    Position = new PointF(
                        _random.Next(this.Width),
                        _random.Next(this.Height)
                    ),
                    Size = _random.Next(3, 8),
                    Speed = (float)_random.NextDouble() * 0.3f + 0.1f,
                    Angle = (float)(_random.NextDouble() * Math.PI * 2),
                    Color = Color.FromArgb(120,
                        Color.FromArgb(_random.Next(180, 220),
                        _random.Next(180, 220),
                        _random.Next(200, 240))),
                    Opacity = (float)_random.Next(30, 70) / 100f,
                    Type = 0
                });
            }

            // Создаем линии
            for (int i = 0; i < 8; i++)
            {
                _floatingElements.Add(new FloatingElement
                {
                    Position = new PointF(
                        _random.Next(this.Width),
                        _random.Next(this.Height)
                    ),
                    Size = _random.Next(20, 60),
                    Speed = (float)_random.NextDouble() * 0.2f + 0.05f,
                    Angle = (float)(_random.NextDouble() * Math.PI * 2),
                    Color = Color.FromArgb(80, PrimaryBlue),
                    Opacity = (float)_random.Next(20, 40) / 100f,
                    Type = 1
                });
            }
        }

        private void DrawFloatingElements(Graphics g)
        {
            foreach (var element in _floatingElements)
            {
                int alpha = (int)(element.Opacity * 255);
                using (var brush = new SolidBrush(Color.FromArgb(alpha, element.Color)))
                {
                    switch (element.Type)
                    {
                        case 0: // Круги
                            float circleSize = element.Size + (float)Math.Sin(DateTime.Now.Millisecond * 0.002) * 1f;
                            g.FillEllipse(brush,
                                element.Position.X - circleSize / 2,
                                element.Position.Y - circleSize / 2,
                                circleSize, circleSize);
                            break;

                        case 1: // Линии
                            float length = element.Size;
                            float endX = element.Position.X + (float)Math.Cos(element.Angle) * length;
                            float endY = element.Position.Y + (float)Math.Sin(element.Angle) * length;

                            using (var pen = new Pen(brush, 0.8f))
                            {
                                g.DrawLine(pen, element.Position.X, element.Position.Y, endX, endY);
                            }
                            break;
                    }
                }
            }
        }

        private void StartCalmAnimation()
        {
            _backgroundAnimationTimer = new System.Windows.Forms.Timer();
            _backgroundAnimationTimer.Interval = 1; // 25 FPS - плавно но не быстро
            _backgroundAnimationTimer.Tick += (s, e) =>
            {
                // Медленно обновляем позиции элементов
                foreach (var element in _floatingElements)
                {
                    // Очень медленное движение
                    float moveX = (float)Math.Cos(element.Angle) * element.Speed;
                    float moveY = (float)Math.Sin(element.Angle) * element.Speed;

                    element.Position = new PointF(
                        (element.Position.X + moveX + this.Width) % this.Width,
                        (element.Position.Y + moveY + this.Height) % this.Height
                    );

                    // Очень плавное изменение прозрачности
                    element.Opacity = 0.3f + 0.2f * (float)Math.Sin(DateTime.Now.Millisecond * 0.001 + element.Position.X * 0.01f);

                    // Медленное вращение для линий
                    if (element.Type == 1)
                    {
                        element.Angle += 0.001f;
                    }
                }

                backgroundPicture.Invalidate();
            };
            _backgroundAnimationTimer.Start();
        }

        private void CreateMinimalBackground()
        {
            backgroundPicture = new System.Windows.Forms.PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            backgroundPicture.Paint += (s, e) =>
            {
                // Градиентный фон
                using (var brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(250, 250, 255),
                    Color.FromArgb(245, 245, 250),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }

                // Еле заметные волны
                DrawSubtleWaves(e.Graphics);
            };

            this.Controls.Add(backgroundPicture);
            backgroundPicture.SendToBack();

            StartWaveAnimation();
        }

        private void DrawSubtleWaves(Graphics g)
        {
            float time = DateTime.Now.Millisecond * 0.001f;

            using (var pen = new Pen(Color.FromArgb(20, PrimaryBlue), 1f))
            {
                for (int y = 0; y < this.Height; y += 40)
                {
                    for (int x = 0; x < this.Width; x += 2)
                    {
                        float wave = (float)Math.Sin(x * 0.02f + time) * 2f;
                        g.DrawLine(pen, x, y + wave, x + 1, y + wave);
                    }
                }
            }
        }

        private void StartWaveAnimation()
        {
            _backgroundAnimationTimer = new System.Windows.Forms.Timer();
            _backgroundAnimationTimer.Interval = 100; // Очень медленно
            _backgroundAnimationTimer.Tick += (s, e) =>
            {
                backgroundPicture.Invalidate();
            };
            _backgroundAnimationTimer.Start();
        }

        private bool dragging = false;
        private System.Drawing.Point dragCursorPoint;
        private System.Drawing.Point dragFormPoint;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Y <= 40)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragging)
            {
                System.Drawing.Point dif = System.Drawing.Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = System.Drawing.Point.Add(dragFormPoint, new Size(dif));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            dragging = false;
        }
    }
}
