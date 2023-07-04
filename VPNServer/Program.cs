using System.Diagnostics;
using System.Reflection;

namespace VPNServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "bash",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            await process.StandardInput.WriteLineAsync("echo pass | sudo -S wg");

            string? output;
            while ((output = await process.StandardOutput.ReadLineAsync()) != null)
            {
                Console.WriteLine($"{output}");
            }

            Console.ReadLine();
        }
    }
}