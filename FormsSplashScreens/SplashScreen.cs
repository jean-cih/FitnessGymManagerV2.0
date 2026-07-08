using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Shadow;

namespace GymApplicationV2._0
{
    public partial class SplashScreen : ShadowedForm
    {
        private Timer animationTimer;
        private Timer particlesTimer;
        private int glowPosition = 0;
        private const int GlowSpeed = 6;
        private List<Particle> particles;
        private const int ParticleCount = 25;
        private double pulseValue = 0;
        private string currentOperation = "Загрузка данных...";
        private int totalLines = 0;
        private int processedLines = 0;
        private List<DateTime> lineProcessingTimes = new List<DateTime>();
        private const int LinesPerMeasurement = 10;

        private readonly Color primaryOrange = Color.FromArgb(255, 110, 0);
        private readonly Color lightOrange = Color.FromArgb(255, 180, 80);
        private readonly Color darkPurple = Color.FromArgb(30, 0, 60);
        private readonly Color mediumPurple = Color.FromArgb(60, 0, 120);
        private readonly Color brightPurple = Color.FromArgb(128, 0, 255);

        private ProgressBar progressBar;
        private Label titleLabel;
        private Label detailsLabel;
        private Label percentageLabel;
        private PictureBox closeButton;
        private Label labelStatus;

        public SplashScreen()
        {
            InitializeComponent();
            InitializeCustomComponents();
            SetupForm();
            InitializeParticles();
            StartAnimations();
        }

        private void InitializeCustomComponents()
        {
            // Настройка формы
            this.ClientSize = new Size(500, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.Paint += SplashScreen_Paint;

            // Кнопка закрытия
            closeButton = new PictureBox();
            closeButton.Size = new Size(25, 25);
            closeButton.Location = new Point(this.Width - 35, 10);
            closeButton.BackColor = Color.Transparent;
            closeButton.Cursor = Cursors.Hand;
            closeButton.Paint += CloseButton_Paint;
            closeButton.Click += (s, e) => this.Close();
            closeButton.MouseEnter += (s, e) => closeButton.Invalidate();
            closeButton.MouseLeave += (s, e) => closeButton.Invalidate();
            this.Controls.Add(closeButton);

            // Заголовок
            titleLabel = new Label();
            titleLabel.Text = "GYM MASTER";
            titleLabel.Size = new Size(400, 50);
            titleLabel.Location = new Point(50, 40);
            titleLabel.ForeColor = Color.White;
            titleLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.BackColor = Color.Transparent;
            this.Controls.Add(titleLabel);

            // Детали операции
            detailsLabel = new Label();
            detailsLabel.Text = "Обработка файлов...";
            detailsLabel.Size = new Size(400, 25);
            detailsLabel.Location = new Point(50, 90);
            detailsLabel.ForeColor = lightOrange;
            detailsLabel.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            detailsLabel.TextAlign = ContentAlignment.MiddleCenter;
            detailsLabel.BackColor = Color.Transparent;
            this.Controls.Add(detailsLabel);

            // Прогресс-бар
            progressBar = new ProgressBar();
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Size = new Size(350, 25);
            progressBar.Location = new Point(75, 150);
            progressBar.ForeColor = primaryOrange;
            progressBar.BackColor = Color.FromArgb(40, 20, 80);
            this.Controls.Add(progressBar);

            // Детали операции
            labelStatus = new Label();
            labelStatus.Text = string.Empty;
            labelStatus.Size = new Size(400, 25);
            labelStatus.Location = new Point(55, 180);
            labelStatus.ForeColor = lightOrange;
            labelStatus.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            labelStatus.TextAlign = ContentAlignment.MiddleCenter;
            labelStatus.BackColor = Color.Transparent;
            this.Controls.Add(labelStatus);

            // Метка процентов
            percentageLabel = new Label();
            percentageLabel.Size = new Size(60, 25);
            percentageLabel.Location = new Point(220, 120);
            percentageLabel.ForeColor = primaryOrange;
            percentageLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            percentageLabel.TextAlign = ContentAlignment.MiddleCenter;
            percentageLabel.BackColor = Color.Transparent;
            percentageLabel.Text = "0%";
            this.Controls.Add(percentageLabel);

            // Информационная панель
            CreateInfoPanel();
        }

        private void CloseButton_Paint(object sender, PaintEventArgs e)
        {
            var button = (PictureBox)sender;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // Фон при наведении
            if (button.ClientRectangle.Contains(button.PointToClient(Cursor.Position)))
            {
                using (var brush = new SolidBrush(Color.FromArgb(50, 255, 255, 255)))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, button.Width, button.Height);
                }
            }

            // Крестик
            using (var pen = new Pen(Color.White, 2))
            {
                e.Graphics.DrawLine(pen, 5, 5, 20, 20);
                e.Graphics.DrawLine(pen, 20, 5, 5, 20);
            }
        }

