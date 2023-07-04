using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNServer.Utils
{
    public static class ProgramUtils
    {
#if DEBUG
        public const bool IsDebug = true;
        public const string DebugSudoPassword = "1";
#else
        public const bool IsDebug = false;
        public const string DebugSudoPassword = "";
#endif
    }
}
