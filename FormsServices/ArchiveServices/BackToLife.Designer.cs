using System.Drawing;

namespace GymApplicationV2._0.FormsServices
{
    partial class BackToLife
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackToLife));
            this.labelNameClient = new System.Windows.Forms.Label();
            this.labelNubmerCard = new System.Windows.Forms.Label();
            this.jeanModernButtonBackToLife = new GymApplicationV2._0.Controls.JeanModernButton();
            this.titleLabel = new System.Windows.Forms.Label();
            this.hintLabel = new System.Windows.Forms.Label();
            this.jeanTextBoxVisits = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxTerm = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxMembership = new GymApplicationV2._0.JeanTextBox();
            this.SuspendLayout();
            // 
            // labelNameClient
            // 
            this.labelNameClient.AutoSize = true;
            this.labelNameClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNameClient.Location = new System.Drawing.Point(49, 108);
            this.labelNameClient.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNameClient.Name = "labelNameClient";
            this.labelNameClient.Size = new System.Drawing.Size(135, 17);
            this.labelNameClient.TabIndex = 0;
            this.labelNameClient.Text = "Почекутов Евгений";
            // 
            // labelNubmerCard
            // 
            this.labelNubmerCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNubmerCard.AutoSize = true;
            this.labelNubmerCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNubmerCard.Location = new System.Drawing.Point(212, 108);
            this.labelNubmerCard.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNubmerCard.Name = "labelNubmerCard";
            this.labelNubmerCard.Size = new System.Drawing.Size(112, 17);
            this.labelNubmerCard.TabIndex = 1;
            this.labelNubmerCard.Text = "2000000012345";
            // 
            // jeanModernButtonBackToLife
            // 
            this.jeanModernButtonBackToLife.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.jeanModernButtonBackToLife.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonBackToLife.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonBackToLife.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonBackToLife.BorderRadius = 20;
            this.jeanModernButtonBackToLife.BorderSize = 2;
            this.jeanModernButtonBackToLife.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonBackToLife.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonBackToLife.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonBackToLife.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonBackToLife.Location = new System.Drawing.Point(126, 320);
            this.jeanModernButtonBackToLife.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonBackToLife.Name = "jeanModernButtonBackToLife";
            this.jeanModernButtonBackToLife.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonBackToLife.TabIndex = 23;
            this.jeanModernButtonBackToLife.Text = "Вернуть";
            this.jeanModernButtonBackToLife.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonBackToLife.UseVisualStyleBackColor = false;
            this.jeanModernButtonBackToLife.Click += new System.EventHandler(this.jeanModernButtonBackToLife_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.AutoSize = false;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(255)))));
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(300, 25);
            this.titleLabel.TabIndex = 25;
            this.titleLabel.Text = "⚡ ВОЗВРАТ ИЗ АРХИВА";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // hintLabel
            // 
            this.hintLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hintLabel.AutoSize = false;
            this.hintLabel.BackColor = System.Drawing.Color.Transparent;
            this.hintLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic);
            this.hintLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(180)))));
            this.hintLabel.Location = new System.Drawing.Point(0, 0);
            this.hintLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(280, 25);
            this.hintLabel.TabIndex = 26;
            this.hintLabel.Text = "Измените необходимые данные";
            this.hintLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // jeanTextBoxVisits
            // 
            this.jeanTextBoxVisits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxVisits.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxVisits.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxVisits.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxVisits.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxVisits.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxVisits.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxVisits.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxVisits.Location = new System.Drawing.Point(52, 208);
            this.jeanTextBoxVisits.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxVisits.Name = "jeanTextBoxVisits";
            this.jeanTextBoxVisits.SelectionStart = 0;
            this.jeanTextBoxVisits.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxVisits.TabIndex = 48;
            this.jeanTextBoxVisits.TextInput = "";
            this.jeanTextBoxVisits.TextPreview = "Посещения";
            this.jeanTextBoxVisits.UseSystemPasswordChar = false;
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
            this.jeanTextBoxTerm.Location = new System.Drawing.Point(52, 170);
            this.jeanTextBoxTerm.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxTerm.Name = "jeanTextBoxTerm";
            this.jeanTextBoxTerm.SelectionStart = 0;
            this.jeanTextBoxTerm.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxTerm.TabIndex = 47;
            this.jeanTextBoxTerm.TextInput = "";
            this.jeanTextBoxTerm.TextPreview = "Срок";
            this.jeanTextBoxTerm.UseSystemPasswordChar = false;
            // 
            // jeanTextBoxMembership
            // 
            this.jeanTextBoxMembership.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxMembership.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxMembership.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxMembership.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxMembership.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxMembership.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxMembership.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxMembership.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxMembership.Location = new System.Drawing.Point(52, 131);
            this.jeanTextBoxMembership.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxMembership.Name = "jeanTextBoxMembership";
            this.jeanTextBoxMembership.SelectionStart = 0;
            this.jeanTextBoxMembership.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxMembership.TabIndex = 46;
            this.jeanTextBoxMembership.TextInput = "";
            this.jeanTextBoxMembership.TextPreview = "Абонемент";
            this.jeanTextBoxMembership.UseSystemPasswordChar = false;
            // 
            // BackToLife
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(382, 402);
            this.Controls.Add(this.jeanTextBoxVisits);
            this.Controls.Add(this.jeanTextBoxTerm);
            this.Controls.Add(this.jeanTextBoxMembership);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.jeanModernButtonBackToLife);
            this.Controls.Add(this.labelNubmerCard);
            this.Controls.Add(this.labelNameClient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "BackToLife";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Возврат из архива";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected internal System.Windows.Forms.Label labelNameClient;
        protected internal System.Windows.Forms.Label labelNubmerCard;
        private Controls.JeanModernButton jeanModernButtonBackToLife;
        protected internal System.Windows.Forms.Label titleLabel;
        protected internal System.Windows.Forms.Label hintLabel;
        public JeanTextBox jeanTextBoxVisits;
        public JeanTextBox jeanTextBoxTerm;
        public JeanTextBox jeanTextBoxMembership;
    }
}