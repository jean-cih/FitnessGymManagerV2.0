using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using GymApplicationV2._0.Data;
using NAudio.MediaFoundation;
using NAudio.Wave;
using Shadow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsSettings
{
    public partial class Design : ShadowedForm
    {
        private FlowLayoutPanel flowLayout;
        private Panel previewPanel;
        private JeanModernButton documentationButton;

        Panel titlePanel;

        Label label_error_sound;
        Label label_success_sound;

        private Action refreshAction;

        // Словарь для хранения ссылок на элементы управления
        private Dictionary<string, ComboBox> fontComboBoxes = new Dictionary<string, ComboBox>();
        private ComboBox formStyleComboBox;
        private ComboBox backgroundStyleComboBox;

        private FadeAnimation _fadeAnimation;

        string[] notChangeableTexts = new string[]
            {
                "✅ Настройки сохранены",
                "Пример",
                "🎨 Настройки дизайна"
            };

        public Design()
        {
            InitializeComponent();
            LoadSettings();

            InitializeComponents();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, notChangeableTexts);

            titlePanel.EnableDrag(this);
        }

        public void SetRefreshAction(Action action)
        {
            refreshAction = action;
        }

        private void LoadSettings()
        {
            try
            {
                DataConfig.sizeFontCaptions = ConfigManager.GetSetting<int>("headlineSize");
                DataConfig.sizeFontButtons = ConfigManager.GetSetting<int>("sizeKeyName");
                DataConfig.sizeFontTables = ConfigManager.GetSetting<int>("sizeTableTitle");
                DataConfig.sizeFontText = ConfigManager.GetSetting<int>("textSize");
                DataConfig.styleForm = ConfigManager.GetSetting<string>("designForm");
                DataConfig.styleBackground = ConfigManager.GetSetting<string>("designBackground");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки настроек: {ex.Message}");
            }
        }

        private void InitializeComponents()
        {
            this.Padding = new Padding(20, 1, 20, 20);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;

            this.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(248, 248, 252),
                    Color.FromArgb(240, 240, 250),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }

                using (var pen = new Pen(Color.FromArgb(80, 120, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
                }
            };

            titlePanel = new Panel
            {
                Size = new Size(933, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0),
            };

            var titleLabel = new Label
            {
                Text = "🎨 Настройки дизайна",
                Font = new Font("Montserrat", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 220, 255),
                BackColor = Color.Transparent,
                Location = new Point(320, 10),
                AutoSize = true,
            };

            var settingsContainer = new Panel
            {
                BackColor = Color.FromArgb(240, 240, 250),
                Padding = new Padding(10),
                Location = new Point(20, 50),
                Size = new Size(893, 500),
            };

            flowLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
            };

            documentationButton = new JeanModernButton
            {
                Text = "📄 Документация",
                Size = new Size(170, 40),
                BackColor = Color.FromArgb(37, 99, 235),
                Location = new Point(620, 430),
                ForeColor = Color.White,
                BorderRadius = 8,
            };
            documentationButton.Click += DocumentationButton_Click;
            settingsContainer.Controls.Add(documentationButton);

            // Элементы для выбора шрифтов
            AddFontSetting("🔤 Шрифт кнопок", "comboBoxFontButtons", DataConfig.sizeFontButtons,
                "Размер текста на всех кнопках интерфейса", 14);

            AddFontSetting("📊 Шрифт таблиц", "comboBoxFontTables", DataConfig.sizeFontTables,
                "Размер текста в таблицах", 14);

            AddFontSetting("📌 Шрифт заголовков", "comboBoxFontCaptions", DataConfig.sizeFontCaptions,
                "Размер текста заголовков", 18);

            AddFontSetting("📝 Шрифт текста", "comboBoxFontText", DataConfig.sizeFontText,
                "Размер обычного текста", 14);

            AddFormStyleSetting();
            AddBackgroundStyleSetting();
            AddErrorSetting();
            AddSuccessSetting();

            titlePanel.Controls.Add(titleLabel);

            var btnClose = UIStyler.CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, 10), new Size(30, 28));
            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);
            settingsContainer.Controls.Add(flowLayout);
            this.Controls.Add(settingsContainer);
        }

        private void DocumentationButton_Click(object sender, EventArgs e)
        {
            Documentation documentation = new Documentation();
            documentation.ShowDialog();
        }

        private void AddFontSetting(string labelText, string comboBoxName, int defaultValue, string tooltipText, int range)
        {
            var settingCard = new JeanPanel
            {
                Size = new Size(500, 100),
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
                Padding = new Padding(20),
                BorderRadius = 20
            };

            settingCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, settingCard.Width - 1, settingCard.Height - 1));
                }
            };

            var label = new Label
            {
                Text = labelText,
                Location = new Point(15, 15),
                Size = new Size(350, 70),
                ForeColor = Color.FromArgb(255, 140, 0)
            };

            var comboBox = new ModernComboBox
            {
                Name = comboBoxName,
                Location = new Point(420, 55),
                Size = new Size(60, 35),
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.FromArgb(255, 140, 0),
                ArrowColor = Color.White,
                IntegralHeight = false
            };

            for (int i = 8; i <= range; i++)
            {
                comboBox.Items.Add(i);
            }

            comboBox.SelectedItem = defaultValue;

            fontComboBoxes[comboBoxName] = comboBox;

            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            var tooltip = new ToolTip();
            tooltip.SetToolTip(comboBox, tooltipText);
            tooltip.SetToolTip(label, tooltipText);

            var previewLabel = new Label
            {
                Text = "Пример",
                Location = new Point(360, 10),
                Size = new Size(120, 50),
                ForeColor = Color.FromArgb(255, 140, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            comboBox.SelectedIndexChanged += (s, e) =>
            {
                if (int.TryParse(comboBox.Text, out int size))
                {
                    previewLabel.Font = new Font("Segoe UI", size, FontStyle.Regular);
                }
            };

            settingCard.Controls.Add(label);
            settingCard.Controls.Add(comboBox);
            settingCard.Controls.Add(previewLabel);
            flowLayout.Controls.Add(settingCard);
        }

        private void AddFormStyleSetting()
        {
            var settingCard = new JeanPanel
            {
                Size = new Size(500, 100),
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
                Padding = new Padding(20),
                BorderRadius = 20
            };

            settingCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, settingCard.Width - 1, settingCard.Height - 1));
                }
            };

            var label = new Label
            {
                Text = "🎭 Интерфейс",
                Location = new Point(15, 15),
                Size = new Size(300, 70),
                ForeColor = Color.FromArgb(255, 140, 0),
            };

            formStyleComboBox = new ModernComboBox
            {
                Name = "comboBoxForm",
                Location = new Point(320, 55),
                Size = new Size(100, 35),
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.FromArgb(255, 140, 0),
                ArrowColor = Color.Black
            };

            formStyleComboBox.Items.AddRange(new[] { "None", "UserStyle", "SimpleDark", "TelegramStyle" });
            formStyleComboBox.SelectedItem = DataConfig.styleForm;
            formStyleComboBox.SelectedIndexChanged += ComboBoxForm_SelectedIndexChanged;

            previewPanel = new Panel
            {
                Location = new Point(440, 47),
                Size = new Size(40, 35),
                BackColor = GetStylePreviewColor(DataConfig.styleForm),
                BorderStyle = BorderStyle.FixedSingle
            };

            formStyleComboBox.SelectedIndexChanged += (s, e) =>
            {
                previewPanel.BackColor = GetStylePreviewColor(formStyleComboBox.Text);
            };

            var tooltip = new ToolTip();
            tooltip.SetToolTip(formStyleComboBox, "Выберите визуальный стиль интерфейса приложения");
            tooltip.SetToolTip(previewPanel, "Предпросмотр цвета стиля");

            settingCard.Controls.Add(label);
            settingCard.Controls.Add(formStyleComboBox);
            settingCard.Controls.Add(previewPanel);
            flowLayout.Controls.Add(settingCard);
        }

        private void AddErrorSetting()
        {
            var settingCard = new JeanPanel
            {
                Size = new Size(500, 100),
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
                Padding = new Padding(20),
                BorderRadius = 20
            };

            settingCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, settingCard.Width - 1, settingCard.Height - 1));
                }
            };

            label_error_sound = new Label
            {
                Text = "🔊 Звук ошибки - " + Path.GetFileName(Properties.Settings.Default.ErrorSoundPath),
                Location = new Point(15, 15),
                Size = new Size(300, 70),
                ForeColor = Color.FromArgb(255, 140, 0),
            };

            var choose = UIStyler.CreateStyledButton("Выбрать", Color.FromArgb(70, 130, 220), 10, 0, Color.FromArgb(255, 140, 0), new Point(380, 40), new Size(100, 40));
            choose.Click += (s, e) => SelectSoundFile(s, e, false);

            var tooltip = new ToolTip();
            tooltip.SetToolTip(choose, "Выберите файл для звука ошибки");

            settingCard.Controls.Add(label_error_sound);
            settingCard.Controls.Add(choose);
            flowLayout.Controls.Add(settingCard);
        }

        private void AddSuccessSetting()
        {
            var settingCard = new JeanPanel
            {
                Size = new Size(500, 100),
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
                Padding = new Padding(20),
                BorderRadius = 20
            };

            settingCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, settingCard.Width - 1, settingCard.Height - 1));
                }
            };

            label_success_sound = new Label
            {
                Text = "🔊 Звук выполнения - " + Path.GetFileName(Properties.Settings.Default.SuccessSoundPath),
                Location = new Point(15, 15),
                Size = new Size(300, 70),
                ForeColor = Color.FromArgb(255, 140, 0),
            };

            var choose = UIStyler.CreateStyledButton("Выбрать", Color.FromArgb(70, 130, 220), 10, 0, Color.FromArgb(255, 140, 0), new Point(380, 40), new Size(100, 40));
            choose.Click += (s, e) => SelectSoundFile(s, e);

            var tooltip = new ToolTip();
            tooltip.SetToolTip(choose, "Выберите файл для звука выполнения");

            settingCard.Controls.Add(label_success_sound);
            settingCard.Controls.Add(choose);
            flowLayout.Controls.Add(settingCard);
        }

        public void SelectSoundFile(object sender, EventArgs e, bool isSuccess = true)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите звуковой файл для ошибки";
                openFileDialog.Filter = "Аудио файлы|*.wav;*.mp3;*.wma;*.aac|WAV файлы|*.wav|MP3 файлы|*.mp3|Все файлы|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;

                    if (File.Exists(selectedFile))
                    {
                        try
                        {
                            MediaFoundationApi.Startup();
                            using (var testReader = new MediaFoundationReader(selectedFile))
                            {
                            }

                            if (isSuccess)
                            {
                                SetSuccessSound(selectedFile);
                            }
                            else
                            {
                                SetErrorSound(selectedFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageHelper.MessageWindowOk($"Выбранный файл не является корректным аудио файлом: {ex.Message}", "Ошибка");
                        }
                    }
                }
            }
        }

        private void SetErrorSound(string selectedFile)
        {
            Properties.Settings.Default.ErrorSoundPath = selectedFile;
            Properties.Settings.Default.Save();

            MessageHelper.MessageWindowOk("Звук ошибки успешно установлен!", "Успех");

            var errorSound = new PlaySoundHelper(false);
            errorSound.PlaySound();

            label_error_sound.Text = "🔊 Звук ошибки - " + Path.GetFileName(selectedFile);
        }
        
        private void SetSuccessSound(string selectedFile)
        {
            Properties.Settings.Default.SuccessSoundPath = selectedFile;
            Properties.Settings.Default.Save();

            MessageHelper.MessageWindowOk("Звук выполнения успешно установлен!", "Успех");

            var successSound = new PlaySoundHelper();
            successSound.PlaySound();

            label_success_sound.Text = "🔊 Звук выполнения - " + Path.GetFileName(selectedFile);
        }

        private void AddBackgroundStyleSetting()
        {
            var settingCard = new JeanPanel
            {
                Size = new Size(500, 100),
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
                Padding = new Padding(20),
                BorderRadius = 20
            };

            settingCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, settingCard.Width - 1, settingCard.Height - 1));
                }
            };

            var label = new Label
            {
                Text = "🌊 Задний фон",
                Location = new Point(15, 15),
                Size = new Size(350, 70),
                ForeColor = Color.FromArgb(255, 140, 0),
            };

            backgroundStyleComboBox = new ModernComboBox
            {
                Name = "comboBoxBackground",
                Location = new Point(360, 55),
                Size = new Size(120, 35),
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.FromArgb(255, 140, 0),
                ArrowColor = Color.Black
            };

            backgroundStyleComboBox.Items.AddRange(new[] { "None", "Dynamic", "Casual", "Minimal", "Static" });
            backgroundStyleComboBox.SelectedItem = DataConfig.styleBackground;
            backgroundStyleComboBox.SelectedIndexChanged += ComboBoxBackground_SelectedIndexChanged;

            var tooltip = new ToolTip();
            tooltip.SetToolTip(backgroundStyleComboBox, "Выберите визуальный стиль заднего фона");

            settingCard.Controls.Add(label);
            settingCard.Controls.Add(backgroundStyleComboBox);
            flowLayout.Controls.Add(settingCard);
        }

        private Color GetStylePreviewColor(string style)
        {
            switch (style)
            {
                case "UserStyle":
                    return Color.FromArgb(188, 122, 121);
                case "SimpleDark":
                    return Color.FromArgb(146, 110, 53);
                case "TelegramStyle":
                    return Color.FromArgb(230, 187, 122);
                default:
                    return Color.White;
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            if (int.TryParse(comboBox.Text, out int size))
            {
                switch (comboBox.Name)
                {
                    case "comboBoxFontButtons":
                        DataConfig.sizeFontButtons = size;
                        UpdateSettingInFile("sizeKeyName", size);
                        break;
                    case "comboBoxFontTables":
                        DataConfig.sizeFontTables = size;
                        UpdateSettingInFile("sizeTableTitle", size);
                        break;
                    case "comboBoxFontCaptions":
                        DataConfig.sizeFontCaptions = size;
                        UpdateSettingInFile("headlineSize", size);
                        break;
                    case "comboBoxFontText":
                        DataConfig.sizeFontCaptions = size;
                        UpdateSettingInFile("textSize", size);
                        break;
                }
            }
        }

        private void ComboBoxForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            DataConfig.styleForm = comboBox.Text;
            UpdateSettingInFile("designForm", comboBox.Text);
        }

        private void ComboBoxBackground_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            DataConfig.styleBackground = comboBox.Text;
            UpdateSettingInFile("designBackground", comboBox.Text);
        }

        private void UpdateSettingInFile(string key, object value)
        {
            try
            {
                JsonElement jsonElement = JsonSerializer.SerializeToElement(value);
                ConfigManager.UpdateSetting(key, jsonElement);
                LoadSettings();

                refreshAction?.Invoke();

                FontHelper.ApplyFontSettings(this, notChangeableTexts);

                ShowSaveIndicator();
            }
            catch (Exception ex)
            {
                MessageHelper.MessageWindowOk($"Ошибка сохранения настроек: {ex.Message}", "Ошибка");
            }
        }

        private void ShowSaveIndicator()
        {
            var indicator = new Label
            {
                Text = "✅ Настройки сохранены",
                ForeColor = Color.FromArgb(255, 140, 0),
                BackColor = Color.MediumSlateBlue,
                AutoSize = true,
                Font = new Font("Segoe UI", DataConfig.sizeFontText > 11 ? 11 : DataConfig.sizeFontText, FontStyle.Bold),
                Location = new Point(this.Width - 270, 10),
                Padding = new Padding(10, 5, 10, 5)
            };

            this.Controls.Add(indicator);
            indicator.BringToFront();

            var timer = new Timer { Interval = 2000 };
            timer.Tick += (s, e) =>
            {
                this.Controls.Remove(indicator);
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _fadeAnimation?.Dispose();
            _fadeAnimation = null;
        }
    }
}