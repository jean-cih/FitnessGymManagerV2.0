using GymApplicationV2._0.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static GymApplicationV2._0.Products;

namespace GymApplicationV2._0
{
    public partial class EditProductsForm : Form
    {
        private Dictionary<string, Products.Product> _originalProducts;
        private Dictionary<string, Products.Product> _editedProducts;

        // Элементы управления
        private JeanPanel _mainPanel;
        private Label _titleLabel;
        private FlowLayoutPanel _productsPanel;
        private JeanModernButton _saveButton;
        private JeanModernButton _cancelButton;
        private JeanModernButton _addProductButton;

        private Timer _fadeTimer;
        private float _opacity = 0;
        private bool _isDragging = false;
        private Point _lastCursor;
        private Point _lastForm;
        int count;

        public EditProductsForm(Dictionary<string, Products.Product> products)
        {
            _originalProducts = products;
            _editedProducts = products.ToDictionary(
                kvp => kvp.Key,
                kvp => new Products.Product(kvp.Value.Name, kvp.Value.Price) { Count = kvp.Value.Count }
            );

            InitializeComponent();
            InitializeDesign();

            count = _originalProducts.Count + 1;

            SetupAnimation();
        }

        private void InitializeDesign()
        {
            //this.SuspendLayout();

            // Настройка формы
            this.Text = "Редактирование продуктов";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.DoubleBuffered = true;
            this.Opacity = 0;

            // Главная панель с тенью
            _mainPanel = new JeanPanel
            {
                Size = new Size(900, 660),
                Location = new Point(0, 0),
                BackColor = Color.White,
                GradientBottomColor = Color.White,
                GradientTapColor = Color.White,
                BorderStyle = BorderStyle.None,
                BorderRadius = 0,
            };

            // Заголовок с панелью перетаскивания
            var titlePanel = new Panel
            {
                Size = new Size(900, 50),
                BackColor = Color.MediumSlateBlue,
                Location = new Point(0, 0)
            };

            titlePanel.MouseDown += (s, e) => { _isDragging = true; _lastCursor = Cursor.Position; _lastForm = this.Location; };
            titlePanel.MouseMove += (s, e) => { if (_isDragging) { var diff = Point.Subtract(Cursor.Position, new Size(_lastCursor)); this.Location = Point.Add(_lastForm, new Size(diff)); } };
            titlePanel.MouseUp += (s, e) => { if (e.Button == MouseButtons.Left) _isDragging = false; };

            _titleLabel = new Label
            {
                Text = "✏️ Редактирование товаров",
                Font = new Font("Montserrat", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 220, 255),
                BackColor = Color.Transparent,
                Location = new Point(280, 10),
                AutoSize = true
            };

            // Кнопка закрытия
            var closeButton = new JeanModernButton
            {
                Text = "X",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 70, 70),
                Size = new Size(30, 28),
                Cursor = Cursors.Hand,
                BorderRadius = 0,
                BorderSize = 0,
                Location = new Point(860, 10)
            };
            closeButton.Click += (s, e) => CloseWithAnimation();

            // Панель продуктов
            _productsPanel = new FlowLayoutPanel
            {
                Size = new Size(820, 500),
                Location = new Point(20, 70),
                AutoScroll = true,
                BackColor = Color.Transparent
            };

            // Кнопки действий
            _addProductButton = new JeanModernButton
            {
                Text = "➕ Добавить",
                Size = new Size(200, 40),
                Location = new Point(50, 590),
                BackgroundColor = Color.FromArgb(100, 200, 100),
                TextColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BorderRadius = 10
            };
            _addProductButton.Click += AddProductButton_Click;

            _saveButton = new JeanModernButton
            {
                Text = "💾 Сохранить",
                Size = new Size(150, 40),
                Location = new Point(500, 590),
                BackgroundColor = Color.FromArgb(70, 130, 200),
                TextColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BorderRadius = 10
            };
            _saveButton.Click += SaveButton_Click;

            _cancelButton = new JeanModernButton
            {
                Text = "❌ Отмена",
                Size = new Size(150, 40),
                Location = new Point(670, 590),
                BackgroundColor = Color.FromArgb(200, 80, 80),
                TextColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BorderRadius = 10
            };
            _cancelButton.Click += CancelButton_Click;

            // Добавление элементов
            titlePanel.Controls.Add(_titleLabel);
            titlePanel.Controls.Add(closeButton);
            _mainPanel.Controls.Add(titlePanel);
            _mainPanel.Controls.Add(_productsPanel);
            _mainPanel.Controls.Add(_addProductButton);
            _mainPanel.Controls.Add(_saveButton);
            _mainPanel.Controls.Add(_cancelButton);
            this.Controls.Add(_mainPanel);

            // Загрузка продуктов
            LoadProducts();

            this.ResumeLayout();
        }

        private void LoadProducts()
        {
            _productsPanel.Controls.Clear();

            int index = 0;
            foreach (var product in _editedProducts.Values)
            {
                var productCard = CreateProductEditCard(product, index);
                _productsPanel.Controls.Add(productCard);
                index++;
            }
        }

