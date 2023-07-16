using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNClient_Windows_Test.Utils.SessionUtils.Response
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DeviceType Device { get; set; }

        public class DeviceType
        {
            public string Name { get; set; }
            public string IPV4Address { get; set; }
        }
    }
}
