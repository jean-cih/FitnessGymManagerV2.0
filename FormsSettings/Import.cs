using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using Microsoft.Office.Interop.Excel;
using Shadow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection.Emit;
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

        private System.Windows.Forms.Timer _fadeTimer;
        private float _opacity = 0;

        Panel titlePanel;

        private FadeAnimation _fadeAnimation;

        public Import()
        {
            InitializeComponent();
            InitializeComponents();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            ApplySettings();

            titlePanel.EnableDrag(this);
        }

        private void ApplySettings()
        {
            string[] notChangeableTexts = new string[]
            {
                "📤 Импорт данных"
            };

            FontHelper.ApplyFontSettings(this, notChangeableTexts);
        }  

        private void InitializeComponents()
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
                Size = new Size(874, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new System.Drawing.Point(0, 0),
            };

            // Заголовок
            var titleLabel = new System.Windows.Forms.Label
            {
                Text = "📤 Импорт данных",
                Font = new System.Drawing.Font("Montserrat", 18, FontStyle.Bold),
                ForeColor = ForeColor = Color.FromArgb(220, 220, 255),
                BackColor = Color.Transparent,
                Location = new System.Drawing.Point(320, 10),
                AutoSize = true,
            };

            // Карточка импорта
            var importCard = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(10),
                Location = new System.Drawing.Point(20, 50),
                Size = new Size(834, 425),
            };

            importCard.Paint += (s, e) =>
            {
                // Рамка с свечением
                using (var pen = new Pen(Color.FromArgb(255, 140, 0), 5))
                {
                    e.Graphics.DrawRectangle(pen, new System.Drawing.Rectangle(0, 0, importCard.Width - 1, importCard.Height - 1));
                }
            };

            // Зона перетаскивания
            InitializeDropZone(importCard);

            // Кнопки действий
            InitializeActionButtons(importCard);

            this.Controls.Add(importCard);
            titlePanel.Controls.Add(titleLabel);

            var btnClose = new JeanModernButton
            {
                Font = new System.Drawing.Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 28),
                Cursor = Cursors.Hand
            };

            btnClose = CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new System.Drawing.Point(834, 10), new Size(30, 28));
            btnClose.Click += (s, e) => _fadeAnimation.CloseWithAnimation();

            titlePanel.Controls.Add(btnClose);

            this.Controls.Add(titlePanel);

            // Разрешаем перетаскивание файлов
            this.AllowDrop = true;
            this.DragEnter += Import_DragEnter;
            this.DragDrop += Import_DragDrop;
        }

        private JeanModernButton CreateStyledButton(string text, Color baseColor, int radius, int radiusSize, Color radiusColor, System.Drawing.Point location, Size size)
        {
            var button = new JeanModernButton
            {
                Text = text,
                Location = location,
                Size = size,
                Font = new System.Drawing.Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold),
                BackColor = baseColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BorderColor = radiusColor,
                BackgroundColor = baseColor,
                TextColor = Color.White,
                BorderRadius = radius,
                BorderSize = radiusSize,
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(baseColor, 0.2f);
            button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(baseColor, 0.2f);

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

            return button;
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
            var line = "📋 Поддерживаемые таблицы:\n    • Clients • Services • Archive \n    • Payments • IssuedMembership";
            tooltip.SetToolTip(dropZonePanel, line);
            tooltip.SetToolTip(dropZoneLabel, line);

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
            Documentation documentation = new Documentation();
            documentation.ShowDialog();
        }

        private void ChooseFile_Click(object sender, EventArgs e)
        {
            ChooseFile();
        }

        private void ChooseFile()
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
                ShowMessage("⚠️ Файл не выбран", "Пожалуйста, выберите файл для импорта",
                    MessageBoxIcon.Warning);
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
                var result = ShowMessageYesNo("⚠️ База данных уже существует",
                    $"База данных '{fileName}.db' уже существует. Хотите удалить её и импортировать заново?",
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Асинхронное удаление файла
                        await Task.Run(() => File.Delete(dbPath));
                        dropZoneLabel.Text = $"🗑️ Удалена существующая база: {fileName}.db\n\nЗагружается новая база";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("❌ Ошибка удаления",
                            $"Не удалось удалить базу данных: {ex.Message}",
                            MessageBoxIcon.Error);
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


        private DialogResult ShowMessageYesNo(string title, string message, MessageBoxIcon icon)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, icon);
        }

        private async Task ImportDataAsync()
        {
            using (var _splashScreen = new SplashScreen())
            {
                _splashScreen.Show();
                importButton.Enabled = false;
                chooseButton.Enabled = false;
                try
                {
                    var progress = new Progress<string>(message =>
                    {
                        _splashScreen.UpdateProgress(message);
                    });

                    await Task.Run(() => ImportExcelData(progress));

                    ShowMessage("✅ Импорт завершен", "Данные успешно импортированы в базу данных!",
                        MessageBoxIcon.Information);

                    ResetForm();
                }
                catch (Exception ex)
                {
                    ShowMessage("❌ Ошибка импорта", $"Произошла ошибка: {ex.Message}",
                        MessageBoxIcon.Error);
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
            Thread.Sleep(1000);

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Workbook workbook = null;

            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                progress.Report("📖 Открытие Excel файла...");
                Thread.Sleep(1000);

                workbook = excelApp.Workbooks.Open(_selectedFilePath);
                var worksheet = (Worksheet)workbook.Worksheets[1];

                progress.Report("📊 Чтение данных...");
                var dataTable = ReadExcelData(worksheet, progress);

                var fileName = Path.GetFileNameWithoutExtension(_selectedFilePath);
                var contextInfo = GetConnectionInfo(fileName);

                if (contextInfo.ConnectionString == null)
                {
                    throw new Exception("Неподдерживаемый формат файла. Ожидаются: Clients, Services, Archive, Payments, IssuedMembership");
                }

                progress.Report("💾 Создание базы данных...");
                Thread.Sleep(1000);
                using (var connection = new SQLiteConnection(contextInfo.ConnectionString))
                {
                    connection.Open();
                    CreateDatabaseTable(connection, dataTable, contextInfo.TableName);

                    progress.Report("📥 Импорт данных...");
                    Thread.Sleep(1000);
                    InsertDataIntoTable(connection, dataTable, contextInfo.TableName, progress);
                }

                progress.Report("✅ Завершение...");
                Thread.Sleep(1000);
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
                    ShowMessage("⚠️ Неверный формат", "Пожалуйста, выберите Excel файл",
                        MessageBoxIcon.Warning);
                }
            }
            dropZonePanel.BackColor = Color.FromArgb(65, 65, 68);
        }

        private bool IsExcelFile(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            return ext == ".xls" || ext == ".xlsx" || ext == ".xlsm";
        }

        private void ShowMessage(string title, string message, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        private void ResetForm()
        {
            _selectedFilePath = string.Empty;
            dropZoneLabel.Text = "📎 Перетащите Excel файл сюда\nили нажмите для выбора";
            dropZoneLabel.ForeColor = Color.LightGray;
            importButton.Enabled = false;
        }

        private GraphicsPath GetRoundRectPath(System.Drawing.Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
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

        private void CreateDatabaseTable(SQLiteConnection connection, System.Data.DataTable dataTable, string tableName)
        {
            switch (tableName)
            {
                case "Contacts":
                    CreateContactsTableWithFixedStructure(connection);
                    break;
                case "Archive":
                    CreateArchiveTableWithFixedStructure(connection);
                    break;
                case "History":
                    CreateHistoryTableWithFixedStructure(connection);
                    break;
                case "Issued":
                    CreateIssuedTableWithFixedStructure(connection);
                    break;
                case "Descriptions":
                    CreateDescriptionsTableWithFixedStructure(connection);
                    break;
                case "Items":
                    CreateProductsTableWithFixedStructure(connection);
                    break;
                default:
                    CreateDynamicTable(connection, dataTable, tableName);
                    break;
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

                                if ((targetColumn.Key == "Сохранено" || targetColumn.Key == "Дата оформления" || targetColumn.Key == "Дата окончания" || targetColumn.Key == "Посетил") && value != DBNull.Value && value != null)
                                {
                                    try
                                    {
                                        // Пытаемся преобразовать в DateTime
                                        DateTime dateValue;
                                        if (value is DateTime dt)
                                        {
                                            dateValue = dt;
                                        }
                                        else if (value is string str)
                                        {
                                            // Пробуем распарсить строку
                                            if (!DateTime.TryParse(str, out dateValue))
                                            {
                                                dateValue = DateTime.MinValue;
                                            }
                                        }
                                        else
                                        {
                                            dateValue = Convert.ToDateTime(value);
                                        }

                                        // Форматируем в нужный формат
                                        value = dateValue.ToString("dd.MM.yyyy HH:mm:ss");
                                    }
                                    catch
                                    {
                                        // Если не удалось преобразовать, оставляем как есть
                                        value = value ?? DBNull.Value;
                                    }
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

        private string GenerateDynamicInsertQueryBasedOnTarget(string tableName, Dictionary<string, string> targetColumns, DataColumnCollection sourceColumns)
        {
            string escapedTableName = EscapeSqlIdentifier(tableName);

            var columnsBuilder = new System.Text.StringBuilder();
            var parametersBuilder = new System.Text.StringBuilder();

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

        private void CreateDynamicTable(SQLiteConnection connection, System.Data.DataTable dataTable, string tableName)
        {
            var columns = new System.Text.StringBuilder();
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

        private string EscapeSqlIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return identifier;

            return $"[{identifier}]";
        }

        private string GenerateInsertQuery(System.Data.DataTable dataTable, string tableName)
        {
            string escapedTableName = EscapeSqlIdentifier(tableName);

            switch (tableName)
            {
                case "Contacts":
                    return GenerateFixedInsertQuery(escapedTableName, new[] {
                        "Id", "Фамилия", "Имя", "Пол", "Телефон", "№Карты",
                        "Покупки", "Посетил", "Абонемент", "Срок_абонемента", "Посещений_осталось",
                        "Отчество", "Email", "Дата_рождения", "Скидка", "Сохранено"
                    });

                case "Archive":
                    return GenerateFixedInsertQuery(escapedTableName, new[] {
                        "Id", "Клиент", "№Карты", "Дата_окончания",
                        "Абонемент", "Оплата", "Посещений_осталось"
                    });

                case "History":
                    return GenerateFixedInsertQuery(escapedTableName, new[] {
                        "Id", "Клиент", "Абонемент", "Дата_начала",
                        "Дата_окончания", "Цена", "Дата_платежа"
                    });

                case "Issued":
                    return GenerateFixedInsertQuery(escapedTableName, new[] {
                        "Id", "Клиент", "№Карты", "Дата_окончания", "Дата_оформления",
                        "Абонемент", "Оплата", "Статус", "Посещений_осталось", "Окончание_заморозки"
                    });

                case "Descriptions":
                    return GenerateFixedInsertQuery(escapedTableName, new[] {
                        "Id", "Наименование", "Цена", "Срок_действия",
                        "Количество", "Проданных_за_месяц"
                    });

                case "Items":
                    return GenerateFixedInsertQuery(escapedTableName, new[] {
                        "Id", "Товары", "Цена", "Количество"
                    });

                default:
                    return GenerateDynamicInsertQuery(escapedTableName, dataTable);
            }
        }

        private string GenerateDynamicInsertQuery(string tableName, System.Data.DataTable dataTable)
        {
            var columns = new System.Text.StringBuilder();
            var parameters = new System.Text.StringBuilder();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                string escapedColumnName = EscapeSqlIdentifier(dataTable.Columns[i].ColumnName);
                columns.Append(escapedColumnName);
                parameters.Append($"@p{i}");

                if (i < dataTable.Columns.Count - 1)
                {
                    columns.Append(", ");
                    parameters.Append(", ");
                }
            }

            return $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";
        }


        private string GenerateFixedInsertQuery(string tableName, string[] columns)
        {
            var columnsBuilder = new System.Text.StringBuilder();
            var parametersBuilder = new System.Text.StringBuilder();

            for (int i = 0; i < columns.Length; i++)
            {
                string escapedColumnName = EscapeSqlIdentifier(columns[i]);
                columnsBuilder.Append(escapedColumnName);
                parametersBuilder.Append($"@p{i}");

                if (i < columns.Length - 1)
                {
                    columnsBuilder.Append(", ");
                    parametersBuilder.Append(", ");
                }
            }

            return $"INSERT INTO {tableName} ({columnsBuilder}) VALUES ({parametersBuilder})";
        }

        private void CreateContactsTableWithFixedStructure(SQLiteConnection connection)
        {
            string createTableQuery = @"
                    CREATE TABLE Contacts(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Фамилия TEXT(20) NOT NULL,
                        Имя TEXT(20) NOT NULL,
                        Пол TEXT(20) DEFAULT NULL,
                        Телефон TEXT(20) DEFAULT NULL,
                        №Карты TEXT(20) DEFAULT NULL,
                        Покупки INTEGER DEFAULT NULL,
                        Отчество TEXT(20) DEFAULT NULL,
                        Email TEXT(100) DEFAULT NULL,
                        Дата_рождения TEXT(20) DEFAULT NULL,
                        Скидка INTEGER DEFAULT 0,
                        Сохранено TEXT(20) NOT NULL
                    )";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void CreateArchiveTableWithFixedStructure(SQLiteConnection connection)
        {
            string createTableQuery = @"
                    CREATE TABLE Archive(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Клиент TEXT(100),
                        №Карты TEXT(20),
                        Дата_окончания TEXT(20),
                        Абонемент TEXT(100),
                        Оплата INTEGER,
                        Посещений_осталось INTEGER DEFAULT NULL
                )";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void CreateHistoryTableWithFixedStructure(SQLiteConnection connection)
        {
            string createTableQuery = @"
                    CREATE TABLE History(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Клиент TEXT(100),
                        Абонемент TEXT(100),
                        Дата_начала TEXT(20),
                        Дата_окончаний TEXT(20),
                        Цена INTEGER,
                        Дата_платежа TEXT(20)
                )";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void CreateIssuedTableWithFixedStructure(SQLiteConnection connection)
        {
            string createTableQuery = @"
                    CREATE TABLE Issued(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Клиент TEXT(100),
                        №Карты TEXT(20),
                        Дата_окончания TEXT(20),
                        Дата_оформления TEXT(20),
                        Абонемент TEXT(100),
                        Посетил TEXT(20),
                        Оплата INTEGER,
                        Статус TEXT(20),
                        Посещений_осталось TEXT(5),
                        Окончание_заморозки TEXT(20)
                )";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void CreateDescriptionsTableWithFixedStructure(SQLiteConnection connection)
        {
            string createTableQuery = @"
                    CREATE TABLE Descriptions(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Абонемент TEXT(100),
                        Цена INTEGER,
                        Срок_действия TEXT(20),
                        Посещений TEXT(5),
                        Проданных_за_месяц INTEGER
                )";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void CreateProductsTableWithFixedStructure(SQLiteConnection connection)
        {
            string createTableQuery = @"
                CREATE TABLE Items(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Товары TEXT(100),
                    Цена TEXT(20),
                    Количество INTEGER,
                    Время_продажи TEXT(20)
            )";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        //private void FillContactsParameters(SQLiteCommand command, DataRow dataRow)
        //{
        //    string[] contactColumns = {
        //        "Id", "Фамилия", "Имя", "Пол", "Телефон", "№Карты",
        //        "Покупки", "Посетил", "Абонемент", "Срок_абонемента", "Посещений_осталось",
        //        "Отчество", "Email", "Дата_рождения", "Скидка", "Сохранено"
        //    };

        //    for (int j = 0; j < contactColumns.Length; j++)
        //    {
        //        object value = j < dataRow.Table.Columns.Count ? dataRow[j] : DBNull.Value;
        //        command.Parameters.AddWithValue($"@p{j}", value ?? DBNull.Value);
        //    }
        //}

        //private void FillArchiveParameters(SQLiteCommand command, DataRow dataRow)
        //{
        //    string[] archiveColumns = {
        //        "Id", "Клиент", "№Карты", "Дата_окончания",
        //        "Абонемент", "Оплата", "Посещений_осталось"
        //    };

        //    for (int j = 0; j < archiveColumns.Length; j++)
        //    {
        //        object value = j < dataRow.Table.Columns.Count ? dataRow[j] : DBNull.Value;
        //        command.Parameters.AddWithValue($"@p{j}", value ?? DBNull.Value);
        //    }
        //}

        //private void FillHistoryParameters(SQLiteCommand command, DataRow dataRow)
        //{
        //    string[] historyColumns = {
        //        "Id", "Клиент", "Абонемент", "Дата_начала",
        //        "Дата_окончания", "Цена", "Дата_платежа"
        //    };

        //    for (int j = 0; j < historyColumns.Length; j++)
        //    {
        //        object value = j < dataRow.Table.Columns.Count ? dataRow[j] : DBNull.Value;
        //        command.Parameters.AddWithValue($"@p{j}", value ?? DBNull.Value);
        //    }
        //}

        //private void FillIssuedParameters(SQLiteCommand command, DataRow dataRow)
        //{
        //    string[] issuedColumns = {
        //        "Id", "Клиент", "№Карты", "Дата_окончания", "Дата_оформления",
        //        "Абонемент", "Оплата", "Статус", "Посещений_осталось", "Окончание_заморозки"
        //    };

        //    for (int j = 0; j < issuedColumns.Length; j++)
        //    {
        //        object value = j < dataRow.Table.Columns.Count ? dataRow[j] : DBNull.Value;
        //        command.Parameters.AddWithValue($"@p{j}", value ?? DBNull.Value);
        //    }
        //}

        //private void FillDescriptionsParameters(SQLiteCommand command, DataRow dataRow)
        //{
        //    string[] descriptionsColumns = {
        //        "Id", "Наименование", "Цена", "Срок_действия",
        //        "Количество", "Проданных_за_месяц"
        //    };

        //    for (int j = 0; j < descriptionsColumns.Length; j++)
        //    {
        //        object value = j < dataRow.Table.Columns.Count ? dataRow[j] : DBNull.Value;
        //        command.Parameters.AddWithValue($"@p{j}", value ?? DBNull.Value);
        //    }
        //}
    }
}