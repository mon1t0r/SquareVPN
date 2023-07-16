using VPNClient_Windows_Test.Forms;
using VPNClient_Windows_Test.Utils.SessionUtils;

namespace VPNClient_Windows_Test
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            SessionManager.LoadSession();
            Application.Run(new MainForm());
        }
    }
}