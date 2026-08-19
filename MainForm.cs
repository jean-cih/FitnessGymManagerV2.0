using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Components;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.FormsSettings;
using GymApplicationV2._0.Helpers;
using GymApplicationV2._0.Helpers.GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static GymApplicationV2._0.AppColors.AppColors;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;


namespace GymApplicationV2._0
{
    public partial class MainForm : ShadowedForm
    {
        private string nameClient = string.Empty;
        private string numberCard = string.Empty;
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

        private DatabaseBackupManager _backupManager;

        public MainForm()
        {
            try
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

                Logger.Info("Запуск основного окна MainForm");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации MainForm", ex);
                throw;
            }
        }

        private void SubscribeEvents()
        {
            jeanModernButtonServices.Click += Button_Click_Services;
            jeanModernButtonSettings.Click += Button_Click_Settings;
        }

        private void InitializeMenus()
        {
            try
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

                Logger.Info("Меню инициализированы");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeMenus", ex);
            }
        }

        private void UpdateButtonLayout()
        {
            try
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

                Logger.Info($"Размеры кнопок обновлены: ширина={buttonWidth}, высота={buttonHeight}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в UpdateButtonLayout", ex);
            }
        }

        private void InitializeCustomDesign()
        {
            try
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

                Logger.Info("Дизайн MainForm инициализирован");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeCustomDesign", ex);
            }
        }

        public void SetBackgroundColor()
        {
            try
            {
                _animBackground?.RemoveBackground();

                if (DataConfig.styleBackground == "Dynamic")
                {
                    _animBackground?.CreateDynamicBackground();
                    Logger.Info("Установлен динамический фон");
                }
                else if (DataConfig.styleBackground == "Casual")
                {
                    _animBackground?.CreateBackground();
                    Logger.Info("Установлен обычный фон");
                }
                else if (DataConfig.styleBackground == "Minimal")
                {
                    _animBackground?.CreateMinimalBackground();
                    Logger.Info("Установлен минималистичный фон");
                }
                else if (DataConfig.styleBackground == "Static")
                {
                    _animBackground?.CreateStaticBackground();
                    Logger.Info("Установлен статический фон");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в SetBackgroundColor", ex);
            }
        }

        private void CreateNavigationPanel()
        {
            try
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

                Logger.Info("Панель навигации создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreateNavigationPanel", ex);
            }
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
            try
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
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в StyleButton для {button?.Name ?? "unknown"}", ex);
            }
        }

        private void CreateMainCard()
        {
            try
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

                Logger.Info("Главная карточка продаж создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreateMainCard", ex);
            }
        }

        private void CreateVisitedCard()
        {
            try
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

                Logger.Info("Карточка посещения создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreateVisitedCard", ex);
            }
        }

        private void ClickErase()
        {
            try
            {
                jeanTextBoxNumberCard.Text = "";
                Logger.Info("Очищено поле номера карты");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ClickErase", ex);
            }
        }

        private void CreateClientPanel()
        {
            try
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

                Logger.Info("Панель клиента создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreateClientPanel", ex);
            }
        }

        private void Button_Click_Services(object sender, EventArgs e)
        {
            try
            {
                _menu_service.Show(jeanModernButtonServices, new Point(0, jeanModernButtonServices.Height));
                Logger.Info("Открыто меню услуг");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в Button_Click_Services", ex);
            }
        }

        private void Button_Click_Settings(object sender, EventArgs e)
        {
            try
            {
                _menu_settings.Show(jeanModernButtonSettings, new Point(0, jeanModernButtonSettings.Height));
                Logger.Info("Открыто меню настроек");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в Button_Click_Settings", ex);
            }
        }

        private void ApplyFormStyle()
        {
            try
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
                Logger.Info($"Применен стиль формы: {DataConfig.styleForm}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ApplyFormStyle", ex);
            }
        }

        private bool isProgrammaticChange = false;

        private void jeanTextBoxNumberCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                isProgrammaticChange = true;

                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (Regex.IsMatch(jeanTextBoxNumberCard.Text, @"^-?\d+(\d+)?$") || jeanTextBoxNumberCard.Text.Length == 0)
                    {
                        numberCard = jeanTextBoxNumberCard.Text.Trim();
                        ClearCardNumber();

                        Logger.Info($"Обработка карты: {numberCard}");
                        ProcessMembership(numberCard);
                    }
                    else
                    {
                        string[] names = jeanTextBoxNumberCard.Text.Split(' ');

                        if (names == null || names.Length == 0) return;

                        var searchQuery = BuildSearchQuery(names);
                        var (card, clientName) = DuplicateResolution(searchQuery, names);

                        nameClient = clientName;
                        jeanModernButtonSell.Text = $"💰 Продать\n{nameClient}";

                        if (card == "" && names.Length >= 2)
                        {
                            string query = $@"SELECT №Карты
                                FROM Archive 
                                WHERE Клиент LIKE '%{names[0]}%' 
                                AND Клиент LIKE '%{names[1]}%'";

                            object archiveClientNumber = GeneralContext.GetElementFromDatabase(query,
                            ArchiveServicesContext.ConnectionStringArchive());

                            if (archiveClientNumber != null)
                            {
                                numberCard = archiveClientNumber.ToString();
                                Logger.Info($"Найден клиент в архиве: {numberCard}");
                            }

                            var errorSound = new PlaySoundHelper(false);
                            errorSound.PlaySound();

                            UpdateDataGrid();

                            return;
                        }

                        numberCard = card;
                        ClearCardNumber();

                        Logger.Info($"Обработка карты после поиска: {numberCard}");
                        ProcessMembership(numberCard);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в jeanTextBoxNumberCard_KeyPress для {jeanTextBoxNumberCard?.Text}", ex);
            }
        }

        private (string SelectedCardNumber, string ClientName) DuplicateResolution(string query, string[] names)
        {
            try
            {
                var data = GeneralContext.GetDataFromDatabase(query,
                            IssuedMembershipContext.ConnectionStringIssued());

                if (data is null || data.Rows.Count == 0) return (string.Empty, string.Empty);

                if (data.Rows.Count == 1) return (data.Rows[0]["№Карты"].ToString(), string.Join(" ", names));

                Logger.Info($"Найдено {data.Rows.Count} дубликатов, открыта форма разрешения");

                using (DuplicateResolution duplicate = new DuplicateResolution(data))
                {
                    duplicate.ShowDialog();
                    return (duplicate.SelectedCardNumber, duplicate.SelectedClient);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в DuplicateResolution для запроса: {query}", ex);
                return (string.Empty, string.Empty);
            }
        }

        private string BuildSearchQuery(string[] names)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в BuildSearchQuery для {string.Join(",", names)}", ex);
                return string.Empty;
            }
        }

        private string BuildFullNameSearchQuery(string[] names)
        {
            try
            {
                var validNames = names.Where(n => !string.IsNullOrWhiteSpace(n)).ToArray();

                if (validNames.Length == 0)
                    return string.Empty;

                var conditions = validNames.Select(name => $"Клиент LIKE '%{name}%'");
                string whereClause = string.Join(" AND ", conditions);

                return $@"
                    SELECT DISTINCT №Карты
                    FROM Issued 
                    WHERE {whereClause}
                    AND №Карты IS NOT NULL
                    AND №Карты != ''";
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в BuildFullNameSearchQuery для {string.Join(",", names)}", ex);
                return string.Empty;
            }
        }

        private void ClearCardNumber()
        {
            try
            {
                jeanTextBoxNumberCard.Text = "";
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ClearCardNumber", ex);
            }
        }

        private void UpdateSellButton(IssuedMembershipContext.IssuedInfo issuedInfo)
        {
            try
            {
                if (issuedInfo != null)
                {
                    nameClient = issuedInfo.FullName;
                    jeanModernButtonSell.Text = $"💰 Продать\n{nameClient}";
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в UpdateSellButton", ex);
            }
        }

        private void UpdateDataGrid()
        {
            try
            {
                dataGridViewClient.DataSource = null;
                dataGridViewClient.Rows.Clear();
                dataGridViewClient.Refresh();
                picture_status.Image = Properties.Resources.redError;
                Logger.Info("DataGridView очищен");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в UpdateDataGrid", ex);
            }
        }

        private void ProcessMembership(string card)
        {
            try
            {
                if (!ValidateIssuedExists(card)) return;

                string query = @"SELECT Абонемент, Дата_окончания FROM Issued 
                    WHERE №Карты = @cardNumber
                    ORDER BY date(Дата_окончания) ASC
                    LIMIT 1";
                var data = GeneralContext.GetDataFromDatabase(query,
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", card));

                if (data == null || data.Rows.Count == 0) return;

                string membership = data.Rows[0]["Абонемент"].ToString();
                string date = data.Rows[0]["Дата_окончания"].ToString();

                checkDelayStartDate(date, card);

                if (!ValidateMembershipStatus(card, membership)) return;

                TryHandleFrozenMembership(card, date);

                ProcessClientVisit(card, membership);

                DisplayClientData(card);

                Logger.Info($"Обработан абонемент для карты {card}, услуга: {membership}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ProcessMembership для карты {card}", ex);
            }
        }

        private void checkDelayStartDate(string date, string card)
        {
            try
            {
                DateTime endDate = DateTime.Parse(date);

                var res = GeneralContext.GetDataFromDatabase(@"SELECT Дата_начала FROM History 
                    WHERE Дата_окончания = @dateEnd",
                    HistoryPaymentContext.ConnectionStringPayment(),
                    new SQLiteParameter("@dateEnd", endDate));

                if (res == null || res.Rows.Count == 0) return;

                DateTime startDate = DateTime.Parse(res.Rows[0]["Дата_начала"].ToString());
                DateTime currentDate = DateTime.Now.Date;

                if (startDate > currentDate)
                {
                    int daysDifference = (startDate - currentDate).Days;

                    DateTime newEndDate = endDate.AddDays(-daysDifference + 1);

                    date = newEndDate.ToString("yyyy-MM-dd");

                    GeneralContext.CommandDataFromDatabase(@"UPDATE Issued 
                        SET Дата_окончания = @newEndDate 
                        WHERE №Карты = @cardNumber 
                        AND Дата_окончания = @oldEndDate",
                        IssuedMembershipContext.ConnectionStringIssued(),
                        new SQLiteParameter("@newEndDate", newEndDate.ToString("yyyy-MM-dd")),
                        new SQLiteParameter("@cardNumber", card),
                        new SQLiteParameter("@oldEndDate", date));

                    Logger.Info($"Скорректирована дата окончания для карты {card}, новая дата: {newEndDate:yyyy-MM-dd}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в checkDelayStartDate для карты {card}", ex);
            }
        }

        private void textNumberClient_TextChanged(object sender, EventArgs e)
        {
            try
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

                numberCard = jeanTextBoxNumberCard.Text.Trim();
                ClearCardNumber();

                Logger.Info($"Обработка карты из TextChanged: {numberCard}");
                ProcessMembership(numberCard);
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в textNumberClient_TextChanged для {jeanTextBoxNumberCard?.Text}", ex);
            }
        }

        // Валидация существования клиента
        private bool ValidateIssuedExists(string cardNumber)
        {
            try
            {
                string query = "SELECT Id FROM Issued WHERE №Карты = @cardNumber";
                object existClient = GeneralContext.GetElementFromDatabase(query,
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber));

                if (existClient == null)
                {
                    var errorSound = new PlaySoundHelper(false);
                    errorSound.PlaySound();

                    UpdateDataGrid();
                    Logger.Info($"{cardNumber} - этого номера нет в действительных абонементах");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ValidateIssuedExists для карты {cardNumber}", ex);
                return false;
            }
        }

        // Обработка замороженного абонемента
        private void TryHandleFrozenMembership(string cardNumber, string date)
        {
            try
            {
                object status = GeneralContext.GetElementFromDatabase("SELECT Статус FROM Issued WHERE №Карты = @cardNumber AND Дата_окончания = @endDate",
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber),
                    new SQLiteParameter("@endDate", date));

                if (status?.ToString() != "заморожен")
                    return;

                UnfreezeMembership(cardNumber, date);

                Logger.Info($"{cardNumber} - заморозка снята");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в TryHandleFrozenMembership для карты {cardNumber}", ex);
            }
        }

        // Разморозка абонемента
        private void UnfreezeMembership(string cardNumber, string date)
        {
            try
            {
                object timeLeft = GeneralContext.GetElementFromDatabase("SELECT Окончание_заморозки FROM Issued WHERE №Карты = @cardNumber AND Дата_окончания = @endDate",
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber),
                    new SQLiteParameter("@endDate", date));

                DateTime endDate = Convert.ToDateTime(timeLeft);
                int daysLeft = (int)(endDate - DateTime.Now).TotalDays + 1;

                if (daysLeft > 0)
                {
                    string updateIssuedQueryDate = @"
                        UPDATE Issued SET 
                            Дата_окончания = date(Дата_окончания, '-' || @daysLeftPlusOne || ' days'),
                            Статус = @status,
                            Окончание_заморозки = @stopFreeze
                        WHERE №Карты = @cardNumber";

                    GeneralContext.CommandDataFromDatabase(updateIssuedQueryDate,
                        IssuedMembershipContext.ConnectionStringIssued(),
                        new SQLiteParameter("@daysLeftPlusOne", daysLeft),
                        new SQLiteParameter("@status", "активирован"),
                        new SQLiteParameter("@stopFreeze", DBNull.Value),
                        new SQLiteParameter("@cardNumber", cardNumber));

                    Logger.Info($"{cardNumber} - разморожен, осталось дней: {daysLeft}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в UnfreezeMembership для карты {cardNumber}", ex);
            }
        }

        // Валидация статуса абонемента
        private bool ValidateMembershipStatus(string cardNumber, string membership)
        {
            try
            {
                object timeLeft = GeneralContext.GetElementFromDatabase(@"SELECT Дата_окончания FROM Issued WHERE №Карты = @cardNumber
                    ORDER BY date(Дата_окончания) ASC
                    LIMIT 1",
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber));

                if (DateTime.Compare(Convert.ToDateTime(timeLeft).Date, DateTime.Now.Date) < 0)
                {
                    HandleExpiredMembership(cardNumber, timeLeft.ToString());

                    ProcessMembership(numberCard);
                    return false;
                }

                var term_membership = GeneralContext.GetElementFromDatabase(@"SELECT Срок_действия 
                            FROM Descriptions 
                            WHERE Абонемент = @membership",
                    ServicesContext.ConnectionStringServices(),
                    new SQLiteParameter("@membership", membership));

                DateTime endDate = Convert.ToDateTime(timeLeft);
                int termMonths = Convert.ToInt32(term_membership);

                DateTime startDate = endDate.AddMonths(-termMonths);

                if (DateTime.Compare(startDate.Date, DateTime.Now.Date) > 0)
                {
                    int daysDifference = (startDate - DateTime.Now).Days;

                    DateTime newEndDate = endDate.AddDays(-daysDifference - 1);

                    string newEndDateStr = newEndDate.ToString("yyyy-MM-dd");
                    string oldEndDateStr = endDate.ToString("yyyy-MM-dd");

                    GeneralContext.CommandDataFromDatabase(
                        @"UPDATE Issued SET 
                            Дата_окончания = @newEndDate 
                          WHERE №Карты = @cardNumber 
                            AND Дата_окончания = @oldEndDate",
                        IssuedMembershipContext.ConnectionStringIssued(),
                        new SQLiteParameter("@newEndDate", newEndDateStr),
                        new SQLiteParameter("@cardNumber", cardNumber),
                        new SQLiteParameter("@oldEndDate", oldEndDateStr));

                    Logger.Info($"Скорректирована дата окончания для карты {cardNumber}: {newEndDateStr}");
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ValidateMembershipStatus для карты {cardNumber}", ex);
                return false;
            }
        }

        // Обработка просроченного абонемента
        private void HandleExpiredMembership(string cardNumber, string date)
        {
            try
            {
                DisplayClientData(cardNumber);
                picture_status.Image = Properties.Resources.redError;

                IssuedMembershipContext.IssuedInfo issuedInfo = GetIssuedInfo(cardNumber, date);
                UpdateSellButton(issuedInfo);

                ArchiveExpiredMembership(cardNumber, issuedInfo);

                ResetClientMembership(cardNumber, date);

                Logger.Info($"Абонемент для карты {cardNumber} закончился по времени");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в HandleExpiredMembership для карты {cardNumber}", ex);
            }
        }

        // Сброс данных абонемента клиента
        private void ResetClientMembership(string cardNumber, string date)
        {
            try
            {
                string resetQuery = @"DELETE FROM Issued WHERE №Карты = @cardNumber AND Дата_окончания = @endDate";
                GeneralContext.CommandDataFromDatabase(resetQuery,
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber),
                    new SQLiteParameter("@endDate", date));

                Logger.Info($"Данные абонемента для карты {cardNumber} сброшены");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ResetClientMembership для карты {cardNumber}", ex);
            }
        }

        // Архивация просроченного абонемента
        private void ArchiveExpiredMembership(string cardNumber, IssuedMembershipContext.IssuedInfo issuedInfo)
        {
            try
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
                        Logger.Info($"Абонемент для карты {cardNumber} заархивирован");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ArchiveExpiredMembership для карты {cardNumber}", ex);
            }
        }

        // Обработка посещения клиента
        private void ProcessClientVisit(string cardNumber, string membership)
        {
            try
            {
                var date = GeneralContext.GetElementFromDatabase(@"SELECT Дата_окончания 
                    FROM Issued 
                    WHERE №Карты = @cardNumber 
                    ORDER BY date(Дата_окончания) ASC
                    LIMIT 1",
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber));

                string new_date_after_unfreeze = date.ToString();

                var type_membership = GeneralContext.GetElementFromDatabase(@"SELECT Тип 
                    FROM Descriptions 
                    WHERE Абонемент = @membership",
                    ServicesContext.ConnectionStringServices(),
                    new SQLiteParameter("@membership", membership));

                if (type_membership.ToString() == "безлимитный")
                {
                    ProcessUnlimitedVisit(cardNumber, new_date_after_unfreeze);
                    Logger.Info($"Безлимитное посещение для карты {cardNumber}");
                    return;
                }

                string visitsQuery = "SELECT Посещений_осталось FROM Issued WHERE №Карты = @cardNumber AND Дата_окончания = @dateEnd";
                object visitsLeft = GeneralContext.GetElementFromDatabase(visitsQuery,
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber),
                    new SQLiteParameter("@dateEnd", new_date_after_unfreeze));

                numberLeft = SafeParseInt(visitsLeft);
                if (numberLeft <= 0)
                {
                    Logger.Info($"Нет посещений для карты {cardNumber}, осталось: {numberLeft}");
                    HandleNoVisitsLeft(cardNumber, new_date_after_unfreeze);

                    ProcessMembership(cardNumber);
                    return;
                }

                ProcessLimitedVisit(cardNumber, numberLeft, new_date_after_unfreeze);
                Logger.Info($"Обработано посещение для карты {cardNumber}, осталось: {numberLeft - 1}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ProcessClientVisit для карты {cardNumber}", ex);
            }
        }

        private int SafeParseInt(object value, int defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            try
            {
                string strValue = value.ToString().Trim();

                if (string.IsNullOrEmpty(strValue))
                    return defaultValue;

                string cleanValue = new string(strValue.Where(c => char.IsDigit(c) || c == '-').ToArray());

                if (string.IsNullOrEmpty(cleanValue))
                    return defaultValue;

                if (int.TryParse(cleanValue, out int result))
                    return result;

                return defaultValue;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в SafeParseInt для значения {value}", ex);
                return defaultValue;
            }
        }

        // Обработка безлимитного посещения
        private void ProcessUnlimitedVisit(string cardNumber, string date)
        {
            try
            {
                GeneralContext.CommandDataFromDatabase(@"UPDATE Issued SET " +
                    "Посетил = '" + DateTime.Now + "' " +
                    "WHERE №Карты = @cardNumber " +
                    "AND Дата_окончания = @dateEnd",
                    IssuedMembershipContext.ConnectionStringIssued(),
                        new SQLiteParameter("@cardNumber", cardNumber),
                        new SQLiteParameter("@dateEnd", date));

                Logger.Info($"Безлимитный абонемент для карты {cardNumber} | {date} - активен");

                var successSound = new PlaySoundHelper();
                successSound.PlaySound();

                picture_status.Image = Properties.Resources.greenSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ProcessUnlimitedVisit для карты {cardNumber}", ex);
            }
        }

        // Обработка отсутствия посещений
        private void HandleNoVisitsLeft(string cardNumber, string date)
        {
            try
            {
                DisplayClientData(cardNumber);
                picture_status.Image = Properties.Resources.redError;

                IssuedMembershipContext.IssuedInfo issuedInfo = GetIssuedInfo(cardNumber, date);
                UpdateSellButton(issuedInfo);

                ArchiveExpiredMembership(cardNumber, issuedInfo);

                ResetClientMembership(cardNumber, date);

                var errorSound = new PlaySoundHelper(false);
                errorSound.PlaySound();

                Logger.Info($"Абонемент для карты {cardNumber} закончился. Посещений 0");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в HandleNoVisitsLeft для карты {cardNumber}", ex);
            }
        }

        // Обработка ограниченного посещения
        private void ProcessLimitedVisit(string cardNumber, int remainingVisits, string date)
        {
            try
            {
                GeneralContext.CommandDataFromDatabase(@"UPDATE Issued SET " +
                    "Посещений_осталось = '" + (remainingVisits - 1).ToString() + "', " +
                    "Посетил = '" + DateTime.Now + "' " +
                    "WHERE №Карты = @cardNumber " +
                    "AND Дата_окончания = @endDate",
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber),
                    new SQLiteParameter("@endDate", date));

                Logger.Info($"Ограниченный абонемент для карты {cardNumber} | {date} - активен, осталось: {remainingVisits - 1}");

                jeanModernButtonReturn.Visible = true;

                var successSound = new PlaySoundHelper();
                successSound.PlaySound();

                picture_status.Image = Properties.Resources.greenSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ProcessLimitedVisit для карты {cardNumber}", ex);
            }
        }

        private void DisplayClientData(string cardNumber)
        {
            try
            {
                string query = @"
            SELECT Клиент,
                   №Карты AS 'Карта', 
                   Абонемент, 
                   Дата_окончания AS 'Дата окончания', 
                   Посещений_осталось AS 'Посещений осталось' 
            FROM Issued 
            WHERE №Карты = @cardNumber
            ORDER BY date(Дата_окончания) ASC
            LIMIT 1";

                dataGridViewClient.DataSource = GeneralContext.GetDataFromDatabase(query,
                    IssuedMembershipContext.ConnectionStringIssued(),
                    new SQLiteParameter("@cardNumber", cardNumber));

                Logger.Info($"Данные клиента для карты {cardNumber} отображены");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в DisplayClientData для карты {cardNumber}", ex);
            }
        }

        private IssuedMembershipContext.IssuedInfo GetIssuedInfo(string cardNumber, string date)
        {
            try
            {
                string clientQuery = @"
            SELECT Клиент, Дата_окончания, Абонемент, Посещений_осталось 
            FROM Issued 
            WHERE №Карты = @cardNumber 
            AND Дата_окончания = @endDate";

                var result = IssuedMembershipContext.GetIssuedData(clientQuery,
                    new SQLiteParameter("@cardNumber", cardNumber),
                    new SQLiteParameter("@endDate", date));

                string priceQuery = "SELECT Цена FROM Descriptions WHERE Абонемент = @membership";
                object price = GeneralContext.GetElementFromDatabase(priceQuery,
                    ServicesContext.ConnectionStringServices(),
                    new SQLiteParameter("@membership", result.Membership));

                Logger.Info($"Получена информация об абонементе для карты {cardNumber}");

                return new IssuedMembershipContext.IssuedInfo
                {
                    FullName = result.FullName,
                    Membership = result.Membership,
                    EndDate = result.EndDate,
                    VisitsLeft = result.VisitsLeft,
                    Price = price?.ToString() ?? "0",
                    NumberCard = cardNumber
                };
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в GetIssuedInfo для карты {cardNumber}", ex);
                return null;
            }
        }

        private void jeanModernButtonPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Открыта форма товаров");
                Products products = new Products();
                products.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonPurchase_Click", ex);
            }
        }

        private void jeanModernButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (numberCard == string.Empty)
                {
                    Logger.Warning("Попытка продажи без выбранного клиента");
                    MessageHelper.MessageWindowOk("Клиент не выбран", "Сообщение");
                    return;
                }

                Logger.Info($"Открыта форма услуг для клиента {nameClient}, карта: {numberCard}");

                Services services = new Services();
                services.jeanModernButtonAdd.Visible = true;
                services.jeanModernButtonAdd.Visible = false;
                services.jeanModernButtonDelete.Visible = false;
                services.jeanModernButtonChange.Visible = false;
                services.jeanModernButtonSell.Visible = true;
                services.labelName.Visible = true;
                services.jeanSoftTextBoxPurchase.Visible = true;
                services.labelName.Text = nameClient;
                services.NumberCard = numberCard;
                services.checkBoxVisited.Visible = true;
                services.dateActivation.Visible = true;

                services.Show();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в jeanModernButton1_Click для клиента {nameClient}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при открытии формы услуг: {ex.Message}", "Ошибка");
            }
        }

        private void ApplySettingsToAllControls()
        {
            try
            {
                FontHelper.ApplyFontSettings(this, notChangeableTexts);
                ApplyFormStyle();
                Logger.Info("Настройки применены ко всем элементам управления");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ApplySettingsToAllControls", ex);
            }
        }

        private void ShowOrActivateForm<T>() where T : Form, new()
        {
            try
            {
                T existingForm = Application.OpenForms.OfType<T>().FirstOrDefault();

                if (existingForm != null && !existingForm.IsDisposed)
                {
                    existingForm.WindowState = FormWindowState.Normal;
                    existingForm.BringToFront();
                    existingForm.Focus();
                    Logger.Info($"Активирована существующая форма {typeof(T).Name}");
                }
                else
                {
                    T newForm = new T();
                    newForm.Show();
                    Logger.Info($"Открыта новая форма {typeof(T).Name}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ShowOrActivateForm<{typeof(T).Name}>", ex);
            }
        }

        private void jeanModernButtonClients_Click(object sender, EventArgs e)
        {
            Logger.Info("Открыта форма клиентов");
            ShowOrActivateForm<Clients>();
        }

        private void jeanModernButtonReport_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Открыта форма отчета");
                using (var report = new Report())
                {
                    report.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonReport_Click", ex);
            }
        }

        private void jeanModernButtonNewMember_Click(object sender, EventArgs e)
        {
            Logger.Info("Открыта форма нового клиента");
            ShowOrActivateForm<NewClient>();
        }

        private void jeanModernButtonSingleTicket_Click(object sender, EventArgs e)
        {
            Logger.Info("Открыта форма продажи разового посещения");
            ShowOrActivateForm<SingleTicket>();
        }

        private void jeanModernButtonChooseClient_Click(object sender, EventArgs e)
        {
            Logger.Info("Открыта форма выбора клиентов");
            ShowOrActivateForm<ChooseClient>();
        }

        private void jeanModernButtonService_Click(object sender, EventArgs e)
        {
            Logger.Info("Открыта форма услуг");
            ShowOrActivateForm<Services>();
        }

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            Logger.Info("Открыта форма выданных абонементов");
            ShowOrActivateForm<IssuedMembership>();
        }

        private void jeanModernButtonArchive_Click(object sender, EventArgs e)
        {
            Logger.Info("Открыта форма архива");
            ShowOrActivateForm<ArchiveServices>();
        }

        private void jeanModernButtonHistoryPayment_Click(object sender, EventArgs e)
        {
            Logger.Info("Открыта история платежей");
            ShowOrActivateForm<HistoryPayment>();
        }

        private void ApplyAllSettings()
        {
            try
            {
                ApplySettingsToAllControls();
                UpdateDataGridViewFont(dataGridViewClient);
                Logger.Info("Все настройки применены");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ApplyAllSettings", ex);
            }
        }

        private void UpdateDataGridViewFont(DataGridView dataGrid)
        {
            try
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
                Logger.Info($"Шрифт DataGridView обновлен, размер: {DataConfig.sizeFontTables}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в UpdateDataGridViewFont", ex);
            }
        }

        private void jeanModernButtonDesign_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Открыта форма настройки дизайна");
                Design design = new Design();
                design.SetRefreshAction(ApplyAllSettings);
                design.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonDesign_Click", ex);
            }
        }

        private void jeanModernButtonImport_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Открыта форма импорта");
                Import import = new Import();
                import.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonImport_Click", ex);
            }
        }

        private void jeanModernButtonDocumentation_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Открыта форма документации");
                Documentation documentation = new Documentation();
                documentation.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonDocumentation_Click", ex);
            }
        }

        private void jeanModernButtonReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageHelper.MessageWindowYesNo("Вы действительно хотите отменить посещение?") != DialogResult.Yes)
                {
                    Logger.Info("Возврат посещения отменен пользователем");
                    return;
                }

                GeneralContext.CommandDataFromDatabase("UPDATE Issued SET " +
                            "Посещений_осталось = '" + numberLeft.ToString() + "' " +
                            "WHERE №Карты = '" + numberCard + "' " +
                            "ORDER BY date(Дата_окончания) ASC " +
                            "LIMIT 1;",
                    IssuedMembershipContext.ConnectionStringIssued());

                Logger.Info($"Возврат посещения для карты {numberCard}, восстановлено посещений: {numberLeft}");

                MessageHelper.MessageWindowOk("Посещения обновлены!", "Сообщение");

                DisplayClientData(numberCard);
                jeanModernButtonReturn.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в jeanModernButtonReturn_Click для карты {numberCard}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при возврате посещения: {ex.Message}", "Ошибка");
            }
        }

        private void jeanModernButtonErase_Click(object sender, EventArgs e)
        {
            try
            {
                jeanTextBoxNumberCard.Text = "";
                Logger.Info("Очищено поле номера карты");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonErase_Click", ex);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                base.OnFormClosed(e);
                _animBackground?.RemoveBackground();
                _animBackground = null;
                Logger.Info("Главная форма закрыта");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в OnFormClosed", ex);
            }
        }
    }
}
