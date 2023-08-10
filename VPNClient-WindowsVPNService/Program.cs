using System.Diagnostics;
using System.Runtime.InteropServices;
using Tunnel;

namespace VPNClient_WindowsVPNService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 3 && args[0] == "/service")
            {
                File.WriteAllText("C:\\Users\\mon1tor\\Downloads\\wg\\realargs", args[1]);
                var t = new Thread(() =>
                {
                    try
                    {
                        var currentProcess = Process.GetCurrentProcess();
                        var uiProcess = Process.GetProcessById(int.Parse(args[2]));
                        uiProcess.WaitForExit();
                    }
                    catch { }
                    Service.Remove(args[1], false);
                });
                t.Start();
                bool res = Service.Run(args[1]);
                t.Interrupt();
                File.WriteAllText("C:\\Users\\mon1tor\\Downloads\\wg\\exit", res.ToString() + " " + Marshal.GetLastPInvokeErrorMessage());
            }
        }
    }
}