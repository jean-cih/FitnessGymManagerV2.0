namespace GymApplicationV2._0.FormsServices
{
    partial class FreezeMembership
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
            this.hintLabel = new System.Windows.Forms.Label();
            this.jeanFormStyle1 = new GymApplicationV2._0.Components.JeanFormStyle(this.components);
            this.SuspendLayout();
            // 
            // hintLabel
            // 
            this.hintLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hintLabel.AutoSize = true;
            this.hintLabel.BackColor = System.Drawing.Color.Transparent;
            this.hintLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.hintLabel.Location = new System.Drawing.Point(83, 402);
            this.hintLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(312, 13);
            this.hintLabel.TabIndex = 36;
            this.hintLabel.Text = "Выберите причину и срок заморозки и нажмите \'Сохранить\'";
            this.hintLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
            // FreezeMembership
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 424);
            this.Controls.Add(this.hintLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FreezeMembership";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FreezeMembership";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected internal System.Windows.Forms.Label hintLabel;
        private Components.JeanFormStyle jeanFormStyle1;
    }
}