using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using Microsoft.Office.Interop.Excel;
using Shadow;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GymApplicationV2._0.AppColors.AppColors;

namespace GymApplicationV2._0
{
    public partial class Report : ShadowedForm
    {
        private string dbFilePath = "";

        SplashScreen splashScreen = new SplashScreen();

        JeanModernButton jeanModernButtonShow;
        JeanModernButton jeanModernButtonExport;
        JeanModernButton jeanModernButtonChooseFile;

        System.Windows.Forms.CheckBox checkBoxSellServices;
        System.Windows.Forms.CheckBox checkBoxClientsForPeriod;

        System.Windows.Forms.CheckBox checkBoxTSV;
        System.Windows.Forms.CheckBox checkBoxJSON;
        System.Windows.Forms.CheckBox checkBoxTXT;
        System.Windows.Forms.CheckBox checkBoxCSV;
        System.Windows.Forms.CheckBox checkBoxXLS;

        RadioButton radioOtherPeriod;
        RadioButton radioForDay;
        RadioButton radioForWeek;
        RadioButton radioForMonth;

        JeanDateTimePicker jeanDateTimePickerBegin;
        JeanDateTimePicker jeanDateTimePickerEnd;

        Panel titlePanel;

        private FadeAnimation _fadeAnimation;

        public Report()
        {
            try
            {
                InitializeComponent();
                InitializeCustomDesign();

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                titlePanel.EnableDrag(this);

                Logger.Info("Форма Report инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации Report", ex);
                throw;
            }
        }

        private void Report_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Загрузка Report_Load");
                string[] notChangeableTexts = new string[]
                {
                    "📊 Отчёт"
                };

