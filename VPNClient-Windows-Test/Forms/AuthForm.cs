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
            await SessionManager.CreateSession(ulong.Parse(UserIdTextBox.Text));
            if (SessionManager.CurrentSession != null)
            {
                MainForm.Instance.Show();
                MainForm.Instance.UpdateSessionDisplay();
                Close();
            }
        }

        private void AuthForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
