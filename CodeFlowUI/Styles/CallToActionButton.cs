using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowUI.Styles
{
    internal class CallToActionButton : Button
    {
        public CallToActionButton(string text, int width, int height)
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(width, height);
            this.BackColor = Colors.CallToActionButton;
            this.Text = text;
            this.ForeColor = Colors.CallToActionText;
           

            this.Font = new Font("Codec Cold Trial", 16, FontStyle.Regular);
        }

    }
}
