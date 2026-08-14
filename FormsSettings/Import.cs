using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using Microsoft.Office.Interop.Excel;
using Shadow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsSettings
{
    public partial class Import : ShadowedForm
    {
        private string _selectedFilePath = string.Empty;
        private readonly SplashScreen _splashScreen = new SplashScreen();
        private JeanModernButton chooseButton;
        private JeanModernButton importButton;
        private JeanModernButton documentationButton;
        private Panel dropZonePanel;
        private System.Windows.Forms.Label dropZoneLabel;
        private Panel titlePanel;
        private FadeAnimation _fadeAnimation;

        private readonly string[] _notChangeableTexts = new[] { "📤 Импорт данных" };
        private readonly object _dbLock = new object();

        public Import()
        {
            InitializeComponent();
            InitializeComponents();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, _notChangeableTexts);

            titlePanel.EnableDrag(this);
        }

        private void InitializeComponents()
        {
            this.Padding = new Padding(20, 1, 20, 20);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;

            SetupFormPaint();
            CreateTitlePanel();
            CreateImportCard();
            SetupDragDropEvents();
        }

        private void SetupFormPaint()
        {
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

                using (var pen = new Pen(Color.FromArgb(80, 120, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, new System.Drawing.Rectangle(0, 0, Width - 1, Height - 1));
                }
            };
        }

        private void CreateTitlePanel()
        {
            titlePanel = new Panel
            {
                Size = new Size(874, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new System.Drawing.Point(0, 0),
            };

            var titleLabel = new System.Windows.Forms.Label
            {
                Text = "📤 Импорт данных",
                Font = new System.Drawing.Font("Montserrat", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 220, 255),
                BackColor = Color.Transparent,
                Location = new System.Drawing.Point(320, 10),
                AutoSize = true,
            };

            var btnClose = UIStyler.CreateStyledButton(
                "X",
                Color.FromArgb(180, 70, 70),
                0, 0,
                Color.FromArgb(255, 140, 0),
                new System.Drawing.Point(this.Width - 40, 10),
                new Size(30, 28));

            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            titlePanel.Controls.Add(titleLabel);
            titlePanel.Controls.Add(btnClose);
            this.Controls.Add(titlePanel);
        }

        private void CreateImportCard()
        {
            var importCard = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(10),
                Location = new System.Drawing.Point(20, 50),
                Size = new Size(834, 425),
            };

            importCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 5))
                {
                    e.Graphics.DrawRectangle(pen, new System.Drawing.Rectangle(0, 0, importCard.Width - 1, importCard.Height - 1));
                }
            };

            InitializeDropZone(importCard);
            InitializeActionButtons(importCard);
            this.Controls.Add(importCard);
        }

        private void SetupDragDropEvents()
        {
            this.AllowDrop = true;
            this.DragEnter += Import_DragEnter;
            this.DragDrop += Import_DragDrop;
        }

        private void InitializeDropZone(Panel parent)
        {
            dropZonePanel = new JeanPanel
            {
                Size = new Size(600, 250),
                Location = new System.Drawing.Point(parent.Width / 2 - 300, 50),
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(55, 55, 58),
                GradientBottomColor = Color.FromArgb(55, 55, 58),
                GradientTapColor = Color.FromArgb(55, 55, 58),
                BorderRadius = 20
            };

            dropZoneLabel = new System.Windows.Forms.Label
            {
                Text = "📎 Перетащите Excel файл сюда\n\nили нажмите Enter",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", DataConfig.sizeFontText, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 0)
            };

            var tooltip = new System.Windows.Forms.ToolTip();
            var tooltipText = "📋 Поддерживаемые таблицы:\n    • Clients • Services • Archive \n    • Payments • IssuedMembership";
            tooltip.SetToolTip(dropZonePanel, tooltipText);
            tooltip.SetToolTip(dropZoneLabel, tooltipText);

            dropZonePanel.Click += (s, e) => ChooseFile_Click(s, e);
            dropZonePanel.Controls.Add(dropZoneLabel);
            parent.Controls.Add(dropZonePanel);
        }

        private void InitializeActionButtons(Panel parent)
        {
            var buttonPanel = new Panel
            {
                Size = new Size(800, 130),
                Location = new System.Drawing.Point(20, 290)
            };

            chooseButton = new JeanModernButton
            {
                Text = "📁 Выбрать файл",
                Size = new Size(170, 40),
                Location = new System.Drawing.Point(0, 75),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold),
                BorderRadius = 8,
            };
            chooseButton.Click += ChooseFile_Click;

            importButton = new JeanModernButton
            {
                Text = "🚀 Импортировать",
                Size = new Size(190, 40),
                Location = new System.Drawing.Point(190, 75),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold),
                BorderRadius = 8,
            };
            importButton.Click += ImportButton_Click;
            importButton.Enabled = false;

            documentationButton = new JeanModernButton
            {
                Text = "📄 Документация",
                Size = new Size(170, 40),
                Location = new System.Drawing.Point(625, 75),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold),
                BorderRadius = 8,
            };
            documentationButton.Click += DocumentationButton_Click;

            buttonPanel.Controls.Add(chooseButton);
            buttonPanel.Controls.Add(importButton);
            buttonPanel.Controls.Add(documentationButton);
            parent.Controls.Add(buttonPanel);
        }

        private void DocumentationButton_Click(object sender, EventArgs e)
        {
            using (var documentation = new Documentation())
            {
                documentation.ShowDialog();
            }
        }

        private void ChooseFile_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm|All files|*.*";
                openFileDialog.Title = "Выберите Excel файл для импорта";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _selectedFilePath = openFileDialog.FileName;
                    UpdateFileSelection();
                }
            }
        }

        private void UpdateFileSelection()
        {
            if (!string.IsNullOrEmpty(_selectedFilePath))
            {
                var fileName = Path.GetFileName(_selectedFilePath);
                dropZoneLabel.Text = $"✅ {fileName}\n\n📊 Готов к импорту";
                dropZoneLabel.ForeColor = Color.LightGreen;
                importButton.Enabled = true;

                AnimateDropZoneSuccess();
            }
        }

        private void AnimateDropZoneSuccess()
        {
            var timer = new System.Windows.Forms.Timer { Interval = 100 };
            int counter = 0;

            timer.Tick += (s, e) =>
            {
                if (counter < 3)
                {
                    dropZonePanel.BackColor = counter % 2 == 0 ?
                        Color.FromArgb(60, 180, 75) :
                        Color.FromArgb(65, 65, 68);
                    counter++;
                }
                else
                {
                    dropZonePanel.BackColor = Color.FromArgb(65, 65, 68);
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();
        }

        private async void ImportButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                MessageHelper.MessageWindowOk("⚠️ Файл не выбран\nПожалуйста, выберите файл для импорта", "Предупреждение");
                return;
            }

            if (!await CheckDatabaseExistsAsync())
            {
                return;
            }

            await ImportDataAsync();
        }

        private async Task<bool> CheckDatabaseExistsAsync()
        {
            var fileName = Path.GetFileNameWithoutExtension(_selectedFilePath);
            var dbPath = Path.Combine("Databases", $"{fileName}.db");

            bool fileExists = await Task.Run(() => File.Exists(dbPath));

            if (fileExists)
            {
                var result = MessageHelper.MessageWindowYesNo($"⚠️ База данных уже существует\nБаза данных '{fileName}.db' уже существует. Хотите удалить её и импортировать заново?");

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        await CloseAllDatabaseConnectionsAsync(fileName);

                        await Task.Delay(500);

                        await Task.Run(() => File.Delete(dbPath));
                        dropZoneLabel.Text = $"🗑️ Удалена существующая база: {fileName}.db\n\nЗагружается новая база";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.MessageWindowOk($"❌ Ошибка удаления\nНе удалось удалить базу данных: {ex.Message}", "Ошибка");
                        return false;
                    }
                }
                else
                {
                    dropZoneLabel.Text = "⏹️ Импорт отменен: база данных уже существует";
                    return false;
                }
            }

            return true;
        }

        private async Task CloseAllDatabaseConnectionsAsync(string fileName)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            await Task.CompletedTask;
        }

        private async Task ImportDataAsync()
        {
            using (var splashScreen = new SplashScreen())
            {
                splashScreen.Show();
                importButton.Enabled = false;
                chooseButton.Enabled = false;

                try
                {
                    var progress = new Progress<string>(splashScreen.UpdateProgress);

                    await Task.Run(() => ImportExcelData(progress));

                    MessageHelper.MessageWindowOk("✅ Импорт завершен\nДанные успешно импортированы в базу данных!", "Успех");
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageHelper.MessageWindowOk($"❌ Ошибка импорта\nПроизошла ошибка: {ex.Message}", "Ошибка");
                }
                finally
                {
                    importButton.Enabled = true;
                    chooseButton.Enabled = true;
                }
            }
        }

        private void ImportExcelData(IProgress<string> progress)
        {
            progress.Report("🔍 Анализ файла...");
            Thread.Sleep(500);

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Workbook workbook = null;

            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                progress.Report("📖 Открытие Excel файла...");
                Thread.Sleep(500);

                workbook = excelApp.Workbooks.Open(_selectedFilePath);
                var worksheet = (Worksheet)workbook.Worksheets[1];

                progress.Report("📊 Чтение данных...");
                var dataTable = ReadExcelData(worksheet, progress);

                var fileName = Path.GetFileNameWithoutExtension(_selectedFilePath);
                var contextInfo = GetConnectionInfo(fileName);

                if (contextInfo.ConnectionString == null)
                {
                    throw new Exception("Неподдерживаемый формат файла. Ожидаются: Clients, Services, Archive, Payments, IssuedMembership, Products");
                }

                progress.Report("💾 Создание базы данных...");
                Thread.Sleep(500);

                lock (_dbLock)
                {
                    SQLiteConnection.ClearAllPools();

                    CreateDatabaseTable(contextInfo.TableName, dataTable, contextInfo.ConnectionString);

                    progress.Report("📥 Импорт данных...");
                    Thread.Sleep(500);

                    InsertDataIntoTable(contextInfo.TableName, dataTable, contextInfo.ConnectionString, progress);
                }

                progress.Report("✅ Завершение...");
                Thread.Sleep(500);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }

                // Принудительная очистка COM объектов
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private (string ConnectionString, string TableName) GetConnectionInfo(string fileName)
        {
            switch (fileName)
            {
                case "Clients": return (ClientsContext.ConnectionStringClients(), "Contacts");
                case "Services": return (ServicesContext.ConnectionStringServices(), "Descriptions");
                case "Archive": return (ArchiveServicesContext.ConnectionStringArchive(), "Archive");
                case "Payments": return (HistoryPaymentContext.ConnectionStringPayment(), "History");
                case "IssuedMembership": return (IssuedMembershipContext.ConnectionStringIssued(), "Issued");
                case "Products": return (ProductsContext.ConnectionStringProducts(), "Items");
                default: return (null, null);
            }
        }

        private void Import_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                dropZonePanel.BackColor = Color.FromArgb(80, 80, 85);
            }
        }

        private void Import_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && IsExcelFile(files[0]))
                {
                    _selectedFilePath = files[0];
                    UpdateFileSelection();
                }
                else
                {
                    MessageHelper.MessageWindowOk("⚠️ Неверный формат\nПожалуйста, выберите Excel файл", "Ошибка");
                }
            }
            dropZonePanel.BackColor = Color.FromArgb(65, 65, 68);
        }

        private bool IsExcelFile(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            return ext == ".xls" || ext == ".xlsx" || ext == ".xlsm";
        }

        private void ResetForm()
        {
            _selectedFilePath = string.Empty;
            dropZoneLabel.Text = "📎 Перетащите Excel файл сюда\nили нажмите для выбора";
            dropZoneLabel.ForeColor = Color.LightGray;
            importButton.Enabled = false;
        }

        private System.Data.DataTable ReadExcelData(Worksheet worksheet, IProgress<string> progress)
        {
            var dataTable = new System.Data.DataTable();
            var usedRange = worksheet.UsedRange;
            var rowCount = usedRange.Rows.Count;
            var colCount = usedRange.Columns.Count;

            for (int i = 1; i <= colCount; i++)
            {
                var cell = worksheet.Cells[1, i];
                string columnName = cell.Value2 != null ? cell.Value2.ToString().Trim() : $"Column{i}";
                dataTable.Columns.Add(columnName);
            }

            for (int i = 2; i <= rowCount; i++)
            {
                progress.Report($"Обработка: {i - 1}/{rowCount - 1}");

                var row = dataTable.NewRow();
                for (int j = 1; j <= colCount; j++)
                {
                    var cell = worksheet.Cells[i, j];
                    row[j - 1] = cell.Value2 != null ? cell.Value2.ToString() : DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private void CreateDatabaseTable(string tableName, System.Data.DataTable dataTable, string connectionString)
        {
            switch (tableName)
            {
                case "Contacts":
                    ClientsContext.CreatingDatabase();
                    break;
                case "Archive":
                    ArchiveServicesContext.CreatingDatabase();
                    break;
                case "History":
                    HistoryPaymentContext.CreatingDatabase();
                    break;
                case "Issued":
                    IssuedMembershipContext.CreatingDatabase();
                    break;
                case "Descriptions":
                    ServicesContext.CreatingDatabase();
                    break;
                case "Items":
                    ProductsContext.CreatingDatabase();
                    break;
                default:
                    CreateDynamicTable(tableName, dataTable, connectionString);
                    break;
            }
        }

        private void InsertDataIntoTable(string tableName, System.Data.DataTable dataTable, string connectionString, IProgress<string> progress)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                InsertDataIntoTable(connection, dataTable, tableName, progress);
            }
        }

        private void InsertDataIntoTable(SQLiteConnection connection, System.Data.DataTable dataTable, string tableName, IProgress<string> progress)
        {
            using (var transaction = connection.BeginTransaction())
            {
                var targetColumns = GetTableColumns(connection, tableName);
                var insertQuery = GenerateDynamicInsertQueryBasedOnTarget(tableName, targetColumns, dataTable.Columns);

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        progress.Report($"Импорт: {i + 1}/{dataTable.Rows.Count}");

                        command.Parameters.Clear();

                        foreach (var targetColumn in targetColumns)
                        {
                            if (targetColumn.Key == "Id") continue;

                            if (dataTable.Columns.Contains(targetColumn.Key))
                            {
                                object value = dataTable.Rows[i][targetColumn.Key];

                                if (IsDateColumn(targetColumn.Key) && value != DBNull.Value && value != null)
                                {
                                    value = ParseDateValue(value);
                                }

                                command.Parameters.AddWithValue($"@{targetColumn.Key}", value ?? DBNull.Value);
                            }
                            else
                            {
                                command.Parameters.AddWithValue($"@{targetColumn.Key}", DBNull.Value);
                            }
                        }

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        private bool IsDateColumn(string columnName)
        {
            string[] dateColumns = new[]
            {
        "Дата рождения", "Дата начала", "Дата платежа", "Время продажи",
        "Окончание заморозки", "Сохранено", "Дата оформления",
        "Дата окончания", "Посетил", "Дата_рождения", "Дата_начала",
        "Дата_платежа", "Время_продажи", "Окончание_заморозки",
        "Сохранено", "Дата_оформления", "Дата_окончания"
    };

            return dateColumns.Contains(columnName) || dateColumns.Contains(columnName.Replace("_", " "));
        }

        private object ParseDateValue(object value)
        {
            try
            {
                DateTime dateValue = DateTime.MinValue;
                bool parsed = false;

                // 1. Если это уже DateTime
                if (value is DateTime dt)
                {
                    dateValue = dt;
                    parsed = true;
                }
                // 2. Если это число (OLE Automation date или Unix timestamp)
                else if (value is double doubleValue && doubleValue > 0)
                {
                    // OLE Automation date (Excel даты)
                    try
                    {
                        dateValue = DateTime.FromOADate(doubleValue);
                        parsed = true;
                    }
                    catch
                    {
                        // Если не получилось, пробуем как Unix timestamp (секунды)
                        try
                        {
                            dateValue = DateTimeOffset.FromUnixTimeSeconds((long)doubleValue).DateTime;
                            parsed = true;
                        }
                        catch
                        {
                            // Пробуем как миллисекунды
                            try
                            {
                                dateValue = DateTimeOffset.FromUnixTimeMilliseconds((long)doubleValue).DateTime;
                                parsed = true;
                            }
                            catch { }
                        }
                    }
                }
                // 3. Если это строка
                else if (value is string str && !string.IsNullOrWhiteSpace(str))
                {
                    // Убираем лишние пробелы
                    str = str.Trim();

                    // Пробуем различные форматы
                    string[] formats = new[]
                    {
                "dd.MM.yyyy HH:mm:ss",
                "dd.MM.yyyy",
                "yyyy-MM-dd",
                "yyyy-MM-dd HH:mm:ss",
                "dd.MM.yy",
                "dd.MM.yy HH:mm:ss",
                "MM/dd/yyyy",
                "MM/dd/yyyy HH:mm:ss",
                "dd/MM/yyyy",
                "dd/MM/yyyy HH:mm:ss",
                "yyyy/MM/dd",
                "yyyy/MM/dd HH:mm:ss",
                "dd-MM-yyyy",
                "dd-MM-yyyy HH:mm:ss",
                "MM-dd-yyyy",
                "MM-dd-yyyy HH:mm:ss",
                "dd.MM.yyyy HH:mm",
                "dd.MM.yy HH:mm",
                "dd.MM.yyyy H:mm:ss",
                "d.M.yyyy",
                "d.M.yy",
                "d.MM.yyyy",
                "dd.M.yyyy"
            };

                    // Пробуем парсить с учетом культуры
                    var cultures = new[]
                    {
                CultureInfo.InvariantCulture,
                new CultureInfo("ru-RU"),
                new CultureInfo("en-US")
            };

                    foreach (var culture in cultures)
                    {
                        if (DateTime.TryParseExact(str, formats, culture, DateTimeStyles.None, out dateValue))
                        {
                            parsed = true;
                            break;
                        }
                    }

                    // Если не получилось, пробуем обычный Parse
                    if (!parsed)
                    {
                        foreach (var culture in cultures)
                        {
                            if (DateTime.TryParse(str, culture, DateTimeStyles.None, out dateValue))
                            {
                                parsed = true;
                                break;
                            }
                        }
                    }
                }
                // 4. Если это DateTimeOffset
                else if (value is DateTimeOffset dto)
                {
                    dateValue = dto.DateTime;
                    parsed = true;
                }

                // Если удалось распарсить и дата не минимальная - возвращаем в ISO формате
                if (parsed && dateValue != DateTime.MinValue && dateValue.Year > 1900)
                {
                    return dateValue.ToString("yyyy-MM-dd");
                }

                // Если не удалось распарсить - возвращаем DBNull
                return DBNull.Value;
            }
            catch
            {
                // Если произошла ошибка - возвращаем DBNull
                return DBNull.Value;
            }
        }

        private string GenerateDynamicInsertQueryBasedOnTarget(string tableName, Dictionary<string, string> targetColumns, DataColumnCollection sourceColumns)
        {
            string escapedTableName = EscapeSqlIdentifier(tableName);

            var columnsBuilder = new StringBuilder();
            var parametersBuilder = new StringBuilder();

            bool first = true;
            foreach (var column in targetColumns)
            {
                if (column.Key == "Id") continue;

                if (!first)
                {
                    columnsBuilder.Append(", ");
                    parametersBuilder.Append(", ");
                }

                string escapedColumnName = EscapeSqlIdentifier(column.Key);
                columnsBuilder.Append(escapedColumnName);
                parametersBuilder.Append($"@{column.Key}");

                first = false;
            }

            return $"INSERT INTO {escapedTableName} ({columnsBuilder}) VALUES ({parametersBuilder})";
        }

        private Dictionary<string, string> GetTableColumns(SQLiteConnection connection, string tableName)
        {
            var columns = new Dictionary<string, string>();

            string query = $"PRAGMA table_info({EscapeSqlIdentifier(tableName)})";

            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string columnName = reader["name"].ToString();
                    string dataType = reader["type"].ToString();
                    columns.Add(columnName, dataType);
                }
            }

            return columns;
        }

        private void CreateDynamicTable(string tableName, System.Data.DataTable dataTable, string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var columns = new StringBuilder();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string columnName = EscapeSqlIdentifier(dataTable.Columns[i].ColumnName);
                    columns.Append($"{columnName} TEXT");

                    if (i < dataTable.Columns.Count - 1)
                    {
                        columns.Append(", ");
                    }
                }

                string escapedTableName = EscapeSqlIdentifier(tableName);
                var createTableQuery = $"CREATE TABLE {escapedTableName} ({columns})";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private string EscapeSqlIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return identifier;

            return $"[{identifier}]";
        }
    }
}