using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class Products : Form
    {
        private Dictionary<string, Product> products;

        private JeanPanel statsPanel;
        private Panel titlePanel;
        private Label totalSalesLabel;
        private Label revenueLabel;
        private Label timeLabel;
        private FlowLayoutPanel productsFlowPanel;
        private JeanModernButton finishSaleButton;
        private JeanModernButton resetButton;
        private JeanModernButton printButton;
        private JeanModernButton changePrice;

        private FadeAnimation _fadeAnimation;

        private readonly Color CardColor = Color.White;

        public Products()
        {
            InitializeComponent();
            InitializeCustomDesign();

            InitializeProducts();
            SetupAdvancedStyling();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            titlePanel.EnableDrag(this);
        }

        private void InitializeCustomDesign()
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

            titlePanel = new Panel
            {
                Size = new Size(1100, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0),
            };

            // Заголовок
            var titleLabel = new Label
            {
                Text = "💰 Товары",
                Font = new Font("Montserrat", 18, FontStyle.Bold),
                ForeColor = ForeColor = Color.FromArgb(220, 220, 255),
                BackColor = Color.Transparent,
                Location = new Point(460, 10),
                AutoSize = true,
            };
          
            // Инициализация всех элементов управления
            statsPanel = new JeanPanel();
            totalSalesLabel = new Label();
            revenueLabel = new Label();
            timeLabel = new Label();
            productsFlowPanel = new FlowLayoutPanel();
            finishSaleButton = new JeanModernButton();
            resetButton = new JeanModernButton();
            printButton = new JeanModernButton();
            changePrice = new JeanModernButton();

            
            // Добавление элементов в Controls формы
            this.Controls.Add(statsPanel);
            this.Controls.Add(productsFlowPanel);
            titlePanel.Controls.Add(titleLabel);
            this.Controls.Add(titlePanel);

            // Добавление элементов в statsPanel
            statsPanel.Controls.Add(totalSalesLabel);
            statsPanel.Controls.Add(revenueLabel);
            statsPanel.Controls.Add(timeLabel);

            var btnClose = new JeanModernButton
            {
                Text = "X",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 28),
                Cursor = Cursors.Hand,
                BorderRadius = 0,
                BorderSize = 0,
                Location = new Point(1060, 10),
            };

            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();
            titlePanel.Controls.Add(btnClose);
        }

        private void InitializeProducts()
        {
            products = new Dictionary<string, Product>
            {
                { "💧 Вода", new Product("Вода", 50) },
                { "☕ Кофе", new Product("Кофе", 120) },
                { "🥛 Протеин", new Product("Протеин", 200) },
                { "⚡ Энергетик", new Product("Энергетик", 150) },
                { "🍫 Шоколад", new Product("Шоколад", 80) },
                { "🍎 Фрукты", new Product("Фрукты", 100) },
                { "🥨 Снеки", new Product("Снеки", 60) },
                { "🏋️ Другое", new Product("Другое", 110) }
            };
        }

        private void SetupAdvancedStyling()
        {
            // Настройка расположения элементов
            SetupLayout();

            // Статистика в реальном времени
            SetupStatsPanel();
            CreateProductCards();
            SetupCustomControls();
        }

        private void SetupLayout()
        {
            productsFlowPanel.Size = new Size(this.Width - 40, this.Height - 400);
            productsFlowPanel.Location = new Point(20, 100);
            productsFlowPanel.AutoScroll = true;
            productsFlowPanel.BackColor = Color.Transparent;
        }

        private void SetupStatsPanel()
        {
            // Настройка размеров и расположения основных элементов
            statsPanel.Size = new Size(300, 120);
            statsPanel.Location = new Point(this.Width / 2 - 150, this.Height - 280);
            statsPanel.GradientBottomColor = Color.White;
            statsPanel.GradientTapColor = Color.White;

            // Настройка меток статистики
            totalSalesLabel.Text = "0";
            totalSalesLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            totalSalesLabel.ForeColor = Color.FromArgb(0, 220, 255);
            totalSalesLabel.BackColor = Color.Transparent;
            totalSalesLabel.Size = new Size(280, 40);
            totalSalesLabel.Location = new Point(10, 10);
            totalSalesLabel.TextAlign = ContentAlignment.MiddleCenter;

            revenueLabel.Text = "0₽";
            revenueLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            revenueLabel.ForeColor = Color.FromArgb(120, 255, 150);
            revenueLabel.BackColor = Color.Transparent;
            revenueLabel.Size = new Size(280, 30);
            revenueLabel.Location = new Point(10, 50);
            revenueLabel.TextAlign = ContentAlignment.MiddleCenter;

            timeLabel.Text = $"Обновлено: {DateTime.Now:HH:mm:ss}";
            timeLabel.Font = new Font("Segoe UI", 10);
            timeLabel.ForeColor = Color.MediumSlateBlue;
            timeLabel.BackColor = Color.Transparent;
            timeLabel.Size = new Size(280, 20);
            timeLabel.Location = new Point(10, 85);
            timeLabel.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void CreateProductCards()
        {
            productsFlowPanel.Controls.Clear();

            int cardIndex = 0;
            foreach (var product in products)
            {
                var card = CreateProductCard(product.Value, cardIndex);
                productsFlowPanel.Controls.Add(card);
                cardIndex++;
            }
        }

        private JeanPanel CreateProductCard(Product product, int index)
        {
            var card = new JeanPanel
            {
                Size = new Size(235, 120),
                BackColor = CardColor,
                GradientBottomColor = CardColor,
                GradientTapColor = CardColor,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10),
                BorderRadius = 20,
                Tag = product
            };

            var emojiLabel = new Label
            {
                Text = product.Emoji,
                Font = new Font("Segoe UI", 20),
                ForeColor = Color.FromArgb(255, 140, 0),
                BackColor = Color.Transparent,
                Location = new Point(15, 15),
                Size = new Size(50, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var nameLabel = new Label
            {
                Text = product.Name,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,              
            };
            nameLabel.Location = new Point((int)(card.Width / 2 - nameLabel.Text.Length / 0.25), 10);

            var priceLabel = new Label
            {
                Text = $"{product.Price:C}",
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            priceLabel.Location = new Point((int)(card.Width / 2 - priceLabel.Text.Length / 0.35), 40);

            // Счетчик
            var countLabel = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 220, 255),
                BackColor = Color.Transparent,
                Location = new Point(85, 70),
                Size = new Size(80, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Name = $"countLabel_{product.Name}"
            };

            var minusBtn = new JeanModernButton
            {
                Text = "─",
                Size = new Size(40, 35),
                Location = new Point(15, 75),
                BackColor = Color.FromArgb(255, 80, 80),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Tag = product
            };

            var plusBtn = new JeanModernButton
            {
                Text = "+",
                Size = new Size(40, 35),
                Location = new Point(180, 75),
                BackColor = Color.FromArgb(80, 220, 120),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Tag = product
            };

            minusBtn.Click += (s, e) => UpdateProductCount(product, -1);
            plusBtn.Click += (s, e) => UpdateProductCount(product, 1);

            // Добавление всех элементов в карточку
            card.Controls.Add(emojiLabel);
            card.Controls.Add(nameLabel);
            card.Controls.Add(priceLabel);
            card.Controls.Add(countLabel);
            card.Controls.Add(minusBtn);
            card.Controls.Add(plusBtn);

            return card;
        }

        private void UpdateProductCount(Product product, int change)
        {
            if (product.Count <= 0 && change == -1)
            {
                return;
            }

            product.Count += change;

            foreach (JeanPanel card in productsFlowPanel.Controls)
            {
                if (card.Tag == product)
                {
                    foreach (Control control in card.Controls)
                    {
                        if (control.Name == $"countLabel_{product.Name}")
                        {
                            control.Text = product.Count.ToString();

                            AnimateCountChange(control, change > 0);
                            break;
                        }
                    }
                    break;
                }
            }

            UpdateStatistics();
            PlaySound(change > 0);

            signReset = true;
        }

        private void AnimateCountChange(Control control, bool isIncrease)
        {
            var originalColor = control.ForeColor;
            control.ForeColor = isIncrease ? Color.FromArgb(120, 255, 150) : Color.FromArgb(255, 120, 120);

            var timer = new Timer { Interval = 100 };
            timer.Tick += (s, e) =>
            {
                control.ForeColor = originalColor;
                timer.Stop();
            };
            timer.Start();
        }

        private void PlaySound(bool isIncrease)
        {
            SystemSounds.Beep.Play();
        }

        private void UpdateStatistics()
        {
            int totalItems = products.Values.Sum(p => p.Count);
            decimal totalRevenue = products.Values.Sum(p => p.Count * p.Price);

            AnimateNumberChange(totalSalesLabel, totalItems);
            AnimateNumberChange(revenueLabel, totalRevenue);

            timeLabel.Text = $"Обновлено: {DateTime.Now:HH:mm:ss}";
        }

        private void AnimateNumberChange(Label label, decimal newValue)
        {
            var timer = new Timer { Interval = 20 };
            decimal currentValue = decimal.Parse(label.Text.Replace("₽", "").Replace(" ", ""));
            decimal step = (newValue - currentValue) / 10;

            timer.Tick += (s, e) =>
            {
                currentValue += step;
                if (Math.Abs(newValue - currentValue) < Math.Abs(step))
                {
                    currentValue = newValue;
                    timer.Stop();
                }

                if (label == revenueLabel)
                    label.Text = $"{currentValue:C}";
                else
                    label.Text = currentValue.ToString("N0");
            };
            timer.Start();
        }

        private void SetupCustomControls()
        {
            finishSaleButton = CreateStyledButton("💰 Завершить продажу", Color.FromArgb(0, 200, 255), new Point(this.Width / 2 - 150, this.Height - 135), new Size(300, 60));
            finishSaleButton.Click += finishSaleButton_Click;

            resetButton = CreateStyledButton("🔄 Сбросить", Color.FromArgb(255, 60, 100), new Point(40, this.Height - 100), new Size(160, 40));
            resetButton.Click += (s, e) => ResetCounts();

            printButton = CreateStyledButton("🖨️ Печать чека", Color.FromArgb(120, 100, 255), new Point(this.Width - 200, this.Height - 100), new Size(160, 40));
            printButton.Click += (s, e) => PrintReceipt();

            changePrice = CreateStyledButton("📝 Изменить продукты", Color.FromArgb(140, 0, 100), new Point(this.Width / 2 - 110, this.Height - 60), new Size(220, 40));
            changePrice.Click += (s, e) => ChangePrice();

            this.Controls.Add(finishSaleButton);
            this.Controls.Add(resetButton);
            this.Controls.Add(printButton);
            this.Controls.Add(changePrice);
        }

        private JeanModernButton CreateStyledButton(string text, Color backColor, Point location, Size size)
        {
            var button = new JeanModernButton
            {
                Text = text,
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.Transparent,
                BackgroundColor = backColor,
                TextColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BorderRadius = 20,
                BorderSize = 0,
                BorderColor = Color.FromArgb(255, 140, 0)
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.2f);
            button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.2f);

            return button;
        }

        private void finishSaleButton_Click(object sender, EventArgs e)
        {
            if (products.Values.Sum(p => p.Count) == 0)
            {
                ShowNotification("❌ Нет товаров для продажи!", Color.FromArgb(255, 100, 100));
                return;
            }

            if (Message.MessageWindowYesNo("Продать товары?") != DialogResult.Yes)
            {
                return;
            }


            try
            {
                using (var conn = new SQLiteConnection(ProductsContext.ConnectionStringProducts()))
                {
                    conn.Open();

                    foreach (var product in products.Values)
                    {
                        if (product.Count > 0)
                        {
                            using (var cmd = new SQLiteCommand(
                                @"INSERT INTO Items (
                                    [Товары], [Цена], [Количество], [Время_продажи]
                                ) VALUES (
                                    @Товары, @Цена, @Количество, @Время_продажи
                                )", conn))
                            {
                                cmd.Parameters.AddWithValue("@Товары", product.Name);
                                cmd.Parameters.AddWithValue("@Цена", product.Price);
                                cmd.Parameters.AddWithValue("@Количество", product.Count);
                                cmd.Parameters.AddWithValue("@Время_продажи", DateTime.Now);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                ResetCounts();
                ShowNotification("✅ Продажа завершена успешно!", Color.FromArgb(100, 255, 150));
            }
            catch (Exception ex)
            {
                ShowNotification($"❌ Ошибка сохранения: {ex.Message}", Color.FromArgb(255, 100, 100));
            }
        }

        private void ChangePrice()
        {
            using (var editForm = new EditProductsForm(products))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    CreateProductCards();
                    ShowNotification("✅ Продукты успешно обновлены!", Color.FromArgb(100, 200, 100));
                }
            }
        }

        private void ShowNotification(string message, Color color)
        {
            var indicator = new Label
            {
                Text = message,
                ForeColor = color,
                BackColor = Color.Transparent,
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                Location = new Point(this.Width - 270, 65)
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

        private void SaveSaleToDatabase()
        {
            return;
        }

        private bool signReset = false;
        private void ResetCounts()
        {
            if (!signReset)
            {
                ShowNotification("❌ Ничего не куплено!", Color.FromArgb(255, 100, 100));
                return;
            }
            foreach (var product in products.Values)
            {
               product.Count = 0;
            }

            CreateProductCards();
            UpdateStatistics();
            ShowNotification("🔃 Данные сброшены!", Color.FromArgb(140, 0, 100));
            signReset = false;
        }

        private void PrintReceipt()
        {
            if (products.Values.Sum(p => p.Count) == 0)
            {
                ShowNotification("❌ Нет товаров для печати!", Color.FromArgb(255, 100, 100));
                return;
            }

            try
            {
                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += new PrintPageEventHandler(PrintReceiptPage);

                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDoc;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDoc.Print();
                    ShowNotification("🖨️ Чек отправлен на печать!", Color.FromArgb(100, 200, 255));
                }
            }
            catch (Exception ex)
            {
                ShowNotification($"❌ Ошибка печати: {ex.Message}", Color.FromArgb(255, 100, 100));
            }
        }

        private void PrintReceiptPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Courier New", 10);
            Font titleFont = new Font("Courier New", 12, FontStyle.Bold);
            float yPos = 0;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;

            graphics.DrawString("ЧЕК ПРОДАЖИ", titleFont, Brushes.Black, leftMargin, yPos);
            yPos += titleFont.GetHeight() + 10;

            graphics.DrawString($"Дата: {DateTime.Now:dd.MM.yyyy HH:mm}", font, Brushes.Black, leftMargin, yPos);
            yPos += font.GetHeight() + 5;

            graphics.DrawLine(new Pen(Color.Black, 1), leftMargin, yPos, leftMargin + 200, yPos);
            yPos += 10;

            graphics.DrawString("ТОВАР".PadRight(20) + "КОЛ-ВО".PadRight(10) + "ЦЕНА".PadRight(10) + "СУММА",
                               font, Brushes.Black, leftMargin, yPos);
            yPos += font.GetHeight() + 5;

            foreach (var product in products.Values)
            {
                if (product.Count > 0)
                {
                    string line = $"{product.Name.PadRight(20)}" +
                                 $"{product.Count.ToString().PadRight(10)}" +
                                 $"{product.Price.ToString("C").PadRight(10)}" +
                                 $"{(product.Count * product.Price):C}";

                    graphics.DrawString(line, font, Brushes.Black, leftMargin, yPos);
                    yPos += font.GetHeight();
                }
            }

            yPos += 10;
            graphics.DrawLine(new Pen(Color.Black, 1), leftMargin, yPos, leftMargin + 200, yPos);
            yPos += 10;

            decimal total = products.Values.Sum(p => p.Count * p.Price);
            graphics.DrawString($"ИТОГО: {total:C}", titleFont, Brushes.Black, leftMargin, yPos);

            font.Dispose();
            titleFont.Dispose();
        }

        public class Product
        {
            public string Name { get; set; }
            public string Emoji
            {
                get
                {
                    switch (Name)
                    {
                        case "Вода": return "💧";
                        case "Кофе": return "☕";
                        case "Протеин": return "🥛";
                        case "Энергетик": return "⚡";
                        case "Шоколад": return "🍫";
                        case "Фрукты": return "🍎";
                        case "Снеки": return "🥨";
                        default: return "🏋️";
                    }
                }
            }
            public decimal Price { get; set; }
            public int Count { get; set; }

            public Product(string name, decimal price)
            {
                Name = name;
                Price = price;
                Count = 0;
            }
        }
    }
}