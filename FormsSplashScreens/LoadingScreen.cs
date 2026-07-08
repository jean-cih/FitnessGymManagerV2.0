using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;

namespace GymApplicationV2._0.FormsSplashScreens
{
    public partial class LoadingScreen : Form
    {
        private Timer animationTimer;
        private Timer particlesTimer;
        private int glowPosition = 0;
        private const int GlowSpeed = 6;
        private List<Particle> particles;
        private const int ParticleCount = 30;
        private double pulseValue = 0;

        // Цветовая палитра
        private readonly Color primaryOrange = Color.FromArgb(255, 110, 0);
        private readonly Color lightOrange = Color.FromArgb(255, 140, 0);
        private readonly Color darkPurple = Color.FromArgb(30, 0, 60);
        private readonly Color mediumPurple = Color.FromArgb(60, 0, 120);
        private readonly Color brightPurple = Color.FromArgb(128, 0, 255);

        System.Windows.Forms.ProgressBar progressBar1;
        System.Windows.Forms.Label labelStatus;
        System.Windows.Forms.Label labelDB;

        public LoadingScreen()
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
            this.ClientSize = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.Paint += LoadingScreen_Paint;

            // Создание и настройка ProgressBar (премиум версия)
            progressBar1 = new System.Windows.Forms.ProgressBar();
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Size = new Size(400, 25);
            progressBar1.Location = new Point(100, 300);
            progressBar1.ForeColor = primaryOrange;
            progressBar1.BackColor = Color.FromArgb(255, 255, 255);
            this.Controls.Add(progressBar1);

            // Создание и настройка Label для статуса
            labelStatus = new System.Windows.Forms.Label();
            labelStatus.Size = new Size(500, 30);
            labelStatus.Location = new Point(50, 260);
            labelStatus.ForeColor = Color.FromArgb(113, 96, 232);
            labelStatus.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            labelStatus.TextAlign = ContentAlignment.MiddleCenter;
            labelStatus.BackColor = Color.Transparent;
            this.Controls.Add(labelStatus);

            labelDB = new System.Windows.Forms.Label();
            labelDB.Size = new Size(500, 30);
            labelDB.Location = new Point(50, 340);
            labelDB.ForeColor = Color.FromArgb(113, 96, 232);
            labelDB.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            labelDB.TextAlign = ContentAlignment.MiddleCenter;
            labelDB.BackColor = Color.Transparent;
            this.Controls.Add(labelDB);

            // Создание и настройка Label для названия
            System.Windows.Forms.Label titleLabel = new System.Windows.Forms.Label();
            titleLabel.Text = "GYM MASTER";
            titleLabel.Size = new Size(500, 60);
            titleLabel.Location = new Point(50, 100);
            titleLabel.ForeColor = Color.White;
            titleLabel.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.BackColor = Color.Transparent;
            this.Controls.Add(titleLabel);

