using VPNClient_Windows_Test.Utils;

namespace VPNClient_Windows_Test.Forms
{
    public partial class MainForm : Form
    {
        public static MainForm Instance { get; private set; }

        public MainForm()
        {
            Instance = this;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (SessionManager.CurrentSession == null)
            {
                var form = new AuthForm();
                form.ShowDialog();
                return;
            }
            UpdateSessionDisplay();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) =>
            SessionManager.SaveSession();

        private async void LogoutButton_Click(object sender, EventArgs e)
        {
            if(SessionManager.CurrentSession != null)
                await SessionManager.CurrentSession.Logout();
            SessionManager.CurrentSession = null;

            var form = new AuthForm();
            form.Show();
            Hide();
        }

        public void UpdateSessionDisplay()
        {
            var session = SessionManager.CurrentSession;
            if (session != null)
            {
                if(session.TokenPair != null)
                {
                    AccessTokenTextBox.Text = session.TokenPair.AccessToken;
                    RefreshTokenTextBox.Text = session.TokenPair.RefreshToken;
                } 
                if(session.Device != null)
                {
                    DeviceNameTextBox.Text = session.Device.Name;
                    IPV4AddressTextBox.Text = session.Device.IPV4Address;
                }
                PrivateKeyTextBox.Text = session.PrivateKey;
            }
        }

        private async void RefreshTokenButton_Click(object sender, EventArgs e)
        {
            if (SessionManager.CurrentSession != null)
                await SessionManager.CurrentSession.RefreshAccessToken();
            UpdateSessionDisplay();
        }

        private async void ConnectPeerButton_Click(object sender, EventArgs e)
        {
            if (SessionManager.CurrentSession != null)
                await SessionManager.CurrentSession.ConnectPeer(HostnameTextBox.Text);
        }

        private async void RefreshRelaysButton_Click(object sender, EventArgs e)
        {
            /*if (SessionManager.CurrentSession != null)
                RelaysTextBox.Text = (await SessionManager.CurrentSession.GetRelays())?.Replace("\n", "\r\n");*/
        }

        private async void PaidUntilButton_Click(object sender, EventArgs e)
        {
            if (SessionManager.CurrentSession != null)
                PaidUntilTextBox.Text = (await SessionManager.CurrentSession.GetPaidUntil())?.ToLocalTime().ToString();
        }
    }
}