        private void UpdateStatistics()
        {
            Panel infoPanel = this.Controls.OfType<Panel>().FirstOrDefault();
            if (infoPanel != null)
            {
                // Файлов обработано
                Label processedLabel = infoPanel.Controls.Find("stat0", true).FirstOrDefault() as Label;
                if (processedLabel != null)
                {
                    processedLabel.Text = $"{processedLines}";
                }

                // Скорость (строк в секунду)
                Label speedLabel = infoPanel.Controls.Find("stat1", true).FirstOrDefault() as Label;
                if (speedLabel != null)
                {
                    double speed = CalculateSpeed();
                    speedLabel.Text = $"{speed:F1} строк/сек";
                }

                // Осталось времени (обновляется плавно)
                Label totalLabels = infoPanel.Controls.Find("stat2", true).FirstOrDefault() as Label;
                if (totalLabels != null)
                {
                    totalLabels.Text = $"{totalLines}";
                }
            }
        }

        private double CalculateSpeed()
        {
            if (processedLines <= LinesPerMeasurement || lineProcessingTimes.Count < 2)
                return 0;

            if (lineProcessingTimes.Count >= LinesPerMeasurement)
            {
                DateTime startTime = lineProcessingTimes[lineProcessingTimes.Count - LinesPerMeasurement];
                DateTime endTime = lineProcessingTimes.Last();

                TimeSpan timeForLines = endTime - startTime;
                if (timeForLines.TotalSeconds == 0)
                    return 0;

                return (int)(LinesPerMeasurement / timeForLines.TotalSeconds);
            }

            return 0;
        }

        private void CreateInfoPanel()
        {
            Panel infoPanel = new Panel();
            infoPanel.Size = new Size(400, 90);
            infoPanel.Location = new Point(50, 220);
            infoPanel.BackColor = Color.FromArgb(40, 30, 70);
            infoPanel.Paint += InfoPanel_Paint;
            this.Controls.Add(infoPanel);

            // Статистика
            var stats = new[]
            {
                new { Text = "Строк обработано:", Value = "0" },
                new { Text = "Скорость:", Value = "0 KB/s" },
                new { Text = "Всего:", Value = "0" }
            };

            for (int i = 0; i < stats.Length; i++)
            {
                var label = new Label();
                label.Text = stats[i].Text;
                label.Size = new Size(120, 20);
                label.Location = new Point(20, 15 + i * 25);
                label.ForeColor = Color.LightGray;
                label.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                infoPanel.Controls.Add(label);

                var valueLabel = new Label();
                valueLabel.Text = stats[i].Value;
                valueLabel.Size = new Size(100, 20);
                valueLabel.Location = new Point(150, 15 + i * 25);
                valueLabel.ForeColor = lightOrange;
                valueLabel.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                valueLabel.Name = "stat" + i;
                infoPanel.Controls.Add(valueLabel);
            }
        }

        private void InfoPanel_Paint(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // Рамка
            using (var pen = new Pen(Color.FromArgb(80, primaryOrange), 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = darkPurple;
            this.MouseDown += SplashScreen_MouseDown;
        }

        private void InitializeParticles()
        {
            particles = new List<Particle>();
            Random rand = new Random();

            for (int i = 0; i < ParticleCount; i++)
            {
                particles.Add(new Particle
                {
                    X = rand.Next(this.Width),
                    Y = rand.Next(this.Height),
                    Size = rand.Next(2, 5),
                    Speed = rand.Next(1, 3),
                    Color = Color.FromArgb(rand.Next(30, 100),
                        rand.Next(2) == 0 ? primaryOrange : Color.White),
                    Direction = rand.Next(360)
                });
            }
        }

        private void StartAnimations()
        {
            animationTimer = new Timer();
            animationTimer.Interval = 20;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();

            particlesTimer = new Timer();
            particlesTimer.Interval = 60;
            particlesTimer.Tick += ParticlesTimer_Tick;
            particlesTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            glowPosition = (glowPosition + GlowSpeed) % 350;
            pulseValue += 0.08;
            this.Invalidate();
        }

        private void ParticlesTimer_Tick(object sender, EventArgs e)
        {
            UpdateParticles();
            this.Invalidate();
        }

        private void UpdateParticles()
        {
            Random rand = new Random();
            foreach (var particle in particles)
            {
                particle.Y -= particle.Speed;
                particle.X += (float)Math.Sin(particle.Direction * Math.PI / 180) * 1f;

                if (particle.Y < -10)
                {
                    particle.Y = this.Height + 10;
                    particle.X = rand.Next(this.Width);
                }
            }
        }

        private void SplashScreen_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // Градиентный фон
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                mediumPurple,
                darkPurple,
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            // Частицы
            DrawParticles(g);

            // Анимированные элементы
            DrawAnimatedElements(g);

            // Кастомный прогресс-бар
            DrawCustomProgressBar(g);

            // Декоративные элементы
            DrawDecorations(g);
        }

        private void DrawParticles(Graphics g)
        {
            foreach (var particle in particles)
            {
                using (SolidBrush brush = new SolidBrush(particle.Color))
                {
                    g.FillEllipse(brush, particle.X, particle.Y, particle.Size, particle.Size);
                }
            }
        }

        private void DrawAnimatedElements(Graphics g)
        {
            // Анимированные волны
            using (Pen wavePen = new Pen(Color.FromArgb(40, primaryOrange), 1))
            {
                int waveY = 170;
                int amplitude = (int)(2 * Math.Sin(pulseValue));

                for (int x = 75; x < 425; x += 3)
                {
                    int y = waveY + (int)(amplitude * Math.Sin(x * 0.07 + glowPosition * 0.08));
                    g.DrawLine(wavePen, x, y, x + 2, y);
                }
            }
        }

        private void DrawCustomProgressBar(Graphics g)
        {
            int progressWidth = (int)(350 * (progressBar.Value / 100.0));

            // Фон прогресс-бара
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(50, 30, 100)))
            {
                g.FillRectangle(bgBrush, 74, 149, 352, 27);
            }

