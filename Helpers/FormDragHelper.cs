using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymApplicationV2._0.Helpers
{
    public static class FormDragHelper
    {
        public static void EnableDrag(this Control control, Form form, int dragAreaHeight = 0)
        {
            if (control == null || form == null) return;

            bool isDragging = false;
            Point lastCursor = Point.Empty;
            Point lastFormLocation = Point.Empty;

            void OnMouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button != MouseButtons.Left) return;
                if (dragAreaHeight > 0 && e.Y > dragAreaHeight) return;

                isDragging = true;
                lastCursor = Cursor.Position;
                lastFormLocation = form.Location;
            }

            void OnMouseMove(object sender, MouseEventArgs e)
            {
                if (!isDragging) return;

                Point diff = Point.Subtract(Cursor.Position, new Size(lastCursor));
                Point newLocation = Point.Add(lastFormLocation, new Size(diff));

                Rectangle screen = Screen.FromControl(form).WorkingArea;
                newLocation.X = Math.Max(screen.Left, Math.Min(newLocation.X, screen.Right - form.Width));
                newLocation.Y = Math.Max(screen.Top, Math.Min(newLocation.Y, screen.Bottom - form.Height));

                form.Location = newLocation;
            }

            void OnMouseUp(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                    isDragging = false;
            }

            control.MouseDown -= OnMouseDown;
            control.MouseMove -= OnMouseMove;
            control.MouseUp -= OnMouseUp;

            control.MouseDown += OnMouseDown;
            control.MouseMove += OnMouseMove;
            control.MouseUp += OnMouseUp;

            if (form.FormBorderStyle == FormBorderStyle.None)
            {
                form.MouseDown -= OnMouseDown;
                form.MouseMove -= OnMouseMove;
                form.MouseUp -= OnMouseUp;

                form.MouseDown += OnMouseDown;
                form.MouseMove += OnMouseMove;
                form.MouseUp += OnMouseUp;
            }
        }
    }
}