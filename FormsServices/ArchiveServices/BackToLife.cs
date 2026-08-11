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
        private FadeAnimation _fadeAnimation;

        Panel titlePanel;

        string[] notChangeableTexts = new string[]
            {
                "⚡ ВОЗВРАТ ИЗ АРХИВА"
            };

        string pastTerm = null;

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

            pastTerm = jeanTextBoxTerm.Text;
        }

        private void InitializeCustomDesign()
        {
            // Настройка стиля формы
            this.BackColor = Color.White;
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

            titleLabel.Location = new Point((this.Width - titleLabel.Width) / 2, (titlePanel.Height - titleLabel.Height) / 2);
            titlePanel.Controls.Add(titleLabel);

            // Стилизация текстовых полей
            StyleTextBox(jeanTextBoxMembership);
            StyleTextBox(jeanTextBoxTerm);
            StyleTextBox(jeanTextBoxVisits);

            // Стилизация кнопок
            StyleButton(jeanModernButtonBackToLife, "Вернуть", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point((this.Width - jeanModernButtonBackToLife.Width) / 2, this.Height - jeanModernButtonBackToLife.Height - 50));

            hintLabel.Location = new Point((this.Width - hintLabel.Width) / 2, this.Height - hintLabel.Height - 10);

            var btnClose = new JeanModernButton
            {
                Font = new Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 28),
                Cursor = Cursors.Hand
            };

            StyleButton(btnClose, "X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, (titlePanel.Height - btnClose.Height) / 2));

            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);
        }

        private void StyleTextBox(JeanTextBox textBox)
        {
            textBox.BorderColor = Color.FromArgb(80, 80, 120);
            textBox.BackColor = Color.White;
            textBox.ForeColor = Color.Black;
            textBox.Font = new Font("Montserrat", 9);
        }

        private void StyleButton(JeanModernButton button, string text, Color baseColor, int radius, int radiusSize, Color radiusColor, Point location)
        {
            button.Text = text;
            button.Font = new Font("Montserrat", 10, FontStyle.Bold);
            button.BackColor = baseColor;
            button.BorderColor = radiusColor;
            button.BackgroundColor = baseColor;
            button.TextColor = Color.White;
            button.BorderRadius = radius;
            button.BorderSize = radiusSize;
            button.Location = location;

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
                new SQLiteParameter("@client", labelNameClient.Text),
                new SQLiteParameter("@cardNumber", labelNubmerCard.Text),
                new SQLiteParameter("@endDate", jeanTextBoxTerm.Text),
                new SQLiteParameter("@registrationDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SQLiteParameter("@membership", jeanTextBoxMembership.Text),
                new SQLiteParameter("@visited", string.Empty),
                new SQLiteParameter("@status", "активирован"),
                new SQLiteParameter("@visitsLeft", jeanTextBoxVisits.Text),
                new SQLiteParameter("@freezeEnd", string.Empty));

            string deleteQuery = "DELETE FROM Archive WHERE №Карты = @cardNumber AND Дата_окончания = @pastEndDate;";

            GeneralContext.CommandDataFromDatabase(deleteQuery,
                ArchiveServicesContext.ConnectionStringArchive(),
                new SQLiteParameter("@cardNumber", labelNubmerCard.Text),
                new SQLiteParameter("@pastEndDate", pastTerm));

            MessageHelper.MessageWindowOk("Абонемент восстановлен", "Сообщение");

            _fadeAnimation.CloseWithAnimation();
        }

        private void jeanModernButton1_Click(object sender, EventArgs e)
        {
            _fadeAnimation.CloseWithAnimation();
        }
    }
}