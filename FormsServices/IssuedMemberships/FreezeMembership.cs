using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsServices
{
    public partial class FreezeMembership : ShadowedForm
    {
        private ComboBox cmbFreezeReason;
        private NumericUpDown numFreezeDays;
        private Label lblTitle;

        private Label txtClientName;
        private Label lblCard;
        private Label lblFreezeReason;
        private Label lblFreezeDays;

        public string _id = string.Empty;
        public string _numberCard = string.Empty;
        public string _client = string.Empty;

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
            this.FormBorderStyle = FormBorderStyle.None;
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
            titlePanel.Controls.Add(lblTitle);

            // Информация о клиенте
            var lblClientInfo = UIStyler.CreateStyledTextBox("Информация о клиенте:", new Point(20, 60), Color.MediumSlateBlue);

            txtClientName = UIStyler.CreateStyledTextBox(string.Empty, new Point(30, 90));

            lblCard = UIStyler.CreateStyledTextBox(string.Empty, new Point(30, 125));

            // Детали заморозки
            var lblFreezeDetails = UIStyler.CreateStyledTextBox("Детали заморозки:", new Point(20, 170), Color.MediumSlateBlue);

            // Поля для заморозки
            var lblFreezeDate = UIStyler.CreateStyledTextBox("Дата заморозки: " + DateTime.Now.ToShortDateString(), new Point(30, 200));

            lblFreezeReason = UIStyler.CreateStyledTextBox("Причина заморозки:", new Point(30, 235));

            cmbFreezeReason = new ComboBox
            {
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

            lblFreezeDays = UIStyler.CreateStyledTextBox("Срок заморозки (дней):", new Point(30, 270));

            numFreezeDays = new NumericUpDown
            {
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
            var btnCancel = UIStyler.CreateStyledButton("Отмена", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point(120, 320), new Size(120, 40));
            var btnSave = UIStyler.CreateStyledButton("Заморозить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point(250, 320), new Size(120, 40));
            var btnClose = UIStyler.CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, 10), new Size(30, 28));

            btnCancel.Click += BtnCancel_Click;
            btnSave.Click += BtnFreeze_Click;
            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            // Добавляем все элементы на главную панель
            this.Controls.AddRange(new Control[]
            {
                lblClientInfo,
                txtClientName,
                lblCard,
                lblFreezeDetails,
                lblFreezeDate,
                lblFreezeReason,
                cmbFreezeReason,
                lblFreezeDays,
                numFreezeDays,
                btnCancel,
                btnSave,
                btnClose,
                titlePanel
            });
        }

        public void UpdateData()
        {
            if (txtClientName != null)
                txtClientName.Text = "Клиент: " + _client;

            if (lblCard != null)
                lblCard.Text = "Номер карты: " + _numberCard;

            lblTitle.Location = new Point((this.Width - lblTitle.Width) / 2, (titlePanel.Height - lblTitle.Height) / 2);
            cmbFreezeReason.Location = new Point(lblFreezeReason.Location.X + lblFreezeReason.Width, 235);
            numFreezeDays.Location = new Point(lblFreezeDays.Location.X + lblFreezeDays.Width, 270);
            hintLabel.Location = new Point((this.Width - hintLabel.Width) / 2, this.Height - hintLabel.Height - 10);
        }

        private void BtnFreeze_Click(object sender, EventArgs e)
        {
            try
            {
                string reason = cmbFreezeReason.SelectedItem.ToString().Substring(2);
                int days = (int)numFreezeDays.Value;

                DateTime newFreezeEndDate = DateTime.Now.AddDays(days);

                using (var conn = new SQLiteConnection(IssuedMembershipContext.ConnectionStringIssued()))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(
                        @"UPDATE Issued SET 
                        Статус = @status,
                        Дата_окончания = date(Дата_окончания, '+' || @days || ' days'),
                        Окончание_заморозки = @freezeEndDate
                    WHERE №Карты = @cardNumber 
                        AND Дата_окончания IS NOT NULL
                        AND Дата_окончания != ''", conn))
                    {
                        cmd.Parameters.AddWithValue("@status", "заморожен");
                        cmd.Parameters.AddWithValue("@days", days);
                        cmd.Parameters.AddWithValue("@freezeEndDate", newFreezeEndDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@cardNumber", _numberCard);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageHelper.MessageWindowOk("❌ Абонемент не найден!", "Ошибка");
                            return;
                        }
                    }
                }

                MessageHelper.ShowNotification(this, $"✅ Абонемент успешно заморожен!\n\n" +
                                   $"👤 Клиент: {_client}\n" +
                                   $"📅 Дата заморозки: {DateTime.Now.ToShortDateString()}\n" +
                                   $"⏰ Срок: {days} дней\n" +
                                   $"📋 Причина: {reason}", 1500);

                _fadeAnimation.CloseWithAnimation();
            }
            catch (Exception ex)
            {
                MessageHelper.MessageWindowOk($"❌ Ошибка при заморозке абонемента:\n{ex.Message}", "Ошибка");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            _fadeAnimation.CloseWithAnimation();
        }
    }
}