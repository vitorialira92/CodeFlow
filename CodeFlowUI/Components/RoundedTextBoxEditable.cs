using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowUI.Components
{
    internal class RoundedTextBoxEditable : RoundedTextBox
    {
        public RoundedTextBoxEditable(string hintText, int width, int height)
                : base(hintText, width, height)
        {
            TextBox.Enter -= base.TextBox_Enter; 
            TextBox.Leave -= base.TextBox_Leave; 

        }
    }
}
