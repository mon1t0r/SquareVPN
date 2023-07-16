using Geralt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VPNClient_Windows_Test.Utils
{
    internal class WireguardKeyUtils
    {
        public static (string, string) GenKeyPair()
        {
            Span<byte> publicKey = stackalloc byte[X25519.PublicKeySize], privateKey = stackalloc byte[X25519.PrivateKeySize];
            X25519.GenerateKeyPair(publicKey, privateKey);
            return (Convert.ToBase64String(privateKey), Convert.ToBase64String(publicKey));
        }
    }
}
