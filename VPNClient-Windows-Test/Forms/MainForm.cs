using VPNClient_Windows_Test.Utils.SessionUtils;

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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SessionManager.SaveSession();
        }

        private async void LogoutButton_Click(object sender, EventArgs e)
        {
            await SessionManager.RemoveSession();
            if (SessionManager.CurrentSession == null)
            {
                var form = new AuthForm();
                form.Show();
                Hide();
            }
        }

        public void UpdateSessionDisplay()
        {
            if (SessionManager.CurrentSession != null)
            {
                AccessTokenTextBox.Text = SessionManager.CurrentSession.AccessToken;
                RefreshTokenTextBox.Text = SessionManager.CurrentSession.RefreshToken;
                DeviceNameTextBox.Text = SessionManager.CurrentSession.DeviceName;
                IPV4AddressTextBox.Text = SessionManager.CurrentSession.IPV4Address;
                PrivateKeyTextBox.Text = SessionManager.CurrentSession.PrivateKey;
            }
        }

        private async void RefreshTokenButton_Click(object sender, EventArgs e)
        {
            await SessionManager.RefreshAccessToken();
            UpdateSessionDisplay();
        }

        private async void ConnectPeerButton_Click(object sender, EventArgs e)
        {
            await SessionManager.ConnectPeer(HostnameTextBox.Text);
        }

        private async void RefreshRelaysButton_Click(object sender, EventArgs e)
        {
            RelaysTextBox.Text = (await SessionManager.GetRelays()).Replace("\n", "\r\n");
        }
    }
}