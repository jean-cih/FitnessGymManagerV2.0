using System.Drawing;

namespace GymApplicationV2._0
{
    partial class DuplicateResolution
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.jeanPanel1 = new GymApplicationV2._0.Controls.JeanPanel();
            this.dataGridViewClients = new System.Windows.Forms.DataGridView();
            this.hintLabel = new System.Windows.Forms.Label();
            this.jeanModernButtonSave = new GymApplicationV2._0.Controls.JeanModernButton();
            this.label = new System.Windows.Forms.Label();
            this.labelCard = new System.Windows.Forms.Label();
            this.jeanPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClients)).BeginInit();
            this.SuspendLayout();
            // 
            // jeanPanel1
            // 
            this.jeanPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanPanel1.BackColor = System.Drawing.Color.White;
            this.jeanPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.jeanPanel1.BorderRadius = 30;
            this.jeanPanel1.Controls.Add(this.dataGridViewClients);
            this.jeanPanel1.ForeColor = System.Drawing.Color.Black;
            this.jeanPanel1.GradientAngle = 90F;
            this.jeanPanel1.GradientBottomColor = System.Drawing.Color.DodgerBlue;
            this.jeanPanel1.GradientTapColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            this.jeanPanel1.Location = new System.Drawing.Point(21, 52);
            this.jeanPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.jeanPanel1.Name = "jeanPanel1";
            this.jeanPanel1.Size = new System.Drawing.Size(554, 221);
            this.jeanPanel1.TabIndex = 73;
            // 
            // dataGridViewClients
            // 
            this.dataGridViewClients.AllowUserToAddRows = false;
            this.dataGridViewClients.AllowUserToDeleteRows = false;
            this.dataGridViewClients.AllowUserToResizeColumns = false;
            this.dataGridViewClients.AllowUserToResizeRows = false;
            this.dataGridViewClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewClients.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewClients.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewClients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewClients.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewClients.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClients.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewClients.ColumnHeadersHeight = 35;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewClients.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewClients.EnableHeadersVisualStyles = false;
            this.dataGridViewClients.GridColor = System.Drawing.Color.Black;
            this.dataGridViewClients.Location = new System.Drawing.Point(10, 9);
            this.dataGridViewClients.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewClients.Name = "dataGridViewClients";
            this.dataGridViewClients.ReadOnly = true;
            this.dataGridViewClients.RowHeadersVisible = false;
            this.dataGridViewClients.RowHeadersWidth = 40;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dataGridViewClients.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewClients.RowTemplate.Height = 24;
            this.dataGridViewClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewClients.Size = new System.Drawing.Size(534, 204);
            this.dataGridViewClients.TabIndex = 37;
            this.dataGridViewClients.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewClients_CellContentClick);
            // 
            // hintLabel
            // 
            this.hintLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hintLabel.BackColor = System.Drawing.Color.Transparent;
            this.hintLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic);
            this.hintLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(180)))));
            this.hintLabel.Location = new System.Drawing.Point(175, 326);
            this.hintLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(250, 25);
            this.hintLabel.TabIndex = 72;
            this.hintLabel.Text = "Выберите клиента";
            this.hintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jeanModernButtonSave
            // 
            this.jeanModernButtonSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.jeanModernButtonSave.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonSave.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonSave.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonSave.BorderRadius = 20;
            this.jeanModernButtonSave.BorderSize = 2;
            this.jeanModernButtonSave.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonSave.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonSave.Location = new System.Drawing.Point(243, 287);
            this.jeanModernButtonSave.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonSave.Name = "jeanModernButtonSave";
            this.jeanModernButtonSave.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonSave.TabIndex = 55;
            this.jeanModernButtonSave.Text = "Сохранить";
            this.jeanModernButtonSave.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonSave.UseVisualStyleBackColor = false;
            this.jeanModernButtonSave.Click += new System.EventHandler(this.jeanModernButtonSave_Click);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.BackColor = System.Drawing.Color.Transparent;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label.Location = new System.Drawing.Point(34, 22);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(75, 15);
            this.label.TabIndex = 38;
            this.label.Text = "№ Карты: ";
            // 
            // labelCard
            // 
            this.labelCard.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelCard.AutoSize = true;
            this.labelCard.BackColor = System.Drawing.Color.Transparent;
            this.labelCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCard.Location = new System.Drawing.Point(118, 22);
            this.labelCard.Name = "labelCard";
            this.labelCard.Size = new System.Drawing.Size(12, 15);
            this.labelCard.TabIndex = 74;
            this.labelCard.Text = "-";
            // 
            // DuplicateResolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(600, 360);
            this.Controls.Add(this.labelCard);
            this.Controls.Add(this.label);
            this.Controls.Add(this.jeanPanel1);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.jeanModernButtonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DuplicateResolution";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ChangeService";
            this.jeanPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected internal Controls.JeanModernButton jeanModernButtonSave;
        private System.Windows.Forms.Label hintLabel;
        private Controls.JeanPanel jeanPanel1;
        protected internal System.Windows.Forms.DataGridView dataGridViewClients;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label labelCard;
    }
}