using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Components;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.FormsSettings;
using GymApplicationV2._0.Helpers;
using NAudio.MediaFoundation;
using NAudio.Wave;
using Shadow;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static GymApplicationV2._0.AppColors.AppColors;


namespace GymApplicationV2._0
{
    public partial class Form1 : ShadowedForm
    {
        private string nameClient = "";
        private string numberCard;
        private int numberLeft;

        private ToolStripDropDownMenu _menu_service;
        private ToolStripDropDownMenu _menu_settings;

        private BackgroundAnimation _animBackground;

        private FadeAnimation _fadeAnimation;

        private int baseButtonWidth = 150;
        private int baseSpacing = 10;
        private int baseStartX;
        private int baseButtonHeight = 40;

        string[] notChangeableTexts = new string[]
            {
                "🏋️ СИБИРЯК"
            };

        PictureBox picture_status;

        Dictionary<string, string> userStatus = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();

            InitializeCustomDesign();
            UpdateButtonLayout();

            SubscribeEvents();

            InitializeMenus();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            _animBackground = new BackgroundAnimation(this);

            ApplySettingsToAllControls();

            SetBackgroundColor();

            this.EnableDrag(this);
        }

        private void SubscribeEvents()
        {
            jeanModernButtonServices.Click += Button_Click_Services;
            jeanModernButtonSettings.Click += Button_Click_Settings;
        }

        private void InitializeMenus()
        {
            // Меню "Услуги"
            _menu_service = new ToolStripDropDownMenu();
            _menu_service.Font = new Font("Segoe UI", DataConfig.sizeFontText, FontStyle.Regular);

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

            // Меню "Настройки"
            _menu_settings = new ToolStripDropDownMenu();
            _menu_settings.Font = new Font("Segoe UI", DataConfig.sizeFontText, FontStyle.Regular);

            ToolStripMenuItem item5 = new ToolStripMenuItem("Дизайн", Properties.Resources.adjustingFont);
            ToolStripMenuItem item6 = new ToolStripMenuItem("Загрузка данных", Properties.Resources.loadData);
            ToolStripMenuItem item7 = new ToolStripMenuItem("Документация", Properties.Resources.documentation);

            _menu_settings.Items.Add(item5);
            _menu_settings.Items.Add(item6);
            _menu_settings.Items.Add(item7);

            _menu_settings.Items[0].Click += jeanModernButtonDesign_Click;
            _menu_settings.Items[1].Click += jeanModernButtonImport_Click;
            _menu_settings.Items[2].Click += jeanModernButtonDocumentation_Click;
        }

