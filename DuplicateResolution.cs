using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public partial class DuplicateResolution : ShadowedForm
    {
        private FadeAnimation _fadeAnimation;

        public string SelectedCardNumber { get; private set; }

        string[] notChangeableTexts = new string[]
            {
                "ВЫБОР КЛИЕНТА"
            };

        public DuplicateResolution(DataTable data)
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

                dataGridViewClients.DataSource = GetClientsByCardNumbersOptimized(data);

                Logger.Info($"Форма DuplicateResolution инициализирована, получено {data?.Rows?.Count ?? 0} записей для разрешения дубликатов");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации DuplicateResolution", ex);
                throw;
            }
        }

        public DataTable GetClientsByCardNumbersOptimized(DataTable cardNumbersTable)
        {
            try
            {
                if (cardNumbersTable is null || cardNumbersTable.Rows.Count == 0)
                {
                    Logger.Warning("Получена пустая таблица номеров карт");
                    return new DataTable();
                }

                // Собираем все номера карт в список
                List<string> cardNumbers = new List<string>();
                foreach (DataRow row in cardNumbersTable.Rows)
                {
                    string cardNumber = row["№Карты"].ToString();
                    if (!string.IsNullOrEmpty(cardNumber))
                    {
                        cardNumbers.Add(cardNumber);
                    }
                }

                if (cardNumbers.Count == 0)
                {
                    Logger.Warning("Не найдено номеров карт для поиска");
                    return new DataTable();
                }

                Logger.Info($"Поиск клиентов по {cardNumbers.Count} номерам карт");

                // Создаем IN-запрос с параметрами
                string query = $"SELECT №Карты, Имя, Фамилия, Отчество FROM Contacts WHERE №Карты IN ({string.Join(",", cardNumbers.Select((_, i) => $"@Card{i}"))})";

                DataTable result = new DataTable();
                using (SQLiteConnection conn = new SQLiteConnection(ClientsContext.ConnectionStringClients()))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        // Добавляем параметры
                        for (int i = 0; i < cardNumbers.Count; i++)
                        {
                            cmd.Parameters.AddWithValue($"@Card{i}", cardNumbers[i]);
                        }

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {
                            conn.Open();
                            adapter.Fill(result);
                        }
                    }
                }

                Logger.Info($"Найдено {result?.Rows?.Count ?? 0} клиентов по номерам карт");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в GetClientsByCardNumbersOptimized", ex);
                return new DataTable();
            }
        }

        private void InitializeCustomDesign()
        {
            try
            {
                // Настройка стиля формы
                this.BackColor = Color.FromArgb(255, 255, 255);
                this.FormBorderStyle = FormBorderStyle.None;
                this.FormBorderStyle = FormBorderStyle.None;
                this.Padding = new Padding(1);
                this.DoubleBuffered = true;

                // Стилизация кнопок
                StyleButton(jeanModernButtonSave, "Отметить", Color.FromArgb(123, 104, 238), 20, 2, Color.FromArgb(255, 140, 0), new Point((this.Width - jeanModernButtonSave.Width) / 2, this.Height - jeanModernButtonSave.Height - 40));

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

                StyleButton(btnClose, "X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, 10));

                btnClose.Click += (s, e) => btnClose_click();

                this.Controls.Add(btnClose);

                Logger.Info("Дизайн формы DuplicateResolution инициализирован");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeCustomDesign", ex);
            }
        }

        private void btnClose_click()
        {
            try
            {
                Logger.Info("Закрытие формы DuplicateResolution без выбора");
                SelectedCardNumber = "";
                _fadeAnimation.CloseWithAnimation();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в btnClose_click", ex);
            }
        }

        private void StyleButton(JeanModernButton button, string text, Color baseColor, int radius, int radiusSize, Color radiusColor, Point location)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в StyleButton для кнопки {button?.Name ?? "unknown"}", ex);
            }
        }

        private void jeanModernButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (labelCard.Text == "-")
                {
                    Logger.Warning("Попытка сохранения без выбранного клиента");
                    return;
                }

                Logger.Info($"Выбран клиент с картой: {labelCard.Text}");
                _fadeAnimation.CloseWithAnimation();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonSave_Click", ex);
            }
        }

        private void dataGridViewClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridViewClients.SelectedRows.Count == 0) return;

                var selectedRow = dataGridViewClients.SelectedRows[0];
                labelCard.Text = selectedRow.Cells[0].Value.ToString();
                SelectedCardNumber = labelCard.Text;

                Logger.Info($"Выбран клиент в таблице: карта {SelectedCardNumber}, " +
                    $"имя {selectedRow.Cells[1]?.Value}, фамилия {selectedRow.Cells[2]?.Value}");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в dataGridViewClients_CellContentClick", ex);
            }
        }
    }
}