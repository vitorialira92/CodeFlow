using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFlowUI.Styles;

namespace CodeFlowUI.Components
{

    public class RoundedTextBox : UserControl
    {
        private string hintText;
        public TextBox TextBox { get; private set; }

        public RoundedTextBox(string hintText, int width, int height)
        {
            this.hintText = hintText;
            TextBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = new Point(16, 16),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Width = width - 32,
                Height = height,
                Text = hintText,
                BackColor = Colors.TextBox,
                ForeColor = Colors.CallToActionText,
                Font = new Font("Ubuntu", 16)
            };

            TextBox.Enter += new EventHandler(TextBox_Enter);
            TextBox.Leave += new EventHandler(TextBox_Leave);

            Size = new Size(width, height);
            Padding = new Padding(10);
            BackColor = Color.White;
            Controls.Add(TextBox);
        }
        internal string GetText()
        {
            return TextBox.Text;
        }

        protected void TextBox_Enter(object sender, EventArgs e)
        {

            TextBox textBox = sender as TextBox;
            if (textBox.Text == hintText)
            {
                textBox.Text = string.Empty;
            }
        }

        protected void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text.Equals(hintText))
            {
                textBox.Text = hintText;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                var radius = 15;
                path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
                path.AddArc(new Rectangle(Width - radius - 1, 0, radius, radius), -90, 90);
                path.AddArc(new Rectangle(Width - radius - 1, Height - radius - 1, radius, radius), 0, 90);
                path.AddArc(new Rectangle(0, Height - radius - 1, radius, radius), 90, 90);
                path.CloseAllFigures();

                e.Graphics.FillPath(new SolidBrush(Colors.TextBox), path);

                e.Graphics.DrawPath(new Pen(Colors.StrokeTextBox, 1), path);
            }
        }
    }

}
