using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowUI.Styles
{
    internal class GradientButton : Button
    {
        public GradientButton(string text, int width, int height)
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(width, height);
            this.BackColor = Color.Transparent; 
            this.Text = text;
            this.ForeColor = Colors.CallToActionText;
           
            this.Font = new Font("Codec Cold Trial", 16, FontStyle.Regular);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            // Configurações para desenho com qualidade
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            pevent.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            GraphicsPath path = new GraphicsPath();
            int radius = 32;
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(0xE3, 0xFF, 0xEE), Color.FromArgb(0xD2, 0xE1, 0xFF), LinearGradientMode.Vertical);

            pevent.Graphics.FillPath(brush, path);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            pevent.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), rect, sf);

            SolidBrush textBrush = new SolidBrush(this.ForeColor);


            path.Dispose();
            brush.Dispose();
        }
    }
}