            // Создание и настройка Label для подзаголовка
            System.Windows.Forms.Label subtitleLabel = new System.Windows.Forms.Label();
            subtitleLabel.Text = "Фитнес менеджмент система";
            subtitleLabel.Size = new Size(400, 30);
            subtitleLabel.Location = new Point(100, 170);
            subtitleLabel.ForeColor = primaryOrange;
            subtitleLabel.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            subtitleLabel.BackColor = Color.Transparent;
            this.Controls.Add(subtitleLabel);
        }
        
        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.BackColor = darkPurple;
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
                    Size = rand.Next(2, 6),
                    Speed = rand.Next(1, 4),
                    Color = Color.FromArgb(rand.Next(50, 150),
                        rand.Next(2) == 0 ? primaryOrange : Color.White),
                    Direction = rand.Next(360)
                });
            }
        }

        private void StartAnimations()
        {
            // Таймер для основной анимации
            animationTimer = new Timer();
            animationTimer.Interval = 20;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();

            // Таймер для частиц
            particlesTimer = new Timer();
            particlesTimer.Interval = 50;
            particlesTimer.Tick += ParticlesTimer_Tick;
            particlesTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            glowPosition = (glowPosition + GlowSpeed) % 400;
            pulseValue += 0.1;
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
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Y -= particles[i].Speed;
                particles[i].X += (float)Math.Sin(particles[i].Direction * Math.PI / 180) * 1.5f;

                if (particles[i].Y < -10 || particles[i].X < -10 || particles[i].X > this.Width + 10)
                {
                    particles[i] = new Particle
                    {
                        X = rand.Next(this.Width),
                        Y = this.Height + 10,
                        Size = rand.Next(2, 6),
                        Speed = rand.Next(1, 4),
                        Color = Color.FromArgb(rand.Next(50, 150),
                            rand.Next(2) == 0 ? primaryOrange : Color.White),
                        Direction = rand.Next(360)
                    };
                }
            }
        }

        private void LoadingScreen_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // Градиентный фон
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(113, 96, 232),
                Color.FromArgb(255, 255, 255),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            // Рисуем частицы
            DrawParticles(g);

            // Рисуем анимированные элементы
            DrawAnimatedElements(g);

            // Рисуем кастомный прогресс-бар
            DrawCustomProgressBar(g);

            // Декоративные элементы
            DrawGymElements(g);

            // Угловые акценты
            DrawCornerAccents(g);
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
            DrawAnimatedWaves(g);

            // Градиентные круги на заднем плане
            DrawGradientCircles(g);
        }

        private void DrawAnimatedWaves(Graphics g)
        {
            using (Pen wavePen = new Pen(Color.FromArgb(60, primaryOrange), 1.5f))
            {
                for (int i = 0; i < 2; i++)
                {
                    int waveY = 380 + i * 10;
                    int amplitude = (int)(3 * Math.Sin(pulseValue + i * 1.2));

                    for (int x = 100; x < 500; x += 2)
                    {
                        int y = waveY + (int)(amplitude * Math.Sin(x * 0.05 + glowPosition * 0.1));
                        g.DrawLine(wavePen, x, y, x + 2, y);
                    }
                }
            }
        }

        private void DrawGradientCircles(Graphics g)
        {
            DrawGradientCircle(g, 100, 100, 100, Color.FromArgb(20, primaryOrange));
            DrawGradientCircle(g, 500, 150, 80, Color.FromArgb(15, brightPurple));
            DrawGradientCircle(g, 300, 350, 120, Color.FromArgb(10, mediumPurple));
        }

        private void DrawGradientCircle(Graphics g, int x, int y, int size, Color centerColor)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(x - size / 2, y - size / 2, size, size);
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = centerColor;
                    brush.SurroundColors = new Color[] { Color.Transparent };
                    g.FillEllipse(brush, x - size / 2, y - size / 2, size, size);
                }
            }
        }

        private void DrawCustomProgressBar(Graphics g)
        {
            // Фон прогресс-бара
            using (SolidBrush bgBrush = new SolidBrush(primaryOrange))
            {
                g.FillRectangle(bgBrush, 99, 299, 402, 27);
            }

            // Заполнение прогресс-бара
            int progressWidth = (int)progressBar1.Value;
            if (progressWidth > 0)
            {
                using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                    new Rectangle(100, 300, progressWidth, 25),
                    primaryOrange,
                    lightOrange,
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(progressBrush, 100, 300, progressWidth, 25);
                }

                // Анимированное свечение
                using (LinearGradientBrush glowBrush = new LinearGradientBrush(
                    new Rectangle(100 + glowPosition - 100, 300, 200, 25),
                    Color.Transparent,
                    Color.FromArgb(80, Color.White),
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(glowBrush, 100, 300, progressWidth, 25);
                }
            }

            // Рамка прогресс-бара
            using (Pen borderPen = new Pen(Color.FromArgb(100, primaryOrange), 2))
            {
                g.DrawRectangle(borderPen, 99, 299, 402, 27);
            }

            // Процент завершения
            string percentText = $"{progressBar1.Value}%";
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            using (Font font = new Font("Segoe UI", 9, FontStyle.Bold))
            {
                g.DrawString(percentText, font, textBrush,
                    new PointF(100 + progressWidth - 25, 303));
            }
        }

        private void DrawGymElements(Graphics g)
        {
            // Рисуем стилизованные гантели
            using (Pen orangePen = new Pen(primaryOrange, 3))
            using (Pen purplePen = new Pen(brightPurple, 2))
            {
                // Левая гантель
                g.DrawEllipse(orangePen, 50, 50, 40, 40);
                g.DrawEllipse(orangePen, 50, 110, 40, 40);
                g.DrawLine(orangePen, 70, 70, 70, 110);

                // Правая гантель
                g.DrawEllipse(orangePen, 510, 50, 40, 40);
                g.DrawEllipse(orangePen, 510, 110, 40, 40);
                g.DrawLine(orangePen, 530, 70, 530, 110);

                // Декоративные линии с точками
                for (int i = 0; i < 5; i++)
                {
                    int x = 160 + i * 60;
                    g.DrawLine(purplePen, x, 200, x + 30, 200);

                    // Точки на концах
                    g.FillEllipse(Brushes.Orange, x - 2, 198, 4, 4);
                    g.FillEllipse(Brushes.Orange, x + 32, 198, 4, 4);
                }
            }
        }

        private void DrawCornerAccents(Graphics g)
        {
            using (Pen accentPen = new Pen(Color.FromArgb(80, primaryOrange), 2))
            {
                // Угловые линии
                g.DrawLine(accentPen, 20, 20, 50, 20);
                g.DrawLine(accentPen, 20, 20, 20, 50);

                g.DrawLine(accentPen, this.Width - 20, 20, this.Width - 50, 20);
                g.DrawLine(accentPen, this.Width - 20, 20, this.Width - 20, 50);

                g.DrawLine(accentPen, 20, this.Height - 20, 50, this.Height - 20);
                g.DrawLine(accentPen, 20, this.Height - 20, 20, this.Height - 50);

                g.DrawLine(accentPen, this.Width - 20, this.Height - 20, this.Width - 50, this.Height - 20);
                g.DrawLine(accentPen, this.Width - 20, this.Height - 20, this.Width - 20, this.Height - 50);
            }
        }

        public void UpdateProgress(string message, string messageDB, int progress)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action<string, string, int>(UpdateProgress), message, messageDB, progress);
            }
            else
            {
                labelStatus.Text = message;
                labelDB.Text = messageDB;
                progressBar1.Value = progress < 0 ? 0 : progress > 100 ? 100 : progress;

                this.Invalidate();
                Application.DoEvents();
            }
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