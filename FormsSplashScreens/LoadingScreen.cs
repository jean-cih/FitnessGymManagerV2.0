using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Helpers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static GymApplicationV2._0.AppColors.AppColors;

namespace GymApplicationV2._0.FormsSplashScreens
{
    public partial class LoadingScreen : Form
    {
        private ScreenAnimation _animation;

        ProgressBar progressBar1;
        Label labelStatus;
        Label labelDB;

        public LoadingScreen()
        {
            InitializeComponent();
            InitializeCustomComponents();
            SetupForm();

            _animation = new ScreenAnimation(this);
            _animation.AnimationTick += (s, e) => this.Invalidate();
            _animation.ParticlesTick += (s, e) => this.Invalidate();
            _animation.StartAnimations();
        }

        private void InitializeCustomComponents()
        {
            // Настройка формы
            this.ClientSize = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.Paint += LoadingScreen_Paint;

            // Создание и настройка ProgressBar (премиум версия)
            progressBar1 = new ProgressBar();
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Size = new Size(400, 25);
            progressBar1.Location = new Point(100, 300);
            progressBar1.ForeColor = primaryOrange;
            progressBar1.BackColor = Color.FromArgb(255, 255, 255);
            this.Controls.Add(progressBar1);

            // Создание и настройка Label для статуса
            labelStatus = new Label();
            labelStatus.Size = new Size(500, 30);
            labelStatus.Location = new Point(50, 260);
            labelStatus.ForeColor = Color.FromArgb(113, 96, 232);
            labelStatus.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            labelStatus.TextAlign = ContentAlignment.MiddleCenter;
            labelStatus.BackColor = Color.Transparent;
            this.Controls.Add(labelStatus);

            labelDB = new Label();
            labelDB.Size = new Size(500, 30);
            labelDB.Location = new Point(50, 340);
            labelDB.ForeColor = Color.FromArgb(113, 96, 232);
            labelDB.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            labelDB.TextAlign = ContentAlignment.MiddleCenter;
            labelDB.BackColor = Color.Transparent;
            this.Controls.Add(labelDB);

            // Создание и настройка Label для названия
            Label titleLabel = new Label();
            titleLabel.Text = "GYM MASTER";
            titleLabel.Size = new Size(500, 60);
            titleLabel.Location = new Point(50, 100);
            titleLabel.ForeColor = Color.White;
            titleLabel.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.BackColor = Color.Transparent;
            this.Controls.Add(titleLabel);

            // Создание и настройка Label для подзаголовка
            Label subtitleLabel = new Label();
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

        private void LoadingScreen_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // Градиентный фон
            _animation.DrawGradientBackground(g);

            // Частицы
            _animation.DrawParticles(g);

            // Анимированные элементы
            _animation.DrawAnimatedWaves(g, 380, this.Width, this.Height);
            _animation.DrawGradientCircles(g,
                (100, 100, 100, Color.FromArgb(20, _animation.PrimaryColor)),
                (500, 150, 80, Color.FromArgb(15, _animation.SecondaryColor)),
                (300, 350, 120, Color.FromArgb(10, Color.FromArgb(60, 0, 120)))
            );

            // Кастомный прогресс-бар
            _animation.DrawCustomProgressBar(g, progressBar1, 100, 300, 400, 25);

            // Декоративные элементы
            _animation.DrawGymElements(g, 50, 50, 510, 50);
            _animation.DrawCornerAccents(g);
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
            _animation?.Dispose();
        }
    }   
}