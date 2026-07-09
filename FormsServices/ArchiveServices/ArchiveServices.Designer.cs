namespace GymApplicationV2._0
{
    partial class ArchiveServices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchiveServices));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBoxSearch = new System.Windows.Forms.PictureBox();
            this.nameClient = new System.Windows.Forms.Label();
            this.card = new System.Windows.Forms.Label();
            this.jeanPanel = new GymApplicationV2._0.Controls.JeanPanel();
            this.dataGridViewArchive = new System.Windows.Forms.DataGridView();
            this.jeanModernButtonChangeData = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanModernButtonErase = new GymApplicationV2._0.Controls.JeanModernButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.jeanSoftTextBoxSearch = new GymApplicationV2._0.Controls.jeanSoftTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).BeginInit();
            this.jeanPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewArchive)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxSearch
            // 
            this.pictureBoxSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxSearch.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxSearch.Image")));
            this.pictureBoxSearch.Location = new System.Drawing.Point(379, 37);
            this.pictureBoxSearch.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxSearch.Name = "pictureBoxSearch";
            this.pictureBoxSearch.Size = new System.Drawing.Size(26, 24);
            this.pictureBoxSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSearch.TabIndex = 52;
            this.pictureBoxSearch.TabStop = false;
            // 
            // nameClient
            // 
            this.nameClient.AutoSize = true;
            this.nameClient.Location = new System.Drawing.Point(90, 25);
            this.nameClient.Name = "nameClient";
            this.nameClient.Size = new System.Drawing.Size(10, 13);
            this.nameClient.TabIndex = 50;
            this.nameClient.Text = "-";
            // 
            // card
            // 
            this.card.AutoSize = true;
            this.card.Location = new System.Drawing.Point(131, 50);
            this.card.Name = "card";
            this.card.Size = new System.Drawing.Size(10, 13);
            this.card.TabIndex = 55;
            this.card.Text = "-";
            // 
            // jeanPanel
            // 
            this.jeanPanel.BackColor = System.Drawing.Color.White;
            this.jeanPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.jeanPanel.BorderRadius = 30;
            this.jeanPanel.Controls.Add(this.dataGridViewArchive);
            this.jeanPanel.ForeColor = System.Drawing.Color.Black;
            this.jeanPanel.GradientAngle = 90F;
            this.jeanPanel.GradientBottomColor = System.Drawing.Color.DodgerBlue;
            this.jeanPanel.GradientTapColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            this.jeanPanel.Location = new System.Drawing.Point(9, 73);
            this.jeanPanel.Margin = new System.Windows.Forms.Padding(2);
            this.jeanPanel.Name = "jeanPanel";
            this.jeanPanel.Size = new System.Drawing.Size(1005, 493);
            this.jeanPanel.TabIndex = 54;
            // 
            // dataGridViewArchive
            // 
            this.dataGridViewArchive.AllowUserToAddRows = false;
            this.dataGridViewArchive.AllowUserToDeleteRows = false;
            this.dataGridViewArchive.AllowUserToResizeRows = false;
            this.dataGridViewArchive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewArchive.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewArchive.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewArchive.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewArchive.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewArchive.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewArchive.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewArchive.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewArchive.EnableHeadersVisualStyles = false;
            this.dataGridViewArchive.GridColor = System.Drawing.Color.Black;
            this.dataGridViewArchive.Location = new System.Drawing.Point(15, 13);
            this.dataGridViewArchive.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewArchive.Name = "dataGridViewArchive";
            this.dataGridViewArchive.ReadOnly = true;
            this.dataGridViewArchive.RowHeadersVisible = false;
            this.dataGridViewArchive.RowHeadersWidth = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dataGridViewArchive.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewArchive.RowTemplate.Height = 24;
            this.dataGridViewArchive.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewArchive.Size = new System.Drawing.Size(976, 470);
            this.dataGridViewArchive.TabIndex = 49;
            this.dataGridViewArchive.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewArchive_CellContentClick);
            // 
            // jeanModernButtonChangeData
            // 
            this.jeanModernButtonChangeData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanModernButtonChangeData.BackColor = System.Drawing.Color.White;
            this.jeanModernButtonChangeData.BackgroundColor = System.Drawing.Color.White;
            this.jeanModernButtonChangeData.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonChangeData.BorderRadius = 20;
            this.jeanModernButtonChangeData.BorderSize = 2;
            this.jeanModernButtonChangeData.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonChangeData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonChangeData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonChangeData.ForeColor = System.Drawing.Color.Black;
            this.jeanModernButtonChangeData.Location = new System.Drawing.Point(892, 32);
            this.jeanModernButtonChangeData.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonChangeData.Name = "jeanModernButtonChangeData";
            this.jeanModernButtonChangeData.Size = new System.Drawing.Size(106, 37);
            this.jeanModernButtonChangeData.TabIndex = 53;
            this.jeanModernButtonChangeData.Text = "Изменить";
            this.jeanModernButtonChangeData.TextColor = System.Drawing.Color.Black;
            this.jeanModernButtonChangeData.UseVisualStyleBackColor = false;
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
            this.jeanModernButtonErase.Location = new System.Drawing.Point(635, 37);
            this.jeanModernButtonErase.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonErase.Name = "jeanModernButtonErase";
            this.jeanModernButtonErase.Size = new System.Drawing.Size(26, 24);
            this.jeanModernButtonErase.TabIndex = 51;
            this.jeanModernButtonErase.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonErase.UseVisualStyleBackColor = false;
            this.jeanModernButtonErase.Visible = false;
            this.jeanModernButtonErase.Click += new System.EventHandler(this.jeanModernButtonErase_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(21, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 56;
            this.label1.Text = "Клиент: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(21, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 16);
            this.label2.TabIndex = 57;
            this.label2.Text = "Номер карты: ";
            // 
            // jeanSoftTextBoxSearch
            // 
            this.jeanSoftTextBoxSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.jeanSoftTextBoxSearch.BorderColor = System.Drawing.Color.Black;
            this.jeanSoftTextBoxSearch.BorderFocusColor = System.Drawing.Color.HotPink;
            this.jeanSoftTextBoxSearch.BorderRadius = 15;
            this.jeanSoftTextBoxSearch.BorderSize = 2;
            this.jeanSoftTextBoxSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanSoftTextBoxSearch.Location = new System.Drawing.Point(370, 32);
            this.jeanSoftTextBoxSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.jeanSoftTextBoxSearch.Multiline = false;
            this.jeanSoftTextBoxSearch.Name = "jeanSoftTextBoxSearch";
            this.jeanSoftTextBoxSearch.Padding = new System.Windows.Forms.Padding(38, 8, 38, 8);
            this.jeanSoftTextBoxSearch.PasswordChar = false;
            this.jeanSoftTextBoxSearch.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.jeanSoftTextBoxSearch.PlaceholderText = "  Фамилия, Имя или №Карты";
            this.jeanSoftTextBoxSearch.Size = new System.Drawing.Size(295, 34);
            this.jeanSoftTextBoxSearch.TabIndex = 50;
            this.jeanSoftTextBoxSearch.Texts = "";
            this.jeanSoftTextBoxSearch.UnderlinedStyle = false;
            this.jeanSoftTextBoxSearch._TextChanged += new System.EventHandler(this.jeanSoftTextBoxSearch__TextChanged);
            // 
            // ArchiveServices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1023, 570);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.card);
            this.Controls.Add(this.nameClient);
            this.Controls.Add(this.jeanPanel);
            this.Controls.Add(this.jeanModernButtonChangeData);
            this.Controls.Add(this.pictureBoxSearch);
            this.Controls.Add(this.jeanModernButtonErase);
            this.Controls.Add(this.jeanSoftTextBoxSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ArchiveServices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Архив";
            this.Load += new System.EventHandler(this.ArchiveServices_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).EndInit();
            this.jeanPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewArchive)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxSearch;
        private Controls.JeanModernButton jeanModernButtonErase;
        private Controls.JeanModernButton jeanModernButtonChangeData;
        private Controls.JeanPanel jeanPanel;
        protected internal System.Windows.Forms.DataGridView dataGridViewArchive;
        private System.Windows.Forms.Label nameClient;
        private System.Windows.Forms.Label card;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        protected internal Controls.jeanSoftTextBox jeanSoftTextBoxSearch;
    }
}