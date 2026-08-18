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
            try
            {
                InitializeComponent();
                InitializeCustomDesign();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                FontHelper.ApplyFontSettings(this, notChangeableTexts);

                this.EnableDrag(this);

                Logger.Info("Форма BackToLife инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации BackToLife", ex);
                throw;
            }
        }

        private void InitializeCustomDesign()
        {
            try
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
                btnClose.Click += (s, e) =>
                {
                    Logger.Info("Закрытие формы BackToLife");
                    _fadeAnimation.CloseWithAnimation();
                };

                titlePanel.Controls.Add(btnClose);

                this.Controls.Add(titlePanel);
                this.Controls.Add(jeanModernButtonBackToLife);

                Logger.Info("Дизайн формы BackToLife инициализирован");
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

                Logger.Info($"Данные обновлены для клиента: {_client}, карта: {_numberCard}, ID: {_id}");
                Logger.Info($"Абонемент: {jeanTextBoxMembership.Text}, срок: {jeanTextBoxTerm.Text}, посещений: {jeanTextBoxVisits.Text}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в UpdateData", ex);
            }
        }

        private void jeanModernButtonBackToLife_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info($"Нажата кнопка 'Вернуть' для клиента: {_client}, ID: {_id}");

                if (MessageHelper.MessageWindowYesNo("Вы уверены что хотите восстановить абонемент?") != DialogResult.Yes)
                {
                    Logger.Info($"Восстановление абонемента отменено пользователем для клиента: {_client}");
                    return;
                }

                var type_membership = GeneralContext.GetElementFromDatabase(@"SELECT Цена 
                                        FROM Descriptions 
                                        WHERE Абонемент = @membership",
                    ServicesContext.ConnectionStringServices(),
                    new SQLiteParameter("@membership", jeanTextBoxMembership.Text));

                if (type_membership == null)
                {
                    Logger.Warning($"Не найдена цена для абонемента: {jeanTextBoxMembership.Text}");
                    return;
                }

                string visitsLeft = jeanTextBoxVisits.Text;
                if (type_membership.ToString() == "обычный" && visitsLeft == string.Empty)
                {
                    visitsLeft = "0";
                    Logger.Info($"Для обычного абонемента установлено значение посещений: 0");
                }

                string insertQuery = @"
                    INSERT INTO Issued (
                        Клиент,
                        №Карты,
                        Дата_окончания,
                        Дата_оформления,
                        Абонемент,
                        Посетил,
                        Оплата,
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
                        @cost,
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
                    new SQLiteParameter("@cost", type_membership.ToString()),
                    new SQLiteParameter("@status", "активирован"),
                    new SQLiteParameter("@visitsLeft", visitsLeft),
                    new SQLiteParameter("@freezeEnd", string.Empty));

                Logger.Info($"Абонемент вставлен в Issued для клиента: {_client}");

                GeneralContext.CommandDataFromDatabase("DELETE FROM Archive WHERE Id = @id;",
                    ArchiveServicesContext.ConnectionStringArchive(),
                    new SQLiteParameter("@id", _id));

                Logger.Info($"Запись удалена из Archive для ID: {_id}, клиент: {_client}");

                MessageHelper.ShowNotification(this, "✅ Абонемент восстановлен", 1500);
                Logger.Info($"Абонемент успешно восстановлен для клиента: {_client}");
                _fadeAnimation.CloseWithAnimation();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при восстановлении абонемента для клиента {_client}, ID: {_id}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при восстановлении: {ex.Message}", "Ошибка");
            }
        }
    }
}