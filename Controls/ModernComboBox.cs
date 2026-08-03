using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

namespace GymApplicationV2._0.Controls
{
    public class ModernComboBox : ComboBox
    {
        public Color BorderColor { get; set; } = Color.Gray;
        public Color ArrowColor { get; set; } = Color.Black;
        public int BorderRadius { get; set; } = 6;

        public ModernComboBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.DoubleBuffer |
                    ControlStyles.ResizeRedraw, true);

            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Фон
            using (var backBrush = new SolidBrush(BackColor))
            using (var borderPen = new Pen(BorderColor, 2))
            using (var path = GetRoundRectPath(ClientRectangle, BorderRadius))
            {
                graphics.FillPath(backBrush, path);
                graphics.DrawPath(borderPen, path);
            }

            // Текст
            if (SelectedItem != null)
            {
                TextRenderer.DrawText(graphics, SelectedItem.ToString(), Font,
                    new Rectangle(10, 0, Width - 30, Height), ForeColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }

            // Стрелка
            DrawArrow(graphics);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            // Выделенный элемент
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                using (var selectionBrush = new SolidBrush(Color.FromArgb(0, 122, 204)))
                {
                    e.Graphics.FillRectangle(selectionBrush, e.Bounds);
                }
            }

            // Текст элемента
            var itemText = Items[e.Index].ToString();
            TextRenderer.DrawText(e.Graphics, itemText, Font, e.Bounds,
                (e.State & DrawItemState.Selected) == DrawItemState.Selected ? Color.White : ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            e.DrawFocusRectangle();
        }

        private void DrawArrow(Graphics graphics)
        {
            var arrowX = Width - 20;
            var arrowY = Height / 2 - 2;

            var points = new Point[]
            {
                new Point(arrowX, arrowY),
                new Point(arrowX + 7, arrowY),
                new Point(arrowX + 3, arrowY + 4)
            };

            using (var arrowBrush = new SolidBrush(ArrowColor))
            {
                graphics.FillPolygon(arrowBrush, points);
            }
        }

        private GraphicsPath GetRoundRectPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
