using API;
using VPNClient_Windows_Test.Utils;

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
            /*var session = new APISession("https://localhost:44317/");
            bool result = await session.Login(ulong.Parse(UserIdTextBox.Text), WireguardKeyUtils.GenKeyPair(), (message) => RemoveDevicesListTextBox.Text = message.Replace("\\n", "\r\n").Replace("\\", ""), RemoveDeviceTextBox.Text);

            if (result)
            {
                SessionManager.CurrentSession = session;
                MainForm.Instance.Show();
                MainForm.Instance.UpdateSessionDisplay();
                Close();
            }*/
        }

        private void AuthForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!MainForm.Instance.Visible)
                MainForm.Instance.Show();
        }
    }
}
