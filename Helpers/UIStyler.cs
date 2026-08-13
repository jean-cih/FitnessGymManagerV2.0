using GymApplicationV2._0.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0.Helpers
{
    public static class UIStyler
    {
        public static void StyleTextBox(Form form, JeanTextBox textBox)
        {
            textBox.BorderColor = Color.FromArgb(80, 80, 120);
            textBox.BackColor = Color.White;
            textBox.ForeColor = Color.Black;
            textBox.Location = new Point((form.Width - textBox.Width) / 2, textBox.Location.Y);
        }

        public static JeanModernButton CreateStyledButton(string text, Color baseColor, int radius, int radiusSize, Color radiusColor, Point location, Size size)
        {
            return new JeanModernButton
            {
                Text = text,
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = baseColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BorderRadius = radius,
                BorderColor = radiusColor,
                BorderSize = radiusSize,
            };
        }

        public static Label CreateStyledTextBox(string text, Point location, Color color = default)
        {
            return new Label
            {
                Text = text,
                Location = location,
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent,
                ForeColor = color == default ? Color.Black : color,
                AutoSize = true,
            };
        }
    }
}
