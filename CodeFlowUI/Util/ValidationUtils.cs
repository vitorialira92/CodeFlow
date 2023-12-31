using CodeFlowBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CodeFlowUI.Util
{
    internal static class ValidationUtils
    {
        internal static bool IsInputAValidName(string input, string? hintText, bool? allowSpaces)
        {
            bool isValid = input.Length > 1;

            if (hintText != null) isValid = isValid && !input.Equals(hintText);

            if(allowSpaces == false) isValid = isValid && !input.Contains(" "); 

            return isValid;
        }

        internal static bool IsEmailValid(string input)
        {
            string regex = @"^[a-zA-Z0-9]+[a-zA-Z0-9_.-]*@[a-zA-Z]+\.[a-zA-Z]+(\.[a-zA-Z]+)?$";

            return Regex.IsMatch(input, regex, RegexOptions.IgnoreCase);
        }
        internal static bool IsUsernameValid(string input)
        {
            return UserService.IsUsernameAvailable(input) && IsInputAValidName(input, "Username", false);
        }

        internal static bool IsPasswordValid(string input)
        {
            return Regex.IsMatch(input, @"(?=.*[a-zA-Z])(?=.*[0-9])") && !input.Contains(" ") && input.Length > 7;
        }
    }
}
