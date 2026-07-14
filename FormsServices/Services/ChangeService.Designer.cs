using System.Drawing;

namespace GymApplicationV2._0
{
    partial class ChangeService
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
            this.hintLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.jeanModernButtonSave = new GymApplicationV2._0.Controls.JeanModernButton();
            this.jeanTextBoxVisited = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxTerm = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxPrice = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxName = new GymApplicationV2._0.JeanTextBox();
            this.SuspendLayout();
            // 
            // hintLabel
            // 
            this.hintLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hintLabel.BackColor = System.Drawing.Color.Transparent;
            this.hintLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic);
            this.hintLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(180)))));
            this.hintLabel.Location = new System.Drawing.Point(0, 0);
            this.hintLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(155, 25);
            this.hintLabel.TabIndex = 67;
            this.hintLabel.Text = "Измените параметры услуги";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(175, 89);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 17);
            this.label1.TabIndex = 66;
            this.label1.Text = "Услуга";
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(255)))));
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(270, 25);
            this.titleLabel.TabIndex = 65;
            this.titleLabel.Text = "РЕДАКТИРОВАНИЕ УСЛУГИ";
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
            this.jeanModernButtonSave.Location = new System.Drawing.Point(146, 275);
            this.jeanModernButtonSave.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonSave.Name = "jeanModernButtonSave";
            this.jeanModernButtonSave.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonSave.TabIndex = 55;
            this.jeanModernButtonSave.Text = "Сохранить";
            this.jeanModernButtonSave.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonSave.UseVisualStyleBackColor = false;
            this.jeanModernButtonSave.Click += new System.EventHandler(this.jeanModernButtonSave_Click);
            // 
            // jeanTextBoxVisited
            // 
            this.jeanTextBoxVisited.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxVisited.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxVisited.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxVisited.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxVisited.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxVisited.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxVisited.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxVisited.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxVisited.Location = new System.Drawing.Point(77, 228);
            this.jeanTextBoxVisited.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxVisited.Name = "jeanTextBoxVisited";
            this.jeanTextBoxVisited.SelectionStart = 0;
            this.jeanTextBoxVisited.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxVisited.TabIndex = 71;
            this.jeanTextBoxVisited.TextInput = "";
            this.jeanTextBoxVisited.TextPreview = "Количество посещений";
            this.jeanTextBoxVisited.UseSystemPasswordChar = false;
            // 
            // jeanTextBoxTerm
            // 
            this.jeanTextBoxTerm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxTerm.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxTerm.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxTerm.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxTerm.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxTerm.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxTerm.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxTerm.Location = new System.Drawing.Point(77, 192);
            this.jeanTextBoxTerm.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxTerm.Name = "jeanTextBoxTerm";
            this.jeanTextBoxTerm.SelectionStart = 0;
            this.jeanTextBoxTerm.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxTerm.TabIndex = 70;
            this.jeanTextBoxTerm.TextInput = "";
            this.jeanTextBoxTerm.TextPreview = "Срок Действия (Мес)";
            this.jeanTextBoxTerm.UseSystemPasswordChar = false;
            // 
            // jeanTextBoxPrice
            // 
            this.jeanTextBoxPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxPrice.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxPrice.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxPrice.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxPrice.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxPrice.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxPrice.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxPrice.Location = new System.Drawing.Point(77, 156);
            this.jeanTextBoxPrice.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxPrice.Name = "jeanTextBoxPrice";
            this.jeanTextBoxPrice.SelectionStart = 0;
            this.jeanTextBoxPrice.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxPrice.TabIndex = 69;
            this.jeanTextBoxPrice.TextInput = "";
            this.jeanTextBoxPrice.TextPreview = "Цена";
            this.jeanTextBoxPrice.UseSystemPasswordChar = false;
            // 
            // jeanTextBoxName
            // 
            this.jeanTextBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxName.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxName.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxName.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxName.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxName.Location = new System.Drawing.Point(77, 119);
            this.jeanTextBoxName.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxName.Name = "jeanTextBoxName";
            this.jeanTextBoxName.SelectionStart = 0;
            this.jeanTextBoxName.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxName.TabIndex = 68;
            this.jeanTextBoxName.TextInput = "";
            this.jeanTextBoxName.TextPreview = "Наименование";
            this.jeanTextBoxName.UseSystemPasswordChar = false;
            // 
            // ChangeService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(398, 362);
            this.Controls.Add(this.jeanTextBoxVisited);
            this.Controls.Add(this.jeanTextBoxTerm);
            this.Controls.Add(this.jeanTextBoxPrice);
            this.Controls.Add(this.jeanTextBoxName);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.jeanModernButtonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ChangeService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ChangeService";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected internal Controls.JeanModernButton jeanModernButtonSave;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label hintLabel;
        public JeanTextBox jeanTextBoxVisited;
        public JeanTextBox jeanTextBoxTerm;
        public JeanTextBox jeanTextBoxPrice;
        public JeanTextBox jeanTextBoxName;
    }
}