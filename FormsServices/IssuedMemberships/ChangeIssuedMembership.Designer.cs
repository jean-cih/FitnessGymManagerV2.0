using System.Drawing;

namespace GymApplicationV2._0.FormsServices
{
    partial class ChangeIssuedMembership
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
            this.components = new System.ComponentModel.Container();
            this.jeanModernButtonChange = new GymApplicationV2._0.Controls.JeanModernButton();
            this.titleLabel = new System.Windows.Forms.Label();
            this.hintLabel = new System.Windows.Forms.Label();
            this.jeanFormStyle1 = new GymApplicationV2._0.Components.JeanFormStyle(this.components);
            this.jeanTextBoxTerm = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxMembership = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxStatus = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxClient = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxVisits = new GymApplicationV2._0.JeanTextBox();
            this.jeanTextBoxCost = new GymApplicationV2._0.JeanTextBox();
            this.SuspendLayout();
            // 
            // jeanModernButtonChange
            // 
            this.jeanModernButtonChange.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.jeanModernButtonChange.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonChange.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.jeanModernButtonChange.BorderColor = System.Drawing.Color.DarkOrange;
            this.jeanModernButtonChange.BorderRadius = 20;
            this.jeanModernButtonChange.BorderSize = 2;
            this.jeanModernButtonChange.FlatAppearance.BorderSize = 0;
            this.jeanModernButtonChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jeanModernButtonChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanModernButtonChange.ForeColor = System.Drawing.Color.White;
            this.jeanModernButtonChange.Location = new System.Drawing.Point(140, 380);
            this.jeanModernButtonChange.Margin = new System.Windows.Forms.Padding(2);
            this.jeanModernButtonChange.Name = "jeanModernButtonChange";
            this.jeanModernButtonChange.Size = new System.Drawing.Size(112, 37);
            this.jeanModernButtonChange.TabIndex = 29;
            this.jeanModernButtonChange.Text = "Изменить";
            this.jeanModernButtonChange.TextColor = System.Drawing.Color.White;
            this.jeanModernButtonChange.UseVisualStyleBackColor = false;
            this.jeanModernButtonChange.Click += new System.EventHandler(this.jeanModernButtonChange_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.AutoSize = false;
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(250, 25);
            this.titleLabel.TabIndex = 34;
            this.titleLabel.Text = "✏️ РЕДАКТИРОВАНИЕ";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            titleLabel.Font = new Font("Montserrat", 13, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(220, 220, 255);
            titleLabel.BackColor = Color.Transparent;
            // 
            // hintLabel
            // 
            this.hintLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hintLabel.AutoSize = false;
            this.hintLabel.BackColor = System.Drawing.Color.Transparent;
            this.hintLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(280, 25);
            this.hintLabel.TabIndex = 35;
            this.hintLabel.Text = "Измените необходимые данные";
            this.hintLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            hintLabel.Font = new Font("Montserrat", 7, FontStyle.Italic);
            hintLabel.ForeColor = Color.FromArgb(140, 140, 180);
            hintLabel.BackColor = Color.Transparent;
            // 
            // jeanFormStyle1
            // 
            this.jeanFormStyle1.AllowUserResize = false;
            this.jeanFormStyle1.BackColor = System.Drawing.Color.White;
            this.jeanFormStyle1.ContextMenuForm = null;
            this.jeanFormStyle1.ControlBoxButtonsWidth = 20;
            this.jeanFormStyle1.EnableControlBoxIconsLight = false;
            this.jeanFormStyle1.EnableControlBoxMouseLight = false;
            this.jeanFormStyle1.Form = null;
            this.jeanFormStyle1.FormStyle = GymApplicationV2._0.Components.JeanFormStyle.fStyle.None;
            this.jeanFormStyle1.HeaderColor = System.Drawing.Color.DimGray;
            this.jeanFormStyle1.HeaderColorAdditional = System.Drawing.Color.White;
            this.jeanFormStyle1.HeaderColorGradientEnable = false;
            this.jeanFormStyle1.HeaderColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.jeanFormStyle1.HeaderHeight = 38;
            this.jeanFormStyle1.HeaderImage = null;
            this.jeanFormStyle1.HeaderTextColor = System.Drawing.Color.White;
            this.jeanFormStyle1.HeaderTextFont = new System.Drawing.Font("Segoe UI", 9.75F);
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
            this.jeanTextBoxTerm.Location = new System.Drawing.Point(66, 210);
            this.jeanTextBoxTerm.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxTerm.Name = "jeanTextBoxTerm";
            this.jeanTextBoxTerm.SelectionStart = 0;
            this.jeanTextBoxTerm.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxTerm.TabIndex = 46;
            this.jeanTextBoxTerm.TextInput = "";
            this.jeanTextBoxTerm.TextPreview = "Срок абонемента";
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
            this.jeanTextBoxMembership.Location = new System.Drawing.Point(66, 173);
            this.jeanTextBoxMembership.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxMembership.Name = "jeanTextBoxMembership";
            this.jeanTextBoxMembership.SelectionStart = 0;
            this.jeanTextBoxMembership.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxMembership.TabIndex = 45;
            this.jeanTextBoxMembership.TextInput = "";
            this.jeanTextBoxMembership.TextPreview = "Абонемент";
            this.jeanTextBoxMembership.UseSystemPasswordChar = false;
            // 
            // jeanTextBoxStatus
            // 
            this.jeanTextBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxStatus.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxStatus.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxStatus.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxStatus.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxStatus.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxStatus.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxStatus.Location = new System.Drawing.Point(66, 136);
            this.jeanTextBoxStatus.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxStatus.Name = "jeanTextBoxStatus";
            this.jeanTextBoxStatus.SelectionStart = 0;
            this.jeanTextBoxStatus.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxStatus.TabIndex = 44;
            this.jeanTextBoxStatus.TextInput = "";
            this.jeanTextBoxStatus.TextPreview = "Статус";
            this.jeanTextBoxStatus.UseSystemPasswordChar = false;
            // 
            // jeanTextBoxClient
            // 
            this.jeanTextBoxClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxClient.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxClient.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxClient.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxClient.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxClient.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxClient.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxClient.Location = new System.Drawing.Point(66, 98);
            this.jeanTextBoxClient.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxClient.Name = "jeanTextBoxClient";
            this.jeanTextBoxClient.SelectionStart = 0;
            this.jeanTextBoxClient.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxClient.TabIndex = 43;
            this.jeanTextBoxClient.TextInput = "";
            this.jeanTextBoxClient.TextPreview = "Наименование";
            this.jeanTextBoxClient.UseSystemPasswordChar = false;
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
            this.jeanTextBoxVisits.Location = new System.Drawing.Point(66, 247);
            this.jeanTextBoxVisits.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxVisits.Name = "jeanTextBoxVisits";
            this.jeanTextBoxVisits.SelectionStart = 0;
            this.jeanTextBoxVisits.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxVisits.TabIndex = 47;
            this.jeanTextBoxVisits.TextInput = "";
            this.jeanTextBoxVisits.TextPreview = "Посещений";
            this.jeanTextBoxVisits.UseSystemPasswordChar = false;
            // 
            // jeanTextBoxCost
            // 
            this.jeanTextBoxCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.jeanTextBoxCost.BackColor = System.Drawing.Color.White;
            this.jeanTextBoxCost.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.jeanTextBoxCost.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.jeanTextBoxCost.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.jeanTextBoxCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.jeanTextBoxCost.FontTextPreview = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.jeanTextBoxCost.ForeColor = System.Drawing.Color.Black;
            this.jeanTextBoxCost.Location = new System.Drawing.Point(66, 284);
            this.jeanTextBoxCost.Margin = new System.Windows.Forms.Padding(2);
            this.jeanTextBoxCost.Name = "jeanTextBoxCost";
            this.jeanTextBoxCost.SelectionStart = 0;
            this.jeanTextBoxCost.Size = new System.Drawing.Size(264, 32);
            this.jeanTextBoxCost.TabIndex = 48;
            this.jeanTextBoxCost.TextInput = "";
            this.jeanTextBoxCost.TextPreview = "Цена";
            this.jeanTextBoxCost.UseSystemPasswordChar = false;
            // 
            // ChangeIssuedMembership
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(410, 450);
            this.Controls.Add(this.jeanTextBoxCost);
            this.Controls.Add(this.jeanTextBoxVisits);
            this.Controls.Add(this.jeanTextBoxTerm);
            this.Controls.Add(this.jeanTextBoxMembership);
            this.Controls.Add(this.jeanTextBoxStatus);
            this.Controls.Add(this.jeanTextBoxClient);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.jeanModernButtonChange);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ChangeIssuedMembership";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ChangeArchiveService";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.JeanModernButton jeanModernButtonChange;
        protected internal System.Windows.Forms.Label titleLabel;
        protected internal System.Windows.Forms.Label hintLabel;
        private Components.JeanFormStyle jeanFormStyle1;
        public JeanTextBox jeanTextBoxTerm;
        public JeanTextBox jeanTextBoxMembership;
        public JeanTextBox jeanTextBoxStatus;
        public JeanTextBox jeanTextBoxClient;
        public JeanTextBox jeanTextBoxVisits;
        public JeanTextBox jeanTextBoxCost;
    }
}