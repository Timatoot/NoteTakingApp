using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTakingApp
{   
    class NewRoundButton : Button
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int borderRadius = 20;
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            using (System.Drawing.Pen pen = new System.Drawing.Pen(ForeColor, 1.5f))
            {
                ControlPaint.DrawBorder(e.Graphics, rect, pen.Color, ButtonBorderStyle.Solid);
            }

            using (GraphicsPath path = CreateRoundRectangle(rect, borderRadius))
            {
                Region = new Region(path);
            }
        }

        private GraphicsPath CreateRoundRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
