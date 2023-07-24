using Newtonsoft.Json;
using API;
using API.Utils;
using VPNClient.Pages;
using API.Responses.Models.Relays;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace VPNClient.Classes
{
    public class SessionManager
    {
        private const string SessionKey = "session";
        private const string SessionEndpoint = "https://91.219.63.100:44317/";
        private const string CertificatePublicKey = "3082020A0282020100D49A6C2DE1BE2BBA92F2E381F28144C358B7D8ACACFF84564EE8D43DC53050B632418A2929D1D1589B756BE55DFDEFD3C759B1D66DD492DB0E48976D7542F42194C5F6FE127A1B2C4BBC619AE51F530C892DBAEE94629ED804E518E8DD13FF326BD82F49D2E2F5535C4B4729D2ED00D859668744AF1DF41DC4B8B7FF985BB6D3BF0EEC71F7CB19333FAC56B7D1BDBF01BA555E8568CD0B177A7F6B023D43E9D1E698E105382379EAC6763934967402B028508B0BF0DA205ACFB58331EC7D6F15735F6D5EB568E0002FD09443CF7497F3B5112019784FA07162583284B0F25BEFFE2D3AA094D19A57EDB94D3F2C30D56C024961780A3488A0EFB85F6A4457A42FF2ED621CD4D60AC105B53BF86009C68A5DB531CEEF4DD662471427D0F719923B93D3ECC70A89620A9F54217C191AFEE108C2BE3C066E77F300505F0182C1D2E481C479C0D8D74F484BE377C7926D5E75E428F3E545CCFE0BA73A8B2FDD77B7911A1A947270139544D550BA39FE2E170ECFA723146A965A2972568A0E6732EECDACE77CB827566ED8E0FC9BC7C24EB3526AA2BFED3C58D608F7CE0FAC9992A4658650FF3030904D577D5E85CDDA0A6FD8D0FD47F397DF5E870E151D1336DA6BBF76BC094FE77E581F2F249A63019330CCF75B83C49818AAEF89C126EE0FF974CF6D5C6E998A43813D8FE1FC72CEDDEB0E3821B36EDB1EDDBDCDD2FB4DD03FDD1B0203010001";

        public static APISession CurrentSession { get; private set; }

        public static DateTime? PaidUntil { get; private set; }
        public static List<APICountry> Countries { get; private set; }

        public static void Initialize()
        {
            APISession.ServerCertificateCustomValidationCallback = OnCertificateValidation;
        }

        public static async Task InitializeSession()
        {
            await LoadSessionAsync();
            RegisterSessionEvents();
            await UpdatePaidUntilAsync();
            await UpdateCountriesAsync();
        }

        public static bool OnCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate == null)
                return false;

            string publicKey = certificate.GetPublicKeyString();
            if (publicKey.Equals(CertificatePublicKey))
                return true;

            return false;
        }

        private static async Task CurrentSession_OnDataUpdated() =>
            await SaveSessionAsync();

        private static async Task CurrentSession_OnLogin()
        {
            await UpdatePaidUntilAsync();
            await UpdateCountriesAsync();
        }

        private static Task CurrentSession_OnLogout()
        {
            if (Application.Current != null)
                Application.Current.MainPage = new LoginPage();
            PaidUntil = null;
            Countries = null;

            return Task.CompletedTask;
        }

        private static Task CurrentSession_OnRequestException(Exception ex)
        {
            if (Application.Current == null || Application.Current.MainPage == null || Application.Current.MainPage is NoInternetConnectionPage)
                throw ex;

            Application.Current.MainPage = new NoInternetConnectionPage();

            return Task.CompletedTask;
        }

        private static async Task UpdatePaidUntilAsync()
        {
            if (CurrentSession.IsActive)
            {
                PaidUntil = await CurrentSession.GetPaidUntil();
                if (PaidUntil != null)
                    PaidUntil = PaidUntil.Value.ToLocalTime();
            }
        }

        private static async Task UpdateCountriesAsync()
        {
            if (CurrentSession.IsActive)
                Countries = await CurrentSession.GetRelays();
        }

        private static async Task SaveSessionAsync()
        {
            if (CurrentSession.IsActive)
                await SecureStorage.Default.SetAsync(SessionKey, JsonConvert.SerializeObject(CurrentSession));
            else
                SecureStorage.Default.Remove(SessionKey);
        }

        private static async Task LoadSessionAsync()
        {
            var value = await SecureStorage.Default.GetAsync(SessionKey);
            if(value != null)
                CurrentSession = JsonConvert.DeserializeObject<APISession>(value);
            else
                CurrentSession = new APISession();

            CurrentSession.APIEndpoints = new APIEndpoints(SessionEndpoint);
        }

        private static void RegisterSessionEvents()
        {
            CurrentSession.OnDataUpdated = CurrentSession_OnDataUpdated;
            CurrentSession.OnLogin = CurrentSession_OnLogin;
            CurrentSession.OnLogout = CurrentSession_OnLogout;
            CurrentSession.OnRequestException = CurrentSession_OnRequestException;
        }
    }
}
