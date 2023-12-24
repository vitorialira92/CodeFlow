using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeFlowUI.Styles;

namespace CodeFlowUI.Components
{
    public class ProjectContainer : Panel
    {
        public ProjectContainer()
        {
            Width = 1216;
            Height = 608;
            BackColor = Color.Transparent;
            BorderStyle = BorderStyle.None;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var path = new GraphicsPath())
            {
                var radius = 12;
                path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
                path.AddArc(new Rectangle(Width - radius - 1, 0, radius, radius), -90, 90);
                path.AddArc(new Rectangle(Width - radius - 1, Height - radius - 1, radius, radius), 0, 90);
                path.AddArc(new Rectangle(0, Height - radius - 1, radius, radius), 90, 90);
                path.CloseAllFigures();

                e.Graphics.FillPath(new SolidBrush(Color.Transparent), path);

                e.Graphics.DrawPath(new Pen(Colors.StrokeContainer, 1), path);
            }
        }
    }
}
