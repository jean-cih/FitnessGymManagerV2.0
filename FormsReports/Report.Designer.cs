namespace GymApplicationV2._0
{
    partial class Report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report));
            this.jeanFormStyle = new GymApplicationV2._0.Components.JeanFormStyle(this.components);
            this.SuspendLayout();
            // 
            // jeanFormStyle
            // 
            this.jeanFormStyle.AllowUserResize = false;
            this.jeanFormStyle.BackColor = System.Drawing.Color.White;
            this.jeanFormStyle.ContextMenuForm = null;
            this.jeanFormStyle.ControlBoxButtonsWidth = 20;
            this.jeanFormStyle.EnableControlBoxIconsLight = false;
            this.jeanFormStyle.EnableControlBoxMouseLight = false;
            this.jeanFormStyle.Form = null;
            this.jeanFormStyle.FormStyle = GymApplicationV2._0.Components.JeanFormStyle.fStyle.None;
            this.jeanFormStyle.HeaderColor = System.Drawing.Color.Black;
            this.jeanFormStyle.HeaderColorAdditional = System.Drawing.Color.DarkOrange;
            this.jeanFormStyle.HeaderColorGradientEnable = true;
            this.jeanFormStyle.HeaderColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.jeanFormStyle.HeaderHeight = 30;
            this.jeanFormStyle.HeaderImage = null;
            this.jeanFormStyle.HeaderTextColor = System.Drawing.Color.White;
            this.jeanFormStyle.HeaderTextFont = new System.Drawing.Font("Segoe UI", 9.75F);
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(918, 580);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчет";
            this.Load += new System.EventHandler(this.Report_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Components.JeanFormStyle jeanFormStyle;
    }
}