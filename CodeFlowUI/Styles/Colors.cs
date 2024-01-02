
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowUI.Styles
{
    internal static class Colors
    {
        //components
        public static readonly Color CallToActionButton = ColorTranslator.FromHtml("#B1C9FC");
        public static readonly Color SecondaryButton = ColorTranslator.FromHtml("#A2FBC3");
        public static readonly Color CallToActionText = ColorTranslator.FromHtml("#1F3461");
        public static readonly Color TextBox = ColorTranslator.FromHtml("#F8FBFF");
        public static readonly Color StrokeTextBox = ColorTranslator.FromHtml("#D1D0D0");
        public static readonly Color StrokeContainer = ColorTranslator.FromHtml("#B5AFAF");
        public static readonly Color DarkBlue = ColorTranslator.FromHtml("#21005D");

        public static readonly Color ErrorColor = ColorTranslator.FromHtml("#FF0000");

        //project status
        public static readonly Color OpenProject = ColorTranslator.FromHtml("#FAFF00");
        public static readonly Color OngoingProject = ColorTranslator.FromHtml("#05FF00");
        public static readonly Color LateProject = ColorTranslator.FromHtml("#FF0000");
        public static readonly Color CanceledProject = ColorTranslator.FromHtml("#FF7A00");
        public static readonly Color DoneProject = ColorTranslator.FromHtml("#026C00");
        public static readonly Color ProjectCardBackgroundColor = ColorTranslator.FromHtml("#F6FBFF");

        //tasks status
        public static readonly Color TodoTask = ColorTranslator.FromHtml("#FAFF00");
        public static readonly Color InProgressTask = ColorTranslator.FromHtml("#05FF00");
        public static readonly Color ReviewTask = ColorTranslator.FromHtml("#FF8A00");
        public static readonly Color DoneTask = ColorTranslator.FromHtml("#026B00");
    }
}
