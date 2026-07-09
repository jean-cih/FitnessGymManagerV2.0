using System.Windows.Forms;

namespace GymApplicationV2._0
{
    partial class Services
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Services));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelName = new System.Windows.Forms.Label();
            this.labelNumberCard = new System.Windows.Forms.Label();
            this.checkBoxVisited = new System.Windows.Forms.CheckBox();
            this.pictureBoxSearch = new System.Windows.Forms.PictureBox();
            this.labelMembership = new System.Windows.Forms.Label();
            this.dataGridViewServices = new System.Windows.Forms.DataGridView();
            this.jeanPanel = new GymApplicationV2._0.Controls.JeanPanel();
            this.jeanModernButtonErase = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanSoftTextBoxPurchase = new GymApplicationV2._0.Controls.jeanSoftTextBox();
            this.jeanModernButtonChange = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonSell = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonAdd = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonDelete = new GymApplicationV2._0.Controls.JeanModernButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServices)).BeginInit();
            this.jeanPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelName.Location = new System.Drawing.Point(28, 36);
            this.labelName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(0, 17);
            this.labelName.TabIndex = 20;
            this.labelName.Visible = false;
            // 
            // labelNumberCard
            // 
            this.labelNumberCard.AutoSize = true;
            this.labelNumberCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCard.Location = new System.Drawing.Point(172, 66);
            this.labelNumberCard.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNumberCard.Name = "labelNumberCard";
            this.labelNumberCard.Size = new System.Drawing.Size(0, 17);
            this.labelNumberCard.TabIndex = 22;
            this.labelNumberCard.Visible = false;
            // 
            // checkBoxVisited
            // 
            this.checkBoxVisited.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.checkBoxVisited.AutoSize = true;
            this.checkBoxVisited.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxVisited.Location = new System.Drawing.Point(11, 418);
            this.checkBoxVisited.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxVisited.Name = "checkBoxVisited";
            this.checkBoxVisited.Size = new System.Drawing.Size(167, 17);
            this.checkBoxVisited.TabIndex = 37;
            this.checkBoxVisited.Text = "Отметить посещение сразу";
            this.checkBoxVisited.UseVisualStyleBackColor = true;
            this.checkBoxVisited.Visible = false;
            // 
            // pictureBoxSearch
            // 
            this.pictureBoxSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxSearch.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxSearch.Image")));
            this.pictureBoxSearch.Location = new System.Drawing.Point(352, 46);
            this.pictureBoxSearch.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxSearch.Name = "pictureBoxSearch";
            this.pictureBoxSearch.Size = new System.Drawing.Size(26, 24);
            this.pictureBoxSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSearch.TabIndex = 57;
            this.pictureBoxSearch.TabStop = false;
            // 
            // labelMembership
            // 
            this.labelMembership.AutoSize = true;
            this.labelMembership.Location = new System.Drawing.Point(19, 71);
            this.labelMembership.Name = "labelMembership";
            this.labelMembership.Size = new System.Drawing.Size(0, 13);
            this.labelMembership.TabIndex = 58;
            // 
            // dataGridViewServices
            // 
            this.dataGridViewServices.AllowUserToAddRows = false;
            this.dataGridViewServices.AllowUserToDeleteRows = false;
            this.dataGridViewServices.AllowUserToResizeColumns = false;
            this.dataGridViewServices.AllowUserToResizeRows = false;
            this.dataGridViewServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewServices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewServices.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewServices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewServices.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewServices.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewServices.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewServices.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewServices.EnableHeadersVisualStyles = false;
            this.dataGridViewServices.GridColor = System.Drawing.Color.Black;
            this.dataGridViewServices.Location = new System.Drawing.Point(13, 11);
            this.dataGridViewServices.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewServices.Name = "dataGridViewServices";
            this.dataGridViewServices.ReadOnly = true;
            this.dataGridViewServices.RowHeadersVisible = false;
            this.dataGridViewServices.RowHeadersWidth = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dataGridViewServices.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewServices.RowTemplate.Height = 24;
            this.dataGridViewServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewServices.Size = new System.Drawing.Size(842, 306);
            this.dataGridViewServices.TabIndex = 58;
            this.dataGridViewServices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewServices_CellContentClick_1);
            // 
            // jeanPanel
            // 
            this.jeanPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            this.jeanPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.jeanPanel.BorderRadius = 30;
            this.jeanPanel.Controls.Add(this.dataGridViewServices);
            this.jeanPanel.ForeColor = System.Drawing.Color.Black;
            this.jeanPanel.GradientAngle = 90F;
            this.jeanPanel.GradientBottomColor = System.Drawing.Color.DodgerBlue;
            this.jeanPanel.GradientTapColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            this.jeanPanel.Location = new System.Drawing.Point(11, 89);
            this.jeanPanel.Margin = new System.Windows.Forms.Padding(2);
            this.jeanPanel.Name = "jeanPanel";
            this.jeanPanel.Size = new System.Drawing.Size(868, 325);
            this.jeanPanel.TabIndex = 54;
            // 
            // jeanModernButtonErase
            // 
            this.jeanModernButtonErase.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.jeanModernButtonErase.BackColor = System.Drawing.Color.White;
            this.jeanModernButtonErase.BackgroundColor = System.Drawing.Color.White;
            this.jeanModernButtonErase.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("jeanModernButtonErase.BackgroundImage")));
            this.jeanModernButtonErase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.jeanModernButtonErase.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.jeanModernButtonErase.BorderRadius = 24;
            this.jeanModernButtonErase.BorderSize = 0;
            this.jeanModernButtonErase.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonErase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonErase.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonErase.Location = new System.Drawing.Point(536, 46);
            this.jeanModernButtonErase.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonErase.Name = "jeanModernButtonErase";
            this.jeanModernButtonErase.Size = new System.Drawing.Size(26, 24);
            this.jeanModernButtonErase.TabIndex = 59;
            this.jeanModernButtonErase.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonErase.UseVisualStyleBackColor = false;
            this.jeanModernButtonErase.Visible = false;
            this.jeanModernButtonErase.Click += new System.EventHandler(this.jeanModernButtonErase_Click);
            // 
            // jeanSoftTextBoxPurchase
            // 
            this.jeanSoftTextBoxPurchase.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.jeanSoftTextBoxPurchase.BorderColor = System.Drawing.Color.Black;
            this.jeanSoftTextBoxPurchase.BorderFocusColor = System.Drawing.Color.HotPink;
            this.jeanSoftTextBoxPurchase.BorderRadius = 15;
            this.jeanSoftTextBoxPurchase.BorderSize = 2;
            this.jeanSoftTextBoxPurchase.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanSoftTextBoxPurchase.Location = new System.Drawing.Point(343, 41);
            this.jeanSoftTextBoxPurchase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.jeanSoftTextBoxPurchase.Multiline = false;
            this.jeanSoftTextBoxPurchase.Name = "jeanSoftTextBoxPurchase";
            this.jeanSoftTextBoxPurchase.Padding = new System.Windows.Forms.Padding(38, 8, 38, 8);
            this.jeanSoftTextBoxPurchase.PasswordChar = false;
            this.jeanSoftTextBoxPurchase.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.jeanSoftTextBoxPurchase.PlaceholderText = "  Услуга";
            this.jeanSoftTextBoxPurchase.Size = new System.Drawing.Size(226, 34);
            this.jeanSoftTextBoxPurchase.TabIndex = 55;
            this.jeanSoftTextBoxPurchase.Texts = "";
            this.jeanSoftTextBoxPurchase.UnderlinedStyle = false;
            this.jeanSoftTextBoxPurchase._TextChanged += new System.EventHandler(this.jeanSoftTextBoxPurchase__TextChanged);
            // 
            // jeanModernButtonChange
            // 
            this.jeanModernButtonChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanModernButtonChange.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonChange.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonChange.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonChange.BorderRadius = 20;
            this.jeanModernButtonChange.BorderSize = 2;
            this.jeanModernButtonChange.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonChange.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonChange.Location = new System.Drawing.Point(752, 29);
            this.jeanModernButtonChange.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonChange.Name = "jeanModernButtonChange";
            this.jeanModernButtonChange.Size = new System.Drawing.Size(112, 45);
            this.jeanModernButtonChange.TabIndex = 53;
            this.jeanModernButtonChange.Text = "Изменить";
            this.jeanModernButtonChange.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonChange.UseVisualStyleBackColor = false;
            this.jeanModernButtonChange.Click += new System.EventHandler(this.jeanModernButton1_Click);
            // 
            // jeanModernButtonSell
            // 
            this.jeanModernButtonSell.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.jeanModernButtonSell.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonSell.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonSell.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonSell.BorderRadius = 20;
            this.jeanModernButtonSell.BorderSize = 2;
            this.jeanModernButtonSell.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonSell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonSell.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonSell.Location = new System.Drawing.Point(403, 430);
            this.jeanModernButtonSell.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonSell.Name = "jeanModernButtonSell";
            this.jeanModernButtonSell.Size = new System.Drawing.Size(112, 45);
            this.jeanModernButtonSell.TabIndex = 51;
            this.jeanModernButtonSell.Text = "Продать";
            this.jeanModernButtonSell.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonSell.UseVisualStyleBackColor = false;
            this.jeanModernButtonSell.Visible = false;
            this.jeanModernButtonSell.Click += new System.EventHandler(this.buttonSell_Click);
            // 
            // jeanModernButtonAdd
            // 
            this.jeanModernButtonAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.jeanModernButtonAdd.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonAdd.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonAdd.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonAdd.BorderRadius = 20;
            this.jeanModernButtonAdd.BorderSize = 2;
            this.jeanModernButtonAdd.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonAdd.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonAdd.Location = new System.Drawing.Point(344, 430);
            this.jeanModernButtonAdd.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonAdd.Name = "jeanModernButtonAdd";
            this.jeanModernButtonAdd.Size = new System.Drawing.Size(112, 45);
            this.jeanModernButtonAdd.TabIndex = 50;
            this.jeanModernButtonAdd.Text = "Добавить";
            this.jeanModernButtonAdd.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonAdd.UseVisualStyleBackColor = false;
            this.jeanModernButtonAdd.Click += new System.EventHandler(this.buttonAddService_Click);
            // 
            // jeanModernButtonDelete
            // 
            this.jeanModernButtonDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.jeanModernButtonDelete.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonDelete.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonDelete.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonDelete.BorderRadius = 20;
            this.jeanModernButtonDelete.BorderSize = 2;
            this.jeanModernButtonDelete.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonDelete.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonDelete.Location = new System.Drawing.Point(460, 430);
            this.jeanModernButtonDelete.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonDelete.Name = "jeanModernButtonDelete";
            this.jeanModernButtonDelete.Size = new System.Drawing.Size(112, 45);
            this.jeanModernButtonDelete.TabIndex = 52;
            this.jeanModernButtonDelete.Text = "Удалить";
            this.jeanModernButtonDelete.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonDelete.UseVisualStyleBackColor = false;
            this.jeanModernButtonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // Services
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(886, 497);
            this.Controls.Add(this.jeanPanel);
            this.Controls.Add(this.jeanModernButtonErase);
            this.Controls.Add(this.labelMembership);
            this.Controls.Add(this.pictureBoxSearch);
            this.Controls.Add(this.jeanSoftTextBoxPurchase);
            this.Controls.Add(this.jeanModernButtonChange);
            this.Controls.Add(this.jeanModernButtonSell);
            this.Controls.Add(this.jeanModernButtonAdd);
            this.Controls.Add(this.checkBoxVisited);
            this.Controls.Add(this.labelNumberCard);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.jeanModernButtonDelete);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Services";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Абонементы";
            this.Load += new System.EventHandler(this.Services_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServices)).EndInit();
            this.jeanPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected internal System.Windows.Forms.Label labelName;
        protected internal System.Windows.Forms.Label labelNumberCard;
        protected internal System.Windows.Forms.CheckBox checkBoxVisited;
        protected internal Controls.JeanModernButton jeanModernButtonAdd;
        protected internal Controls.JeanModernButton jeanModernButtonSell;
        protected internal Controls.JeanModernButton jeanModernButtonDelete;
        protected internal Controls.JeanModernButton jeanModernButtonChange;
        private Controls.JeanPanel jeanPanel;
        private System.Windows.Forms.PictureBox pictureBoxSearch;
        protected internal Controls.jeanSoftTextBox jeanSoftTextBoxPurchase;
        protected internal System.Windows.Forms.DataGridView dataGridViewServices;
        private System.Windows.Forms.Label labelMembership;
        private Controls.JeanModernButton jeanModernButtonErase;
    }
}