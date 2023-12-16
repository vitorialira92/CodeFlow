using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowUI.Styles
{
    internal class LightBlueTextBox : TextBox
    {
        private string hintText;
        internal LightBlueTextBox(string hintText, int width, int height)
        {
            this.Size = new Size(width, height);
            this.hintText = hintText;
            this.Text = hintText;
            SetColors();

            this.Enter += new EventHandler(TextBoxEnter);
            this.Leave += new EventHandler(TextBoxLeave);
        }
        private void SetColors()
        {
            this.BackColor = Colors.TextBox;
            this.ForeColor = Colors.CallToActionText;
            this.Font = new Font("Codec Warm Trial", 16);

        }
        private void TextBoxEnter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == this.hintText)
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBoxLeave(object sender, EventArgs e)
        {
            LightBlueTextBox textBox = sender as LightBlueTextBox;
            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text.Equals(hintText))
            {
                textBox.Text = this.hintText;
            }
        }
    }
}
