namespace GymApplicationV2._0
{
    partial class HistoryPayment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryPayment));
            this.dataGridViewHistory = new System.Windows.Forms.DataGridView();
            this.radioForMonth = new System.Windows.Forms.RadioButton();
            this.radioForDay = new System.Windows.Forms.RadioButton();
            this.radioOtherPeriod = new System.Windows.Forms.RadioButton();
            this.labelWith = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.radioForWeek = new System.Windows.Forms.RadioButton();
            this.jeanModernButtonShow = new GymApplicationV2._0.Controls.JeanModernButton();
            this.labelPayments = new System.Windows.Forms.Label();
            this.jeanModernButtonRefresh = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanPanel = new GymApplicationV2._0.Controls.JeanPanel();
            this.jeanDateTimePickerBegin = new GymApplicationV2._0.Controls.JeanDateTimePicker();
            this.jeanDateTimePickerEnd = new GymApplicationV2._0.Controls.JeanDateTimePicker();
            this.pictureBoxSearch = new System.Windows.Forms.PictureBox();
            this.jeanModernButtonErase = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanSoftTextBoxSearch = new GymApplicationV2._0.Controls.jeanSoftTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewHistory
            // 
            this.dataGridViewHistory.AllowUserToAddRows = false;
            this.dataGridViewHistory.AllowUserToDeleteRows = false;
            this.dataGridViewHistory.AllowUserToResizeColumns = false;
            this.dataGridViewHistory.AllowUserToResizeRows = false;
            this.dataGridViewHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewHistory.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewHistory.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewHistory.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewHistory.ColumnHeadersHeight = 35;
            this.dataGridViewHistory.EnableHeadersVisualStyles = false;
            this.dataGridViewHistory.GridColor = System.Drawing.Color.Black;
            this.dataGridViewHistory.Location = new System.Drawing.Point(23, 145);
            this.dataGridViewHistory.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewHistory.Name = "dataGridViewHistory";
            this.dataGridViewHistory.ReadOnly = true;
            this.dataGridViewHistory.RowHeadersVisible = false;
            this.dataGridViewHistory.RowHeadersWidth = 51;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dataGridViewHistory.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewHistory.RowTemplate.Height = 24;
            this.dataGridViewHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewHistory.Size = new System.Drawing.Size(885, 336);
            this.dataGridViewHistory.TabIndex = 1;
            // 
            // radioForMonth
            // 
            this.radioForMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioForMonth.AutoSize = true;
            this.radioForMonth.Location = new System.Drawing.Point(754, 10);
            this.radioForMonth.Margin = new System.Windows.Forms.Padding(2);
            this.radioForMonth.Name = "radioForMonth";
            this.radioForMonth.Size = new System.Drawing.Size(73, 17);
            this.radioForMonth.TabIndex = 29;
            this.radioForMonth.Text = "За месяц";
            this.radioForMonth.UseVisualStyleBackColor = true;
            // 
            // radioForDay
            // 
            this.radioForDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioForDay.AutoSize = true;
            this.radioForDay.Location = new System.Drawing.Point(754, 52);
            this.radioForDay.Margin = new System.Windows.Forms.Padding(2);
            this.radioForDay.Name = "radioForDay";
            this.radioForDay.Size = new System.Drawing.Size(65, 17);
            this.radioForDay.TabIndex = 30;
            this.radioForDay.Text = "За день";
            this.radioForDay.UseVisualStyleBackColor = true;
            // 
            // radioOtherPeriod
            // 
            this.radioOtherPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioOtherPeriod.AutoSize = true;
            this.radioOtherPeriod.Checked = true;
            this.radioOtherPeriod.Location = new System.Drawing.Point(754, 73);
            this.radioOtherPeriod.Margin = new System.Windows.Forms.Padding(2);
            this.radioOtherPeriod.Name = "radioOtherPeriod";
            this.radioOtherPeriod.Size = new System.Drawing.Size(101, 17);
            this.radioOtherPeriod.TabIndex = 35;
            this.radioOtherPeriod.TabStop = true;
            this.radioOtherPeriod.Text = "Другой период";
            this.radioOtherPeriod.UseVisualStyleBackColor = true;
            // 
            // labelWith
            // 
            this.labelWith.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWith.AutoSize = true;
            this.labelWith.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelWith.Location = new System.Drawing.Point(669, 109);
            this.labelWith.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelWith.Name = "labelWith";
            this.labelWith.Size = new System.Drawing.Size(15, 17);
            this.labelWith.TabIndex = 28;
            this.labelWith.Text = "с";
            // 
            // labelTo
            // 
            this.labelTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTo.AutoSize = true;
            this.labelTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTo.Location = new System.Drawing.Point(791, 109);
            this.labelTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(24, 17);
            this.labelTo.TabIndex = 34;
            this.labelTo.Text = "по";
            // 
            // radioForWeek
            // 
            this.radioForWeek.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioForWeek.AutoSize = true;
            this.radioForWeek.Location = new System.Drawing.Point(754, 31);
            this.radioForWeek.Margin = new System.Windows.Forms.Padding(2);
            this.radioForWeek.Name = "radioForWeek";
            this.radioForWeek.Size = new System.Drawing.Size(79, 17);
            this.radioForWeek.TabIndex = 31;
            this.radioForWeek.Text = "За неделю";
            this.radioForWeek.UseVisualStyleBackColor = true;
            // 
            // jeanModernButtonShow
            // 
            this.jeanModernButtonShow.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.jeanModernButtonShow.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonShow.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonShow.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonShow.BorderRadius = 20;
            this.jeanModernButtonShow.BorderSize = 2;
            this.jeanModernButtonShow.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonShow.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonShow.Location = new System.Drawing.Point(351, 498);
            this.jeanModernButtonShow.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonShow.Name = "jeanModernButtonShow";
            this.jeanModernButtonShow.Size = new System.Drawing.Size(112, 45);
            this.jeanModernButtonShow.TabIndex = 36;
            this.jeanModernButtonShow.Text = "Показать";
            this.jeanModernButtonShow.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonShow.UseVisualStyleBackColor = false;
            this.jeanModernButtonShow.Click += new System.EventHandler(this.jeanModernButtonShow_Click);
            // 
            // labelPayments
            // 
            this.labelPayments.AutoSize = true;
            this.labelPayments.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPayments.Location = new System.Drawing.Point(20, 109);
            this.labelPayments.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPayments.Name = "labelPayments";
            this.labelPayments.Size = new System.Drawing.Size(0, 20);
            this.labelPayments.TabIndex = 37;
            // 
            // jeanModernButtonRefresh
            // 
            this.jeanModernButtonRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.jeanModernButtonRefresh.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonRefresh.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonRefresh.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonRefresh.BorderRadius = 20;
            this.jeanModernButtonRefresh.BorderSize = 2;
            this.jeanModernButtonRefresh.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonRefresh.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonRefresh.Location = new System.Drawing.Point(468, 498);
            this.jeanModernButtonRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonRefresh.Name = "jeanModernButtonRefresh";
            this.jeanModernButtonRefresh.Size = new System.Drawing.Size(112, 45);
            this.jeanModernButtonRefresh.TabIndex = 38;
            this.jeanModernButtonRefresh.Text = "Обновить";
            this.jeanModernButtonRefresh.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonRefresh.UseVisualStyleBackColor = false;
            this.jeanModernButtonRefresh.Click += new System.EventHandler(this.jeanModernButtonRefresh_Click);
            // 
            // jeanPanel
            // 
            this.jeanPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanPanel.BackColor = System.Drawing.Color.White;
            this.jeanPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.jeanPanel.BorderRadius = 30;
            this.jeanPanel.ForeColor = System.Drawing.Color.Black;
            this.jeanPanel.GradientAngle = 90F;
            this.jeanPanel.GradientBottomColor = System.Drawing.Color.DodgerBlue;
            this.jeanPanel.GradientTapColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            this.jeanPanel.Location = new System.Drawing.Point(9, 137);
            this.jeanPanel.Margin = new System.Windows.Forms.Padding(2);
            this.jeanPanel.Name = "jeanPanel";
            this.jeanPanel.Size = new System.Drawing.Size(913, 354);
            this.jeanPanel.TabIndex = 39;
            // 
            // jeanDateTimePickerBegin
            // 
            this.jeanDateTimePickerBegin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanDateTimePickerBegin.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            this.jeanDateTimePickerBegin.BorderSize = 2;
            this.jeanDateTimePickerBegin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanDateTimePickerBegin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.jeanDateTimePickerBegin.Location = new System.Drawing.Point(686, 103);
            this.jeanDateTimePickerBegin.Margin = new System.Windows.Forms.Padding(2);
            this.jeanDateTimePickerBegin.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.jeanDateTimePickerBegin.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.jeanDateTimePickerBegin.MinimumSize = new System.Drawing.Size(4, 30);
            this.jeanDateTimePickerBegin.Name = "jeanDateTimePickerBegin";
            this.jeanDateTimePickerBegin.Size = new System.Drawing.Size(102, 30);
            this.jeanDateTimePickerBegin.SkinColor = System.Drawing.Color.White;
            this.jeanDateTimePickerBegin.TabIndex = 41;
            this.jeanDateTimePickerBegin.TextColor = System.Drawing.Color.Black;
            // 
            // jeanDateTimePickerEnd
            // 
            this.jeanDateTimePickerEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanDateTimePickerEnd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            this.jeanDateTimePickerEnd.BorderSize = 2;
            this.jeanDateTimePickerEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanDateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.jeanDateTimePickerEnd.Location = new System.Drawing.Point(817, 103);
            this.jeanDateTimePickerEnd.Margin = new System.Windows.Forms.Padding(2);
            this.jeanDateTimePickerEnd.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.jeanDateTimePickerEnd.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.jeanDateTimePickerEnd.MinimumSize = new System.Drawing.Size(4, 30);
            this.jeanDateTimePickerEnd.Name = "jeanDateTimePickerEnd";
            this.jeanDateTimePickerEnd.Size = new System.Drawing.Size(102, 30);
            this.jeanDateTimePickerEnd.SkinColor = System.Drawing.Color.White;
            this.jeanDateTimePickerEnd.TabIndex = 42;
            this.jeanDateTimePickerEnd.TextColor = System.Drawing.Color.Black;
            // 
            // pictureBoxSearch
            // 
            this.pictureBoxSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxSearch.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxSearch.Image")));
            this.pictureBoxSearch.Location = new System.Drawing.Point(320, 102);
            this.pictureBoxSearch.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxSearch.Name = "pictureBoxSearch";
            this.pictureBoxSearch.Size = new System.Drawing.Size(26, 24);
            this.pictureBoxSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSearch.TabIndex = 50;
            this.pictureBoxSearch.TabStop = false;
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
            this.jeanModernButtonErase.Location = new System.Drawing.Point(576, 102);
            this.jeanModernButtonErase.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonErase.Name = "jeanModernButtonErase";
            this.jeanModernButtonErase.Size = new System.Drawing.Size(26, 24);
            this.jeanModernButtonErase.TabIndex = 49;
            this.jeanModernButtonErase.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonErase.UseVisualStyleBackColor = false;
            this.jeanModernButtonErase.Visible = false;
            this.jeanModernButtonErase.Click += new System.EventHandler(this.jeanModernButtonErase_Click);
            // 
            // jeanSoftTextBoxSearch
            // 
            this.jeanSoftTextBoxSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.jeanSoftTextBoxSearch.BorderColor = System.Drawing.Color.Black;
            this.jeanSoftTextBoxSearch.BorderFocusColor = System.Drawing.Color.HotPink;
            this.jeanSoftTextBoxSearch.BorderRadius = 15;
            this.jeanSoftTextBoxSearch.BorderSize = 2;
            this.jeanSoftTextBoxSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanSoftTextBoxSearch.Location = new System.Drawing.Point(311, 97);
            this.jeanSoftTextBoxSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.jeanSoftTextBoxSearch.Multiline = false;
            this.jeanSoftTextBoxSearch.Name = "jeanSoftTextBoxSearch";
            this.jeanSoftTextBoxSearch.Padding = new System.Windows.Forms.Padding(38, 8, 38, 8);
            this.jeanSoftTextBoxSearch.PasswordChar = false;
            this.jeanSoftTextBoxSearch.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.jeanSoftTextBoxSearch.PlaceholderText = "  Фамилия, Имя или №Карты";
            this.jeanSoftTextBoxSearch.Size = new System.Drawing.Size(295, 34);
            this.jeanSoftTextBoxSearch.TabIndex = 48;
            this.jeanSoftTextBoxSearch.Texts = "";
            this.jeanSoftTextBoxSearch.UnderlinedStyle = false;
            this.jeanSoftTextBoxSearch._TextChanged += new System.EventHandler(this.jeanSoftTextBoxSearch__TextChanged);
            // 
            // HistoryPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(931, 604);
            this.Controls.Add(this.pictureBoxSearch);
            this.Controls.Add(this.jeanModernButtonErase);
            this.Controls.Add(this.jeanSoftTextBoxSearch);
            this.Controls.Add(this.jeanDateTimePickerEnd);
            this.Controls.Add(this.jeanDateTimePickerBegin);
            this.Controls.Add(this.dataGridViewHistory);
            this.Controls.Add(this.jeanPanel);
            this.Controls.Add(this.jeanModernButtonRefresh);
            this.Controls.Add(this.labelPayments);
            this.Controls.Add(this.jeanModernButtonShow);
            this.Controls.Add(this.radioOtherPeriod);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.labelWith);
            this.Controls.Add(this.radioForWeek);
            this.Controls.Add(this.radioForDay);
            this.Controls.Add(this.radioForMonth);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "HistoryPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HistoryPayment";
            this.Load += new System.EventHandler(this.HistoryPayment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected internal System.Windows.Forms.DataGridView dataGridViewHistory;
        private System.Windows.Forms.Label labelWith;
        private System.Windows.Forms.Label labelTo;
        private Controls.JeanModernButton jeanModernButtonShow;
        private System.Windows.Forms.Label labelPayments;
        private Controls.JeanModernButton jeanModernButtonRefresh;
        private Controls.JeanPanel jeanPanel;
        public System.Windows.Forms.RadioButton radioForMonth;
        public System.Windows.Forms.RadioButton radioForDay;
        public System.Windows.Forms.RadioButton radioOtherPeriod;
        public System.Windows.Forms.RadioButton radioForWeek;
        public Controls.JeanDateTimePicker jeanDateTimePickerEnd;
        public Controls.JeanDateTimePicker jeanDateTimePickerBegin;
        private System.Windows.Forms.PictureBox pictureBoxSearch;
        private Controls.JeanModernButton jeanModernButtonErase;
        protected internal Controls.jeanSoftTextBox jeanSoftTextBoxSearch;
    }
}