        private JeanPanel CreateProductEditCard(Products.Product product, int index)
        {
            var card = new JeanPanel
            {
                Size = new Size(780, 80),
                BackColor = Color.White,
                GradientBottomColor = Color.White,
                GradientTapColor = Color.FromArgb(245, 245, 255),
                BorderRadius = 10,
                Margin = new Padding(10),
                Tag = product
            };

            // Эмодзи
            var emojiLabel = new Label
            {
                Text = product.Emoji,
                Font = new Font("Segoe UI", 16),
                ForeColor = Color.FromArgb(255, 140, 0),
                BackColor = Color.Transparent,
                Location = new Point(15, 25),
                Size = new Size(40, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Название продукта
            var nameLabel = new Label
            {
                Text = "Название:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Location = new Point(70, 15),
                Size = new Size(70, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var nameTextBox = new TextBox
            {
                Text = product.Name,
                Font = new Font("Segoe UI", 10),
                Location = new Point(70, 35),
                Size = new Size(150, 25),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = product
            };
            nameTextBox.TextChanged += (s, e) =>
            {
                product.Name = nameTextBox.Text;
                UpdateProductCard(card, product);
            };

            // Цена продукта
            var priceLabel = new Label
            {
                Text = "Цена (₽):",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Location = new Point(240, 15),
                Size = new Size(60, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var priceNumeric = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 10000,
                Value = Math.Min(Math.Max((decimal)product.Price, 0), 10000),
                DecimalPlaces = 0,
                Increment = 10,
                Location = new Point(240, 35),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10),
                Tag = product
            };
            priceNumeric.ValueChanged += (s, e) => product.Price = priceNumeric.Value;

            // Кнопка удаления
            var deleteButton = new JeanModernButton
            {
                Text = "🗑️",
                Size = new Size(40, 30),
                Location = new Point(720, 25),
                BackgroundColor = Color.FromArgb(220, 80, 80),
                TextColor = Color.White,
                Font = new Font("Segoe UI", 10),
                BorderRadius = 5,
                Tag = product
            };
            deleteButton.Click += DeleteButton_Click;

            // Разделитель
            var separator = new Label
            {
                BackColor = Color.FromArgb(240, 240, 240),
                Location = new Point(10, 75),
                Size = new Size(760, 1)
            };

            card.Controls.Add(emojiLabel);
            card.Controls.Add(nameLabel);
            card.Controls.Add(nameTextBox);
            card.Controls.Add(priceLabel);
            card.Controls.Add(priceNumeric);
            card.Controls.Add(deleteButton);
            card.Controls.Add(separator);

            return card;
        }

        private void UpdateProductCard(JeanPanel card, Products.Product product)
        {
            // Обновляем эмодзи в карточке
            foreach (Control control in card.Controls)
            {
                if (control is Label label && label.Text.Length == 2 && char.IsSurrogate(label.Text[0]))
                {
                    label.Text = product.Emoji;
                    break;
                }
            }
        }

        private void AddProductButton_Click(object sender, EventArgs e)
        {
            var newProduct = new Products.Product($"Новый {count}", 100);
            _editedProducts.Add(newProduct.Emoji + " " + newProduct.Name, newProduct);

            var newCard = CreateProductEditCard(newProduct, _editedProducts.Count - 1);
            _productsPanel.Controls.Add(newCard);

            ShowNotification("✅ Продукт добавлен!", Color.FromArgb(100, 200, 100));
            count++;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var button = sender as JeanModernButton;
            var product = button?.Tag as Products.Product;

            if (product != null && _editedProducts.Count > 1)
            {
                if (Message.MessageWindowYesNo($"Удалить продукт '{product.Name}'?") == DialogResult.Yes)
                {
                    var keyToRemove = _editedProducts.First(kvp => kvp.Value == product).Key;
                    _editedProducts.Remove(keyToRemove);
                    LoadProducts();
                    ShowNotification("🗑️ Продукт удален!", Color.FromArgb(200, 100, 100));
                }
            }
            else
            {
                ShowNotification("❌ Нельзя удалить последний продукт!", Color.FromArgb(255, 100, 100));
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Проверка на пустые названия
            foreach (var product in _editedProducts.Values)
            {
                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    ShowNotification("❌ Заполните названия всех продуктов!", Color.FromArgb(255, 100, 100));
                    return;
                }
            }

            // Обновляем оригинальные продукты
            _originalProducts.Clear();
            foreach (var kvp in _editedProducts)
            {
                _originalProducts[kvp.Key] = kvp.Value;
            }

            this.DialogResult = DialogResult.OK;
            CloseWithAnimation();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (HasChanges() && Message.MessageWindowYesNo("Отменить изменения?") != DialogResult.Yes)
            {
                return;
            }

            this.DialogResult = DialogResult.Cancel;
            CloseWithAnimation();
        }

        private bool HasChanges()
        {
            if (_originalProducts.Count != _editedProducts.Count)
                return true;

            foreach (var kvp in _originalProducts)
            {
                if (!_editedProducts.ContainsKey(kvp.Key) ||
                    _editedProducts[kvp.Key].Name != kvp.Value.Name ||
                    _editedProducts[kvp.Key].Price != kvp.Value.Price)
                {
                    return true;
                }
            }

            return false;
        }

        private void ShowNotification(string message, Color color)
        {
            var indicator = new Label
            {
                Text = message,
                ForeColor = color,
                BackColor = Color.Transparent,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(300, 55)
            };

            _mainPanel.Controls.Add(indicator);
            indicator.BringToFront();

            var timer = new Timer { Interval = 2000 };
            timer.Tick += (s, e) =>
            {
                _mainPanel.Controls.Remove(indicator);
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        private void SetupAnimation()
        {
            _fadeTimer = new Timer { Interval = 10 };
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

        private void CloseWithAnimation()
        {
            var closeTimer = new Timer { Interval = 10 };
            float closeOpacity = 1;
            closeTimer.Tick += (s, e) =>
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

        public Dictionary<string, Products.Product> GetUpdatedProducts()
        {
            return _editedProducts;
        }
    }
}