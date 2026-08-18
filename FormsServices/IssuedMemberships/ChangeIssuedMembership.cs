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
            try
            {
                InitializeComponent();
                InitializeCustomDesign();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                FontHelper.ApplyFontSettings(this, notChangeableTexts);

                titlePanel.EnableDrag(this);

                Logger.Info("Форма ChangeIssuedMembership инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации ChangeIssuedMembership", ex);
                throw;
            }
        }

        private void InitializeCustomDesign()
        {
            try
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
                btnClose.Click += (s, e) =>
                {
                    Logger.Info("Закрытие формы ChangeIssuedMembership");
                    _fadeAnimation.CloseWithAnimation();
                };

                titlePanel.Controls.Add(btnClose);

                this.Controls.Add(titlePanel);
                this.Controls.Add(jeanModernButtonChange);

                Logger.Info("Дизайн формы ChangeIssuedMembership инициализирован");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeCustomDesign", ex);
            }
        }

        public void UpdateData()
        {
            try
            {
                titleLabel.Location = new Point((this.Width - titleLabel.Width) / 2, (titlePanel.Height - titleLabel.Height) / 2);
                hintLabel.Location = new Point((this.Width - hintLabel.Width) / 2, this.Height - hintLabel.Height - 10);

                Logger.Info($"Данные обновлены в форме для записи ID: {_id}");
                Logger.Info($"Клиент: {jeanTextBoxClient.Text}");
                Logger.Info($"Абонемент: {jeanTextBoxMembership.Text}, срок: {jeanTextBoxTerm.Text}");
                Logger.Info($"Оплата: {jeanTextBoxCost.Text}, статус: {jeanTextBoxStatus.Text}");
                Logger.Info($"Посещений: {jeanTextBoxVisits.Text}, заморозка: {jeanTextBoxFreezeDate.Text}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в UpdateData", ex);
            }
        }

        private void jeanModernButtonChange_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info($"Нажата кнопка 'Сохранить' для записи ID: {_id}");

                if (MessageHelper.MessageWindowYesNo("Вы уверены что хотите изменить данные?") != DialogResult.Yes)
                {
                    Logger.Info($"Изменение данных отменено пользователем для ID: {_id}");
                    return;
                }

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

                Logger.Info($"Данные успешно обновлены в Issued для ID: {_id}");
                Logger.Info($"Новые данные: клиент={jeanTextBoxClient.Text}, " +
                    $"абонемент={jeanTextBoxMembership.Text}, срок={jeanTextBoxTerm.Text}, " +
                    $"оплата={jeanTextBoxCost.Text}, статус={jeanTextBoxStatus.Text}, " +
                    $"посещений={jeanTextBoxVisits.Text}");

                MessageHelper.ShowNotification(this, "✅ Данные обновлены", 1500);
                _fadeAnimation.CloseWithAnimation();

                Logger.Info($"Форма ChangeIssuedMembership закрыта после успешного обновления");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при изменении данных в Issued для ID: {_id}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при изменении данных: {ex.Message}", "Ошибка");
            }
        }
    }
}