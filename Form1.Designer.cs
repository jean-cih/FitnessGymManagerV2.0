namespace GymApplicationV2._0
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.jeanTextBoxNumberCard = new GymApplicationV2._0.JeanTextBox();
            this.dataGridViewClient = new System.Windows.Forms.DataGridView();
            this.jeanFormStyle = new GymApplicationV2._0.Components.JeanFormStyle(this.components);
            this.jeanModernButtonSell = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonChooseClient = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonReturn = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonSingleTicket = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonNewMember = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonReport = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonClients = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonPurchase = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonServices = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonSettings = new GymApplicationV2._0.Controls.JeanModernButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClient)).BeginInit();
            this.SuspendLayout();
            // 
            // jeanTextBoxNumberCard
            // 
            this.jeanTextBoxNumberCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxNumberCard.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxNumberCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxNumberCard.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxNumberCard.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxNumberCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxNumberCard.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxNumberCard.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxNumberCard.Location = new System.Drawing.Point(514, 155);
            this.jeanTextBoxNumberCard.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxNumberCard.Name = "jeanTextBoxNumberCard";
            this.jeanTextBoxNumberCard.SelectionStart = 0;
            this.jeanTextBoxNumberCard.Size = new System.Drawing.Size(253, 37);
            this.jeanTextBoxNumberCard.TabIndex = 39;
            this.jeanTextBoxNumberCard.TextInput = "";
            this.jeanTextBoxNumberCard.TextPreview = "Номер Клиента / ФИО";
            this.jeanTextBoxNumberCard.UseSystemPasswordChar = false;
            this.jeanTextBoxNumberCard.TextChanged += new System.EventHandler(this.textNumberClient_TextChanged);
            this.jeanTextBoxNumberCard.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.jeanTextBoxNumberCard_KeyPress);
            // 
            // dataGridViewClient
            // 
            this.dataGridViewClient.AllowUserToAddRows = false;
            this.dataGridViewClient.AllowUserToDeleteRows = false;
            this.dataGridViewClient.AllowUserToResizeRows = false;
            this.dataGridViewClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewClient.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewClient.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewClient.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewClient.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClient.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewClient.ColumnHeadersHeight = 25;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewClient.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewClient.EnableHeadersVisualStyles = false;
            this.dataGridViewClient.GridColor = System.Drawing.Color.Black;
            this.dataGridViewClient.Location = new System.Drawing.Point(310, 322);
            this.dataGridViewClient.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewClient.MultiSelect = false;
            this.dataGridViewClient.Name = "dataGridViewClient";
            this.dataGridViewClient.ReadOnly = true;
            this.dataGridViewClient.RowHeadersVisible = false;
            this.dataGridViewClient.RowHeadersWidth = 40;
            this.dataGridViewClient.RowTemplate.Height = 24;
            this.dataGridViewClient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewClient.Size = new System.Drawing.Size(615, 48);
            this.dataGridViewClient.TabIndex = 10;
            // 
            // jeanFormStyle
            // 
            this.jeanFormStyle.AllowUserResize = false;
            this.jeanFormStyle.BackColor = System.Drawing.Color.White;
            this.jeanFormStyle.ContextMenuForm = null;
            this.jeanFormStyle.ControlBoxButtonsWidth = 20;
            this.jeanFormStyle.EnableControlBoxIconsLight = false;
            this.jeanFormStyle.EnableControlBoxMouseLight = false;
            this.jeanFormStyle.Form = this;
            this.jeanFormStyle.FormStyle = GymApplicationV2._0.Components.JeanFormStyle.fStyle.SimpleDark;
            this.jeanFormStyle.HeaderColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanFormStyle.HeaderColorAdditional = System.Drawing.Color.DarkOrange;
            this.jeanFormStyle.HeaderColorGradientEnable = true;
            this.jeanFormStyle.HeaderColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.jeanFormStyle.HeaderHeight = 30;
            this.jeanFormStyle.HeaderImage = null;
            this.jeanFormStyle.HeaderTextColor = System.Drawing.Color.White;
            this.jeanFormStyle.HeaderTextFont = new System.Drawing.Font("Segoe UI", 9.75F);
            // 
            // jeanModernButtonSell
            // 
            this.jeanModernButtonSell.BackColor = System.Drawing.Color.White;
            this.jeanModernButtonSell.BackgroundColor = System.Drawing.Color.White;
            this.jeanModernButtonSell.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonSell.BorderRadius = 20;
            this.jeanModernButtonSell.BorderSize = 0;
            this.jeanModernButtonSell.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonSell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonSell.ForeColor = System.Drawing.Color.Black;
            this.jeanModernButtonSell.Location = new System.Drawing.Point(38, 292);
            this.jeanModernButtonSell.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonSell.Name = "jeanModernButtonSell";
            this.jeanModernButtonSell.Size = new System.Drawing.Size(218, 48);
            this.jeanModernButtonSell.TabIndex = 21;
            this.jeanModernButtonSell.Text = "Продать";
            this.jeanModernButtonSell.TextColor = System.Drawing.Color.Black;
            this.jeanModernButtonSell.UseVisualStyleBackColor = false;
            this.jeanModernButtonSell.Click += new System.EventHandler(this.jeanModernButton1_Click);
            // 
            // jeanModernButtonChooseClient
            // 
            this.jeanModernButtonChooseClient.BackColor = System.Drawing.Color.White;
            this.jeanModernButtonChooseClient.BackgroundColor = System.Drawing.Color.White;
            this.jeanModernButtonChooseClient.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonChooseClient.BorderRadius = 20;
            this.jeanModernButtonChooseClient.BorderSize = 0;
            this.jeanModernButtonChooseClient.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonChooseClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonChooseClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonChooseClient.ForeColor = System.Drawing.Color.Black;
            this.jeanModernButtonChooseClient.Location = new System.Drawing.Point(38, 253);
            this.jeanModernButtonChooseClient.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonChooseClient.Name = "jeanModernButtonChooseClient";
            this.jeanModernButtonChooseClient.Size = new System.Drawing.Size(218, 37);
            this.jeanModernButtonChooseClient.TabIndex = 20;
            this.jeanModernButtonChooseClient.Text = "Выбрать клиента";
            this.jeanModernButtonChooseClient.TextColor = System.Drawing.Color.Black;
            this.jeanModernButtonChooseClient.UseVisualStyleBackColor = false;
            this.jeanModernButtonChooseClient.Click += new System.EventHandler(this.jeanModernButtonChooseClient_Click);
            // 
            // jeanModernButtonReturn
            // 
            this.jeanModernButtonReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanModernButtonReturn.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonReturn.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonReturn.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonReturn.BorderRadius = 20;
            this.jeanModernButtonReturn.BorderSize = 0;
            this.jeanModernButtonReturn.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonReturn.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonReturn.Location = new System.Drawing.Point(602, 209);
            this.jeanModernButtonReturn.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonReturn.Name = "jeanModernButtonReturn";
            this.jeanModernButtonReturn.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonReturn.TabIndex = 24;
            this.jeanModernButtonReturn.Text = "Возврат";
            this.jeanModernButtonReturn.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonReturn.UseVisualStyleBackColor = false;
            this.jeanModernButtonReturn.Visible = false;
            this.jeanModernButtonReturn.Click += new System.EventHandler(this.jeanModernButtonReturn_Click);
            // 
            // jeanModernButtonSingleTicket
            // 
            this.jeanModernButtonSingleTicket.BackColor = System.Drawing.Color.White;
            this.jeanModernButtonSingleTicket.BackgroundColor = System.Drawing.Color.White;
            this.jeanModernButtonSingleTicket.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonSingleTicket.BorderRadius = 20;
            this.jeanModernButtonSingleTicket.BorderSize = 0;
            this.jeanModernButtonSingleTicket.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonSingleTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonSingleTicket.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonSingleTicket.ForeColor = System.Drawing.Color.Black;
            this.jeanModernButtonSingleTicket.Location = new System.Drawing.Point(150, 183);
            this.jeanModernButtonSingleTicket.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonSingleTicket.Name = "jeanModernButtonSingleTicket";
            this.jeanModernButtonSingleTicket.Size = new System.Drawing.Size(106, 37);
            this.jeanModernButtonSingleTicket.TabIndex = 19;
            this.jeanModernButtonSingleTicket.Text = "Разовый";
            this.jeanModernButtonSingleTicket.TextColor = System.Drawing.Color.Black;
            this.jeanModernButtonSingleTicket.UseVisualStyleBackColor = false;
            this.jeanModernButtonSingleTicket.Click += new System.EventHandler(this.jeanModernButtonSingleTicket_Click);
            // 
            // jeanModernButtonNewMember
            // 
            this.jeanModernButtonNewMember.BackColor = System.Drawing.Color.White;
            this.jeanModernButtonNewMember.BackgroundColor = System.Drawing.Color.White;
            this.jeanModernButtonNewMember.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.jeanModernButtonNewMember.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonNewMember.BorderRadius = 20;
            this.jeanModernButtonNewMember.BorderSize = 0;
            this.jeanModernButtonNewMember.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonNewMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonNewMember.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonNewMember.ForeColor = System.Drawing.Color.Black;
            this.jeanModernButtonNewMember.Location = new System.Drawing.Point(38, 183);
            this.jeanModernButtonNewMember.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonNewMember.Name = "jeanModernButtonNewMember";
            this.jeanModernButtonNewMember.Size = new System.Drawing.Size(106, 37);
            this.jeanModernButtonNewMember.TabIndex = 18;
            this.jeanModernButtonNewMember.Text = "Новый";
            this.jeanModernButtonNewMember.TextColor = System.Drawing.Color.Black;
            this.jeanModernButtonNewMember.UseVisualStyleBackColor = false;
            this.jeanModernButtonNewMember.Click += new System.EventHandler(this.jeanModernButtonNewMember_Click);
            // 
            // jeanModernButtonReport
            // 
            this.jeanModernButtonReport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.jeanModernButtonReport.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonReport.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonReport.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonReport.BorderRadius = 20;
            this.jeanModernButtonReport.BorderSize = 2;
            this.jeanModernButtonReport.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonReport.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonReport.Location = new System.Drawing.Point(688, 10);
            this.jeanModernButtonReport.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonReport.Name = "jeanModernButtonReport";
            this.jeanModernButtonReport.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonReport.TabIndex = 22;
            this.jeanModernButtonReport.Text = "Отчет";
            this.jeanModernButtonReport.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonReport.UseVisualStyleBackColor = false;
            this.jeanModernButtonReport.Click += new System.EventHandler(this.jeanModernButtonReport_Click);
            // 
            // jeanModernButtonClients
            // 
            this.jeanModernButtonClients.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.jeanModernButtonClients.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonClients.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonClients.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonClients.BorderRadius = 20;
            this.jeanModernButtonClients.BorderSize = 2;
            this.jeanModernButtonClients.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonClients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonClients.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonClients.Location = new System.Drawing.Point(571, 10);
            this.jeanModernButtonClients.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonClients.Name = "jeanModernButtonClients";
            this.jeanModernButtonClients.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonClients.TabIndex = 21;
            this.jeanModernButtonClients.Text = "Клиенты";
            this.jeanModernButtonClients.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonClients.UseVisualStyleBackColor = false;
            this.jeanModernButtonClients.Click += new System.EventHandler(this.jeanModernButtonClients_Click);
            // 
            // jeanModernButtonPurchase
            // 
            this.jeanModernButtonPurchase.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.jeanModernButtonPurchase.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonPurchase.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonPurchase.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonPurchase.BorderRadius = 20;
            this.jeanModernButtonPurchase.BorderSize = 2;
            this.jeanModernButtonPurchase.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonPurchase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonPurchase.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonPurchase.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonPurchase.Location = new System.Drawing.Point(454, 10);
            this.jeanModernButtonPurchase.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonPurchase.Name = "jeanModernButtonPurchase";
            this.jeanModernButtonPurchase.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonPurchase.TabIndex = 20;
            this.jeanModernButtonPurchase.Text = "Товары";
            this.jeanModernButtonPurchase.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonPurchase.UseVisualStyleBackColor = false;
            this.jeanModernButtonPurchase.Click += new System.EventHandler(this.jeanModernButtonPurchase_Click);
            // 
            // jeanModernButtonServices
            // 
            this.jeanModernButtonServices.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.jeanModernButtonServices.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonServices.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonServices.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonServices.BorderRadius = 20;
            this.jeanModernButtonServices.BorderSize = 2;
            this.jeanModernButtonServices.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonServices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonServices.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonServices.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonServices.Location = new System.Drawing.Point(337, 10);
            this.jeanModernButtonServices.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonServices.Name = "jeanModernButtonServices";
            this.jeanModernButtonServices.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonServices.TabIndex = 19;
            this.jeanModernButtonServices.Text = "Услуги";
            this.jeanModernButtonServices.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonServices.UseVisualStyleBackColor = false;
            // 
            // jeanModernButtonSettings
            // 
            this.jeanModernButtonSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.jeanModernButtonSettings.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonSettings.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonSettings.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonSettings.BorderRadius = 20;
            this.jeanModernButtonSettings.BorderSize = 2;
            this.jeanModernButtonSettings.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonSettings.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonSettings.Location = new System.Drawing.Point(220, 10);
            this.jeanModernButtonSettings.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonSettings.Name = "jeanModernButtonSettings";
            this.jeanModernButtonSettings.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonSettings.TabIndex = 18;
            this.jeanModernButtonSettings.Text = "Настройки";
            this.jeanModernButtonSettings.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonSettings.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(960, 531);
            this.Controls.Add(this.jeanTextBoxNumberCard);
            this.Controls.Add(this.jeanModernButtonSell);
            this.Controls.Add(this.dataGridViewClient);
            this.Controls.Add(this.jeanModernButtonChooseClient);
            this.Controls.Add(this.jeanModernButtonReturn);
            this.Controls.Add(this.jeanModernButtonSingleTicket);
            this.Controls.Add(this.jeanModernButtonNewMember);
            this.Controls.Add(this.jeanModernButtonReport);
            this.Controls.Add(this.jeanModernButtonClients);
            this.Controls.Add(this.jeanModernButtonPurchase);
            this.Controls.Add(this.jeanModernButtonServices);
            this.Controls.Add(this.jeanModernButtonSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private JeanTextBox jeanTextBoxNumberCard;
        private Controls.JeanModernButton jeanModernButtonNewMember;
        private Controls.JeanModernButton jeanModernButtonSingleTicket;
        private Controls.JeanModernButton jeanModernButtonChooseClient;
        private Controls.JeanModernButton jeanModernButtonSell;
        private Controls.JeanModernButton jeanModernButtonClients;
        private Controls.JeanModernButton jeanModernButtonPurchase;
        private Controls.JeanModernButton jeanModernButtonServices;
        private Controls.JeanModernButton jeanModernButtonSettings;
        private Components.JeanFormStyle jeanFormStyle;
        private System.Windows.Forms.DataGridView dataGridViewClient;
        private Controls.JeanModernButton jeanModernButtonReturn;
        public Controls.JeanModernButton jeanModernButtonReport;
    }
}

