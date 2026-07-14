using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0.AnimationTools
{
    public class ScreenAnimation : IDisposable
    {
        private Form _form;
        private Timer _animationTimer;
        private Timer _particlesTimer;
        private int _glowPosition = 0;
        private const int GlowSpeed = 6;
        private List<Particle> _particles;
        private const int ParticleCount = 30;
        private double _pulseValue = 0;
        private Random _random = new Random();
        private bool _isDisposed = false;

        // Настройки анимации
        public float ParticleSpeed { get; set; } = 1.5f;
        public int AnimationInterval { get; set; } = 20;
        public int ParticlesInterval { get; set; } = 50;
        public int GlowSpeedValue { get; set; } = 6;
        public int ParticleCountValue { get; set; } = 30;

        // Цвета (можно настраивать)
        public Color PrimaryColor { get; set; } = Color.FromArgb(255, 110, 0);
        public Color LightColor { get; set; } = Color.FromArgb(255, 180, 80);
        public Color SecondaryColor { get; set; } = Color.FromArgb(128, 0, 255);

        public event EventHandler<EventArgs> AnimationTick;
        public event EventHandler<EventArgs> ParticlesTick;

        public ScreenAnimation(Form form)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));
            _particles = new List<Particle>();
            InitializeParticles();
        }

        public ScreenAnimation(Form form, Color primaryColor, Color lightColor, Color secondaryColor)
            : this(form)
        {
            PrimaryColor = primaryColor;
            LightColor = lightColor;
            SecondaryColor = secondaryColor;
        }

        private void InitializeParticles()
        {
            _particles.Clear();
            for (int i = 0; i < ParticleCountValue; i++)
            {
                _particles.Add(new Particle
                {
                    X = _random.Next(_form.Width),
                    Y = _random.Next(_form.Height),
                    Size = _random.Next(2, 6),
                    Speed = _random.Next(1, 4),
                    Color = Color.FromArgb(_random.Next(50, 150),
                        _random.Next(2) == 0 ? PrimaryColor : Color.White),
                    Direction = _random.Next(360)
                });
            }
        }

        public void StartAnimations()
        {
            StopAnimations();

            // Таймер для основной анимации
            _animationTimer = new Timer();
            _animationTimer.Interval = AnimationInterval;
            _animationTimer.Tick += AnimationTimer_Tick;
            _animationTimer.Start();

            // Таймер для частиц
            _particlesTimer = new Timer();
            _particlesTimer.Interval = ParticlesInterval;
            _particlesTimer.Tick += ParticlesTimer_Tick;
            _particlesTimer.Start();
        }

        public void StopAnimations()
        {
            _animationTimer?.Stop();
            _particlesTimer?.Stop();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            _glowPosition = (_glowPosition + GlowSpeedValue) % 400;
            _pulseValue += 0.1;
            AnimationTick?.Invoke(this, EventArgs.Empty);
        }

        private void ParticlesTimer_Tick(object sender, EventArgs e)
        {
            UpdateParticles();
            ParticlesTick?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateParticles()
        {
            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                _particles[i].Y -= _particles[i].Speed;
                _particles[i].X += (float)Math.Sin(_particles[i].Direction * Math.PI / 180) * ParticleSpeed;

                if (_particles[i].Y < -10 || _particles[i].X < -10 || _particles[i].X > _form.Width + 10)
                {
                    _particles[i] = new Particle
                    {
                        X = _random.Next(_form.Width),
                        Y = _form.Height + 10,
                        Size = _random.Next(2, 6),
                        Speed = _random.Next(1, 4),
                        Color = Color.FromArgb(_random.Next(50, 150),
                            _random.Next(2) == 0 ? PrimaryColor : Color.White),
                        Direction = _random.Next(360)
                    };
                }
            }
        }

        public void DrawParticles(Graphics g)
        {
            foreach (var particle in _particles)
            {
                using (SolidBrush brush = new SolidBrush(particle.Color))
                {
                    g.FillEllipse(brush, particle.X, particle.Y, particle.Size, particle.Size);
                }
            }
        }

        public void DrawAnimatedWaves(Graphics g, int startY, int width, int height, int amplitude = 3)
        {
            using (Pen wavePen = new Pen(Color.FromArgb(60, PrimaryColor), 1.5f))
            {
                for (int i = 0; i < 2; i++)
                {
                    int waveY = startY + i * 10;
                    int amp = (int)(amplitude * Math.Sin(_pulseValue + i * 1.2));

                    for (int x = 100; x < width - 100; x += 2)
                    {
                        int y = waveY + (int)(amp * Math.Sin(x * 0.05 + _glowPosition * 0.1));
                        g.DrawLine(wavePen, x, y, x + 2, y);
                    }
                }
            }
        }

        public void DrawGradientCircle(Graphics g, int x, int y, int size, Color centerColor)
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

        public void DrawGradientCircles(Graphics g, params (int x, int y, int size, Color color)[] circles)
        {
            foreach (var circle in circles)
            {
                DrawGradientCircle(g, circle.x, circle.y, circle.size, circle.color);
            }
        }

        public void DrawCustomProgressBar(Graphics g, ProgressBar progressBar, int x, int y, int width, int height, bool showPercent = true)
        {
            // Фон прогресс-бара
            using (SolidBrush bgBrush = new SolidBrush(PrimaryColor))
            {
                g.FillRectangle(bgBrush, x - 1, y - 1, width + 2, height + 2);
            }

            // Заполнение прогресс-бара
            int progressWidth = (int)(width * (progressBar.Value / 100.0));
            if (progressWidth > 0)
            {
                using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                    new Rectangle(x, y, progressWidth, height),
                    PrimaryColor,
                    LightColor,
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(progressBrush, x, y, progressWidth, height);
                }

                // Анимированное свечение
                using (LinearGradientBrush glowBrush = new LinearGradientBrush(
                    new Rectangle(x + _glowPosition - 100, y, 200, height),
                    Color.Transparent,
                    Color.FromArgb(80, Color.White),
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(glowBrush, x, y, progressWidth, height);
                }
            }

            // Рамка прогресс-бара
            using (Pen borderPen = new Pen(Color.FromArgb(100, PrimaryColor), 2))
            {
                g.DrawRectangle(borderPen, x - 1, y - 1, width + 2, height + 2);
            }

            // Процент завершения
            if (showPercent)
            {
                string percentText = $"{progressBar.Value}%";
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                using (Font font = new Font("Segoe UI", 9, FontStyle.Bold))
                {
                    var size = g.MeasureString(percentText, font);
                    g.DrawString(percentText, font, textBrush,
                        new PointF(x + progressWidth - size.Width - 5, y + (height - size.Height) / 2));
                }
            }
        }

        public void DrawCornerAccents(Graphics g, int cornerSize = 30, int offset = 20)
        {
            using (Pen accentPen = new Pen(Color.FromArgb(80, PrimaryColor), 2))
            {
                // Левый верхний
                g.DrawLine(accentPen, offset, offset, offset + cornerSize, offset);
                g.DrawLine(accentPen, offset, offset, offset, offset + cornerSize);

                // Правый верхний
                g.DrawLine(accentPen, _form.Width - offset, offset, _form.Width - offset - cornerSize, offset);
                g.DrawLine(accentPen, _form.Width - offset, offset, _form.Width - offset, offset + cornerSize);

                // Левый нижний
                g.DrawLine(accentPen, offset, _form.Height - offset, offset + cornerSize, _form.Height - offset);
                g.DrawLine(accentPen, offset, _form.Height - offset, offset, _form.Height - offset - cornerSize);

                // Правый нижний
                g.DrawLine(accentPen, _form.Width - offset, _form.Height - offset, _form.Width - offset - cornerSize, _form.Height - offset);
                g.DrawLine(accentPen, _form.Width - offset, _form.Height - offset, _form.Width - offset, _form.Height - offset - cornerSize);
            }
        }

        public void DrawGymElements(Graphics g, int x1, int y1, int x2, int y2)
        {
            using (Pen orangePen = new Pen(PrimaryColor, 3))
            using (Pen purplePen = new Pen(SecondaryColor, 2))
            {
                // Левая гантель
                g.DrawEllipse(orangePen, x1, y1, 40, 40);
                g.DrawEllipse(orangePen, x1, y1 + 60, 40, 40);
                g.DrawLine(orangePen, x1 + 20, y1 + 20, x1 + 20, y1 + 60);

                // Правая гантель
                g.DrawEllipse(orangePen, x2, y1, 40, 40);
                g.DrawEllipse(orangePen, x2, y1 + 60, 40, 40);
                g.DrawLine(orangePen, x2 + 20, y1 + 20, x2 + 20, y1 + 60);

                // Декоративные линии с точками
                int startX = x1 + 70;
                int endX = x2 - 20;
                int step = (endX - startX) / 5;

                for (int i = 0; i < 5; i++)
                {
                    int x = startX + i * step;
                    g.DrawLine(purplePen, x, y1 + 120, x + 30, y1 + 120);

                    // Точки на концах
                    using (SolidBrush orangeBrush = new SolidBrush(PrimaryColor))
                    {
                        g.FillEllipse(orangeBrush, x - 2, y1 + 118, 4, 4);
                        g.FillEllipse(orangeBrush, x + 32, y1 + 118, 4, 4);
                    }
                }
            }
        }

        public void DrawGradientBackground(Graphics g, Color topColor, Color bottomColor)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                _form.ClientRectangle,
                topColor,
                bottomColor,
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, _form.ClientRectangle);
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            StopAnimations();
            _animationTimer?.Dispose();
            _particlesTimer?.Dispose();
            _particles?.Clear();
            _isDisposed = true;
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

    // Расширения для удобства использования
    public static class ScreenAnimationExtensions
    {
        public static void DrawGradientBackground(this ScreenAnimation animation, Graphics g)
        {
            // Стандартный фон для LoadingScreen
            animation.DrawGradientBackground(g,
                Color.FromArgb(113, 96, 232),
                Color.FromArgb(255, 255, 255));
        }

        public static void DrawDarkGradientBackground(this ScreenAnimation animation, Graphics g)
        {
            // Стандартный фон для SplashScreen
            animation.DrawGradientBackground(g,
                Color.FromArgb(60, 0, 120),
                Color.FromArgb(30, 0, 60));
        }
    }
}