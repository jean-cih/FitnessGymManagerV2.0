using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsServices
{
    public partial class FreezeMembership : ShadowedForm
    {
        public Label txtClientName;
        public Label txtCardNumber;
        public Label txtFreezeDate;
        private ComboBox cmbFreezeReason;
        private NumericUpDown numFreezeDays;
        private JeanModernButton btnSave;
        private JeanModernButton btnCancel;
        private Label lblTitle;
        public string _dateOver;

        private FadeAnimation _fadeAnimation;

        Panel titlePanel;

        string[] notChangeableTexts = new string[]
            {
                "❄️ ЗАМОРОЗКА АБОНЕМЕНТА"
            };

        public FreezeMembership()
        {
            InitializeComponent();
            InitializeCustomDesign();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, notChangeableTexts);

            titlePanel.EnableDrag(this);
        }

        private void InitializeCustomDesign()
        {
            // Основные настройки формы
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(10);
            this.DoubleBuffered = true;

            titlePanel = new Panel
            {
                Size = new Size(this.Width, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0),
            };

            // Заголовок
            lblTitle = new Label
            {
                Text = "❄️ ЗАМОРОЗКА АБОНЕМЕНТА",
                Font = new Font("Montserrat", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 220, 255),
                Size = new Size(400, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            lblTitle.Location = new Point((this.Width - lblTitle.Width) / 2, (titlePanel.Height - lblTitle.Height) / 2);
            titlePanel.Controls.Add(lblTitle);

            // Информация о клиенте
            Label lblClientInfo = new Label
            {
                Text = "Информация о клиенте:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.MediumSlateBlue,
                Location = new Point(20, 60),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            txtClientName = CreateStyledTextBox(new Point(30, 90), 400);

            Label lblCard = new Label
            {
                Text = "Номер карты:",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(30, 130),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            txtCardNumber = CreateStyledTextBox(new Point(130, 130), 200);

            // Детали заморозки
            Label lblFreezeDetails = new Label
            {
                Text = "Детали заморозки:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.MediumSlateBlue,
                Location = new Point(20, 170),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Поля для заморозки
            Label lblFreezeDate = new Label
            {
                Text = "Дата заморозки:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(30, 200),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            txtFreezeDate = CreateStyledTextBox(new Point(180, 202), 180);

            Label lblFreezeReason = new Label
            {
                Text = "Причина заморозки:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(30, 235),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            cmbFreezeReason = new ComboBox
            {
                Location = new Point(180, 235),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };

            // Заполняем причины заморозки
            cmbFreezeReason.Items.AddRange(new object[]
            {
                "🏥 Болезнь",
                "✈️ Отпуск",
                "💼 Командировка",
                "👨‍👩‍👧‍👦 Семейные обстоятельства",
                "❓ Другая причина"
            });
            cmbFreezeReason.SelectedIndex = 0;

            Label lblFreezeDays = new Label
            {
                Text = "Срок заморозки (дней):",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(30, 270),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            numFreezeDays = new NumericUpDown
            {
                Location = new Point(190, 270),
                Size = new Size(60, 30),
                Font = new Font("Segoe UI", 9),
                Minimum = 1,
                Maximum = 90,
                Value = 30,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                TextAlign = HorizontalAlignment.Center
            };

            // Кнопки
            btnCancel = CreateStyledButton("Отмена", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point(120, 320), new Size(120, 40));
            btnSave = CreateStyledButton("Сохранить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point(250, 320), new Size(120, 40));

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            // Добавляем все элементы на главную панель
            this.Controls.AddRange(new Control[]
            {
                lblClientInfo,
                txtClientName,
                lblCard,
                txtCardNumber,
                lblFreezeDetails,
                lblFreezeDate,
                txtFreezeDate,
                lblFreezeReason,
                cmbFreezeReason,
                lblFreezeDays,
                numFreezeDays,
                btnCancel,
                btnSave
            });

            var btnClose = new JeanModernButton
            {
                Font = new Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 28),
                Cursor = Cursors.Hand
            };

            btnClose = CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(460, 10), new Size(30, 28));
            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            // Добавление подсказки внизу формы
            hintLabel.Font = new Font("Montserrat", 7, FontStyle.Italic);
            hintLabel.ForeColor = Color.FromArgb(140, 140, 180);
            hintLabel.BackColor = Color.Transparent;
            hintLabel.Size = new Size(250, 25);
            hintLabel.Location = new Point((this.Width - 250) / 2, this.Height - hintLabel.Height - 10);

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);
        }

        private Label CreateStyledTextBox(Point location, int width)
        {
            return new Label
            {
                Location = location,
                Size = new Size(width, 16),
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
            };
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    DateTime freezeDate = DateTime.ParseExact(txtFreezeDate.Text, "dd.MM.yy", null);
                    string reason = cmbFreezeReason.SelectedItem.ToString().Substring(2);
                    int days = (int)numFreezeDays.Value;

                    var updateQuery = $@"UPDATE Issued SET 
                               Статус = 'заморожен',
                               Дата_окончания = '{Convert.ToDateTime(_dateOver).AddDays(days).ToShortDateString()}',
                               Окончание_заморозки = '{freezeDate.AddDays(days).ToShortDateString()}'
                               WHERE №Карты = '{txtCardNumber.Text}'";

                    GeneralContext.CommandDataFromDatabase(updateQuery,
                IssuedMembershipContext.ConnectionStringIssued());

                    MessageBox.Show($"✅ Абонемент успешно заморожен!\n\n" +
                                  $"👤 Клиент: {txtClientName.Text}\n" +
                                  $"📅 Дата: {freezeDate:dd.MM.yyyy}\n" +
                                  $"⏰ Срок: {days} дней\n" +
                                  $"📋 Причина: {reason}",
                                  "Успешно",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);

                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (FormatException)
                {
                    MessageBox.Show("❌ Неверный формат даты!", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            _fadeAnimation.CloseWithAnimation();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtFreezeDate.Text))
            {
                MessageBox.Show("❌ Укажите дату заморозки", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (cmbFreezeReason.SelectedItem == null)
            {
                MessageBox.Show("❌ Выберите причину заморозки", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public void SetClientData(string clientName, string cardNumber)
        {
            txtClientName.Text = clientName;
            txtCardNumber.Text = cardNumber;
        }
    }
}