            // Заполнение
            if (progressWidth > 0)
            {
                using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                    new Rectangle(75, 150, progressWidth, 25),
                    primaryOrange,
                    lightOrange,
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(progressBrush, 75, 150, progressWidth, 25);
                }

                // Свечение
                using (LinearGradientBrush glowBrush = new LinearGradientBrush(
                    new Rectangle(75 + glowPosition - 100, 150, 200, 25),
                    Color.Transparent,
                    Color.FromArgb(60, Color.White),
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(glowBrush, 75, 150, progressWidth, 25);
                }
            }

            // Рамка
            using (Pen borderPen = new Pen(Color.FromArgb(80, primaryOrange), 2))
            {
                g.DrawRectangle(borderPen, 74, 149, 352, 27);
            }
        }

        private void DrawDecorations(Graphics g)
        {
            // Угловые акценты
            using (Pen accentPen = new Pen(Color.FromArgb(60, primaryOrange), 2))
            {
                g.DrawLine(accentPen, 20, 20, 40, 20);
                g.DrawLine(accentPen, 20, 20, 20, 40);

                g.DrawLine(accentPen, this.Width - 20, 20, this.Width - 40, 20);
                g.DrawLine(accentPen, this.Width - 20, 20, this.Width - 20, 40);

                g.DrawLine(accentPen, 20, this.Height - 20, 40, this.Height - 20);
                g.DrawLine(accentPen, 20, this.Height - 20, 20, this.Height - 40);

                g.DrawLine(accentPen, this.Width - 20, this.Height - 20, this.Width - 40, this.Height - 20);
                g.DrawLine(accentPen, this.Width - 20, this.Height - 20, this.Width - 20, this.Height - 40);
            }
        }

        public void UpdateProgress(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateProgress), message);
            }
            else
            {
                if (!message.Contains("/"))
                {
                    labelStatus.Text = message;
                }

                if (message != "    Подготовление    ")
                {
                    Match match = Regex.Match(message, @"(\d+)/(\d+)");
                    if (match.Success)
                    {
                        processedLines = int.Parse(match.Groups[1].Value);
                        totalLines = int.Parse(match.Groups[2].Value);

                        percentageLabel.Text = ((int)(processedLines * 100 / totalLines)).ToString() + "%";
                        progressBar.Value = processedLines * 100 / totalLines;

                        lineProcessingTimes.Add(DateTime.Now);

                        if (lineProcessingTimes.Count > 100)
                        {
                            lineProcessingTimes.RemoveAt(0);
                        }
                    }
                }

                UpdateStatistics();
                this.Invalidate();
            }
        }

        public void SetOperationType(string operationType)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(SetOperationType), operationType);
            }
            else
            {
                detailsLabel.Text = operationType;
                currentOperation = operationType;
            }
        }

        public void CompleteOperation(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(CompleteOperation), message);
            }
            else
            {
                progressBar.Value = 100;
                percentageLabel.Text = "100%";

                // Автоматическое закрытие через 2 секунды
                Timer closeTimer = new Timer();
                closeTimer.Interval = 2000;
                closeTimer.Tick += (s, e) => { closeTimer.Stop(); this.Close(); };
                closeTimer.Start();
            }
        }

        private void SplashScreen_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            System.Windows.Forms.Message m = System.Windows.Forms.Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            animationTimer?.Stop();
            particlesTimer?.Stop();
            animationTimer?.Dispose();
            particlesTimer?.Dispose();
        }
    }

    public class Particle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Size { get; set; }
        public int Speed { get; set; }
        public Color Color { get; set; }
        public float Direction { get; set; }
    }
}