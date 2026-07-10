using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsSettings
{
    public partial class Documentation : ShadowedForm
    {
        private TabControl tabControl;
        private JeanModernButton userGuideBtn;
        private JeanModernButton faqBtn;
        private JeanModernButton supportBtn;
        private JeanPanel indicatorPanel;
        private JeanPanel navigationPanel;

        Panel titlePanel;

        private FadeAnimation _fadeAnimation;

        public Documentation()
        {
            InitializeComponent();
            InitializeComponents();

            SubscribeEvents();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            ApplySettings();
        }

        private void ApplySettings()
        {
            string[] notChangeableTexts = new string[]
            {
                "📚 Документация и справка"
            };

            FontHelper.ApplyFontSettings(this, notChangeableTexts);
        }

        private void SubscribeEvents()
        {
            titlePanel.MouseDown += Panel_MouseDown;
            titlePanel.MouseMove += Panel_MouseMove;
            titlePanel.MouseUp += Panel_MouseUp;
        }

        // В вашем классе формы объявляем переменные для хранения состояния перетаскивания
        private bool isDragging = false;
        private Point lastCursor;
        private Point lastForm;

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursor = Cursor.Position;
                lastForm = this.Location;
            }
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // Вычисляем смещение курсора
                Point diff = Point.Subtract(Cursor.Position, new Size(lastCursor));
                // Перемещаем форму
                this.Location = Point.Add(lastForm, new Size(diff));
            }
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
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

                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(80, 120, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
                }
            };

            titlePanel = new Panel
            {
                Size = new Size(1048, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0),
            };

            // Заголовок
            var titleLabel = new Label
            {
                Text = "📚 Документация и справка",
                Font = new Font("Montserrat", 18, FontStyle.Bold),
                ForeColor = ForeColor = Color.FromArgb(220, 220, 255),
                BackColor = Color.Transparent,
                Location = new Point(320, 10),
                AutoSize = true,
            };

            // Панель навигации
            navigationPanel = new JeanPanel
            {
                Size = new Size(1008, 30),
                Location = new Point(20, 50),
                BorderRadius = 10,
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
            };

            InitializeNavigation();

            // TabControl
            tabControl = new TabControl
            {
                //Dock = DockStyle.Fill,
                Appearance = TabAppearance.FlatButtons,
                ItemSize = new Size(0, 1),
                SizeMode = TabSizeMode.Fixed,
                Padding = new Point(0, 0),
                Size = new Size(1008, 513),
                Location = new Point(20, 80)
            };

            // Вкладки
            InitializeTabs();

            this.Controls.Add(tabControl);
            this.Controls.Add(navigationPanel);
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

            btnClose = CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(1008, 10), new Size(30, 28));
            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);
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

        private void InitializeNavigation()
        {
            userGuideBtn = new JeanModernButton
            {
                Text = "📖 Руководство",
                Dock = DockStyle.Left,
                Size = new Size(150, 40),
                Margin = new Padding(10, 5, 0, 5),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(255, 140, 0),
                Font = new Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12: DataConfig.sizeFontButtons, FontStyle.Regular),
                BorderRadius = 0,
            };
            userGuideBtn.Click += (s, e) => SwitchTab(0);

            faqBtn = new JeanModernButton
            {
                Text = "❓ FAQ",
                Dock = DockStyle.Left,
                Size = new Size(120, 40),
                Margin = new Padding(10, 5, 0, 5),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(255, 140, 0),
                Font = new Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Regular),
                BorderRadius = 0,
            };
            faqBtn.Click += (s, e) => SwitchTab(1);

            supportBtn = new JeanModernButton
            {
                Text = "💬 Поддержка",
                Dock = DockStyle.Left,
                Size = new Size(150, 40),
                Margin = new Padding(10, 5, 0, 5),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(255, 140, 0),
                Font = new Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Regular),
                BorderRadius = 0,
            };
            supportBtn.Click += (s, e) => SwitchTab(2);

            // Индикатор выбранной вкладки
            indicatorPanel = new JeanPanel
            {
                Height = 3,
                Width = 150,
                Location = new Point(10, 47),
                BorderRadius = 20
            };

            navigationPanel.Controls.Add(indicatorPanel);
            navigationPanel.Controls.Add(supportBtn);
            navigationPanel.Controls.Add(faqBtn);
            navigationPanel.Controls.Add(userGuideBtn);
        }

        private void InitializeTabs()
        {
            // Вкладка руководства пользователя
            var userGuideTab = new TabPage();
            InitializeUserGuideTab(userGuideTab);
            tabControl.TabPages.Add(userGuideTab);

            // Вкладка FAQ
            var faqTab = new TabPage();
            InitializeFaqTab(faqTab);
            tabControl.TabPages.Add(faqTab);

            // Вкладка поддержки
            var supportTab = new TabPage();
            InitializeSupportTab(supportTab);
            tabControl.TabPages.Add(supportTab);
        }

        private void InitializeUserGuideTab(TabPage tabPage)
        {
            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(240, 240, 250)
            };

            var contentPanel = new Panel
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(30),
                BackColor = Color.FromArgb(240, 240, 250)
            };

            var sections = new[]
            {
                new {
                    Title = "🎨 Настройки дизайна",
                    Content = @"• Изменение размеров шрифтов кнопок, таблиц и надписей
• Выбор стиля интерфейса (UserStyle, SimpleDark, TelegramStyle)
• Выбор стиля заднего фона (Dynamic, Casual, Minimal, Static, None)
• Сохранение пользовательских настроек"
                },
                new {
                    Title = "📊 Импорт данных",
                    Content = @"• Подготовка Excel файлов (.xls, .xlsx, .xlsm)
• Поддерживаемые таблицы: Clients, Services, Archive, Payments, IssuedMembership
• Автоматическое создание структуры базы данных
• Валидация данных перед импортом
• Отслеживание прогресса импорта"
                },
                new {
                    Title = "💾 Управление базой данных",
                    Content = @"• Ручное резервное копирование
• Очистка и оптимизация базы данных
• Экспорт данных в Excel, .txt, .json, .csv, .tsv форматы"
                },
                new {
                    Title = "👥 Управление клиентами",
                    Content = @"• Добавление и редактирование клиентов
• История покупок и посещений
• Система абонементов и скидок
• Напоминания об окончании абонементов
• Статистика и аналитика"
                },
                new {
                    Title = "🗃️ Структура базы данных и импорт из Excel",
                    Content = @"• Все таблицы используют кодировку UTF-8 для поддержки кириллицы
• Формат дат: DD.MM.YYYY (текстовый формат для удобства импорта)
• Числовые поля используют INTEGER тип для целых чисел

Структуры таблиц для импорта:

📋 Таблица Contacts (Клиенты):
- Фамилия, Имя, Отчество (TEXT)
- Телефон, Email (TEXT)
- Пол, Дата_рождения (TEXT)
- №Карты, Покупки, Скидка (TEXT/INTEGER)
- Сохранено (флаг архивации)

📁 Таблица Archive (Архив абонементов):
- Клиент, №Карты (TEXT)
- Дата_окончания, Абонемент (TEXT)
- Оплата, Посещений_осталось (INTEGER)

📊 Таблица History (История покупок):
- Клиент, Абонемент (TEXT)
- Дата_начала, Дата_окончаний, Дата_платежа (TEXT)
- Цена (INTEGER)

🎫 Таблица Issued (Активные абонементы):
- Клиент, №Карты, Абонемент (TEXT)
- Дата_окончания, Дата_оформления (TEXT)
- Посетил, Статус, Посещений_осталось (TEXT)
- Оплата (INTEGER)
- Окончание_заморозки (TEXT)

💰 Таблица Descriptions (Виды абонементов):
- Наименование (TEXT)
- Цена, Количество, Проданных_за_месяц (INTEGER)
- Срок_действия (TEXT)

Требования к Excel файлам:
• Соответствие названий столбцов структуре БД
• Первая строка - заголовки столбцов
• Данные начиная со второй строки
• Поддержка кириллических названий полей"
}
            };

            int yPos = 0;
            foreach (var section in sections)
            {
                var sectionPanel = CreateSectionPanel(section.Title, section.Content, yPos);
                contentPanel.Controls.Add(sectionPanel);
                yPos += sectionPanel.Height + 10;
            }

            contentPanel.Height = yPos;
            scrollPanel.Controls.Add(contentPanel);
            tabPage.Controls.Add(scrollPanel);
        }

        private void InitializeFaqTab(TabPage tabPage)
        {
            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(240, 240, 250)
            };

            var contentPanel = new Panel
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(30),
                BackColor = Color.FromArgb(240, 240, 250)
            };

            var faqItems = new[]
            {
                new
                {
                    Question = "Какие форматы экспорта отчетов поддерживаются?",
                    Answer = "Приложение поддерживает экспорт отчетов в форматы: Excel (.xls), TSV (.tsv), CSV (.csv), Text (.txt) и JSON (.json). Вы можете выбрать нужный формат в разделе 'Отчёт' → 'Формат экспорта'."
                },
                new
                {
                    Question = "Какие таблицы можно импортировать из Excel?",
                    Answer = "Поддерживается импорт следующих таблиц: Clients (клиенты), Services (услуги), Archive (архив), Payments (платежи), IssuedMembership (выданные абонементы). Файл должен содержать заголовки столбцов в первой строке."
                },
                new
                {
                    Question = "Как настроить размер шрифта в интерфейсе?",
                    Answer = "Размер шрифта можно настроить в разделе 'Настройки дизайна'. Доступны настройки для: шрифта кнопок, шрифта таблиц и шрифта надписей. Изменения применяются сразу после сохранения."
                },
                new
                {
                    Question = "Какие типы отчетов доступны?",
                    Answer = "Доступны отчеты по посещаемости (все клиенты, посещаемость по дням) и по абонементам/услугам (количество проданных, история платежей). Отчеты можно формировать за различные периоды: день, неделя, месяц или произвольный период."
                },
                new
                {
                    Question = "Как работает система абонементов?",
                    Answer = "Система поддерживает различные типы абонементов: студенческий, семейный, самостоятельные тренировки, персональные тренировки, безлимитный и разовый. Для каждого абонемента указывается цена, срок действия и количество посещений."
                },
                new
                {
                    Question = "Как добавить новый продукт для продажи?",
                    Answer = "Новые продукты (вода, батончики, кофе и т.д.) добавляются в разделе 'Товары'. Для каждого продукта указывается наименование, цена и количество на складе."
                },
                new
                {
                    Question = "Как отметить посещение клиента?",
                    Answer = "Для отметки посещения перейдите в раздел 'Клиенты' и введите номер карты клиента. Система автоматически проверит действующий абонемент и уменьшит количество оставшихся посещений."
                },
                new
                {
                    Question = "Что делать если абонемент закончился?",
                    Answer = "При окончании абонемента система автоматически переносит его в архив и уведомляет администратора. Клиенту будет предложено приобрести новый абонемент в разделе 'Продажи'."
                },
                new
                {
                    Question = "Как просмотреть историю платежей?",
                    Answer = "История всех платежей доступна в разделе 'История платежей'. Можно фильтровать платежи по клиентам, дате и типу абонемента."
                },
                new
                {
                    Question = "Как работает заморозка абонементов?",
                    Answer = "Абонементы можно временно заморозить через раздел 'Выданные абонементы'. В период заморозки срок действия абонемента приостанавливается. После разморозки абонемент продолжает действовать."
                },
                new
                {
                    Question = "Какие стили интерфейса доступны?",
                    Answer = "Доступны различные стили интерфейса, включая UserStyle, SimpleDark и TelegramStyle. Стиль можно изменить в разделе 'Настройки дизайна'."
                },
                new
                {
                    Question = "Как импортировать данные из старой системы?",
                    Answer = "Данные импортируются через раздел 'Импорт данных'. Подготовьте Excel-файлы согласно шаблонам (Clients, Services, Archive, Payments, IssuedMembership) и выполните импорт."
                },
                new
                {
                    Question = "Как создать резервную копию данных?",
                    Answer = "Резервное копирование выполняется автоматически при каждом закрытии приложения. Копии сохраняются в папке Databases в корневой директории приложения. Также можно выполнить ручное копирование через раздел 'Отчет' → 'Экспорт'."
                },
                new
                {
                    Question = "Как добавить новый тип абонемента?",
                    Answer = "Новые типы абонементов добавляются в разделе 'Услуги' → 'Добавить'. Укажите наименование, цену, срок действия и количество посещений для нового абонемента."
                }
            };

            int yPos = 0;
            foreach (var item in faqItems)
            {
                var faqPanel = CreateFaqPanel(item.Question, item.Answer, yPos);
                contentPanel.Controls.Add(faqPanel);
                yPos += faqPanel.Height + 10;
            }

            contentPanel.Height = yPos;
            scrollPanel.Controls.Add(contentPanel);
            tabPage.Controls.Add(scrollPanel);
        }

        private void InitializeSupportTab(TabPage tabPage)
        {
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40),
                BackColor = Color.FromArgb(240, 240, 250)
            };

            var supportCard = new JeanPanel
            {
                BorderStyle = BorderStyle.None,
                Padding = new Padding(30),
                MaximumSize = new Size(500, 0),
                AutoSize = true,
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
                BorderRadius = 20
            };

            supportCard.Paint += (s, e) =>
            {
                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, supportCard.Width - 1, supportCard.Height - 1));
                }
            };

            var titleLabel = new Label
            {
                Text = "💬 Техническая поддержка",
                Font = new Font("Segoe UI", DataConfig.sizeFontCaptions, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 0),
                AutoSize = true,
                Location = new Point(15, 15),
                BackColor = Color.Transparent
            };

            var contactInfo = new Label
            {
                Text = @"📧 Email: evgenijpockutov6@gmail.com
📞 Телефон: +7 (950) 998-00-59
🕒 Время работы: Пн-Пт 9:00-18:00

Для оперативной помощи:
1. Опишите проблему подробно
2. Укажите версию приложения
3. Приложите скриншоты ошибок
4. Сообщите о шагах воспроизведения",

                Tag = "content",
                Font = new Font("Segoe UI", DataConfig.sizeFontText > 13 ? 13 : DataConfig.sizeFontText),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(35, 55)
            };

            var emergencyButton = new JeanModernButton
            {
                Text = "🚨 Экстренная помощь",
                Size = new Size(200, 50),
                Location = new Point(supportCard.Width / 2 - 75, 300),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12: DataConfig.sizeFontButtons, FontStyle.Bold),
                BorderRadius = 10
            };
            emergencyButton.Click += (s, e) => MessageBox.Show("Экстренная служба поддержки будет доступна в ближайшее время!");

            supportCard.Controls.Add(titleLabel);
            supportCard.Controls.Add(contactInfo);
            supportCard.Controls.Add(emergencyButton);
            supportCard.Height = titleLabel.Height + contactInfo.Height + emergencyButton.Height + 100;

            mainPanel.Controls.Add(supportCard);
            tabPage.Controls.Add(mainPanel);
        }

        private Panel CreateSectionPanel(string title, string content, int yPos)
        {
            var panel = new JeanPanel
            {
                BorderStyle = BorderStyle.None,
                Size = new Size(750, 0),
                Location = new Point(0, yPos),
                Padding = new Padding(20),
                AutoSize = true,
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
                BorderRadius = 20
            };

            panel.Paint += (s, e) =>
            {
                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, panel.Width - 1, panel.Height - 1));
                }
            };

            var titleLabel = new Label
            {
                Text = title,
                Tag = "Title",
                Font = new Font("Segoe UI", DataConfig.sizeFontCaptions, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 0),
                AutoSize = true,
                Location = new Point(15, 15),
                BackColor = Color.Transparent
            };

            var contentLabel = new Label
            {
                Text = content,
                Tag = "content",
                Font = new Font("Segoe UI", DataConfig.sizeFontText > 13 ? 13 : DataConfig.sizeFontText),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(35, 50),
                MaximumSize = new Size(660, 0)
            };

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(contentLabel);
            panel.Height = titleLabel.Height + contentLabel.Height + 40;

            return panel;
        }

        private Panel CreateFaqPanel(string question, string answer, int yPos)
        {
            var panel = new JeanPanel
            {
                BorderStyle = BorderStyle.None,
                Size = new Size(700, 0),
                Location = new Point(0, yPos),
                Padding = new Padding(20),
                AutoSize = true,
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
                BorderRadius = 20
            };

            panel.Paint += (s, e) =>
            {
                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, panel.Width - 1, panel.Height - 1));
                }
            };

            var questionLabel = new Label
            {
                Text = "❓ " + question,
                Font = new Font("Segoe UI", DataConfig.sizeFontCaptions, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 0),
                AutoSize = true,
                Location = new Point(15, 15),
                BackColor = Color.Transparent
            };

            var answerLabel = new Label
            {
                Text = "💡 " + answer,
                Tag = "content",
                Font = new Font("Segoe UI", DataConfig.sizeFontText > 13 ? 13 : DataConfig.sizeFontText),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(35, 50),
                MaximumSize = new Size(640, 0)
            };

            panel.Controls.Add(questionLabel);
            panel.Controls.Add(answerLabel);
            panel.Height = questionLabel.Height + answerLabel.Height + 40;

            return panel;
        }

        private void SwitchTab(int tabIndex)
        {
            tabControl.SelectedIndex = tabIndex;

            // Анимация перемещения индикатора
            var targetButton = tabIndex == 0 ? userGuideBtn : tabIndex == 1 ? faqBtn : supportBtn;

            // Подсветка активной кнопки
            userGuideBtn.BackColor = tabIndex == 0 ? Color.FromArgb(255, 140, 0) : Color.FromArgb(55, 55, 58);
            userGuideBtn.ForeColor = tabIndex == 0 ? Color.Black : Color.FromArgb(255, 140, 0);

            faqBtn.BackColor = tabIndex == 1 ? Color.FromArgb(255, 140, 0) : Color.FromArgb(55, 55, 58);
            faqBtn.ForeColor = tabIndex == 1 ? Color.Black : Color.FromArgb(255, 140, 0);

            supportBtn.BackColor = tabIndex == 2 ? Color.FromArgb(255, 140, 0) : Color.FromArgb(55, 55, 58);
            supportBtn.ForeColor = tabIndex == 2 ? Color.Black : Color.FromArgb(255, 140, 0);
        }
    }
}