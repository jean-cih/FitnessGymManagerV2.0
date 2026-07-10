using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0.AnimationTools
{
    public class BackgroundAnimation
    {
        private Form _form;
        private List<FloatingElement> _floatingElements = new List<FloatingElement>();
        private PictureBox backgroundPicture;
        private Timer _backgroundAnimationTimer;
        private List<Point> _animatedPoints = new List<Point>();
        private Random _random = new Random();

        private readonly Color PrimaryBlue = Color.MediumSlateBlue;

        private class FloatingElement
        {
            public PointF Position { get; set; }
            public float Size { get; set; }
            public float Speed { get; set; }
            public float Angle { get; set; }
            public Color Color { get; set; }
            public float Opacity { get; set; }
            public int Type { get; set; }
        }

        public BackgroundAnimation(Form form)
        {
            _form = form;
        }

        public void CreateDynamicBackground()
        {
            backgroundPicture = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Tag = "Background"
            };

            backgroundPicture.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    _form.ClientRectangle,
                    Color.FromArgb(240, 240, 245),
                    Color.FromArgb(220, 220, 230),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, _form.ClientRectangle);
                }

                DrawBackgroundPattern(e.Graphics);
                DrawAnimatedElements(e.Graphics);
            };

            _form.Controls.Add(backgroundPicture);
            backgroundPicture.SendToBack();

            StartBackgroundAnimation();
        }

        public void CreateStaticBackground()
        {
            try
            {
                var backgroundImage = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    Tag = "Background"
                };

                // Создаем программное изображение с спортивной тематикой
                var bitmap = new Bitmap(_form.Width, _form.Height);
                using (var g = Graphics.FromImage(bitmap))
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    Color.FromArgb(245, 248, 252), // Более светлый градиент
                    Color.FromArgb(235, 238, 248),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);

                    // Добавляем абстрактные спортивные элементы
                    DrawAbstractSportsElements(g, bitmap.Width, bitmap.Height);

                    // Добавляем геометрический паттерн
                    DrawGeometricPattern(g, bitmap.Width, bitmap.Height);

                    // Добавляем легкий branding
                    DrawSubtleBranding(g, bitmap.Width, bitmap.Height);
                }

                backgroundImage.Image = bitmap;
                _form.Controls.Add(backgroundImage);
                backgroundImage.SendToBack();
            }
            catch
            {
                // Резервный вариант - простой градиент
                _form.Paint += (s, e) =>
                {
                    using (var brush = new LinearGradientBrush(
                        _form.ClientRectangle,
                        Color.FromArgb(240, 240, 245),
                        Color.FromArgb(220, 220, 230),
                        LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(brush, _form.ClientRectangle);
                    }
                };
            }
        }

        public void CreateBackground()
        {
            backgroundPicture = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Tag = "Background"
            };

            backgroundPicture.Paint += (s, e) =>
            {
                // Основной градиентный фон
                DrawMainGradient(e.Graphics);

                // Тонкий узор
                DrawSubtlePattern(e.Graphics);

                // Плавающие элементы
                DrawFloatingElements(e.Graphics);
            };

            _form.Controls.Add(backgroundPicture);
            backgroundPicture.SendToBack();

            // Инициализируем плавающие элементы
            InitializeFloatingElements();

            // Запускаем плавную анимацию
            StartCalmAnimation();
        }

        public void RemoveBackground()
        {
            if (backgroundPicture != null)
            {
                _form.Controls.Remove(backgroundPicture);
                backgroundPicture.Image?.Dispose();
                backgroundPicture.Dispose();
                backgroundPicture = null;
            }

            _backgroundAnimationTimer?.Stop();
            _backgroundAnimationTimer?.Dispose();
            _backgroundAnimationTimer = null;

            _floatingElements.Clear();
            _animatedPoints.Clear();

            _form.BackColor = Color.FromArgb(245, 248, 252);
        }

        private void DrawMainGradient(Graphics g)
        {
            using (var brush = new LinearGradientBrush(
                _form.ClientRectangle,
                Color.FromArgb(248, 248, 252),
                Color.FromArgb(240, 240, 250),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, _form.ClientRectangle);
            }
        }

        private void DrawSubtlePattern(Graphics g)
        {
            // Очень тонкая сетка
            using (var pen = new Pen(Color.FromArgb(15, PrimaryBlue), 0.5f))
            {
                for (int x = 0; x < _form.Width; x += 80)
                {
                    g.DrawLine(pen, x, 0, x, _form.Height);
                }
                for (int y = 0; y < _form.Height; y += 80)
                {
                    g.DrawLine(pen, 0, y, _form.Width, y);
                }
            }

            // Еле заметные точки в углах
            using (var brush = new SolidBrush(Color.FromArgb(10, PrimaryBlue)))
            {
                int cornerSize = 150;
                g.FillEllipse(brush, -cornerSize / 2, -cornerSize / 2, cornerSize, cornerSize);
                g.FillEllipse(brush, _form.Width - cornerSize / 2, -cornerSize / 2, cornerSize, cornerSize);
                g.FillEllipse(brush, -cornerSize / 2, _form.Height - cornerSize / 2, cornerSize, cornerSize);
                g.FillEllipse(brush, _form.Width - cornerSize / 2, _form.Height - cornerSize / 2, cornerSize, cornerSize);
            }
        }

        private void InitializeFloatingElements()
        {
            _floatingElements.Clear();

            // Создаем круги
            for (int i = 0; i < 12; i++)
            {
                _floatingElements.Add(new FloatingElement
                {
                    Position = new PointF(
                        _random.Next(_form.Width),
                        _random.Next(_form.Height)
                    ),
                    Size = _random.Next(3, 8),
                    Speed = (float)_random.NextDouble() * 0.3f + 0.1f,
                    Angle = (float)(_random.NextDouble() * Math.PI * 2),
                    Color = Color.FromArgb(120,
                        Color.FromArgb(_random.Next(180, 220),
                        _random.Next(180, 220),
                        _random.Next(200, 240))),
                    Opacity = (float)_random.Next(30, 70) / 100f,
                    Type = 0
                });
            }

            // Создаем линии
            for (int i = 0; i < 8; i++)
            {
                _floatingElements.Add(new FloatingElement
                {
                    Position = new PointF(
                        _random.Next(_form.Width),
                        _random.Next(_form.Height)
                    ),
                    Size = _random.Next(20, 60),
                    Speed = (float)_random.NextDouble() * 0.2f + 0.05f,
                    Angle = (float)(_random.NextDouble() * Math.PI * 2),
                    Color = Color.FromArgb(80, PrimaryBlue),
                    Opacity = (float)_random.Next(20, 40) / 100f,
                    Type = 1
                });
            }
        }

        private void DrawFloatingElements(Graphics g)
        {
            foreach (var element in _floatingElements)
            {
                int alpha = (int)(element.Opacity * 255);
                using (var brush = new SolidBrush(Color.FromArgb(alpha, element.Color)))
                {
                    switch (element.Type)
                    {
                        case 0: // Круги
                            float circleSize = element.Size + (float)Math.Sin(DateTime.Now.Millisecond * 0.002) * 1f;
                            g.FillEllipse(brush,
                                element.Position.X - circleSize / 2,
                                element.Position.Y - circleSize / 2,
                                circleSize, circleSize);
                            break;

                        case 1: // Линии
                            float length = element.Size;
                            float endX = element.Position.X + (float)Math.Cos(element.Angle) * length;
                            float endY = element.Position.Y + (float)Math.Sin(element.Angle) * length;

                            using (var pen = new Pen(brush, 0.8f))
                            {
                                g.DrawLine(pen, element.Position.X, element.Position.Y, endX, endY);
                            }
                            break;
                    }
                }
            }
        }

        private void StartCalmAnimation()
        {
            _backgroundAnimationTimer = new Timer();
            _backgroundAnimationTimer.Interval = 1; // 25 FPS - плавно но не быстро
            _backgroundAnimationTimer.Tick += (s, e) =>
            {
                // Медленно обновляем позиции элементов
                foreach (var element in _floatingElements)
                {
                    // Очень медленное движение
                    float moveX = (float)Math.Cos(element.Angle) * element.Speed;
                    float moveY = (float)Math.Sin(element.Angle) * element.Speed;

                    element.Position = new PointF(
                        (element.Position.X + moveX + _form.Width) % _form.Width,
                        (element.Position.Y + moveY + _form.Height) % _form.Height
                    );

                    // Очень плавное изменение прозрачности
                    element.Opacity = 0.3f + 0.2f * (float)Math.Sin(DateTime.Now.Millisecond * 0.001 + element.Position.X * 0.01f);

                    // Медленное вращение для линий
                    if (element.Type == 1)
                    {
                        element.Angle += 0.001f;
                    }
                }

                backgroundPicture.Invalidate();
            };
            _backgroundAnimationTimer.Start();
        }

        public void CreateMinimalBackground()
        {
            backgroundPicture = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Tag = "Background"
            };

            backgroundPicture.Paint += (s, e) =>
            {
                // Градиентный фон
                using (var brush = new LinearGradientBrush(
                    _form.ClientRectangle,
                    Color.FromArgb(250, 250, 255),
                    Color.FromArgb(245, 245, 250),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, _form.ClientRectangle);
                }

                // Еле заметные волны
                DrawSubtleWaves(e.Graphics);
            };

            _form.Controls.Add(backgroundPicture);
            backgroundPicture.SendToBack();

            StartWaveAnimation();
        }

        private void DrawSubtleWaves(Graphics g)
        {
            float time = DateTime.Now.Millisecond * 0.001f;

            using (var pen = new Pen(Color.FromArgb(20, PrimaryBlue), 1f))
            {
                for (int y = 0; y < _form.Height; y += 40)
                {
                    for (int x = 0; x < _form.Width; x += 2)
                    {
                        float wave = (float)Math.Sin(x * 0.02f + time) * 2f;
                        g.DrawLine(pen, x, y + wave, x + 1, y + wave);
                    }
                }
            }
        }

        private void StartWaveAnimation()
        {
            _backgroundAnimationTimer = new Timer();
            _backgroundAnimationTimer.Interval = 100;
            _backgroundAnimationTimer.Tick += (s, e) =>
            {
                backgroundPicture?.Invalidate();
            };
            _backgroundAnimationTimer.Start();
        }

        private void DrawSubtleBranding(Graphics g, int width, int height)
        {
            // Логотип в углу
            using (var font = new Font("Segoe UI", 24, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(40, 60, 100, 160)))
            {
                g.DrawString("СИБИРЯК", font, brush, width - 170, 30);
            }

            // Спортивные иконки по углам
            string[] sportsIcons = { "🏋️", "🤸", "🚴", "🏃" };
            using (var iconFont = new Font("Segoe UI Emoji", 32))
            using (var iconBrush = new SolidBrush(Color.FromArgb(25, 80, 140, 200)))
            {
                // Левый верхний
                g.DrawString(sportsIcons[0], iconFont, iconBrush, 30, 30);
                // Правый верхний
                g.DrawString(sportsIcons[1], iconFont, iconBrush, width - 70, 30);
                // Левый нижний
                g.DrawString(sportsIcons[2], iconFont, iconBrush, 30, height - 70);
                // Правый нижний
                g.DrawString(sportsIcons[3], iconFont, iconBrush, width - 70, height - 70);
            }

            // Тонкая надпись по центру
            using (var textFont = new Font("Segoe UI", 36, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.FromArgb(12, 70, 130, 190)))
            {
                string text = "ФИТНЕС КЛУБ";
                var size = g.MeasureString(text, textFont);
                g.DrawString(text, textFont, textBrush,
                    (width - size.Width) / 2,
                    Convert.ToInt32((height - size.Height) / 1.5));
            }
        }

        private void DrawHexagon(Graphics g, Pen pen, int centerX, int centerY, int size)
        {
            PointF[] points = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                double angle = 2 * Math.PI / 6 * i;
                points[i] = new PointF(
                    centerX + size * (float)Math.Cos(angle),
                    centerY + size * (float)Math.Sin(angle)
                );
            }
            g.DrawPolygon(pen, points);
        }

        private void DrawGeometricPattern(Graphics g, int width, int height)
        {
            // Шестиугольный паттерн (соты)
            using (var hexagonPen = new Pen(Color.FromArgb(15, 80, 120, 180), 1f))
            {
                int hexSize = 60;
                for (int y = -hexSize; y < height + hexSize; y += (int)(hexSize * 1.5))
                {
                    for (int x = -hexSize; x < width + hexSize; x += (int)(hexSize * Math.Sqrt(3)))
                    {
                        DrawHexagon(g, hexagonPen, x + ((y / hexSize) % 2 == 0 ? (int)(hexSize * Math.Sqrt(3) / 2) : 0), y, hexSize);
                    }
                }
            }

            // Точки в узлах сетки
            using (var dotBrush = new SolidBrush(Color.FromArgb(20, 120, 160, 220)))
            {
                for (int x = 30; x < width; x += 50)
                {
                    for (int y = 30; y < height; y += 50)
                    {
                        g.FillEllipse(dotBrush, x - 1, y - 1, 3, 3);
                    }
                }
            }
        }

        private void DrawArrowhead(Graphics g, Pen pen, int startX, int startY, int endX, int endY)
        {
            float angle = (float)Math.Atan2(endY - startY, endX - startX);
            int size = 8;

            PointF[] arrowPoints =
            {
        new PointF(endX, endY),
        new PointF(endX - size * (float)Math.Cos(angle - Math.PI/6),
                  endY - size * (float)Math.Sin(angle - Math.PI/6)),
        new PointF(endX - size * (float)Math.Cos(angle + Math.PI/6),
                  endY - size * (float)Math.Sin(angle + Math.PI/6))
    };

            g.FillPolygon(new SolidBrush(pen.Color), arrowPoints);
        }

        private void DrawAbstractSportsElements(Graphics g, int width, int height)
        {
            // Цвета для спортивных элементов
            var sportColors = new[]
            {
                Color.FromArgb(25, 65, 150, 220),  // Синий
                Color.FromArgb(20, 80, 200, 180),  // Бирюзовый
                Color.FromArgb(18, 140, 100, 220), // Фиолетовый
                Color.FromArgb(15, 230, 100, 80)   // Оранжевый
            };

            var random = new Random(42); // Фиксированное seed для статичности

            // Абстрактные круги (гантели, мячи)
            for (int i = 0; i < 12; i++)
            {
                int size = random.Next(40, 120);
                int x = random.Next(-size / 3, width - size * 2 / 3);
                int y = random.Next(-size / 3, height - size * 2 / 3);

                using (var brush = new SolidBrush(sportColors[random.Next(sportColors.Length)]))
                {
                    g.FillEllipse(brush, x, y, size, size);

                    // Внутренний круг для объема
                    if (size > 60)
                    {
                        using (var innerBrush = new SolidBrush(Color.FromArgb(40, Color.White)))
                        {
                            g.FillEllipse(innerBrush, x + size / 4, y + size / 4, size / 2, size / 2);
                        }
                    }
                }
            }

            // Абстрактные линии (траектории движения)
            using (var linePen = new Pen(Color.FromArgb(30, 100, 140, 200), 2f))
            {
                linePen.EndCap = LineCap.Round;

                for (int i = 0; i < 8; i++)
                {
                    int startX = random.Next(width);
                    int startY = random.Next(height);
                    int endX = startX + random.Next(-100, 100);
                    int endY = startY + random.Next(-100, 100);

                    g.DrawLine(linePen, startX, startY, endX, endY);

                    // Стрелочка на конце
                    DrawArrowhead(g, linePen, startX, startY, endX, endY);
                }
            }
        }

        private void StartBackgroundAnimation()
        {
            _animatedPoints.Clear();
            for (int i = 0; i < 30; i++)
            {
                _animatedPoints.Add(new Point(
                    _random.Next(_form.Width),
                    _random.Next(_form.Height)
                ));
            }

            _backgroundAnimationTimer = new Timer();
            _backgroundAnimationTimer.Interval = 50;
            _backgroundAnimationTimer.Tick += (s, e) =>
            {
                for (int i = 0; i < _animatedPoints.Count; i++)
                {
                    var point = _animatedPoints[i];
                    _animatedPoints[i] = new Point(
                        (point.X + _random.Next(-2, 3)) % _form.Width,
                        (point.Y + _random.Next(-2, 3)) % _form.Height
                    );
                }

                backgroundPicture?.Invalidate();
            };
            _backgroundAnimationTimer.Start();
        }

        private void DrawAnimatedElements(Graphics g)
        {
            foreach (var point in _animatedPoints)
            {
                float size = 3 + (float)Math.Sin(DateTime.Now.Millisecond * 0.01) * 2;
                using (var brush = new SolidBrush(Color.FromArgb(150, PrimaryBlue)))
                {
                    g.FillEllipse(brush, point.X - size / 2, point.Y - size / 2, size, size);
                }
            }
        }

        private void DrawBackgroundPattern(Graphics g)
        {
            string[] gymIcons = { "🏋️", "🤸", "🚴", "🏃", "🧘" };

            for (int i = 0; i < 20; i++)
            {
                int x = _random.Next(_form.Width);
                int y = _random.Next(_form.Height);
                float size = _random.Next(20, 40);
                float opacity = _random.Next(20, 60) / 100f;

                using (var brush = new SolidBrush(Color.FromArgb((int)(opacity * 255), Color.LightGray)))
                using (var font = new Font("Segoe UI Emoji", size))
                {
                    string icon = gymIcons[_random.Next(gymIcons.Length)];
                    g.DrawString(icon, font, brush, x, y);
                }
            }

            using (var pen = new Pen(Color.FromArgb(30, PrimaryBlue), 1))
            {
                for (int x = 0; x < _form.Width; x += 50)
                {
                    g.DrawLine(pen, x, 0, x, _form.Height);
                }
                for (int y = 0; y < _form.Height; y += 50)
                {
                    g.DrawLine(pen, 0, y, _form.Width, y);
                }
            }
        }
    }
}