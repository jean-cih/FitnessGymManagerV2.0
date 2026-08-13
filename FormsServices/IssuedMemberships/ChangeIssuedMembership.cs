using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using GymApplicationV2._0.Data;
using Shadow;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsServices
{
    public partial class ChangeIssuedMembership : ShadowedForm
    {
        public string _id = string.Empty;

        private FadeAnimation _fadeAnimation;

        Panel titlePanel;

        string[] notChangeableTexts = new string[]
            {
                "✏️ РЕДАКТИРОВАНИЕ"
            };

        public ChangeIssuedMembership()
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
            // Настройка стиля формы
            this.BackColor = Color.FromArgb(255, 255, 255);
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
            UIStyler.StyleTextBox(this, jeanTextBoxClient);
            UIStyler.StyleTextBox(this, jeanTextBoxTerm);
            UIStyler.StyleTextBox(this, jeanTextBoxMembership);
            UIStyler.StyleTextBox(this, jeanTextBoxCost);
            UIStyler.StyleTextBox(this, jeanTextBoxVisits);
            UIStyler.StyleTextBox(this, jeanTextBoxStatus);
            UIStyler.StyleTextBox(this, jeanTextBoxFreezeDate);

            // Стилизация кнопок
            var jeanModernButtonChange = UIStyler.CreateStyledButton("Сохранить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point(this.Width / 2 - 60, this.Height - 80), new Size(120, 40));
            var btnClose = UIStyler.CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, 10), new Size(30, 28));

            jeanModernButtonChange.Click += jeanModernButtonChange_Click;
            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);
            this.Controls.Add(jeanModernButtonChange);
        }

        public void UpdateData()
        {
            titleLabel.Location = new Point((this.Width - titleLabel.Width) / 2, (titlePanel.Height - titleLabel.Height) / 2);
            hintLabel.Location = new Point((this.Width - hintLabel.Width) / 2, this.Height - hintLabel.Height - 10);
        }

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            if (MessageHelper.MessageWindowYesNo("Вы уверены что хотите изменить данные?") != DialogResult.Yes)
                return;

            var updateQuery = $@"UPDATE Issued SET 
                        Клиент = '{jeanTextBoxClient.Text}',
                        Дата_окончания = '{jeanTextBoxTerm.Text}',
                        Абонемент = '{jeanTextBoxMembership.Text}',
                        Оплата = '{jeanTextBoxCost.Text}',
                        Статус = '{jeanTextBoxStatus.Text}',
                        Посещений_осталось = '{jeanTextBoxVisits.Text}'
                        WHERE Id = '{_id}';";

            GeneralContext.CommandDataFromDatabase(updateQuery,
                IssuedMembershipContext.ConnectionStringIssued());

            MessageHelper.ShowNotification(this, "✅ Данные обновлены", 1500);
            _fadeAnimation.CloseWithAnimation();
        }
    }
}