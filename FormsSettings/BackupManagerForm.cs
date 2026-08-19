using GymApplicationV2._0.AnimationTools;
using GymApplicationV2._0.Controls;
using GymApplicationV2._0.Helpers;
using GymApplicationV2._0.Helpers.GymApplicationV2._0.Helpers;
using Shadow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace GymApplicationV2._0.FormsSettings
{
    public partial class BackupManagerForm : ShadowedForm
    {
        private List<BackupInfo> _backups;
        private ListBox listBox;
        private Label infoLabel;
        public Label titleLabel;
        private JeanModernButton btnRestore;
        private JeanModernButton btnCreate;
        private JeanModernButton btnDelete;

        private FadeAnimation _fadeAnimation;

        private Panel titlePanel;

        string[] notChangeableTexts = new string[]
            {
                "📋 Резервные копии"
            };

        public BackupManagerForm()
        {
            InitializeComponent();
            InitializeCustomDesign();

            _fadeAnimation = new FadeAnimation(this);
            _fadeAnimation.FadeIn();

            FontHelper.ApplyFontSettings(this, notChangeableTexts);

            titlePanel.EnableDrag(this);

            LoadBackups();
        }

        private void InitializeCustomDesign()
        {
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0;

            // Заголовок
            titleLabel = new Label
            {
                Text = "📋 Резервные копии",
                Font = new Font("Montserrat", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 220, 255),
                BackColor = Color.Transparent,
                AutoSize = true,
            };

            titlePanel = new Panel
            {
                Size = new Size(this.Width, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0),
            };
            titlePanel.Controls.Add(titleLabel);

            // Информация о количестве
            infoLabel = new Label
            {
                ForeColor = Color.Gray,
                Location = new Point(20, 65),
                AutoSize = true
            };

            // Список бэкапов
            listBox = new ListBox
            {
                Location = new Point(20, 100),
                Size = new Size(535, 300),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                BackColor = Color.FromArgb(248, 248, 252),
                BorderStyle = BorderStyle.FixedSingle
            };
            listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;

            // Кнопки
            btnRestore = UIStyler.CreateStyledButton("🔄 Восстановить",Color.FromArgb(40, 167, 69),8, 0, Color.White, new Point(40, 405), new Size(160, 40));
            btnCreate = UIStyler.CreateStyledButton("💾 Создать", Color.FromArgb(0, 123, 255), 8, 0, Color.White, new Point(215, 405), new Size(150, 40));
            btnDelete = UIStyler.CreateStyledButton("🗑️ Удалить", Color.FromArgb(220, 53, 69), 8, 0, Color.White, new Point(380, 405), new Size(150, 40));

            var btnClose = UIStyler.CreateStyledButton("X", Color.FromArgb(180, 70, 70), 0, 0, Color.FromArgb(255, 140, 0), new Point(this.Width - 40, 10), new Size(30, 28));
            btnClose.Click += (s, e) =>
            {
                Logger.Info("Закрытие формы BackupManagerForm");
                _fadeAnimation.CloseWithAnimation();
            };
            titlePanel.Controls.Add(btnClose);

            // Добавляем элементы
            this.Controls.Add(titlePanel);
            this.Controls.Add(infoLabel);
            this.Controls.Add(listBox);
            this.Controls.Add(btnRestore);
            this.Controls.Add(btnCreate);
            this.Controls.Add(btnDelete);

            // Подписываемся на события
            btnRestore.Click += BtnRestore_Click;
            btnCreate.Click += BtnCreate_Click;
            btnDelete.Click += BtnDelete_Click;
        }

        public void UpdateData()
        {
            try
            {
                titleLabel.Location = new Point((this.Width - titleLabel.Width) / 2, (titlePanel.Height - titleLabel.Height) / 2);
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка в UpdateData", ex);
            }
        }

        private void LoadBackups()
        {
            try
            {
                _backups = AutoBackup.GetBackups();

                if (_backups == null || _backups.Count == 0)
                {
                    infoLabel.Text = "📋 Резервные копии не найдены";
                    btnRestore.Enabled = false;
                    btnDelete.Enabled = false;
                    return;
                }

                infoLabel.Text = $"Всего: {_backups.Count} копий | Общий размер: {FormatTotalSize(_backups)}";

                listBox.Items.Clear();
                foreach (var backup in _backups)
                {
                    string displayText = $"{backup.FolderName}  |  {backup.CreationDateFormatted}  |  {backup.FileCount} файлов  |  {backup.SizeFormatted}";
                    listBox.Items.Add(displayText);
                }

                if (listBox.Items.Count > 0)
                {
                    listBox.SelectedIndex = 0;
                    btnRestore.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка загрузки списка бэкапов", ex);
                infoLabel.Text = "❌ Ошибка загрузки списка бэкапов";
                btnRestore.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = listBox.SelectedIndex >= 0 && listBox.SelectedIndex < _backups?.Count;
            btnRestore.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private async void BtnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                btnCreate.Enabled = false;
                btnCreate.Text = "⏳ Создание...";

                bool result = await AutoBackup.CreateBackupAsync();

                if (result)
                {
                    MessageHelper.ShowNotification(this, "✅ Бэкап создан успешно!", 1500);
                    Logger.Info("Бэкап создан из диалога просмотра");
                    LoadBackups();
                }
                else
                {
                    MessageHelper.MessageWindowOk("❌ Ошибка при создании бэкапа", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка создания бэкапа из диалога", ex);
                MessageHelper.MessageWindowOk($"Ошибка: {ex.Message}", "Ошибка");
            }
            finally
            {
                btnCreate.Enabled = true;
                btnCreate.Text = "💾 Создать";
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex < 0 || listBox.SelectedIndex >= _backups?.Count)
            {
                MessageHelper.MessageWindowOk("⚠️ Пожалуйста, выберите бэкап для удаления", "Предупреждение");
                return;
            }

            var selectedBackup = _backups[listBox.SelectedIndex];

            if (MessageHelper.MessageWindowYesNo(
                $"⚠️ Вы уверены, что хотите удалить бэкап?\n\n" +
                $"📁 {selectedBackup.FolderName}\n" +
                $"📅 {selectedBackup.CreationDateFormatted}\n" +
                $"📄 {selectedBackup.FileCount} файлов\n" +
                $"💾 {selectedBackup.SizeFormatted}") == DialogResult.Yes)
            {
                try
                {
                    if (Directory.Exists(selectedBackup.FolderPath))
                    {
                        Directory.Delete(selectedBackup.FolderPath, true);
                        Logger.Info($"Удален бэкап: {selectedBackup.FolderName}");
                        MessageHelper.ShowNotification(this, "🗑️ Бэкап удален", 1500);
                        LoadBackups();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Ошибка удаления бэкапа {selectedBackup.FolderName}", ex);
                    MessageHelper.MessageWindowOk($"Ошибка: {ex.Message}", "Ошибка");
                }
            }
        }

        private void BtnRestore_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex < 0 || listBox.SelectedIndex >= _backups?.Count)
            {
                MessageHelper.MessageWindowOk("⚠️ Пожалуйста, выберите бэкап для восстановления", "Предупреждение");
                return;
            }

            var selectedBackup = _backups[listBox.SelectedIndex];

            // Закрываем форму и передаем выбранный бэкап
            this.DialogResult = DialogResult.Yes;
            this.Tag = selectedBackup;
            _fadeAnimation.CloseWithAnimation();
        }

        private string FormatTotalSize(List<BackupInfo> backups)
        {
            if (backups == null || backups.Count == 0) return "0 Б";

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

        public BackupInfo GetSelectedBackup()
        {
            return this.Tag as BackupInfo;
        }
    }
}