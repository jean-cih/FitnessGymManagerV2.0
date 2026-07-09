using GymApplicationV2._0.Components;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.FormsServices;
using Microsoft.Office.Interop.Excel;
using Shadow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GymApplicationV2._0
{
    public partial class Report : ShadowedForm
    {
        private string dbFilePath = "";

        SplashScreen splashScreen = new SplashScreen();

        JeanModernButton jeanModernButtonShow;
        JeanModernButton jeanModernButtonExport;
        JeanModernButton jeanModernButtonChooseFile;

        System.Windows.Forms.CheckBox checkBoxHistoryPayment;
        System.Windows.Forms.CheckBox checkBoxSellServices;
        System.Windows.Forms.CheckBox checkBoxClientsForPeriod;
        System.Windows.Forms.CheckBox checkBoxAllClients;

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

        // Цветовая схема
        private readonly Color PrimaryColor = Color.FromArgb(63, 81, 181);
        private readonly Color SecondaryColor = Color.FromArgb(103, 58, 183);
        private readonly Color AccentColor = Color.FromArgb(0, 150, 136);
        private readonly Color BackgroundColor = Color.FromArgb(245, 245, 245);
        private readonly Color CardColor = Color.White;
        private readonly Color TextColor = Color.FromArgb(33, 33, 33);
        private readonly Color LightTextColor = Color.FromArgb(117, 117, 117);

        private Timer _fadeTimer;
        private float _opacity = 0;

        Panel titlePanel;

        public Report()
        {
            InitializeComponent();
            InitializeCustomDesign();
            titlePanel.MouseDown += Panel_MouseDown;
            titlePanel.MouseMove += Panel_MouseMove;
            titlePanel.MouseUp += Panel_MouseUp;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;
            SetupAnimation();
        }

        private bool isDragging = false;
        private System.Drawing.Point lastCursor;
        private System.Drawing.Point lastForm;

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursor = Cursor.Position;
                lastForm = this.Location;
            }
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                System.Drawing.Point diff = System.Drawing.Point.Subtract(Cursor.Position, new Size(lastCursor));
                this.Location = System.Drawing.Point.Add(lastForm, new Size(diff));
            }
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void InitializeCustomDesign()
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

            titlePanel = new System.Windows.Forms.Panel
            {
                Size = new Size(918, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new System.Drawing.Point(0,0),
            };

            // Заголовок
            var titleLabel = new System.Windows.Forms.Label
            {
                Text = "📊 Отчёт",
                Font = new System.Drawing.Font("Montserrat", 18, FontStyle.Bold),
                ForeColor = ForeColor = Color.FromArgb(220, 220, 255),
                BackColor = Color.Transparent,
                Location = new System.Drawing.Point(380, 10),
                AutoSize = true,
            };
            titlePanel.Controls.Add(titleLabel);
            this.Controls.Add(titlePanel);

            // Создаем карточки
            CreateReportTypeCard();
            CreatePeriodCard();
            CreateExportCard();
            CreateButtons();
        }

        private void CreateReportTypeCard()
        {
            var card = new JeanPanel
            {
                Size = new Size(250, 280),
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
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new System.Drawing.Point(60, 5),
                AutoSize = true
            };

            var visitsLabel = new System.Windows.Forms.Label
            {
                Text = "Посещения",
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new System.Drawing.Point(10, 45),
                AutoSize = true
            };

            checkBoxAllClients = CreateStyledCheckBox("Все клиенты", new System.Drawing.Point(10, 75));
            checkBoxClientsForPeriod = CreateStyledCheckBox("Посещаемость по дням", new System.Drawing.Point(10, 105));

            var servicesLabel = new System.Windows.Forms.Label
            {
                Text = "Абонементы и услуги",
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new System.Drawing.Point(10, 145),
                AutoSize = true
            };

            checkBoxSellServices = CreateStyledCheckBox("Количество проданных", new System.Drawing.Point(10, 175));
            checkBoxHistoryPayment = CreateStyledCheckBox("История платежей", new System.Drawing.Point(10, 205));

            card.Controls.AddRange(new Control[] { title, visitsLabel, checkBoxAllClients, checkBoxClientsForPeriod,
                servicesLabel, checkBoxSellServices, checkBoxHistoryPayment });

            this.Controls.Add(card);
        }

        private void CreatePeriodCard()
        {
            var card = new JeanPanel
            {
                Size = new Size(320, 210),
                Location = new System.Drawing.Point(300, 80),
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
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new System.Drawing.Point(100, 5),
                AutoSize = true
            };

            radioForMonth = CreateStyledRadioButton("За месяц", new System.Drawing.Point(10, 45));
            radioForWeek = CreateStyledRadioButton("За неделю", new System.Drawing.Point(10, 75));
            radioForDay = CreateStyledRadioButton("За день", new System.Drawing.Point(10, 105));
            radioOtherPeriod = CreateStyledRadioButton("Другой период", new System.Drawing.Point(10, 135));

            jeanDateTimePickerBegin = CreateStyledDateTimePicker(new Size(140, 15), new System.Drawing.Point(10, 165));
            jeanDateTimePickerEnd = CreateStyledDateTimePicker(new Size(140, 15), new System.Drawing.Point(170, 165));

            card.Controls.AddRange(new Control[] { title, radioForMonth, radioForWeek, radioForDay, radioOtherPeriod, jeanDateTimePickerBegin, jeanDateTimePickerEnd });
            this.Controls.Add(card);
        }

        private JeanDateTimePicker CreateStyledDateTimePicker(Size size, System.Drawing.Point location)
        {
            var dateTime = new JeanDateTimePicker
            {
                Size = size,
                Location = location,
                Font = new System.Drawing.Font("Segoe UI", 8),
                TextColor = Color.Black,
                BorderColor = Color.MediumSlateBlue,
                SkinColor = Color.Transparent,
                BorderSize = 2,
                AutoSize = true,
            };

            return dateTime;
        }

        private void CreateExportCard()
        {
            var card = new JeanPanel
            {
                Size = new Size(250, 200),
                Location = new System.Drawing.Point(640, 80),
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
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new System.Drawing.Point(35, 5),
                AutoSize = true
            };

            checkBoxXLS = CreateStyledCheckBox("Excel (.xls)", new System.Drawing.Point(10, 45));
            checkBoxCSV = CreateStyledCheckBox("CSV (.csv)", new System.Drawing.Point(10, 75));
            checkBoxTXT = CreateStyledCheckBox("Text (.txt)", new System.Drawing.Point(10, 105));
            checkBoxJSON = CreateStyledCheckBox("JSON (.json)", new System.Drawing.Point(10, 135));
            checkBoxTSV = CreateStyledCheckBox("TSV (.tsv)", new System.Drawing.Point(120, 45));

            card.Controls.AddRange(new Control[] { title, checkBoxXLS, checkBoxCSV, checkBoxTXT, checkBoxJSON, checkBoxTSV });
            this.Controls.Add(card);
        }

        private void CreateButtons()
        {
            // Кнопка выбора файла
            jeanModernButtonChooseFile = CreateStyledButton("📁 Выбрать файл", PrimaryColor, new System.Drawing.Point(695, 365));
            jeanModernButtonChooseFile.Click += jeanModernButtonChooseFile_Click;

            // Кнопка экспорта
            jeanModernButtonExport = CreateStyledButton("🚀 Экспорт", AccentColor, new System.Drawing.Point(695, 315));
            jeanModernButtonExport.Click += jeanModernButtonExport_Click;

            // Кнопка показа
            jeanModernButtonShow = CreateStyledButton("👁️ Показать", SecondaryColor, new System.Drawing.Point(395, 315));
            jeanModernButtonShow.Click += buttonShow_Click;

            var btnClose = new JeanModernButton
            {
                Text = "X",
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 28),
                Cursor = Cursors.Hand,
                BorderRadius = 0,
                BorderSize = 0,
                Location = new System.Drawing.Point(878, 10),
            };

            btnClose.Click += (s, e) => CloseWithAnimation();
            titlePanel.Controls.Add(btnClose);

            this.Controls.AddRange(new Control[] { jeanModernButtonChooseFile, jeanModernButtonExport, jeanModernButtonShow });
        }

        private void CloseWithAnimation()
        {
            var closeTimer = new System.Windows.Forms.Timer();
            closeTimer.Interval = 10;
            float closeOpacity = 1;
            closeTimer.Tick += (s, args) =>
            {
                closeOpacity -= 0.05f;
                this.Opacity = closeOpacity;

                if (closeOpacity <= 0)
                {
                    closeTimer.Stop();
                    this.Close();
                }
            };
            closeTimer.Start();
        }

        private System.Windows.Forms.CheckBox CreateStyledCheckBox(string text, System.Drawing.Point location)
        {
            var checkBox = new System.Windows.Forms.CheckBox
            {
                Text = text,
                Location = location,
                Font = new System.Drawing.Font("Segoe UI", 9),
                ForeColor = TextColor,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            checkBox.CheckedChanged += (s, e) =>
            {
                checkBox.ForeColor = checkBox.Checked ? PrimaryColor : TextColor;
            };

            return checkBox;
        }

        private RadioButton CreateStyledRadioButton(string text, System.Drawing.Point location)
        {
            var radio = new RadioButton

            {
                Text = text,
                Location = location,
                Font = new System.Drawing.Font("Segoe UI", 9),
                ForeColor = TextColor,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            radio.CheckedChanged += (s, e) =>
            {
                radio.ForeColor = radio.Checked ? PrimaryColor : TextColor;
            };

            return radio;
        }

        private JeanModernButton CreateStyledButton(string text, Color backColor, System.Drawing.Point location)
        {
            var button = new JeanModernButton
            {
                Text = text,
                Location = location,
                Size = new Size(150, 40),
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.Transparent,
                BackgroundColor = backColor,
                TextColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BorderRadius = 20,
                BorderSize = 0,
                BorderColor = Color.FromArgb(255, 140, 0)
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.2f);
            button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.2f);

            return button;
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

        private void Report_Load(object sender, EventArgs e)
        {
            SetFonts();
        }

        private void SetFonts()
        {
            jeanModernButtonChooseFile.Font = new System.Drawing.Font("Выбрать", DataConfig.sizeFontButtons);
            jeanModernButtonExport.Font = new System.Drawing.Font("Экспортировать", DataConfig.sizeFontButtons);
            jeanModernButtonShow.Font = new System.Drawing.Font("Показать", DataConfig.sizeFontButtons);

            checkBoxAllClients.Font = new System.Drawing.Font("Все клиенты", DataConfig.sizeFontCaptions);
            checkBoxClientsForPeriod.Font = new System.Drawing.Font("Посещаемость по дням", DataConfig.sizeFontCaptions);
            checkBoxSellServices.Font = new System.Drawing.Font("Количество проданных", DataConfig.sizeFontCaptions);

            radioForMonth.Font = new System.Drawing.Font("За месяц", DataConfig.sizeFontCaptions - 2);
            radioForWeek.Font = new System.Drawing.Font("За неделю", DataConfig.sizeFontCaptions - 2);
            radioForDay.Font = new System.Drawing.Font("За день", DataConfig.sizeFontCaptions - 2);
            radioOtherPeriod.Font = new System.Drawing.Font("Другой период", DataConfig.sizeFontCaptions - 2);

            checkBoxXLS.Font = new System.Drawing.Font(".xls", DataConfig.sizeFontCaptions);
            checkBoxTXT.Font = new System.Drawing.Font("txt", DataConfig.sizeFontCaptions);
            checkBoxJSON.Font = new System.Drawing.Font(".json", DataConfig.sizeFontCaptions);
            checkBoxCSV.Font = new System.Drawing.Font(".csv", DataConfig.sizeFontCaptions);
            checkBoxTSV.Font = new System.Drawing.Font(".tsv", DataConfig.sizeFontCaptions);
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox[] otherCheckBoxe = { checkBoxAllClients, checkBoxClientsForPeriod, checkBoxSellServices, checkBoxHistoryPayment };
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
                Message.MessageWindowOk("Выберите тип отчета");
                return;
            }

            if (checkBoxHistoryPayment.Checked)
            {
                using (var history = new HistoryPayment())
                {
                    history.radioForMonth.Checked = radioForMonth.Checked;
                    history.radioForWeek.Checked = radioForWeek.Checked;
                    history.radioForDay.Checked = radioForDay.Checked;
                    history.radioOtherPeriod.Checked = radioOtherPeriod.Checked;

                    history.jeanDateTimePickerBegin.Value = jeanDateTimePickerBegin.Value;
                    history.jeanDateTimePickerEnd.Value = jeanDateTimePickerEnd.Value;

                    history.ShowDialog();
                }

                return;
            }

            using (var infoReport = new InformationReport())
            {
                infoReport.periodForMonth = radioForMonth.Checked;
                infoReport.periodForWeek = radioForWeek.Checked;
                infoReport.periodForDay = radioForDay.Checked;
                infoReport.otherPeriond = radioOtherPeriod.Checked;

                infoReport.dateBegin = jeanDateTimePickerBegin.Value;
                infoReport.dateEnd = jeanDateTimePickerEnd.Value;

                infoReport.allClient = checkBoxAllClients.Checked;
                infoReport.forPeriod = checkBoxClientsForPeriod.Checked;
                infoReport.sellServices = checkBoxSellServices.Checked;
                infoReport.historyPayment = checkBoxHistoryPayment.Checked;

                infoReport.ShowDialog();
            }
        }

        private void jeanModernButtonChooseFile_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog { Filter = "Database | *.db;" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    dbFilePath = openFileDialog.FileName;
                }
            }
        }
        private async void jeanModernButtonExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dbFilePath))
            {
                Message.MessageWindowOk("Файл не выбран");
                return;
            }

            string fileName = Path.GetFileName(dbFilePath);
            string outputPath = Path.ChangeExtension(dbFilePath, GetFileExtension());
            string sqlQuery = GetSqlQuery(fileName, out string connectionString);

            if (sqlQuery == null)
            {
                Message.MessageWindowOk("Некорректный файл");
                return;
            }

            await ExportDataAsync(outputPath, sqlQuery, connectionString);
            Message.MessageWindowOk($"Файл {fileName} экспортирован в формат {Path.GetExtension(outputPath)}");
            dbFilePath = "";
        }

        private void HandleCheckBoxChanged(System.Windows.Forms.CheckBox changedCheckBox, params System.Windows.Forms.CheckBox[] otherCheckBoxes)
        {
            if (changedCheckBox.Checked)
            {
                foreach (var checkBox in otherCheckBoxes)
                {
                    checkBox.Checked = false;
                }
            }
        }

        private void checkBoxClientsForPeriod_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxClientsForPeriod, checkBoxAllClients, checkBoxSellServices, checkBoxHistoryPayment);

        private void checkBoxAllClients_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxAllClients, checkBoxClientsForPeriod, checkBoxSellServices, checkBoxHistoryPayment);

        private void checkBoxSellServices_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxSellServices, checkBoxClientsForPeriod, checkBoxAllClients, checkBoxHistoryPayment);

        private void checkBoxHistoryPayment_CheckedChanged(object sender, EventArgs e) =>
            HandleCheckBoxChanged(checkBoxHistoryPayment, checkBoxClientsForPeriod, checkBoxAllClients, checkBoxSellServices);

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
            if (checkBoxXLS.Checked) return ".xls";
            if (checkBoxTXT.Checked) return ".txt";
            if (checkBoxJSON.Checked) return ".json";
            if (checkBoxCSV.Checked) return ".csv";
            if (checkBoxTSV.Checked) return ".tsv";
            return string.Empty;
        }

        private string GetSqlQuery(string fileName, out string connectionString)
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
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand($"SELECT COUNT(*) FROM {tableName}", conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            });
        }

        private string GetTableName(string fileName)
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

        private async Task ExportDataAsync(string filePath, string sqlQuery, string connectionString)
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

        private async Task ExportToExcelAsync(string excelFilePath, string sqlQuery, string connectionString, IProgress<string> progress, int rowCount)
        {
            if (File.Exists(excelFilePath))
            {
                Message.MessageWindowOk("Файл уже экспортирован");
                return;
            }

            splashScreen.Show();
            progress.Report("    Подготовление    ");

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
                    }
                }
            });

            splashScreen.Close();
        }

        private void ExportToTextFile(string txtFilePath, string sqlQuery, string connectionString)
        {
            if (File.Exists(txtFilePath))
            {
                Message.MessageWindowOk("Файл уже экспортирован");
                return;
            }

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
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            int columnWidth = GetColumnWidth(i);
                            writer.Write(reader[i].ToString().PadRight(columnWidth) + "|");
                        }
                        writer.WriteLine();
                    }
                }
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
            if (File.Exists(jsonFilePath))
            {
                Message.MessageWindowOk("Файл уже экспортирован");
                return;
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sqlQuery, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    var jsonArray = new System.Text.Json.Nodes.JsonArray();
                    while (reader.Read())
                    {
                        var jsonObject = new System.Text.Json.Nodes.JsonObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            jsonObject.Add(reader.GetName(i), JsonSerializer.Serialize(reader[i]));
                        }
                        jsonArray.Add(jsonObject);
                    }

                    File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(jsonArray));
                }
            }
        }

        private void ExportToDelimitedFile(string filePath, string sqlQuery, string connectionString, string delimiter)
        {
            if (File.Exists(filePath))
            {
                Message.MessageWindowOk("Файл уже экспортирован");
                return;
            }

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
                    }
                }
            }
        }

        private void SetupAnimation()
        {
            _fadeTimer = new Timer();
            _fadeTimer.Interval = 10;
            _fadeTimer.Tick += (s, e) =>
            {
                _opacity += 0.05f;
                this.Opacity = _opacity;

                if (_opacity >= 1)
                {
                    _fadeTimer.Stop();
                    _fadeTimer.Dispose();
                }
            };
            _fadeTimer.Start();
        }
    }
}