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

        public Dictionary<string, string> userStatus = new Dictionary<string, string>();

        public Report()
        {
            InitializeComponent();
            InitializeCustomDesign();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            titlePanel.EnableDrag(this);
        }

        private void Report_Load(object sender, EventArgs e)
        {
            string[] notChangeableTexts = new string[]
            {
                "📊 Отчёт"
            };

            FontHelper.ApplyFontSettings(this, notChangeableTexts);
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

            titlePanel = new Panel
            {
                Size = new Size(1000, 50),
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
        }

        private void CreateReportTypeCard()
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

            checkBoxClientsForPeriod = CreateStyledCheckBox("Посещаемость", new System.Drawing.Point((card.Width - 130) / 2, 95));
            checkBoxClientsForPeriod.Checked = true;
            checkBoxClientsForPeriod.CheckedChanged += checkBoxClientsForPeriod_CheckedChanged;


            checkBoxSellServices = CreateStyledCheckBox("Количество проданных\nабонементов", new System.Drawing.Point((card.Width - 130) / 2, 175));
            checkBoxSellServices.CheckedChanged += checkBoxSellServices_CheckedChanged;

            card.Controls.AddRange(new Control[] { title, checkBoxClientsForPeriod, checkBoxSellServices });

            this.Controls.Add(card);
        }

        private void CreatePeriodCard()
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
                Location = new System.Drawing.Point((card.Width - 120) / 2 , 20),
            };

            radioForMonth = CreateStyledRadioButton("За месяц", new System.Drawing.Point(card.Width / 3 + 5, 55));
            radioForWeek = CreateStyledRadioButton("За неделю", new System.Drawing.Point(card.Width / 3 + 5, 85));
            radioForDay = CreateStyledRadioButton("За день", new System.Drawing.Point(card.Width / 3 + 5,115), true);
            radioOtherPeriod = CreateStyledRadioButton("Другой период", new System.Drawing.Point(card.Width / 3 + 5, 145));

            jeanDateTimePickerBegin = CreateStyledDateTimePicker(new Size(140, 15), new System.Drawing.Point(card.Width / 2 - 150, 175));
            jeanDateTimePickerEnd = CreateStyledDateTimePicker(new Size(140, 15), new System.Drawing.Point(card.Width / 2 + 10, 175));

            card.Controls.AddRange(new Control[] { title, radioForMonth, radioForWeek, radioForDay, radioOtherPeriod, jeanDateTimePickerBegin, jeanDateTimePickerEnd });
            this.Controls.Add(card);
        }

        private JeanDateTimePicker CreateStyledDateTimePicker(Size size, System.Drawing.Point location)
        {
            var dateTime = new JeanDateTimePicker
            {
                Size = size,
                Location = location,
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

            checkBoxXLS = CreateStyledCheckBox("Excel (.xls)", new System.Drawing.Point(card.Width / 3 + 5, 55), true);
            checkBoxCSV = CreateStyledCheckBox("CSV (.csv)", new System.Drawing.Point(card.Width / 3 + 5, 85));
            checkBoxTXT = CreateStyledCheckBox("Text (.txt)", new System.Drawing.Point(card.Width / 3 + 5, 115));
            checkBoxJSON = CreateStyledCheckBox("JSON (.json)", new System.Drawing.Point(card.Width / 3 + 5, 145));
            checkBoxTSV = CreateStyledCheckBox("TSV (.tsv)", new System.Drawing.Point(card.Width / 3 + 5, 175));

            // Кнопка выбора файла
            jeanModernButtonChooseFile = CreateStyledButton("📁 Выбрать файл", PrimaryColor, new System.Drawing.Point((card.Width - 150) / 2, 205), new Size(150, 45));
            jeanModernButtonChooseFile.Click += jeanModernButtonChooseFile_Click;

            // Кнопка экспорта
            jeanModernButtonExport = CreateStyledButton("🚀 Экспорт", AccentColor, new System.Drawing.Point((card.Width - 180) / 2, 260), new Size(180, 50));
            jeanModernButtonExport.Click += jeanModernButtonExport_Click;

            card.Controls.AddRange(new Control[] { title, checkBoxXLS, checkBoxCSV, checkBoxTXT, checkBoxJSON, checkBoxTSV, jeanModernButtonChooseFile, jeanModernButtonExport });
            this.Controls.Add(card);
        }

        private void CreateButtons()
        {
            // Кнопка показа
            jeanModernButtonShow = CreateStyledButton("👁️ Показать", SecondaryColor, new System.Drawing.Point((this.Width - 200) / 2, 335), new Size(200, 60));
            jeanModernButtonShow.Click += buttonShow_Click;

            var btnClose = new JeanModernButton
            {
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 28),
                Cursor = Cursors.Hand,
                BorderRadius = 0,
                BorderSize = 0,
                Location = new System.Drawing.Point(958, 10),
            };

            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();
            titlePanel.Controls.Add(btnClose);

            this.Controls.AddRange(new Control[] { jeanModernButtonShow });
        }
        private System.Windows.Forms.CheckBox CreateStyledCheckBox(string text, System.Drawing.Point location, bool flag = false)
        {
            var checkBox = new System.Windows.Forms.CheckBox
            {
                Text = text,
                Location = location,
                ForeColor = TextColor,
                BackColor = Color.Transparent,
                Checked = flag,
                AutoSize = true
            };

            checkBox.CheckedChanged += (s, e) =>
            {
                checkBox.ForeColor = checkBox.Checked ? PrimaryColor : TextColor;
            };

            return checkBox;
        }

        private RadioButton CreateStyledRadioButton(string text, System.Drawing.Point location, bool flag = false)
        {
            var radio = new RadioButton

            {
                Text = text,
                Location = location,
                ForeColor = TextColor,
                AutoSize = true,
                BackColor = Color.Transparent,
                Checked = flag
            };

            radio.CheckedChanged += (s, e) =>
            {
                radio.ForeColor = radio.Checked ? PrimaryColor : TextColor;
            };

            return radio;
        }

        private JeanModernButton CreateStyledButton(string text, Color backColor, System.Drawing.Point location, Size size)
        {
            var button = new JeanModernButton
            {
                Text = text,
                Location = location,
                Size = size,
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

        private void buttonShow_Click(object sender, EventArgs e)
        {
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
                Message.MessageWindowOk("Выберите тип отчета");
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

                infoReport.forPeriod = checkBoxClientsForPeriod.Checked;
                infoReport.sellServices = checkBoxSellServices.Checked;

                infoReport.userStatus = userStatus;

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

                    jeanModernButtonExport.Text = "🚀 Экспортировать\n" + Path.GetFileName(dbFilePath);
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
    }
}