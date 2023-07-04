using System.Diagnostics;
using System.Reflection;
using VPNServer.Utils;

namespace VPNServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            do
            {
                IEnumerable<string> result = await ConsoleUtils.ExecuteCommand("wg");
                foreach (string s in result)
                    Console.WriteLine(s);
                Thread.Sleep(1000);
            } while (true);

            Console.ReadLine();
        }
    }
}