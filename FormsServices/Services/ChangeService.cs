using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class ChangeService : ShadowedForm
    {
        public string _id = string.Empty;
        public string _typeMembership = string.Empty;

        private FadeAnimation _fadeAnimation;

        private ComboBox typeMembership;
        private Panel titlePanel;

        string[] notChangeableTexts = new string[]
            {
                "РЕДАКТИРОВАНИЕ УСЛУГИ"
            };

        public ChangeService()
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

                Logger.Info("Форма ChangeService инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации ChangeService", ex);
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
                    Size = new Size(874, 50),
                    BackColor = Color.MediumSlateBlue,
                    Location = new Point(0, 0),
                };
                titlePanel.Controls.Add(titleLabel);

                // Стилизация текстовых полей
                UIStyler.StyleTextBox(this, jeanTextBoxName);
                UIStyler.StyleTextBox(this, jeanTextBoxPrice);
                UIStyler.StyleTextBox(this, jeanTextBoxTerm);
                UIStyler.StyleTextBox(this, jeanTextBoxVisited);

                typeMembership = new ComboBox
                {
                    Size = new Size(250, 30),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(250, 250, 250)
                };

                // Заполняем причины заморозки
                typeMembership.Items.AddRange(new object[]
                {
                    "обычный",
                    "безлимитный"
                });

                // Стилизация кнопок
                var jeanModernButtonSave = UIStyler.CreateStyledButton("Сохранить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point(this.Width / 2 - 60, this.Height - 80), new Size(120, 40));
                var btnClose = UIStyler.CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, 10), new Size(30, 28));

                jeanModernButtonSave.Click += jeanModernButtonSave_Click;
                btnClose.Click += (s, e) =>
                {
                    Logger.Info("Закрытие формы ChangeService");
                    _fadeAnimation.CloseWithAnimation();
                };

                titlePanel.Controls.Add(btnClose);

                this.Controls.Add(titlePanel);
                this.Controls.Add(typeMembership);
                this.Controls.Add(jeanModernButtonSave);

                Logger.Info("Дизайн формы ChangeService инициализирован");
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
                labelService.Location = new Point((this.Width - labelService.Width) / 2, labelService.Location.Y);
                typeMembership.Location = new Point((this.Width - typeMembership.Width) / 2, jeanTextBoxVisited.Location.Y + 50);
                typeMembership.Text = _typeMembership;
                titleLabel.Location = new Point((this.Width - titleLabel.Width) / 2, (titlePanel.Height - titleLabel.Height) / 2);
                hintLabel.Location = new Point((this.Width - hintLabel.Width) / 2, this.Height - hintLabel.Height - 10);

                Logger.Info($"Данные обновлены для услуги ID: {_id}");
                Logger.Info($"Название: {jeanTextBoxName.Text}, цена: {jeanTextBoxPrice.Text}");
                Logger.Info($"Срок: {jeanTextBoxTerm.Text}, посещений: {jeanTextBoxVisited.Text}");
                Logger.Info($"Тип: {_typeMembership}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в UpdateData", ex);
            }
        }

        private void jeanModernButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info($"Нажата кнопка 'Сохранить' для услуги ID: {_id}");

                if (string.IsNullOrWhiteSpace(jeanTextBoxName.Text) ||
                    string.IsNullOrWhiteSpace(jeanTextBoxPrice.Text) ||
                    string.IsNullOrWhiteSpace(jeanTextBoxTerm.Text))
                {
                    Logger.Warning("Попытка сохранения с незаполненными полями");
                    MessageHelper.MessageWindowOk("Заполните все поля", "Предупреждение");
                    return;
                }

                if (MessageHelper.MessageWindowYesNo("Вы уверены что хотите изменить услугу?") != DialogResult.Yes)
                {
                    Logger.Info($"Изменение услуги отменено пользователем для ID: {_id}");
                    return;
                }

                using (var conn = new SQLiteConnection(ServicesContext.ConnectionStringServices()))
                {
                    conn.Open();

                    string updateQuery = @"
                        UPDATE Descriptions SET 
                            Абонемент = @name,
                            Цена = @price,
                            Срок_действия = @term,
                            Посещений = @visits,
                            Тип = @type
                        WHERE Id = @id";

                    using (var cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", jeanTextBoxName.Text.Trim());
                        cmd.Parameters.AddWithValue("@price", Convert.ToInt32(jeanTextBoxPrice.Text.Trim()));
                        cmd.Parameters.AddWithValue("@term", Convert.ToInt32(jeanTextBoxTerm.Text.Trim()));

                        int visits;
                        if (int.TryParse(jeanTextBoxVisited.Text.Trim(), out visits) && visits > 0)
                        {
                            cmd.Parameters.AddWithValue("@visits", visits);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@visits", DBNull.Value);
                        }

                        string type = typeMembership.SelectedItem?.ToString() ?? "обычный";
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@id", _id);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        Logger.Info($"Услуга обновлена. Обновлено строк: {rowsAffected}");
                        Logger.Info($"Новые данные: название='{jeanTextBoxName.Text}', цена={jeanTextBoxPrice.Text}, " +
                            $"срок={jeanTextBoxTerm.Text}, посещений={jeanTextBoxVisited.Text}, тип='{type}'");
                    }
                }

                MessageHelper.ShowNotification(this, "✅ Услуга изменена", 1500);
                Logger.Info($"Услуга успешно изменена для ID: {_id}");
                _fadeAnimation.CloseWithAnimation();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при изменении услуги ID: {_id}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при изменении услуги: {ex.Message}", "Ошибка");
            }
        }
    }
}