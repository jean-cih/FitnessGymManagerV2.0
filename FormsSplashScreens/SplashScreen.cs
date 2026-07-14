using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static GymApplicationV2._0.AppColors.AppColors;

namespace GymApplicationV2._0
{
    public partial class SplashScreen : ShadowedForm
    {
        private int totalLines = 0;
        private int processedLines = 0;
        private List<DateTime> lineProcessingTimes = new List<DateTime>();
        private const int LinesPerMeasurement = 10;

        private ProgressBar progressBar;
        private Label titleLabel;
        private Label detailsLabel;
        private Label percentageLabel;
        private PictureBox closeButton;
        private Label labelStatus;

        private ScreenAnimation _animation;

        public SplashScreen()
        {
            InitializeComponent();
            InitializeCustomComponents();
            SetupForm();

            _animation = new ScreenAnimation(this,
            primaryColor: Color.FromArgb(255, 110, 0),
            lightColor: Color.FromArgb(255, 180, 80),
            secondaryColor: Color.FromArgb(128, 0, 255)
        );

            _animation.AnimationTick += (s, e) => this.Invalidate();
            _animation.ParticlesTick += (s, e) => this.Invalidate();
            _animation.StartAnimations();

            this.EnableDrag(this);
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
            detailsLabel.ForeColor = primaryOrange;
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
            progressBar.BackColor = Color.FromArgb(255, 255, 255);
            this.Controls.Add(progressBar);

            // Детали операции
            labelStatus = new Label();
            labelStatus.Text = string.Empty;
            labelStatus.Size = new Size(400, 25);
            labelStatus.Location = new Point(55, 180);
            labelStatus.ForeColor = primaryPurple;
            labelStatus.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
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
                // Строк обработано
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

                // Всего
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
            infoPanel.Size = new Size(350, 90);
            infoPanel.Location = new Point(75, 200);
            infoPanel.BackColor = Color.White;
            infoPanel.Paint += InfoPanel_Paint;
            this.Controls.Add(infoPanel);

            // Статистика
            var stats = new[]
            {
                new { Text = "Строк обработано:", Value = "0" },
                new { Text = "Скорость:", Value = "0 строк/сек" },
                new { Text = "Всего строк:", Value = "0" }
            };

            for (int i = 0; i < stats.Length; i++)
            {
                var label = new Label();
                label.Text = stats[i].Text;
                label.Size = new Size(120, 20);
                label.Location = new Point(20, 15 + i * 25);
                label.ForeColor = primaryPurple;
                label.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                infoPanel.Controls.Add(label);

                var valueLabel = new Label();
                valueLabel.Text = stats[i].Value;
                valueLabel.Size = new Size(100, 20);
                valueLabel.Location = new Point(150, 15 + i * 25);
                valueLabel.ForeColor = primaryOrange;
                valueLabel.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                valueLabel.Name = "stat" + i;
                infoPanel.Controls.Add(valueLabel);
            }
        }

        private void InfoPanel_Paint(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // Рамка
            using (var pen = new Pen(Color.FromArgb(80, primaryPurple), 1))
            {
                e.Graphics.DrawRectangle(pen, 2, 7, panel.Width - 5, panel.Height - 10);
            }
        }

        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = darkPurple;
            this.MouseDown += SplashScreen_MouseDown;
        }

        private void SplashScreen_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                primaryPurple,
                Color.FromArgb(255, 255, 255),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            _animation.DrawParticles(g);

            _animation.DrawAnimatedWaves(g, 170, this.Width, this.Height, 2);

            _animation.DrawGradientCircles(g,
                (100, 100, 100, Color.FromArgb(20, primaryOrange)),
                (450, 150, 80, Color.FromArgb(15, brightPurple)),
                (300, 350, 120, Color.FromArgb(10, mediumPurple))
            );

            _animation.DrawCustomProgressBar(g, progressBar, 75, 150, 350, 25);

            DrawGymElements(g);

            _animation.DrawCornerAccents(g);
        }

        private void DrawGymElements(Graphics g)
        {
            using (Pen orangePen = new Pen(primaryOrange, 3))
            using (Pen purplePen = new Pen(brightPurple, 2))
            {
                // Левая гантель
                g.DrawEllipse(orangePen, 30, 250, 30, 30);
                g.DrawEllipse(orangePen, 30, 290, 30, 30);
                g.DrawLine(orangePen, 45, 265, 45, 290);

                // Правая гантель
                g.DrawEllipse(orangePen, 440, 250, 30, 30);
                g.DrawEllipse(orangePen, 440, 290, 30, 30);
                g.DrawLine(orangePen, 455, 265, 455, 290);

                // Декоративные линии с точками
                for (int i = 0; i < 5; i++)
                {
                    int x = 120 + i * 55;
                    g.DrawLine(purplePen, x, 310, x + 20, 310);

                    // Точки на концах
                    using (SolidBrush orangeBrush = new SolidBrush(primaryOrange))
                    {
                        g.FillEllipse(orangeBrush, x - 2, 308, 4, 4);
                        g.FillEllipse(orangeBrush, x + 22, 308, 4, 4);
                    }
                }
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

                        int percent = (int)(processedLines * 100 / totalLines);
                        percentageLabel.Text = percent.ToString() + "%";
                        progressBar.Value = Math.Min(percent, 100);

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
                labelStatus.Text = message;

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
            _animation?.Dispose();
        }
    }
}