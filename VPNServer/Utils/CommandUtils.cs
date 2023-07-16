using System.Diagnostics;
using VPNServer.Classes;

namespace VPNServer.Utils
{
    public static class CommandUtils
    {
        private static async Task<Process> StartBashProcess(string command)
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

            return process;
        }

        public static async Task ExecuteCommand(string command) => await StartBashProcess(command);

        public static async Task<IEnumerable<string>> ExecuteCommandWithOutput(string command)
        {
            Process process = await StartBashProcess(command);

            List<string> resultList = new();
            string? line;
            while (process.StandardOutput.Peek() > -1)
            {
                line = await process.StandardOutput.ReadLineAsync();
                if (line != null)
                    resultList.Add(line);
            }

            return resultList;
        }

        public static async Task AddPeer(Peer peer)
        {
            await ExecuteCommand($"wg set wg0 peer \"{Convert.ToBase64String(peer.PublicKey)}\" allowed-ips {peer.IPV4Address}/32");
            await ExecuteCommand($"ip -4 route add {peer.IPV4Address}/32 dev wg0");
        }

        public static async Task RemovePeer(Peer peer)
        {
            await ExecuteCommand($"wg set wg0 peer \"{Convert.ToBase64String(peer.PublicKey)}\" remove");
            await ExecuteCommand($"ip -4 route delete {peer.IPV4Address}/32 dev wg0");
        }
    }
}
