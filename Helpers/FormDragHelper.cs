using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymApplicationV2._0.Helpers
{
    public static class FormDragHelper
    {
        private static bool _isDragging = false;
        private static Point _lastCursor;
        private static Point _lastForm;
        private static Form _targetForm;
        private static int _dragAreaHeight = 40;

        public static void EnableDrag(this Control control, Form form, int dragAreaHeight = 0)
        {
            if (control == null || form == null) return;

            _targetForm = form;
            _dragAreaHeight = dragAreaHeight;

            // Отписываемся от старых событий, чтобы избежать дублирования
            control.MouseDown -= Control_MouseDown;
            control.MouseMove -= Control_MouseMove;
            control.MouseUp -= Control_MouseUp;

            // Подписываемся на новые
            control.MouseDown += Control_MouseDown;
            control.MouseMove += Control_MouseMove;
            control.MouseUp += Control_MouseUp;

            // Если форма не имеет границ, подписываемся и на форму
            if (form.FormBorderStyle == FormBorderStyle.None)
            {
                form.MouseDown -= Control_MouseDown;
                form.MouseMove -= Control_MouseMove;
                form.MouseUp -= Control_MouseUp;

                form.MouseDown += Control_MouseDown;
                form.MouseMove += Control_MouseMove;
                form.MouseUp += Control_MouseUp;
            }
        }

        private static void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_dragAreaHeight > 0 && e.Y > _dragAreaHeight)
                    return;

                _isDragging = true;
                _lastCursor = Cursor.Position;
                _lastForm = _targetForm.Location;
            }
        }

        private static void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(_lastCursor));
                Point newLocation = Point.Add(_lastForm, new Size(diff));

                Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
                newLocation.X = Math.Max(screenBounds.Left, Math.Min(newLocation.X, screenBounds.Right - _targetForm.Width));
                newLocation.Y = Math.Max(screenBounds.Top, Math.Min(newLocation.Y, screenBounds.Bottom - _targetForm.Height));

                _targetForm.Location = newLocation;
            }
        }

        private static void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
            }
        }
    }
}