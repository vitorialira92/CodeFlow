using System.Runtime.InteropServices;

namespace CodeFlowUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LandingPage());

        }
    }
}


