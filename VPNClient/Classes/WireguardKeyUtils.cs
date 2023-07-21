using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace VPNClient.Classes
{
    public class WireguardKeyUtils
    {
        private static readonly X25519KeyPairGenerator keyGen = new();

        public static (string, string) GenKeyPair()
        {
            keyGen.Init(new KeyGenerationParameters(new SecureRandom(), 256));
            var keyPair = keyGen.GenerateKeyPair();

            X25519PrivateKeyParameters privateKey = (X25519PrivateKeyParameters) keyPair.Private;
            X25519PublicKeyParameters publicKey = (X25519PublicKeyParameters) keyPair.Public;

            byte[] privateKeyBytes = new byte[X25519PrivateKeyParameters.KeySize];
            byte[] publicKeyBytes = new byte[X25519PublicKeyParameters.KeySize];

            privateKey.Encode(privateKeyBytes, 0);
            publicKey.Encode(publicKeyBytes, 0);

            return (Convert.ToBase64String(privateKeyBytes), Convert.ToBase64String(publicKeyBytes));
        }
    }
}