        private void UpdateButtonLayout()
        {
            this.Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.70);
            this.Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.70);

            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            int screenWidth = screenBounds.Width;

            baseStartX = this.Width;


            float scaleX = (float)screenWidth / baseStartX;

            int buttonWidth = (int)(baseButtonWidth);

            int spacing = (int)(baseSpacing * scaleX);

            int startX = this.Width - (buttonWidth * 6) - 40;
            if (screenWidth <= 1400)
            {
                startX += (int)(1.5 * buttonWidth) - 20;
                buttonWidth -= 40;
            }
            else if (screenWidth <= 1600)
            {
                startX += buttonWidth;
                buttonWidth -= 30;
            }

            int buttonHeight = baseButtonHeight;

            jeanModernButtonSettings.Width = buttonWidth;
            jeanModernButtonSettings.Height = buttonHeight;
            jeanModernButtonSettings.Location = new Point(startX, 15);

            jeanModernButtonServices.Width = buttonWidth;
            jeanModernButtonServices.Height = buttonHeight;
            jeanModernButtonServices.Location = new Point(startX + buttonWidth + spacing, 15);

            jeanModernButtonPurchase.Width = buttonWidth;
            jeanModernButtonPurchase.Height = buttonHeight;
            jeanModernButtonPurchase.Location = new Point(startX + (buttonWidth + spacing) * 2, 15);

            jeanModernButtonClients.Width = buttonWidth;
            jeanModernButtonClients.Height = buttonHeight;
            jeanModernButtonClients.Location = new Point(startX + (buttonWidth + spacing) * 3, 15);

            jeanModernButtonReport.Width = buttonWidth;
            jeanModernButtonReport.Height = buttonHeight;
            jeanModernButtonReport.Location = new Point(startX + (buttonWidth + spacing) * 4, 15);
        }

        private void InitializeCustomDesign()
        {
            this.Text = "GYM MASTER";
            this.ForeColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Padding = new Padding(20);

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
        }

        public void SetBackgroundColor()
        {
            _animBackground?.RemoveBackground();

            if (DataConfig.styleBackground == "Dynamic")
            {
                _animBackground?.CreateDynamicBackground();
            }
            else if (DataConfig.styleBackground == "Casual")
            {
                _animBackground?.CreateBackground();
            }
            else if (DataConfig.styleBackground == "Minimal")
            {
                _animBackground?.CreateMinimalBackground();
            }
            else if (DataConfig.styleBackground == "Static")
            {
                _animBackground?.CreateStaticBackground();
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

            var titleLabel = new Label
            {
                Text = "🏋️ СИБИРЯК",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                AutoSize = true
            };

            // Стилизуем кнопки навигации в синей гамме
            StyleButton(jeanModernButtonSettings, "⚙️ Настройки", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), 2, jeanModernButtonSettings.Location);
            StyleButton(jeanModernButtonServices, "🎫 Услуги", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), 2, jeanModernButtonServices.Location);
            StyleButton(jeanModernButtonPurchase, "🛒 Товары", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), 2, jeanModernButtonPurchase.Location);
            StyleButton(jeanModernButtonClients, "👥 Клиенты", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), 2, jeanModernButtonClients.Location);
            StyleButton(jeanModernButtonReport, "📊 Отчет", PrimaryBlue, White, HoverBlue, PressedBlue, new Size(120, 40), 2, jeanModernButtonReport.Location);

            navPanel.Controls.Add(titleLabel);
            navPanel.Controls.AddRange(new Control[] { jeanModernButtonSettings, jeanModernButtonServices, jeanModernButtonPurchase, jeanModernButtonClients, jeanModernButtonReport });

            navPanel.EnableDrag(this);
            this.Controls.Add(navPanel);
        }

        private void StyleButton(JeanModernButton button,
                         string text,
                         Color backColor,
                         Color foreColor,
                         Color mouseOverBackColor,
                         Color mouseDownBackColor,
                         Size size,
                         int borderSize,
                         Point point)
        {
            button.Text = text;
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Size = size;
            button.Font = new Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = borderSize;
            button.FlatAppearance.MouseOverBackColor = mouseOverBackColor;
            button.FlatAppearance.MouseDownBackColor = mouseDownBackColor;
            button.BorderSize = borderSize;
            button.Location = point;
        }

        private void CreateMainCard()
        {
            var mainCard = new JeanPanel
            {
                Size = new Size(320, 300),
                Location = new Point(80, 100),
                BackColor = White,
                GradientBottomColor = White,
                GradientTapColor = White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15),
                BorderRadius = 20,
            };

            var titleLabel = new Label
            {
                Text = "🎯 Продажи",
                Font = new Font("Segoe UI", DataConfig.sizeFontCaptions, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(20, 20),
                AutoSize = true,
                BackColor = White,
            };

            // Стилизуем кнопки продаж в оранжевой гамме
            StyleButton(jeanModernButtonNewMember, "🆕 Новый", PrimaryOrange, White, SoftOrange, Color.FromArgb(220, 120, 0), new Size(130, 40), 0, new Point(20, 75));

            StyleButton(jeanModernButtonSingleTicket, "🎫 Разовый", PrimaryOrange, White, SoftOrange, Color.FromArgb(220, 120, 0), new Size(130, 40), 0, new Point(mainCard.Width - 130 - 20, 75));

            StyleButton(jeanModernButtonSell, "💰 Продать", Color.FromArgb(220, 80, 60), White, Color.FromArgb(240, 100, 80), Color.FromArgb(200, 60, 40), new Size(160, 45), 0, new Point(mainCard.Width / 2 - 160 / 2, 140));

            StyleButton(jeanModernButtonChooseClient, "👤 Выбрать клиента", PrimaryBlue, White, LightBlue, DarkBlue, new Size(140, 50), 0, new Point(mainCard.Width / 2 - 140 / 2, mainCard.Height - 50 - 40));

            mainCard.Controls.AddRange(new Control[] { titleLabel, jeanModernButtonNewMember, jeanModernButtonSingleTicket, jeanModernButtonChooseClient, jeanModernButtonSell });

            this.Controls.Add(mainCard);
        }

        private void CreateVisitedCard()
        {
            var visitedCard = new JeanPanel
            {
                Size = new Size(420, 150),
                Location = new Point(450, 100),
                BackColor = CardBackground,
                GradientBottomColor = CardBackground,
                GradientTapColor = CardBackground,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15),
                BorderRadius = 20,
            };

            var titleLabel = new Label
            {
                Text = "👣 Посещение",
                Font = new Font("Segoe UI", DataConfig.sizeFontCaptions, FontStyle.Bold),
                ForeColor = DarkGray,
                Location = new Point(20, 20),
                AutoSize = true
            };

            jeanTextBoxNumberCard.Location = new Point(40, 60);
            jeanTextBoxNumberCard.Size = new Size(300, 40);
            jeanTextBoxNumberCard.Font = new Font("Segoe UI", DataConfig.sizeFontText, FontStyle.Bold); 
            jeanTextBoxNumberCard.BackColor = White;
            jeanTextBoxNumberCard.BorderColor = MediumGray;

            StyleButton(jeanModernButtonReturn, "↩️ Возврат", Color.FromArgb(220, 53, 69), White, Color.FromArgb(220, 220, 225), MediumGray, new Size(150, 35), 0, new Point(45, 105));

            // Стилизуем кнопку очистки
            var eraseButton = new JeanModernButton
            {
                Location = new Point(345, 65),
                Size = new Size(35, 35),
                Text = "✕",
                Font = new Font("Segoe UI", DataConfig.sizeFontText, FontStyle.Bold),
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
                Location = new Point(450, 270),
                BackColor = CardBackground,
                GradientBottomColor = CardBackground,
                GradientTapColor = CardBackground,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15),
                BorderRadius = 20,
            };

            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            int screenWidth = screenBounds.Width;

            if (screenWidth <= 1600)
            {
                clientPanel.Location = new Point(80, 420);
            }

            var titleLabel = new Label
            {
                Text = "👥 Информация о клиенте",
                Font = new Font("Segoe UI", DataConfig.sizeFontCaptions, FontStyle.Bold),
                ForeColor = DarkGray,
                Location = new Point(20, 20),
                AutoSize = true
            };

            picture_status = new PictureBox
            {
                Visible = true,
                Size = new Size(40, 35),
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(clientPanel.Width - 40 - 10, 10)
            };

            // Стилизуем DataGridView с новой цветовой схемой
            dataGridViewClient.Location = new Point(20, 60);
            dataGridViewClient.Size = new Size(750, 2 * jeanModernButtonSettings.Width);
            dataGridViewClient.BackgroundColor = White;
            dataGridViewClient.BorderStyle = BorderStyle.None;
            dataGridViewClient.EnableHeadersVisualStyles = false;

            // Заголовки столбцов - MediumSlateBlue
            dataGridViewClient.ColumnHeadersDefaultCellStyle.BackColor = PrimaryBlue;
            dataGridViewClient.ColumnHeadersDefaultCellStyle.ForeColor = White;
            dataGridViewClient.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", DataConfig.sizeFontTables, FontStyle.Bold);
            dataGridViewClient.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Основные ячейки
            dataGridViewClient.DefaultCellStyle.Font = new Font("Segoe UI", DataConfig.sizeFontTables - 2);
            dataGridViewClient.DefaultCellStyle.BackColor = White;
            dataGridViewClient.DefaultCellStyle.ForeColor = DarkGray;
            dataGridViewClient.DefaultCellStyle.SelectionBackColor = SoftSlateBlue;
            dataGridViewClient.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Чередующиеся строки
            dataGridViewClient.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 252);

            clientPanel.Controls.Add(picture_status);
            clientPanel.Controls.Add(titleLabel);
            clientPanel.Controls.Add(dataGridViewClient);

            this.Controls.Add(clientPanel);
        }

        private void Button_Click_Services(object sender, EventArgs e)
        {
            _menu_service.Show(jeanModernButtonServices, new Point(0, jeanModernButtonServices.Height));
        }

        private void Button_Click_Settings(object sender, EventArgs e)
        {
            _menu_settings.Show(jeanModernButtonSettings, new Point(0, jeanModernButtonSettings.Height));
        }


        private void ApplyFormStyle()
        {
            JeanFormStyle.fStyle style;
            if (DataConfig.styleForm == "UserStyle")
            {
                style = JeanFormStyle.fStyle.UserStyle;
            }
            else if (DataConfig.styleForm == "SimpleDark")
            {
                style = JeanFormStyle.fStyle.SimpleDark;
            }
            else if (DataConfig.styleForm == "TelegramStyle")
            {
                style = JeanFormStyle.fStyle.TelegramStyle;
            }
            else
            {
                style = JeanFormStyle.fStyle.None;
            }

            jeanFormStyle.FormStyle = style;
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

        private WaveOutEvent outputDevice;
        private MediaFoundationReader audioFile;
        private void PlayErrorSound()
        {
            string soundPath = Properties.Settings.Default.ErrorSoundPath;

            if (!string.IsNullOrEmpty(soundPath) && File.Exists(soundPath))
            {
                try
                {
                    MediaFoundationApi.Startup();

                    StopErrorSound();

                    outputDevice = new WaveOutEvent();

                    audioFile = new MediaFoundationReader(soundPath);
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                }
                catch (Exception ex)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show($"Не удалось воспроизвести звук ошибки: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    StopErrorSound();
                }
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private void StopErrorSound()
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            outputDevice = null;
            audioFile?.Dispose();
            audioFile = null;
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

        private void UpdateDataGrid()
        {
            dataGridViewClient.DataSource = null;
            dataGridViewClient.Rows.Clear();
            dataGridViewClient.Refresh();
            picture_status.Image = Properties.Resources.redError;
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
            ClearCardNumber();

            if (!ValidateIssuedExists(cardNumber))
                return;

            DisplayClientData(cardNumber);
            if (!ValidateMembershipStatus(cardNumber))
                return;

            if (TryHandleFrozenMembership(cardNumber))
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
                if (!userStatus.ContainsKey(cardNumber))
                {
                    userStatus.Add(cardNumber, "Этого номера нет в действительных абонементах");
                }
                else
                {
                    userStatus[cardNumber] = "Этого номера нет в действительных абонементах (Повторно)";
                }

                UpdateDataGrid();
                ShowMessage("Этого номера нет в действительных абонементах");
                return false;
            }

            return true;
        }

        // Обработка замороженного абонемента
        private bool TryHandleFrozenMembership(string cardNumber)
        {
            object status = GeneralContext.GetElementFromDatabase("SELECT Статус FROM Issued WHERE №Карты = @cardNumber",
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            if (status?.ToString() != "заморожен")
                return false;

            UnfreezeMembership(cardNumber);

            if (!userStatus.ContainsKey(cardNumber))
            {
                userStatus.Add(cardNumber, "Заморозка снята");
            }
            else
            {
                userStatus[cardNumber] = "Заморозка снята (Повторно)";
            }

            return false;
        }

        // Разморозка абонемента
        private void UnfreezeMembership(string cardNumber)
        {
            string updateIssuedQuery = @"
                UPDATE Issued SET 
                    Статус = @status,
                WHERE №Карты = @cardNumber";

            GeneralContext.CommandDataFromDatabase(updateIssuedQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@status", "активирован"),
                new SQLiteParameter("@cardNumber", cardNumber));

            ShowMessage("Заморозка снята");
        }

        // Валидация статуса абонемента
        private bool ValidateMembershipStatus(string cardNumber)
        {
            object timeLeft = GeneralContext.GetElementFromDatabase("SELECT Дата_окончания FROM Issued WHERE №Карты = @cardNumber",
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            if (DateTime.Compare(Convert.ToDateTime(timeLeft), DateTime.Now) < 0)
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

            ArchiveExpiredMembership(cardNumber, issuedInfo);

            ResetClientMembership(cardNumber);

            PlayErrorSound();
            if (!userStatus.ContainsKey(cardNumber))
            {
                userStatus.Add(cardNumber, "Абонемент закончился по времени");
            }
            else
            {
                userStatus[cardNumber] = "Абонемент закончился по времени (Повторно)";
            }
            UpdateDataGrid();
            ShowMessage("Абонемент закончился по времени");
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
                "Посетил = '" + DateTime.Now + "' " +
                "WHERE №Карты = @cardNumber",
                IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber));

            if (!userStatus.ContainsKey(cardNumber))
            {
                userStatus.Add(cardNumber, "Активен");
            }
            else
            {
                userStatus[cardNumber] = "Активен (Повторно)";
            }

            picture_status.Image = Properties.Resources.greenSuccess;
        }

        // Обработка отсутствия посещений
        private void HandleNoVisitsLeft(string cardNumber)
        {
            IssuedMembershipContext.IssuedInfo issuedInfo = GetIssuedInfo(cardNumber);
            UpdateSellButton(issuedInfo);

            ArchiveExpiredMembership(cardNumber, issuedInfo);

            PlayErrorSound();
            if (!userStatus.ContainsKey(cardNumber))
            {
                userStatus.Add(cardNumber, "Абонемент закончился. Посещений 0");
            }
            else
            {
                userStatus[cardNumber] = "Абонемент закончился. Посещений 0 (Повторно)";
            }

            UpdateDataGrid();
            ShowMessage("Абонемент закончился. Посещений 0");
        }

        // Обработка ограниченного посещения
        private void ProcessLimitedVisit(string cardNumber, int remainingVisits)
        {
            GeneralContext.CommandDataFromDatabase(@"UPDATE Issued SET " +
                "Посещений_осталось = '" + (remainingVisits - 1).ToString() + "', " +
                "Посетил = '" + DateTime.Now + "' " +
                "WHERE №Карты = @cardNumber",
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@cardNumber", cardNumber));

            if (!userStatus.ContainsKey(cardNumber))
            {
                userStatus.Add(cardNumber, "Активен");
            }
            else
            {
                userStatus[cardNumber] = "Активен (Повторно)";
            }

            numberCard = cardNumber;
            jeanModernButtonReturn.Visible = true;

            picture_status.Image = Properties.Resources.greenSuccess;
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
            products.ShowDialog();
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

        private void ApplySettingsToAllControls()
        {
            FontHelper.ApplyFontSettings(this, notChangeableTexts);

            ApplyFormStyle();
        }

        private void ShowOrActivateForm<T>() where T : Form, new()
        {
            T existingForm = Application.OpenForms.OfType<T>().FirstOrDefault();

            if (existingForm != null && !existingForm.IsDisposed)
            {
                existingForm.WindowState = FormWindowState.Normal;
                existingForm.BringToFront();
                existingForm.Focus();
            }
            else
            {
                T newForm = new T();
                newForm.Show();
            }
        }

        private void jeanModernButtonClients_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<Clients>();
        }

        private void jeanModernButtonReport_Click(object sender, EventArgs e)
        {
            using (var report = new Report())
            {
                report.userStatus = userStatus;
                report.ShowDialog();
            }
        }

        private void jeanModernButtonNewMember_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<NewClient>();
        }

        private void jeanModernButtonSingleTicket_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<SingleTicket>();
        }

        private void jeanModernButtonChooseClient_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<ChooseClient>();
        }

        private void jeanModernButtonService_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<Services>();
        }

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<IssuedMembership>();
        }

        private void jeanModernButtonArchive_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<ArchiveServices>();
        }

        private void jeanModernButtonHistoryPayment_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<HistoryPayment>();
        }

        private void ApplyAllSettings()
        {
            ApplySettingsToAllControls();
            //SetBackgroundColor();
            //_animBackground?.RemoveBackground();

            UpdateDataGridViewFont(dataGridViewClient);
        }

        private void UpdateDataGridViewFont(DataGridView dataGrid)
        {
            string sortColumn = dataGrid.SortedColumn?.Name;
            SortOrder sortOrder = dataGrid.SortOrder;

            dataGrid.Font = new Font(dataGrid.Font.FontFamily, DataConfig.sizeFontTables, dataGrid.Font.Style);

            dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(
                dataGrid.ColumnHeadersDefaultCellStyle.Font.FontFamily,
                DataConfig.sizeFontTables,
                dataGrid.ColumnHeadersDefaultCellStyle.Font.Style
            );

            dataGrid.DefaultCellStyle.Font = new Font(
                dataGrid.DefaultCellStyle.Font.FontFamily,
                DataConfig.sizeFontTables - 1,
                dataGrid.DefaultCellStyle.Font.Style
            );

            dataGrid.RowHeadersDefaultCellStyle.Font = new Font(
                dataGrid.RowHeadersDefaultCellStyle.Font.FontFamily,
                DataConfig.sizeFontTables - 1,
                dataGrid.RowHeadersDefaultCellStyle.Font.Style
            );

            dataGrid.Refresh();
            dataGrid.Invalidate();

            if (!string.IsNullOrEmpty(sortColumn) && dataGrid.Columns.Contains(sortColumn))
            {
                var direction = sortOrder == SortOrder.Ascending ?
                    System.ComponentModel.ListSortDirection.Ascending :
                    System.ComponentModel.ListSortDirection.Descending;

                dataGrid.Sort(dataGrid.Columns[sortColumn], direction);
            }

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Height = dataGrid.RowTemplate.Height;
                }
            }

            if (dataGrid.DataSource is BindingSource bindingSource)
            {
                bindingSource.ResetBindings(false);
            }

            Application.DoEvents();
        }

        private void jeanModernButtonDesign_Click(object sender, EventArgs e)
        {
            Design design = new Design();
            design.SetRefreshAction(ApplyAllSettings);
            design.ShowDialog();
        }

        private void jeanModernButtonImport_Click(object sender, EventArgs e)
        {
            Import import = new Import();
            import.ShowDialog();
        }

        private void jeanModernButtonDocumentation_Click(object sender, EventArgs e)
        {
            Documentation documentation = new Documentation();
            documentation.ShowDialog();
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
                }
                ;

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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _animBackground?.RemoveBackground();
            _animBackground = null;
        }
    }
}
