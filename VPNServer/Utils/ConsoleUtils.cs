using System.Diagnostics;

namespace VPNServer.Utils
{
    public static class ConsoleUtils
    {
        public static async Task<IEnumerable<string>> ExecuteCommand(string command)
        {
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
            await process.StandardInput.WriteLineAsync($"{(ProgramUtils.IsDebug ? $"echo {ProgramUtils.DebugSudoPassword} | sudo -S " : string.Empty)}{command}");

            List<string> resultList = new();
            string? line;
            while (process.StandardOutput.Peek() > -1)
            {
                line = await process.StandardOutput.ReadLineAsync();
                if(line != null)
                    resultList.Add(line);
            }

            return resultList;
        }
    }
}