                FontHelper.ApplyFontSettings(this, notChangeableTexts);
                Logger.Info("Report_Load завершена успешно");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в Report_Load", ex);
            }
        }

        private void InitializeCustomDesign()
        {
            try
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
                        e.Graphics.DrawRectangle(pen, new System.Drawing.Rectangle(0, 0, Width - 1, Height - 1));
                    }
                };

                titlePanel = new Panel
                {
                    Size = new Size(1000, 50),
                    BackColor = Color.MediumSlateBlue,
                    Location = new System.Drawing.Point(0, 0),
                };

                // Заголовок
                var titleLabel = new System.Windows.Forms.Label
                {
                    Text = "📊 Отчёт",
                    Font = new System.Drawing.Font("Montserrat", 18, FontStyle.Bold),
                    ForeColor = ForeColor = Color.FromArgb(220, 220, 255),
                    BackColor = Color.Transparent,
                    Size = new Size(150, 25),
                    Location = new System.Drawing.Point((this.Width - 150) / 2, 10),
                };
                titlePanel.Controls.Add(titleLabel);
                this.Controls.Add(titlePanel);

                // Создаем карточки
                CreateReportTypeCard();
                CreatePeriodCard();
                CreateExportCard();
                CreateButtons();

                Logger.Info("Дизайн формы Report инициализирован");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeCustomDesign", ex);
            }
        }

        private void CreateReportTypeCard()
        {
            try
            {
                var card = new JeanPanel
                {
                    Size = new Size(275, 280),
                    Location = new System.Drawing.Point(30, 80),
                    BackColor = CardColor,
                    GradientBottomColor = CardColor,
                    GradientTapColor = CardColor,
                    BorderStyle = BorderStyle.None,
                    Padding = new Padding(15),
                    BorderRadius = 20,
                };

                var title = new System.Windows.Forms.Label
                {
                    Text = "📈 Тип отчёта",
                    ForeColor = PrimaryColor,
                    Size = new Size(150, 25),
                    Location = new System.Drawing.Point((card.Width - 150) / 2, 20),
                };

                checkBoxClientsForPeriod = UIStyler.CreateStyledCheckBox("Посещаемость", new System.Drawing.Point((card.Width - 130) / 2, 95));
                checkBoxClientsForPeriod.Checked = true;
                checkBoxClientsForPeriod.CheckedChanged += checkBoxClientsForPeriod_CheckedChanged;

                checkBoxSellServices = UIStyler.CreateStyledCheckBox("Количество проданных\nабонементов", new System.Drawing.Point((card.Width - 130) / 2, 175));
                checkBoxSellServices.CheckedChanged += checkBoxSellServices_CheckedChanged;

                card.Controls.AddRange(new Control[] { title, checkBoxClientsForPeriod, checkBoxSellServices });

                this.Controls.Add(card);
                Logger.Info("Карточка типа отчета создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreateReportTypeCard", ex);
            }
        }

        private void CreatePeriodCard()
        {
            try
            {
                var card = new JeanPanel
                {
                    Size = new Size(345, 230),
                    Location = new System.Drawing.Point(325, 80),
                    BackColor = CardColor,
                    GradientBottomColor = CardColor,
                    GradientTapColor = CardColor,
                    BorderStyle = BorderStyle.None,
                    Padding = new Padding(15),
                    BorderRadius = 20,
                };

                var title = new System.Windows.Forms.Label
                {
                    Text = "📅 Период",
                    ForeColor = PrimaryColor,
                    Size = new Size(120, 25),
                    Location = new System.Drawing.Point((card.Width - 120) / 2, 20),
                };

                radioForMonth = UIStyler.CreateStyledRadioButton("За месяц", new System.Drawing.Point(card.Width / 3 + 5, 55));
                radioForWeek = UIStyler.CreateStyledRadioButton("За неделю", new System.Drawing.Point(card.Width / 3 + 5, 85));
                radioForDay = UIStyler.CreateStyledRadioButton("За день", new System.Drawing.Point(card.Width / 3 + 5, 115), true);
                radioOtherPeriod = UIStyler.CreateStyledRadioButton("Другой период", new System.Drawing.Point(card.Width / 3 + 5, 145));

                jeanDateTimePickerBegin = new JeanDateTimePicker();
                jeanDateTimePickerBegin.CreateStyledDateTimePicker(new Size(140, 15), new System.Drawing.Point(card.Width / 2 - 150, 175));

                jeanDateTimePickerEnd = new JeanDateTimePicker();
                jeanDateTimePickerEnd.CreateStyledDateTimePicker(new Size(140, 15), new System.Drawing.Point(card.Width / 2 + 10, 175));

                card.Controls.AddRange(new Control[] { title, radioForMonth, radioForWeek, radioForDay, radioOtherPeriod, jeanDateTimePickerBegin, jeanDateTimePickerEnd });
                this.Controls.Add(card);
                Logger.Info("Карточка периода создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreatePeriodCard", ex);
            }
        }

        private void CreateExportCard()
        {
            try
            {
                var card = new JeanPanel
                {
                    Size = new Size(275, this.Height / 2 + 30),
                    Location = new System.Drawing.Point(690, 80),
                    BackColor = CardColor,
                    GradientBottomColor = CardColor,
                    GradientTapColor = CardColor,
                    BorderStyle = BorderStyle.None,
                    Padding = new Padding(15),
                    BorderRadius = 20,
                };

                var title = new System.Windows.Forms.Label
                {
                    Text = "💾 Формат экспорта",
                    ForeColor = PrimaryColor,
                    Size = new Size(130, 25),
                    Location = new System.Drawing.Point((card.Width - 130) / 2, 20),
                };

                checkBoxXLS = UIStyler.CreateStyledCheckBox("Excel (.xls)", new System.Drawing.Point(card.Width / 3 + 5, 55), true);
                checkBoxCSV = UIStyler.CreateStyledCheckBox("CSV (.csv)", new System.Drawing.Point(card.Width / 3 + 5, 85));
                checkBoxTXT = UIStyler.CreateStyledCheckBox("Text (.txt)", new System.Drawing.Point(card.Width / 3 + 5, 115));
                checkBoxJSON = UIStyler.CreateStyledCheckBox("JSON (.json)", new System.Drawing.Point(card.Width / 3 + 5, 145));
                checkBoxTSV = UIStyler.CreateStyledCheckBox("TSV (.tsv)", new System.Drawing.Point(card.Width / 3 + 5, 175));

                // Кнопка выбора файла
                jeanModernButtonChooseFile = UIStyler.CreateStyledButton("📁 Выбрать файл", PrimaryColor, 20, 0, Color.FromArgb(255, 140, 0), new System.Drawing.Point((card.Width - 150) / 2, 205), new Size(150, 45));
                jeanModernButtonChooseFile.Click += jeanModernButtonChooseFile_Click;

                // Кнопка экспорта
                jeanModernButtonExport = UIStyler.CreateStyledButton("🚀 Экспорт", AccentColor, 20, 0, Color.FromArgb(255, 140, 0), new System.Drawing.Point((card.Width - 180) / 2, 260), new Size(180, 50));
                jeanModernButtonExport.Click += jeanModernButtonExport_Click;

                card.Controls.AddRange(new Control[] { title, checkBoxXLS, checkBoxCSV, checkBoxTXT, checkBoxJSON, checkBoxTSV, jeanModernButtonChooseFile, jeanModernButtonExport });
                this.Controls.Add(card);
                Logger.Info("Карточка экспорта создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreateExportCard", ex);
            }
        }

        private void CreateButtons()
        {
            try
            {
                // Кнопка показа
                jeanModernButtonShow = UIStyler.CreateStyledButton("👁️ Показать", SecondaryColor, 20, 0, Color.FromArgb(255, 140, 0), new System.Drawing.Point((this.Width - 200) / 2, 335), new Size(200, 60));
                jeanModernButtonShow.Click += buttonShow_Click;

                var btnClose = UIStyler.CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new System.Drawing.Point(this.Width - 40, 10), new Size(30, 28));
                btnClose.Click += (s, e) =>
                {
                    Logger.Info("Закрытие формы Report");
                    _fadeAnimation.CloseWithAnimation();
                };
                titlePanel.Controls.Add(btnClose);

                this.Controls.AddRange(new Control[] { jeanModernButtonShow });
                Logger.Info("Кнопки созданы");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreateButtons", ex);
            }
        }

        private GraphicsPath GetRoundedRectangle(System.Drawing.Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Нажата кнопка 'Показать'");

                System.Windows.Forms.CheckBox[] otherCheckBoxe = { checkBoxClientsForPeriod, checkBoxSellServices };
                bool checkBoxOn = false;
                foreach (var checkBox in otherCheckBoxe)
                {
                    if (checkBox.Checked)
                    {
                        checkBoxOn = true;
                        break;
                    }
                }

                if (!checkBoxOn)
                {
                    Logger.Warning("Попытка показа отчета без выбранного типа");
                    MessageHelper.MessageWindowOk("Выберите тип отчета", "Предупреждение");
                    return;
                }

                using (var infoReport = new InformationReport())
                {
                    infoReport.periodForMonth = radioForMonth.Checked;
                    infoReport.periodForWeek = radioForWeek.Checked;
                    infoReport.periodForDay = radioForDay.Checked;
                    infoReport.otherPeriod = radioOtherPeriod.Checked;

                    infoReport.dateBegin = jeanDateTimePickerBegin.Value;
                    infoReport.dateEnd = jeanDateTimePickerEnd.Value;

                    infoReport.forPeriod = checkBoxClientsForPeriod.Checked;
                    infoReport.sellServices = checkBoxSellServices.Checked;

                    Logger.Info($"Открытие отчета: forPeriod={infoReport.forPeriod}, sellServices={infoReport.sellServices}, " +
                        $"periodForDay={infoReport.periodForDay}, periodForWeek={infoReport.periodForWeek}, periodForMonth={infoReport.periodForMonth}");
                    infoReport.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в buttonShow_Click", ex);
                MessageHelper.MessageWindowOk($"Ошибка при открытии отчета: {ex.Message}", "Ошибка");
            }
        }

        private void jeanModernButtonChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Нажата кнопка 'Выбрать файл'");
                using (var openFileDialog = new OpenFileDialog { Filter = "Database | *.db;" })
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        dbFilePath = openFileDialog.FileName;
                        jeanModernButtonExport.Text = "🚀 Экспортировать\n" + Path.GetFileName(dbFilePath);
                        Logger.Info($"Выбран файл для экспорта: {dbFilePath}");
                    }
                    else
                    {
                        Logger.Info("Выбор файла отменен");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonChooseFile_Click", ex);
                MessageHelper.MessageWindowOk($"Ошибка при выборе файла: {ex.Message}", "Ошибка");
            }
        }

        private async void jeanModernButtonExport_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Нажата кнопка 'Экспорт'");

                if (string.IsNullOrEmpty(dbFilePath))
                {
                    Logger.Warning("Попытка экспорта без выбранного файла");
                    MessageHelper.MessageWindowOk("Файл не выбран", "Предупреждение");
                    return;
                }

                string fileName = Path.GetFileName(dbFilePath);
                string outputPath = Path.ChangeExtension(dbFilePath, GetFileExtension());
                string sqlQuery = GetSqlQuery(fileName, out string connectionString);

                if (sqlQuery == null)
                {
                    Logger.Warning($"Некорректный файл: {fileName}");
                    MessageHelper.MessageWindowOk("Некорректный файл", "Предупреждение");
                    return;
                }

                Logger.Info($"Начало экспорта: файл={fileName}, формат={GetFileExtension()}");
                await ExportDataAsync(outputPath, sqlQuery, connectionString);
                MessageHelper.MessageWindowOk($"Файл {fileName} экспортирован в формат {Path.GetExtension(outputPath)}", "Сообщение");
                Logger.Info($"Экспорт завершен: {outputPath}");
                dbFilePath = "";
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в jeanModernButtonExport_Click", ex);
                MessageHelper.MessageWindowOk($"Ошибка при экспорте: {ex.Message}", "Ошибка");
            }
        }

        private void HandleCheckBoxChanged(System.Windows.Forms.CheckBox changedCheckBox, params System.Windows.Forms.CheckBox[] otherCheckBoxes)
        {
            try
            {
                if (changedCheckBox.Checked)
                {
                    foreach (var checkBox in otherCheckBoxes)
                    {
                        checkBox.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в HandleCheckBoxChanged для {changedCheckBox?.Name}", ex);
            }
        }

        private void checkBoxClientsForPeriod_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxClientsForPeriod, checkBoxSellServices);

        private void checkBoxSellServices_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxSellServices, checkBoxClientsForPeriod);

        private void checkBoxXLS_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxXLS, checkBoxTXT, checkBoxJSON, checkBoxCSV, checkBoxTSV);

        private void checkBoxTXT_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxTXT, checkBoxXLS, checkBoxJSON, checkBoxCSV, checkBoxTSV);

        private void checkBoxJSON_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxJSON, checkBoxXLS, checkBoxTXT, checkBoxCSV, checkBoxTSV);

        private void checkBoxCSV_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxCSV, checkBoxXLS, checkBoxTXT, checkBoxJSON, checkBoxTSV);

        private void checkBoxTSV_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxTSV, checkBoxXLS, checkBoxTXT, checkBoxJSON, checkBoxCSV);

        private string GetFileExtension()
        {
            try
            {
                if (checkBoxXLS.Checked) return ".xls";
                if (checkBoxTXT.Checked) return ".txt";
                if (checkBoxJSON.Checked) return ".json";
                if (checkBoxCSV.Checked) return ".csv";
                if (checkBoxTSV.Checked) return ".tsv";
                return string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в GetFileExtension", ex);
                return string.Empty;
            }
        }

        private string GetSqlQuery(string fileName, out string connectionString)
        {
            try
            {
                switch (fileName)
                {
                    case "Clients.db":
                        connectionString = ClientsContext.ConnectionStringClients();
                        return "SELECT * FROM Contacts";
                    case "Services.db":
                        connectionString = ServicesContext.ConnectionStringServices();
                        return "SELECT * FROM Descriptions";
                    case "Payments.db":
                        connectionString = HistoryPaymentContext.ConnectionStringPayment();
                        return "SELECT * FROM History";
                    case "Archive.db":
                        connectionString = ArchiveServicesContext.ConnectionStringArchive();
                        return "SELECT * FROM Archive";
                    case "IssuedMembership.db":
                        connectionString = IssuedMembershipContext.ConnectionStringIssued();
                        return "SELECT * FROM Issued";
                    case "Products.db":
                        connectionString = ProductsContext.ConnectionStringProducts();
                        return "SELECT * FROM Items";
                    default:
                        connectionString = null;
                        Logger.Warning($"Неизвестный файл БД: {fileName}");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в GetSqlQuery для файла {fileName}", ex);
                connectionString = null;
                return null;
            }
        }

        private void UpdateLoadingScreen(string message)
        {
            splashScreen.UpdateProgress(message);
        }

        private async Task<int> GetRowCountAsync(string connectionString, string tableName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var conn = new SQLiteConnection(connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SQLiteCommand($"SELECT COUNT(*) FROM {tableName}", conn))
                        {
                            return Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Ошибка в GetRowCountAsync для таблицы {tableName}", ex);
                    return 0;
                }
            });
        }

        private string GetTableName(string fileName)
        {
            try
            {
                switch (fileName)
                {
                    case "Clients.db": return "Contacts";
                    case "Services.db": return "Descriptions";
                    case "Payments.db": return "History";
                    case "Archive.db": return "Archive";
                    case "IssuedMembership.db": return "Issued";
                    case "Products.db": return "Items";
                    default: return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в GetTableName для файла {fileName}", ex);
                return null;
            }
        }

        private async Task ExportDataAsync(string filePath, string sqlQuery, string connectionString)
        {
            try
            {
                if (checkBoxXLS.Checked)
                {
                    var progress = new Progress<string>(UpdateLoadingScreen);
                    int rowCount = await GetRowCountAsync(connectionString, GetTableName(Path.GetFileName(dbFilePath)));
                    await ExportToExcelAsync(filePath, sqlQuery, connectionString, progress, rowCount);
                }
                else if (checkBoxTXT.Checked)
                {
                    ExportToTextFile(filePath, sqlQuery, connectionString);
                }
                else if (checkBoxJSON.Checked)
                {
                    ExportToJson(filePath, sqlQuery, connectionString);
                }
                else if (checkBoxCSV.Checked)
                {
                    ExportToDelimitedFile(filePath, sqlQuery, connectionString, ";");
                }
                else if (checkBoxTSV.Checked)
                {
                    ExportToDelimitedFile(filePath, sqlQuery, connectionString, "\t");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ExportDataAsync для {filePath}", ex);
                throw;
            }
        }

        private async Task ExportToExcelAsync(string excelFilePath, string sqlQuery, string connectionString, IProgress<string> progress, int rowCount)
        {
            try
            {
                if (File.Exists(excelFilePath))
                {
                    Logger.Warning($"Файл уже существует: {excelFilePath}");
                    MessageHelper.MessageWindowOk("Файл уже экспортирован", "Предупреждение");
                    return;
                }

                splashScreen.Show();
                progress.Report("    Подготовление    ");
                Logger.Info($"Начало экспорта в Excel: {excelFilePath}");

                await Task.Run(() =>
                {
                    using (var conn = new SQLiteConnection(connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SQLiteCommand(sqlQuery, conn))
                        using (var reader = cmd.ExecuteReader())
                        {
                            var excelApp = new Microsoft.Office.Interop.Excel.Application();
                            var workbook = excelApp.Workbooks.Add();
                            var worksheet = (Worksheet)workbook.Worksheets[1];

                            int row = 1;
                            while (reader.Read())
                            {
                                progress.Report($"Обработано {row}/{rowCount}");
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    worksheet.Cells[row, i + 1].Value2 = reader[i];
                                }
                                row++;
                            }

                            workbook.SaveAs(excelFilePath);
                            workbook.Close();
                            excelApp.Quit();
                            Logger.Info($"Экспорт в Excel завершен: {excelFilePath}, строк: {row - 1}");
                        }
                    }
                });

                splashScreen.Close();
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ExportToExcelAsync для {excelFilePath}", ex);
                splashScreen.Close();
                throw;
            }
        }

        private void ExportToTextFile(string txtFilePath, string sqlQuery, string connectionString)
        {
            try
            {
                if (File.Exists(txtFilePath))
                {
                    Logger.Warning($"Файл уже существует: {txtFilePath}");
                    MessageHelper.MessageWindowOk("Файл уже экспортирован", "Предупреждение");
                    return;
                }

                Logger.Info($"Начало экспорта в TXT: {txtFilePath}");

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sqlQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    using (var writer = new StreamWriter(txtFilePath))
                    {
                        // Write headers
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            int columnWidth = GetColumnWidth(i);
                            writer.Write(reader.GetName(i).PadRight(columnWidth) + "|");
                        }
                        writer.WriteLine();

                        // Write data
                        int rowCount = 0;
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int columnWidth = GetColumnWidth(i);
                                writer.Write(reader[i].ToString().PadRight(columnWidth) + "|");
                            }
                            writer.WriteLine();
                            rowCount++;
                        }
                        Logger.Info($"Экспорт в TXT завершен: {txtFilePath}, строк: {rowCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ExportToTextFile для {txtFilePath}", ex);
                throw;
            }
        }

        private int GetColumnWidth(int columnIndex)
        {
            if (columnIndex == 0)
                return 3;
            if (columnIndex == 1)
                return 30;
            if (columnIndex < 6)
                return 15;
            if (columnIndex < 8)
                return 20;
            if (columnIndex < 9)
                return 30;
            return 8;
        }

        private void ExportToJson(string jsonFilePath, string sqlQuery, string connectionString)
        {
            try
            {
                if (File.Exists(jsonFilePath))
                {
                    Logger.Warning($"Файл уже существует: {jsonFilePath}");
                    MessageHelper.MessageWindowOk("Файл уже экспортирован", "Предупреждение");
                    return;
                }

                Logger.Info($"Начало экспорта в JSON: {jsonFilePath}");

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sqlQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var jsonArray = new System.Text.Json.Nodes.JsonArray();
                        int rowCount = 0;
                        while (reader.Read())
                        {
                            var jsonObject = new System.Text.Json.Nodes.JsonObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                jsonObject.Add(reader.GetName(i), JsonSerializer.Serialize(reader[i]));
                            }
                            jsonArray.Add(jsonObject);
                            rowCount++;
                        }

                        File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(jsonArray));
                        Logger.Info($"Экспорт в JSON завершен: {jsonFilePath}, записей: {rowCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ExportToJson для {jsonFilePath}", ex);
                throw;
            }
        }

        private void ExportToDelimitedFile(string filePath, string sqlQuery, string connectionString, string delimiter)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    Logger.Warning($"Файл уже существует: {filePath}");
                    MessageHelper.MessageWindowOk("Файл уже экспортирован", "Предупреждение");
                    return;
                }

                Logger.Info($"Начало экспорта в {Path.GetExtension(filePath)} (разделитель: '{delimiter}'): {filePath}");

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sqlQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    using (var writer = new StreamWriter(filePath))
                    {
                        // Write headers
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            writer.Write(reader.GetName(i));
                            if (i < reader.FieldCount - 1)
                            {
                                writer.Write(delimiter);
                            }
                        }
                        writer.WriteLine();

                        // Write data
                        int rowCount = 0;
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                writer.Write(reader[i]);
                                if (i < reader.FieldCount - 1)
                                {
                                    writer.Write(delimiter);
                                }
                            }
                            writer.WriteLine();
                            rowCount++;
                        }
                        Logger.Info($"Экспорт в {Path.GetExtension(filePath)} завершен: {filePath}, строк: {rowCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ExportToDelimitedFile для {filePath}", ex);
                throw;
            }
        }
    }
}