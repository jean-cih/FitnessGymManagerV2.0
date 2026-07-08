using GymApplicationV2._0.Components;
using GymApplicationV2._0.Controls;
using Shadow;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GymApplicationV2._0.FormsSettings
{
    public partial class Design : ShadowedForm
    {
        private string FontFilePath = Path.Combine("AppFiles", "Font.txt");
        private FlowLayoutPanel flowLayout;
        private Panel previewPanel;
        private JeanModernButton documentationButton;

        private Timer _fadeTimer;
        private float _opacity = 0;

        Panel titlePanel;

        public Design()
        {
            InitializeComponent();
            InitializeComponents();
            titlePanel.MouseDown += TitlePanel_MouseDown;
            titlePanel.MouseMove += TitlePanel_MouseMove;
            titlePanel.MouseUp += TitlePanel_MouseUp;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;
            SetupAnimation();
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

                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(80, 120, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
                }
            };

            titlePanel = new System.Windows.Forms.Panel
            {
                Size = new Size(933, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new System.Drawing.Point(0, 0),
            };

            // Заголовок
            var titleLabel = new Label
            {
                Text = "🎨 Настройки дизайна",
                Font = new Font("Montserrat", 18, FontStyle.Bold),
                ForeColor = ForeColor = Color.FromArgb(220, 220, 255),
                BackColor = Color.Transparent,
                Location = new Point(320, 10),
                AutoSize = true,
            };

            // Контейнер для настроек
            var settingsContainer = new Panel
            {
                BackColor = Color.White,
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
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BorderRadius = 8,
            };
            documentationButton.Click += DocumentationButton_Click;
            settingsContainer.Controls.Add(documentationButton);

            // Элементы для выбора шрифтов
            AddFontSetting("🔤 Размер шрифта кнопок:", "comboBoxFontButtons", DataClass.sizeFontButtons,
                "Размер текста на всех кнопках интерфейса");

            AddFontSetting("📊 Размер шрифта таблиц:", "comboBoxFontTables", DataClass.sizeFontTables,
                "Размер текста в таблицах и списках");

            AddFontSetting("📝 Размер шрифта надписей:", "comboBoxFontCaptions", DataClass.sizeFontCaptions,
                "Размер текста заголовков и меток");

            // Выбор стиля формы
            AddFormStyleSetting();

            // Выбор стиля заднего фона
            AddBackgroundStyleSetting();

            titlePanel.Controls.Add(titleLabel);

            var btnClose = new JeanModernButton
            {
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 28),
                Cursor = Cursors.Hand
            };

            btnClose = CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(893, 10), new Size(30, 28));
            btnClose.Click += (s, e) => CloseWithAnimation();

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);
            settingsContainer.Controls.Add(flowLayout);
            this.Controls.Add(settingsContainer);
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

        private void DocumentationButton_Click(object sender, EventArgs e)
        {
            Documentation documentation = new Documentation();
            documentation.Show();
        }

        private void AddFontSetting(string labelText, string comboBoxName, int defaultValue, string tooltipText)
        {
            var settingCard = new Panel
            {
                Size = new Size(500, 100),
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.FromArgb(55, 55, 58),
                Padding = new Padding(20),
                Cursor = Cursors.Default
            };

            settingCard.Paint += (s, e) =>
            {
                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, settingCard.Width - 1, settingCard.Height - 1));
                }
            };

            var label = new Label
            {
                Text = labelText,
                Location = new Point(15, 15),
                Width = 250,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 0)
            };

            // Стилизованный ComboBox
            var comboBox = new ModernComboBox
            {
                Name = comboBoxName,
                Location = new Point(390, 50),
                Width = 80,
                Height = 35,
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.FromArgb(255, 140, 0),
                ArrowColor = Color.White
            };

            // Заполнение размеров шрифта
            for (int i = 8; i <= 20; i++)
            {
                comboBox.Items.Add(i);
            }

            comboBox.SelectedItem = defaultValue;
            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            // Tooltip
            var tooltip = new System.Windows.Forms.ToolTip();
            tooltip.SetToolTip(comboBox, tooltipText);
            tooltip.SetToolTip(label, tooltipText);

            // Preview label
            var previewLabel = new Label
            {
                Text = "Пример текста",
                Location = new Point(360, 15),
                Width = 120,
                Font = new Font("Segoe UI", defaultValue, FontStyle.Regular),
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
            var settingCard = new Panel
            {
                Size = new Size(500, 100),
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.FromArgb(55, 55, 58),
                Padding = new Padding(20)
            };

            settingCard.Paint += (s, e) =>
            {
                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, settingCard.Width - 1, settingCard.Height - 1));
                }
            };

            var label = new Label
            {
                Text = "🎭 Стиль интерфейса:",
                Location = new Point(15, 15),
                Width = 250,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 0),
            };

            // Стилизованный ComboBox для выбора стиля
            var comboBox = new ModernComboBox
            {
                Name = "comboBoxForm",
                Location = new Point(270, 50),
                Width = 150,
                Height = 35,
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.FromArgb(255, 140, 0),
                ArrowColor = Color.Black,
                Text = DataClass.styleForm
            };

            comboBox.Items.AddRange(new[] { "None", "UserStyle", "SimpleDark", "TelegramStyle" });
            comboBox.SelectedItem = DataClass.styleForm;
            comboBox.SelectedIndexChanged += ComboBoxForm_SelectedIndexChanged;

            // Preview стилей
            previewPanel = new Panel
            {
                Location = new Point(430, 46),
                Size = new Size(40, 35),
                BackColor = GetStylePreviewColor(comboBox.Text),
                BorderStyle = BorderStyle.FixedSingle
            };

            comboBox.SelectedIndexChanged += (s, e) =>
            {
                previewPanel.BackColor = GetStylePreviewColor(comboBox.Text);
            };

            // Tooltip
            var tooltip = new System.Windows.Forms.ToolTip();
            tooltip.SetToolTip(comboBox, "Выберите визуальный стиль интерфейса приложения");
            tooltip.SetToolTip(previewPanel, "Предпросмотр цвета стиля");

            settingCard.Controls.Add(label);
            settingCard.Controls.Add(comboBox);
            settingCard.Controls.Add(previewPanel);
            flowLayout.Controls.Add(settingCard);
        }

        private void AddBackgroundStyleSetting()
        {
            var settingCard = new Panel
            {
                Size = new Size(500, 100),
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.FromArgb(55, 55, 58),
                Padding = new Padding(20)
            };

            settingCard.Paint += (s, e) =>
            {
                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, settingCard.Width - 1, settingCard.Height - 1));
                }
            };

            var label = new Label
            {
                Text = "🌊 Стиль заднего фона:",
                Location = new Point(15, 15),
                Width = 250,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 0),
            };

            // Стилизованный ComboBox для выбора стиля
            var comboBox = new ModernComboBox
            {
                Name = "comboBoxBackground",
                Location = new Point(320, 50),
                Width = 150,
                Height = 35,
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.FromArgb(255, 140, 0),
                ArrowColor = Color.Black,
                Text = DataClass.styleBackground
            };

            comboBox.Items.AddRange(new[] { "Dynamic", "Casual", "Minimal", "Static", "None" });
            comboBox.SelectedItem = DataClass.styleBackground;
            comboBox.SelectedIndexChanged += ComboBoxBackground_SelectedIndexChanged;

            // Tooltip
            var tooltip = new System.Windows.Forms.ToolTip();
            tooltip.SetToolTip(comboBox, "Выберите визуальный стиль заднего фона");

            settingCard.Controls.Add(label);
            settingCard.Controls.Add(comboBox);
            flowLayout.Controls.Add(settingCard);
        }

        private void CloseWithAnimation()
        {
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

        private JeanModernButton CreateStyledButton(string text, Color baseColor, int radius, int radiusSize, Color radiusColor, Point location, Size size)
        {
            var button = new JeanModernButton
            {
                Text = text,
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = baseColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(baseColor, 0.2f);
            button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(baseColor, 0.2f);

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

            return button;
        }

        private Color GetStylePreviewColor(string style)
        {
            switch (style)
            {
                case "UserStyle":
                    return Color.FromArgb(120, 73, 18);  
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
            var comboBox = (System.Windows.Forms.ComboBox)sender;
            if (int.TryParse(comboBox.Text, out int size))
            {
                switch (comboBox.Name)
                {
                    case "comboBoxFontButtons":
                        DataClass.sizeFontButtons = size;
                        UpdateSettingInFile(5, size.ToString());
                        break;
                    case "comboBoxFontTables":
                        DataClass.sizeFontTables = size;
                        UpdateSettingInFile(7, size.ToString());
                        break;
                    case "comboBoxFontCaptions":
                        DataClass.sizeFontCaptions = size;
                        UpdateSettingInFile(3, size.ToString());
                        break;
                }

                // Визуальная обратная связь
                ShowSaveIndicator();
            }
        }

        private void ComboBoxForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (System.Windows.Forms.ComboBox)sender;
            DataClass.styleForm = comboBox.Text;
            UpdateSettingInFile(9, comboBox.Text);

            // Визуальная обратная связь
            ShowSaveIndicator();
        }

        private void ComboBoxBackground_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (System.Windows.Forms.ComboBox)sender;
            DataClass.styleBackground = comboBox.Text;
            UpdateSettingInFile(11, comboBox.Text);

            // Визуальная обратная связь
            ShowSaveIndicator();
        }

        private void UpdateSettingInFile(int lineIndex, string value)
        {
            try
            {
                var lines = File.ReadAllLines(FontFilePath);
                if (lineIndex < lines.Length)
                {
                    lines[lineIndex] = value;
                    File.WriteAllLines(FontFilePath, lines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения настроек: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Font = new Font("Segoe UI", 9),
                Location = new Point(this.Width - 200, 15)
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

        private bool isDragging = false;
        private Point lastCursor;
        private Point lastForm;

        private void TitlePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursor = Cursor.Position;
                lastForm = this.Location;
            }
        }

        private void TitlePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(lastCursor));
                this.Location = Point.Add(lastForm, new Size(diff));
            }
        }

        private void TitlePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }
    }

    // Кастомный стилизованный ComboBox
    public class ModernComboBox : System.Windows.Forms.ComboBox
    {
        public Color BorderColor { get; set; } = Color.Gray;
        public Color ArrowColor { get; set; } = Color.Black;
        public int BorderRadius { get; set; } = 6;

        public ModernComboBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.DoubleBuffer |
                    ControlStyles.ResizeRedraw, true);

            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Фон
            using (var backBrush = new SolidBrush(BackColor))
            using (var borderPen = new Pen(BorderColor, 2))
            using (var path = GetRoundRectPath(ClientRectangle, BorderRadius))
            {
                graphics.FillPath(backBrush, path);
                graphics.DrawPath(borderPen, path);
            }

            // Текст
            if (SelectedItem != null)
            {
                TextRenderer.DrawText(graphics, SelectedItem.ToString(), Font,
                    new Rectangle(10, 0, Width - 30, Height), ForeColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }

            // Стрелка
            DrawArrow(graphics);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            // Выделенный элемент
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                using (var selectionBrush = new SolidBrush(Color.FromArgb(0, 122, 204)))
                {
                    e.Graphics.FillRectangle(selectionBrush, e.Bounds);
                }
            }

            // Текст элемента
            var itemText = Items[e.Index].ToString();
            TextRenderer.DrawText(e.Graphics, itemText, Font, e.Bounds,
                (e.State & DrawItemState.Selected) == DrawItemState.Selected ? Color.White : ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            e.DrawFocusRectangle();
        }

        private void DrawArrow(Graphics graphics)
        {
            var arrowX = Width - 20;
            var arrowY = Height / 2 - 2;

            var points = new Point[]
            {
                new Point(arrowX, arrowY),
                new Point(arrowX + 7, arrowY),
                new Point(arrowX + 3, arrowY + 4)
            };

            using (var arrowBrush = new SolidBrush(ArrowColor))
            {
                graphics.FillPolygon(arrowBrush, points);
            }
        }

        private GraphicsPath GetRoundRectPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}