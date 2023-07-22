using Com.Wireguard.Android.Backend;
using Java.Lang;
using Kotlin.Jvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNClient.Platforms.Android.Classes
{
    public class PersistentConnectionProperties
    {
        private static PersistentConnectionProperties _Instance;
        public static PersistentConnectionProperties Instance { get => _Instance ??= new PersistentConnectionProperties() ; }

        private WgTunnel _Tunnel;

        public WgTunnel Tunnel { get => _Tunnel ??= new WgTunnel(); }
        public GoBackend Backend { get; set; }
    }
}
