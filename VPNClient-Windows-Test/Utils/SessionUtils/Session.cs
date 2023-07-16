using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNClient_Windows_Test.Utils.SessionUtils
{
    public class Session
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string DeviceName { get; set; }
        public string IPV4Address { get; set; }
        public string PrivateKey { get; set; }
    }
}
