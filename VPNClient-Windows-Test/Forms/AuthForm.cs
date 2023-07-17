using VPNClient_Windows_Test.Utils;
using VPNClient_Windows_Test.Utils.SessionUtils;

namespace VPNClient_Windows_Test.Forms
{
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            var session = new Session();
            bool result = await session.Login(ulong.Parse(UserIdTextBox.Text), WireguardKeyUtils.GenKeyPair(), (message) => RemoveDevicesListTextBox.Text = message, RemoveDeviceTextBox.Text.Replace("\n", "\r\n"));

            if (result)
            {
                SessionManager.CurrentSession = session;
                MainForm.Instance.Show();
                MainForm.Instance.UpdateSessionDisplay();
                Close();
            }
        }

        private void AuthForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!MainForm.Instance.Visible)
                MainForm.Instance.Show();
        }
    }
}
