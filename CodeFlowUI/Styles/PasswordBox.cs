using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowUI.Styles
{
    internal class PasswordBox : TextBox
    {
        private string hintText;
        internal PasswordBox(string hintText, int width, int height)
        {
            this.Size = new Size(width, height);
            this.UseSystemPasswordChar = false;
            this.hintText = hintText;
            this.Text = hintText;
            SetColors();
            this.Font = new Font("Codec Warm Trial", 16);

            this.Enter += new EventHandler(PasswordEnter);
            this.Leave += new EventHandler(PasswordLeave);
        }

        private void SetColors()
        {
            this.BackColor = Colors.TextBox;
            this.ForeColor = Colors.CallToActionText;
            
        }

        private void PasswordEnter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == this.hintText)
            {
                this.PasswordChar = '*';
                textBox.Text = string.Empty;
                textBox.UseSystemPasswordChar = true;
            }
        }

        private void PasswordLeave(object sender, EventArgs e)
        {
            PasswordBox textBox = sender as PasswordBox;
            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text.Equals(hintText))
            {
                Console.WriteLine("entrou no primeiro if ");
                
                textBox.Text = this.hintText;
                textBox.PasswordChar = '\0';
                textBox.UseSystemPasswordChar = false;
            }
        }
    }
}
