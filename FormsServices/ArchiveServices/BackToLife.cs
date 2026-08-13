using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsServices
{
    public partial class BackToLife : ShadowedForm
    {
        public string _id = string.Empty;
        public string _numberCard = string.Empty;
        public string _client = string.Empty;

        private FadeAnimation _fadeAnimation;

        Panel titlePanel;

        string[] notChangeableTexts = new string[]
            {
                "⚡ ВОЗВРАТ ИЗ АРХИВА"
            };

        public BackToLife()
        {

            InitializeComponent();
            InitializeCustomDesign();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, notChangeableTexts);

            this.EnableDrag(this);
        }

        private void InitializeCustomDesign()
        {
            // Настройка стиля формы
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            titlePanel = new Panel
            {
                Size = new Size(this.Width, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0),
            };
            titlePanel.Controls.Add(titleLabel);

            // Стилизация текстовых полей
            UIStyler.StyleTextBox(this, jeanTextBoxMembership);
            UIStyler.StyleTextBox(this, jeanTextBoxTerm);
            UIStyler.StyleTextBox(this, jeanTextBoxVisits);

            // Стилизация кнопок
            var jeanModernButtonBackToLife = UIStyler.CreateStyledButton("Вернуть", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point(this.Width / 2 - 60, this.Height - 80), new Size(120, 40));
            var btnClose = UIStyler.CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, 10), new Size(30, 28));

            jeanModernButtonBackToLife.Click += jeanModernButtonBackToLife_Click;
            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);
            this.Controls.Add(jeanModernButtonBackToLife);
        }

        public void UpdateData()
        {
            titleLabel.Location = new Point((this.Width - titleLabel.Width) / 2, (titlePanel.Height - titleLabel.Height) / 2);
            hintLabel.Location = new Point((this.Width - hintLabel.Width) / 2, this.Height - hintLabel.Height - 10);
        }

        private void jeanModernButtonBackToLife_Click(object sender, EventArgs e)
        {
            if (MessageHelper.MessageWindowYesNo("Вы уверены что хотите восстановить абонемент?") != DialogResult.Yes)
                return;

            string insertQuery = @"
                INSERT INTO Issued (
                    Клиент,
                    №Карты,
                    Дата_окончания,
                    Дата_оформления,
                    Абонемент,
                    Посетил,
                    Статус,
                    Посещений_осталось,
                    Окончание_заморозки
                ) VALUES (
                    @client,
                    @cardNumber,
                    @endDate,
                    @registrationDate,
                    @membership,
                    @visited,
                    @status,
                    @visitsLeft,
                    @freezeEnd
                )";

            GeneralContext.CommandDataFromDatabase(insertQuery,
                IssuedMembershipContext.ConnectionStringIssued(),
                new SQLiteParameter("@client", _client),
                new SQLiteParameter("@cardNumber", _numberCard),
                new SQLiteParameter("@endDate", jeanTextBoxTerm.Text),
                new SQLiteParameter("@registrationDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SQLiteParameter("@membership", jeanTextBoxMembership.Text),
                new SQLiteParameter("@visited", string.Empty),
                new SQLiteParameter("@status", "активирован"),
                new SQLiteParameter("@visitsLeft", jeanTextBoxVisits.Text),
                new SQLiteParameter("@freezeEnd", string.Empty));

            GeneralContext.CommandDataFromDatabase("DELETE FROM Archive WHERE Id = @id;",
                ArchiveServicesContext.ConnectionStringArchive(),
                new SQLiteParameter("@id", _id));

            MessageHelper.ShowNotification(this, "✅ Абонемент восстановлен", 1500);
            _fadeAnimation.CloseWithAnimation();
        }
    }
}