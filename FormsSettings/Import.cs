using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Connections;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Data;
using GymApplicationV2._0.Helpers;
using GymApplicationV2._0.Helpers.GymApplicationV2._0.Helpers;
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
        private JeanModernButton backupsButton;
        private Panel dropZonePanel;
        private System.Windows.Forms.Label dropZoneLabel;
        private Panel titlePanel;
        private FadeAnimation _fadeAnimation;

        private ToolStripDropDownMenu _menu_backups;

        private readonly string[] _notChangeableTexts = new[] { "📤 Импорт данных" };
        private readonly object _dbLock = new object();


        public Import()
        {
            try
            {
                InitializeComponent();
                InitializeComponents();

                _fadeAnimation = new FadeAnimation(this);
                _fadeAnimation.FadeIn();

                FontHelper.ApplyFontSettings(this, _notChangeableTexts);

                titlePanel.EnableDrag(this);

                Logger.Info("Форма Import инициализирована");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при инициализации Import", ex);
                throw;
            }
        }

        private void InitializeComponents()
        {
            try
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                this.Opacity = 0;
                this.Padding = new Padding(20, 1, 20, 20);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.DoubleBuffered = true;

                SetupFormPaint();
                CreateTitlePanel();
                CreateImportCard();
                SetupDragDropEvents();

                Logger.Info("Компоненты формы Import инициализированы");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeComponents", ex);
            }
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
            try
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

                btnClose.Click += (s, e) =>
                {
                    Logger.Info("Закрытие формы Import");
                    _fadeAnimation.CloseWithAnimation();
                };

                titlePanel.Controls.Add(titleLabel);
                titlePanel.Controls.Add(btnClose);
                this.Controls.Add(titlePanel);

                Logger.Info("Панель заголовка создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreateTitlePanel", ex);
            }
        }

        private void CreateImportCard()
        {
            try
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

                Logger.Info("Карточка импорта создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CreateImportCard", ex);
            }
        }

        private void SetupDragDropEvents()
        {
            this.AllowDrop = true;
            this.DragEnter += Import_DragEnter;
            this.DragDrop += Import_DragDrop;
        }

        private void InitializeDropZone(Panel parent)
        {
            try
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

                Logger.Info("Зона перетаскивания создана");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeDropZone", ex);
            }
        }

        private void InitializeActionButtons(Panel parent)
        {
            try
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

                backupsButton = new JeanModernButton
                {
                    Text = "🔄 Backups",
                    Size = new Size(170, 40),
                    Location = new System.Drawing.Point(625, 75),
                    BackColor = Color.FromArgb(37, 99, 235),
                    ForeColor = Color.White,
                    Font = new System.Drawing.Font("Segoe UI", DataConfig.sizeFontButtons > 12 ? 12 : DataConfig.sizeFontButtons, FontStyle.Bold),
                    BorderRadius = 8,
                };
                backupsButton.Click += BackupsButton_Click;

                buttonPanel.Controls.Add(chooseButton);
                buttonPanel.Controls.Add(importButton);
                buttonPanel.Controls.Add(backupsButton);
                parent.Controls.Add(buttonPanel);

                Logger.Info("Кнопки действий созданы");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в InitializeActionButtons", ex);
            }
        }

        private void BackupsButton_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Просмотр списка бэкапов");

                var backups = AutoBackup.GetBackups();

                if (backups == null || backups.Count == 0)
                {
                    MessageHelper.MessageWindowOk("📋 Резервные копии не найдены\n\nСоздайте первую копию через 'Создать бэкап'", "Информация");
                    return;
                }

                // Используем выделенную форму
                using (var backupForm = new BackupManagerForm())
                {
                    backupForm.UpdateData();

                    var dialogResult = backupForm.ShowDialog();

                    if (dialogResult == DialogResult.Yes)
                    {
                        var selectedBackup = backupForm.GetSelectedBackup();
                        if (selectedBackup != null)
                        {
                            RestoreFromBackup(selectedBackup.FolderPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при просмотре бэкапов", ex);
                MessageHelper.MessageWindowOk($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private async void CreateBackup_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Создание ручного бэкапа");

                ShowSavingIndicator("Создание резервной копии...");

                bool result = await AutoBackup.CreateBackupAsync();

                HideSavingIndicator();

                if (result)
                {
                    MessageHelper.ShowNotification(this, "✅ Резервная копия создана успешно!", 2000);
                    Logger.Info("Ручной бэкап создан успешно");

                    var backups = AutoBackup.GetBackups();
                    if (backups.Count > 0)
                    {
                        var latest = backups.First();
                        MessageHelper.MessageWindowOk(
                            $"✅ Бэкап создан!\n\n" +
                            $"📁 Папка: {latest.FolderName}\n" +
                            $"📅 Время: {latest.CreationDateFormatted}\n" +
                            $"📄 Файлов: {latest.FileCount}\n" +
                            $"💾 Размер: {latest.SizeFormatted}",
                            "Успех");
                    }
                }
                else
                {
                    MessageHelper.MessageWindowOk("❌ Ошибка при создании резервной копии", "Ошибка");
                    Logger.Warning("Ручной бэкап не создан");
                }
            }
            catch (Exception ex)
            {
                HideSavingIndicator();
                Logger.Error("Ошибка при создании ручного бэкапа", ex);
                MessageHelper.MessageWindowOk($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private async void RestoreFromBackup(string backupPath)
        {
            try
            {
                if (string.IsNullOrEmpty(backupPath) || !Directory.Exists(backupPath))
                {
                    MessageHelper.MessageWindowOk("❌ Папка с бэкапом не найдена", "Ошибка");
                    return;
                }

                // Получаем информацию о бэкапе
                var dirInfo = new DirectoryInfo(backupPath);
                var files = Directory.GetFiles(backupPath, "*.db");

                // Подтверждение восстановления
                string confirmMessage =
                    "⚠️ ВНИМАНИЕ! Восстановление из бэкапа ЗАМЕНИТ текущие данные!\n\n" +
                    $"📁 Бэкап: {dirInfo.Name}\n" +
                    $"📅 Создан: {dirInfo.CreationTime:dd.MM.yyyy HH:mm:ss}\n" +
                    $"📄 Файлов: {files.Length}\n\n" +
                    "📋 Будут восстановлены следующие файлы:\n" +
                    string.Join("\n", files.Select(f => $"   • {Path.GetFileName(f)}")) + "\n\n" +
                    "💡 Рекомендуется создать бэкап текущих данных перед восстановлением.\n\n" +
                    "Продолжить?";

                if (MessageHelper.MessageWindowYesNo(confirmMessage) != DialogResult.Yes)
                {
                    Logger.Info("Восстановление отменено пользователем");
                    return;
                }

                // Создаем бэкап текущих данных
                Logger.Info("Создание бэкапа текущих данных перед восстановлением");
                ShowSavingIndicator("Сохранение текущих данных...");
                await AutoBackup.CreateBackupAsync();
                HideSavingIndicator();

                // Выполняем восстановление
                Logger.Info($"Начало восстановления из бэкапа: {backupPath}");
                ShowSavingIndicator("Восстановление данных...");

                bool result = await Task.Run(() => AutoBackup.RestoreFromBackup(backupPath));

                HideSavingIndicator();

                if (result)
                {
                    MessageHelper.MessageWindowOk(
                        "✅ Данные успешно восстановлены из бэкапа!\n\n" +
                        $"📁 Бэкап: {dirInfo.Name}\n" +
                        $"📅 Восстановлено: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n\n" +
                        "⚠️ Для применения изменений требуется перезапуск приложения.",
                        "Успех");

                    Logger.Info($"Восстановление завершено из {backupPath}");

                    // Предлагаем перезапустить приложение
                    if (MessageHelper.MessageWindowYesNo("🔄 Перезапустить приложение сейчас?") == DialogResult.Yes)
                    {
                        Logger.Info("Перезапуск приложения после восстановления");
                        System.Windows.Forms.Application.Restart();
                    }
                }
                else
                {
                    MessageHelper.MessageWindowOk("❌ Ошибка при восстановлении данных из бэкапа", "Ошибка");
                    Logger.Warning($"Ошибка восстановления из {backupPath}");
                }
            }
            catch (Exception ex)
            {
                HideSavingIndicator();
                Logger.Error($"Ошибка при восстановлении из {backupPath}", ex);
                MessageHelper.MessageWindowOk($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private string FormatTotalSize(List<BackupInfo> backups)
        {
            long totalSize = backups.Sum(b => b.TotalSize);
            string[] sizes = { "Б", "КБ", "МБ", "ГБ" };
            double len = totalSize;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void ShowSavingIndicator(string message)
        {
            try
            {
                // Проверяем, нет ли уже индикатора
                var existing = this.Controls.OfType<System.Windows.Forms.Label>()
                    .FirstOrDefault(l => l.Tag?.ToString() == "savingIndicator");

                if (existing != null) return;

                var indicator = new System.Windows.Forms.Label
                {
                    Text = $"⏳ {message}",
                    ForeColor = System.Drawing.Color.White,
                    BackColor = System.Drawing.Color.FromArgb(52, 73, 94),
                    AutoSize = true,
                    Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold),
                    Location = new System.Drawing.Point(this.Width / 2 - 150, this.Height / 2 - 25),
                    Padding = new Padding(20, 10, 20, 10),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Tag = "savingIndicator"
                };

                // Делаем форму поверх всех
                this.Controls.Add(indicator);
                indicator.BringToFront();
                this.Refresh();
            }
            catch { }
        }

        private void HideSavingIndicator()
        {
            try
            {
                var indicators = this.Controls.OfType<System.Windows.Forms.Label>()
                    .Where(l => l.Tag?.ToString() == "savingIndicator")
                    .ToList();

                foreach (var indicator in indicators)
                {
                    this.Controls.Remove(indicator);
                    indicator.Dispose();
                }
                this.Refresh();
            }
            catch { }
        }

        private void ChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Выбор файла для импорта");
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm|All files|*.*";
                    openFileDialog.Title = "Выберите Excel файл для импорта";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _selectedFilePath = openFileDialog.FileName;
                        UpdateFileSelection();
                        Logger.Info($"Выбран файл: {_selectedFilePath}");
                    }
                    else
                    {
                        Logger.Info("Выбор файла отменен");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при выборе файла", ex);
                MessageHelper.MessageWindowOk($"Ошибка при выборе файла: {ex.Message}", "Ошибка");
            }
        }

        private void UpdateFileSelection()
        {
            try
            {
                if (!string.IsNullOrEmpty(_selectedFilePath))
                {
                    var fileName = Path.GetFileName(_selectedFilePath);
                    dropZoneLabel.Text = $"✅ {fileName}\n\n📊 Готов к импорту";
                    dropZoneLabel.ForeColor = Color.LightGreen;
                    importButton.Enabled = true;

                    AnimateDropZoneSuccess();
                    Logger.Info($"Файл выбран: {fileName}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в UpdateFileSelection", ex);
            }
        }

        private void AnimateDropZoneSuccess()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error("Ошибка в AnimateDropZoneSuccess", ex);
            }
        }

        private async void ImportButton_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("Нажата кнопка 'Импортировать'");

                if (string.IsNullOrEmpty(_selectedFilePath))
                {
                    Logger.Warning("Попытка импорта без выбранного файла");
                    MessageHelper.MessageWindowOk("⚠️ Файл не выбран\nПожалуйста, выберите файл для импорта", "Предупреждение");
                    return;
                }

                Logger.Info($"Начало импорта файла: {_selectedFilePath}");

                if (!await CheckDatabaseExistsAsync())
                {
                    Logger.Info("Импорт отменен из-за существующей базы данных");
                    return;
                }

                await ImportDataAsync();
                Logger.Info("Импорт завершен успешно");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ImportButton_Click: {_selectedFilePath}", ex);
                MessageHelper.MessageWindowOk($"Ошибка при импорте: {ex.Message}", "Ошибка");
            }
        }

        private async Task<bool> CheckDatabaseExistsAsync()
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(_selectedFilePath);
                var dbPath = Path.Combine("Databases", $"{fileName}.db");

                bool fileExists = await Task.Run(() => File.Exists(dbPath));

                if (fileExists)
                {
                    Logger.Info($"База данных уже существует: {dbPath}");
                    var result = MessageHelper.MessageWindowYesNo($"⚠️ База данных уже существует\nБаза данных '{fileName}.db' уже существует. Хотите удалить её и импортировать заново?");

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            await CloseAllDatabaseConnectionsAsync(fileName);
                            await Task.Delay(500);
                            await Task.Run(() => File.Delete(dbPath));
                            dropZoneLabel.Text = $"🗑️ Удалена существующая база: {fileName}.db\n\nЗагружается новая база";
                            Logger.Info($"База данных удалена: {dbPath}");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Ошибка удаления базы данных: {dbPath}", ex);
                            MessageHelper.MessageWindowOk($"❌ Ошибка удаления\nНе удалось удалить базу данных: {ex.Message}", "Ошибка");
                            return false;
                        }
                    }
                    else
                    {
                        dropZoneLabel.Text = "⏹️ Импорт отменен: база данных уже существует";
                        Logger.Info("Импорт отменен пользователем");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в CheckDatabaseExistsAsync", ex);
                return false;
            }
        }

        private async Task CloseAllDatabaseConnectionsAsync(string fileName)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Logger.Info($"Соединения с БД закрыты для {fileName}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при закрытии соединений для {fileName}", ex);
            }
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
                    Logger.Info("Импорт данных успешно завершен");
                    ResetForm();
                }
                catch (Exception ex)
                {
                    Logger.Error($"Ошибка при импорте данных: {_selectedFilePath}", ex);
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
            try
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

                    Logger.Info($"Импорт таблицы: {contextInfo.TableName}, строк: {dataTable.Rows.Count}");

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

                    Logger.Info($"Данные успешно импортированы в таблицу {contextInfo.TableName}, {dataTable.Rows.Count} строк");

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

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ImportExcelData для файла {_selectedFilePath}", ex);
                throw;
            }
        }

        private (string ConnectionString, string TableName) GetConnectionInfo(string fileName)
        {
            try
            {
                switch (fileName)
                {
                    case "Clients": return (ClientsContext.ConnectionStringClients(), "Contacts");
                    case "Services": return (ServicesContext.ConnectionStringServices(), "Descriptions");
                    case "Archive": return (ArchiveServicesContext.ConnectionStringArchive(), "Archive");
                    case "Payments": return (HistoryPaymentContext.ConnectionStringPayment(), "History");
                    case "IssuedMembership": return (IssuedMembershipContext.ConnectionStringIssued(), "Issued");
                    case "Products": return (ProductsContext.ConnectionStringProducts(), "Items");
                    default:
                        Logger.Warning($"Неподдерживаемый файл: {fileName}");
                        return (null, null);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в GetConnectionInfo для {fileName}", ex);
                return (null, null);
            }
        }

        private void Import_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                    dropZonePanel.BackColor = Color.FromArgb(80, 80, 85);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в Import_DragEnter", ex);
            }
        }

        private void Import_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files.Length > 0 && IsExcelFile(files[0]))
                    {
                        _selectedFilePath = files[0];
                        UpdateFileSelection();
                        Logger.Info($"Файл перетащен: {_selectedFilePath}");
                    }
                    else
                    {
                        Logger.Warning("Перетащен файл не Excel формата");
                        MessageHelper.MessageWindowOk("⚠️ Неверный формат\nПожалуйста, выберите Excel файл", "Ошибка");
                    }
                }
                dropZonePanel.BackColor = Color.FromArgb(65, 65, 68);
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в Import_DragDrop", ex);
            }
        }

        private bool IsExcelFile(string filePath)
        {
            try
            {
                var ext = Path.GetExtension(filePath).ToLower();
                return ext == ".xls" || ext == ".xlsx" || ext == ".xlsm";
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в IsExcelFile для {filePath}", ex);
                return false;
            }
        }

        private void ResetForm()
        {
            try
            {
                _selectedFilePath = string.Empty;
                dropZoneLabel.Text = "📎 Перетащите Excel файл сюда\nили нажмите для выбора";
                dropZoneLabel.ForeColor = Color.LightGray;
                importButton.Enabled = false;
                Logger.Info("Форма импорта сброшена");
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ResetForm", ex);
            }
        }

        private System.Data.DataTable ReadExcelData(Worksheet worksheet, IProgress<string> progress)
        {
            try
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

                Logger.Info($"Прочитано {dataTable.Rows.Count} строк из Excel");
                return dataTable;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в ReadExcelData", ex);
                throw;
            }
        }

        private void CreateDatabaseTable(string tableName, System.Data.DataTable dataTable, string connectionString)
        {
            try
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
                Logger.Info($"Таблица {tableName} создана");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при создании таблицы {tableName}", ex);
                throw;
            }
        }

        private void InsertDataIntoTable(string tableName, System.Data.DataTable dataTable, string connectionString, IProgress<string> progress)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    InsertDataIntoTable(connection, dataTable, tableName, progress);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при вставке данных в таблицу {tableName}", ex);
                throw;
            }
        }

        private void InsertDataIntoTable(SQLiteConnection connection, System.Data.DataTable dataTable, string tableName, IProgress<string> progress)
        {
            try
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
                Logger.Info($"Вставлено {dataTable.Rows.Count} строк в таблицу {tableName}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при вставке данных в таблицу {tableName}", ex);
                throw;
            }
        }

        private bool IsDateColumn(string columnName)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в IsDateColumn для {columnName}", ex);
                return false;
            }
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
                    try
                    {
                        dateValue = DateTime.FromOADate(doubleValue);
                        parsed = true;
                    }
                    catch
                    {
                        try
                        {
                            dateValue = DateTimeOffset.FromUnixTimeSeconds((long)doubleValue).DateTime;
                            parsed = true;
                        }
                        catch
                        {
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
                    str = str.Trim();

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
                else if (value is DateTimeOffset dto)
                {
                    dateValue = dto.DateTime;
                    parsed = true;
                }

                if (parsed && dateValue != DateTime.MinValue && dateValue.Year > 1900)
                {
                    return dateValue.ToString("yyyy-MM-dd");
                }

                return DBNull.Value;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в ParseDateValue для значения {value}", ex);
                return DBNull.Value;
            }
        }

        private string GenerateDynamicInsertQueryBasedOnTarget(string tableName, Dictionary<string, string> targetColumns, DataColumnCollection sourceColumns)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в GenerateDynamicInsertQueryBasedOnTarget для {tableName}", ex);
                throw;
            }
        }

        private Dictionary<string, string> GetTableColumns(SQLiteConnection connection, string tableName)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error($"Ошибка в GetTableColumns для {tableName}", ex);
                throw;
            }
        }

        private void CreateDynamicTable(string tableName, System.Data.DataTable dataTable, string connectionString)
        {
            try
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
                Logger.Info($"Динамическая таблица {tableName} создана");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при создании динамической таблицы {tableName}", ex);
                throw;
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