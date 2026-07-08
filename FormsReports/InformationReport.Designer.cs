namespace GymApplicationV2._0
{
    partial class InformationReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformationReport));
            this.labelAllClients = new System.Windows.Forms.Label();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.labelShowPeriod = new System.Windows.Forms.Label();
            this.jeanPanel1 = new GymApplicationV2._0.Controls.JeanPanel();
            this.dataGridViewShowReport = new System.Windows.Forms.DataGridView();
            this.jeanPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShowReport)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAllClients
            // 
            this.labelAllClients.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelAllClients.AutoSize = true;
            this.labelAllClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelAllClients.Location = new System.Drawing.Point(12, 646);
            this.labelAllClients.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAllClients.Name = "labelAllClients";
            this.labelAllClients.Size = new System.Drawing.Size(142, 18);
            this.labelAllClients.TabIndex = 16;
            this.labelAllClients.Text = "Всего клиентов: ";
            // 
            // labelQuantity
            // 
            this.labelQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelQuantity.Location = new System.Drawing.Point(146, 647);
            this.labelQuantity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(17, 18);
            this.labelQuantity.TabIndex = 17;
            this.labelQuantity.Text = "0";
            // 
            // labelShowPeriod
            // 
            this.labelShowPeriod.AutoSize = true;
            this.labelShowPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelShowPeriod.Location = new System.Drawing.Point(23, 17);
            this.labelShowPeriod.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelShowPeriod.Name = "labelShowPeriod";
            this.labelShowPeriod.Size = new System.Drawing.Size(137, 24);
            this.labelShowPeriod.TabIndex = 18;
            this.labelShowPeriod.Text = "Посещения с";
            // 
            // jeanPanel1
            // 
            this.jeanPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanPanel1.BackColor = System.Drawing.Color.White;
            this.jeanPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.jeanPanel1.BorderRadius = 30;
            this.jeanPanel1.Controls.Add(this.dataGridViewShowReport);
            this.jeanPanel1.ForeColor = System.Drawing.Color.Black;
            this.jeanPanel1.GradientAngle = 90F;
            this.jeanPanel1.GradientBottomColor = System.Drawing.Color.DodgerBlue;
            this.jeanPanel1.GradientTapColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            this.jeanPanel1.Location = new System.Drawing.Point(6, 54);
            this.jeanPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.jeanPanel1.Name = "jeanPanel1";
            this.jeanPanel1.Size = new System.Drawing.Size(1102, 581);
            this.jeanPanel1.TabIndex = 19;
            // 
            // dataGridViewShowReport
            // 
            this.dataGridViewShowReport.AllowUserToAddRows = false;
            this.dataGridViewShowReport.AllowUserToDeleteRows = false;
            this.dataGridViewShowReport.AllowUserToResizeRows = false;
            this.dataGridViewShowReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewShowReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewShowReport.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewShowReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewShowReport.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewShowReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewShowReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewShowReport.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewShowReport.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewShowReport.EnableHeadersVisualStyles = false;
            this.dataGridViewShowReport.GridColor = System.Drawing.Color.Black;
            this.dataGridViewShowReport.Location = new System.Drawing.Point(21, 13);
            this.dataGridViewShowReport.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewShowReport.Name = "dataGridViewShowReport";
            this.dataGridViewShowReport.ReadOnly = true;
            this.dataGridViewShowReport.RowHeadersVisible = false;
            this.dataGridViewShowReport.RowHeadersWidth = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dataGridViewShowReport.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewShowReport.RowTemplate.Height = 24;
            this.dataGridViewShowReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewShowReport.Size = new System.Drawing.Size(1063, 558);
            this.dataGridViewShowReport.TabIndex = 1;
            // 
            // InformationReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1119, 676);
            this.Controls.Add(this.jeanPanel1);
            this.Controls.Add(this.labelShowPeriod);
            this.Controls.Add(this.labelQuantity);
            this.Controls.Add(this.labelAllClients);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "InformationReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Посещаемость";
            this.Load += new System.EventHandler(this.Attendance_Load);
            this.jeanPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShowReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelAllClients;
        private System.Windows.Forms.Label labelQuantity;
        private System.Windows.Forms.Label labelShowPeriod;
        private Controls.JeanPanel jeanPanel1;
        protected internal System.Windows.Forms.DataGridView dataGridViewShowReport;
    }
}