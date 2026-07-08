using GymApplicationV2._0.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class SaleSummaryForm : Form
    {
        private Dictionary<string, Products.Product> products;
        private Timer animationTimer;
        private float animationProgress = 0f;

        // Элементы управления
        private JeanPanel mainPanel;
        private Label titleLabel;
        private FlowLayoutPanel itemsPanel;
        private Label totalLabel;
        private Label subtotalLabel;
        private Label taxLabel;
        private JeanModernButton confirmButton;
        private JeanModernButton cancelButton;
        private Label timeLabel;
        private Label saleIdLabel;

        public SaleSummaryForm(Dictionary<string, Products.Product> products)
        {
            this.products = products;
            InitializeComponent();
            InitializeDesign();
            SetupStyling();
            LoadSaleData();
            InitializeAnimations();
        }

        private void InitializeDesign()
        {
            this.SuspendLayout();

            // Настройка формы
            this.Text = "Сводка продажи";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(15, 12, 41);
            this.Padding = new Padding(20);
            this.DoubleBuffered = true;

            // Главная панель
            mainPanel = new JeanPanel
            {
                Size = new Size(460, 560),
                Location = new Point(10, 10),
                BorderRadius = 20,
                Dock = DockStyle.Fill
            };

            // Заголовок
            titleLabel = new Label
            {
                Text = "🧾 СВОДКА ПРОДАЖИ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 220, 255),
                Size = new Size(400, 50),
                Location = new Point(30, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ID продажи
            saleIdLabel = new Label
            {
                Text = $"№ {DateTime.Now:yyyyMMddHHmmss}",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(180, 180, 255),
                Size = new Size(400, 20),
                Location = new Point(30, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Время
            timeLabel = new Label
            {
                Text = $"Время: {DateTime.Now:dd.MM.yyyy HH:mm:ss}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(150, 150, 200),
                Size = new Size(400, 20),
                Location = new Point(30, 95),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Панель элементов
            itemsPanel = new FlowLayoutPanel
            {
                Size = new Size(400, 300),
                Location = new Point(30, 130),
                AutoScroll = true,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            // Итоговая информация
            subtotalLabel = new Label
            {
                Text = "Промежуточный итог: 0₽",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.White,
                Size = new Size(400, 25),
                Location = new Point(30, 450),
                TextAlign = ContentAlignment.MiddleLeft
            };

            taxLabel = new Label
            {
                Text = "НДС (20%): 0₽",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.White,
                Size = new Size(400, 25),
                Location = new Point(30, 475),
                TextAlign = ContentAlignment.MiddleLeft
            };

            totalLabel = new Label
            {
                Text = "ИТОГО: 0₽",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 220, 255),
                Size = new Size(400, 35),
                Location = new Point(30, 500),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Кнопки
            confirmButton = new JeanModernButton
            {
                Text = "✅ ПОДТВЕРДИТЬ ПРОДАЖУ",
                Size = new Size(180, 45),
                Location = new Point(50, 550),
                BackColor = Color.FromArgb(80, 220, 120),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            cancelButton = new JeanModernButton
            {
                Text = "❌ ОТМЕНА",
                Size = new Size(180, 45),
                Location = new Point(250, 550),
                BackColor = Color.FromArgb(220, 80, 80),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Добавление элементов на форму
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(saleIdLabel);
            mainPanel.Controls.Add(timeLabel);
            mainPanel.Controls.Add(itemsPanel);
            mainPanel.Controls.Add(subtotalLabel);
            mainPanel.Controls.Add(taxLabel);
            mainPanel.Controls.Add(totalLabel);
            mainPanel.Controls.Add(confirmButton);
            mainPanel.Controls.Add(cancelButton);

            this.Controls.Add(mainPanel);

            // Обработчики событий
            confirmButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            };

            cancelButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            };

            // Закрытие по ESC
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.DialogResult = DialogResult.No;
                    this.Close();
                }
            };

            this.ResumeLayout();
        }

        private void SetupStyling()
        {
            // Дополнительные настройки стиля
        }

        private void LoadSaleData()
        {
            itemsPanel.Controls.Clear();

            decimal subtotal = 0;
            int totalItems = 0;

            // Добавляем только товары с ненулевым количеством
            foreach (var product in products.Values.Where(p => p.Count > 0))
            {
                decimal itemTotal = product.Count * product.Price;
                subtotal += itemTotal;
                totalItems += product.Count;

                var itemPanel = CreateItemPanel(product);
                itemsPanel.Controls.Add(itemPanel);
            }

            // Расчет налогов и итогов
            decimal tax = subtotal * 0.20m; // 20% НДС
            decimal total = subtotal + tax;

            // Анимированное обновление сумм
            AnimateLabelChange(subtotalLabel, $"Промежуточный итог: {subtotal:C}");
            AnimateLabelChange(taxLabel, $"НДС (20%): {tax:C}");
            AnimateLabelChange(totalLabel, $"ИТОГО: {total:C}");

            // Обновление заголовка
            titleLabel.Text = $"🧾 СВОДКА ПРОДАЖИ ({totalItems} шт.)";
        }

        private Panel CreateItemPanel(Products.Product product)
        {
            var panel = new JeanPanel
            {
                Size = new Size(380, 60),
                Margin = new Padding(5),
                BorderRadius = 10,
            };

            // Эмодзи и название
            var emojiLabel = new Label
            {
                Text = product.Emoji,
                Font = new Font("Segoe UI", 14),
                Location = new Point(10, 15),
                Size = new Size(30, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var nameLabel = new Label
            {
                Text = product.Name,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 10),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var priceLabel = new Label
            {
                Text = $"{product.Price:C} × {product.Count}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(180, 180, 255),
                Location = new Point(50, 30),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var totalLabel = new Label
            {
                Text = $"{(product.Price * product.Count):C}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 220, 255),
                Location = new Point(250, 15),
                Size = new Size(100, 30),
                TextAlign = ContentAlignment.MiddleRight
            };

            panel.Controls.Add(emojiLabel);
            panel.Controls.Add(nameLabel);
            panel.Controls.Add(priceLabel);
            panel.Controls.Add(totalLabel);

            return panel;
        }

        private void AnimateLabelChange(Label label, string newText)
        {
            var timer = new Timer { Interval = 30 };
            int steps = 10;
            int currentStep = 0;

            // Простая анимация изменения текста
            timer.Tick += (s, e) =>
            {
                currentStep++;
                if (currentStep >= steps)
                {
                    label.Text = newText;
                    timer.Stop();

                    // Анимация выделения
                    var highlightTimer = new Timer { Interval = 200 };
                    var originalColor = label.ForeColor;
                    label.ForeColor = Color.FromArgb(120, 255, 150);

                    highlightTimer.Tick += (s2, e2) =>
                    {
                        label.ForeColor = originalColor;
                        highlightTimer.Stop();
                    };
                    highlightTimer.Start();
                }
            };
            timer.Start();
        }

        private void InitializeAnimations()
        {
            animationTimer = new Timer { Interval = 60 };
            animationTimer.Tick += (s, e) =>
            {
                animationProgress += 0.03f;
                if (animationProgress >= 1f) animationProgress = 0f;
                this.Invalidate();
            };
            animationTimer.Start();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            // Анимированный фон с частицами
            DrawAnimatedBackground(e.Graphics);
        }

        private void DrawAnimatedBackground(Graphics g)
        {
            // Градиентный фон
            using (var brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(15, 12, 41),
                Color.FromArgb(48, 43, 98),
                45f))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            // Анимированные элементы
            DrawFloatingParticles(g);
        }

        private void DrawFloatingParticles(Graphics g)
        {
            var rnd = new Random(1); // Фиксированный seed для повторяемости
            for (int i = 0; i < 15; i++)
            {
                float x = (float)(Math.Sin(animationProgress * 8 + i * 0.5) * 50 + this.Width / 2);
                float y = (float)(Math.Cos(animationProgress * 6 + i * 0.3) * 30 + this.Height / 2);
                float size = 1 + (float)Math.Sin(animationProgress * 4 + i) * 0.8f;

                using (var brush = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
                {
                    g.FillEllipse(brush, x, y, size, size);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Анимация появления
            this.Opacity = 0;
            var fadeTimer = new Timer { Interval = 20 };
            fadeTimer.Tick += (s, ev) =>
            {
                this.Opacity += 0.05f;
                if (this.Opacity >= 1f)
                    fadeTimer.Stop();
            };
            fadeTimer.Start();
        }
    }
}