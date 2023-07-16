namespace VPNClient_Windows_Test.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SessionGroupBox = new GroupBox();
            PrivateKeyLabel = new Label();
            PrivateKeyTextBox = new TextBox();
            IPV4AddressLabel = new Label();
            IPV4AddressTextBox = new TextBox();
            DeviceNameLabel = new Label();
            DeviceNameTextBox = new TextBox();
            RefreshTokenLabel = new Label();
            RefreshTokenTextBox = new TextBox();
            AccessTokenLabel = new Label();
            AccessTokenTextBox = new TextBox();
            LogoutButton = new Button();
            RefreshTokenButton = new Button();
            ConnectPeerButton = new Button();
            RelaysTextBox = new TextBox();
            RefreshRelaysButton = new Button();
            HostnameTextBox = new TextBox();
            PaidUntilTextBox = new TextBox();
            PaidUntilButton = new Button();
            SessionGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // SessionGroupBox
            // 
            SessionGroupBox.Controls.Add(PrivateKeyLabel);
            SessionGroupBox.Controls.Add(PrivateKeyTextBox);
            SessionGroupBox.Controls.Add(IPV4AddressLabel);
            SessionGroupBox.Controls.Add(IPV4AddressTextBox);
            SessionGroupBox.Controls.Add(DeviceNameLabel);
            SessionGroupBox.Controls.Add(DeviceNameTextBox);
            SessionGroupBox.Controls.Add(RefreshTokenLabel);
            SessionGroupBox.Controls.Add(RefreshTokenTextBox);
            SessionGroupBox.Controls.Add(AccessTokenLabel);
            SessionGroupBox.Controls.Add(AccessTokenTextBox);
            SessionGroupBox.Location = new Point(12, 12);
            SessionGroupBox.Name = "SessionGroupBox";
            SessionGroupBox.Size = new Size(776, 286);
            SessionGroupBox.TabIndex = 0;
            SessionGroupBox.TabStop = false;
            SessionGroupBox.Text = "Session";
            // 
            // PrivateKeyLabel
            // 
            PrivateKeyLabel.AutoSize = true;
            PrivateKeyLabel.Location = new Point(5, 232);
            PrivateKeyLabel.Name = "PrivateKeyLabel";
            PrivateKeyLabel.Size = new Size(80, 20);
            PrivateKeyLabel.TabIndex = 9;
            PrivateKeyLabel.Text = "Private key";
            // 
            // PrivateKeyTextBox
            // 
            PrivateKeyTextBox.Location = new Point(5, 255);
            PrivateKeyTextBox.Name = "PrivateKeyTextBox";
            PrivateKeyTextBox.ReadOnly = true;
            PrivateKeyTextBox.Size = new Size(765, 27);
            PrivateKeyTextBox.TabIndex = 8;
            // 
            // IPV4AddressLabel
            // 
            IPV4AddressLabel.AutoSize = true;
            IPV4AddressLabel.Location = new Point(5, 179);
            IPV4AddressLabel.Name = "IPV4AddressLabel";
            IPV4AddressLabel.Size = new Size(93, 20);
            IPV4AddressLabel.TabIndex = 7;
            IPV4AddressLabel.Text = "IPV4 address";
            // 
            // IPV4AddressTextBox
            // 
            IPV4AddressTextBox.Location = new Point(5, 202);
            IPV4AddressTextBox.Name = "IPV4AddressTextBox";
            IPV4AddressTextBox.ReadOnly = true;
            IPV4AddressTextBox.Size = new Size(765, 27);
            IPV4AddressTextBox.TabIndex = 6;
            // 
            // DeviceNameLabel
            // 
            DeviceNameLabel.AutoSize = true;
            DeviceNameLabel.Location = new Point(6, 126);
            DeviceNameLabel.Name = "DeviceNameLabel";
            DeviceNameLabel.Size = new Size(95, 20);
            DeviceNameLabel.TabIndex = 5;
            DeviceNameLabel.Text = "Device name";
            // 
            // DeviceNameTextBox
            // 
            DeviceNameTextBox.Location = new Point(6, 149);
            DeviceNameTextBox.Name = "DeviceNameTextBox";
            DeviceNameTextBox.ReadOnly = true;
            DeviceNameTextBox.Size = new Size(764, 27);
            DeviceNameTextBox.TabIndex = 4;
            // 
            // RefreshTokenLabel
            // 
            RefreshTokenLabel.AutoSize = true;
            RefreshTokenLabel.Location = new Point(6, 73);
            RefreshTokenLabel.Name = "RefreshTokenLabel";
            RefreshTokenLabel.Size = new Size(99, 20);
            RefreshTokenLabel.TabIndex = 3;
            RefreshTokenLabel.Text = "Refresh token";
            // 
            // RefreshTokenTextBox
            // 
            RefreshTokenTextBox.Location = new Point(6, 96);
            RefreshTokenTextBox.Name = "RefreshTokenTextBox";
            RefreshTokenTextBox.ReadOnly = true;
            RefreshTokenTextBox.Size = new Size(764, 27);
            RefreshTokenTextBox.TabIndex = 2;
            // 
            // AccessTokenLabel
            // 
            AccessTokenLabel.AutoSize = true;
            AccessTokenLabel.Location = new Point(6, 20);
            AccessTokenLabel.Name = "AccessTokenLabel";
            AccessTokenLabel.Size = new Size(94, 20);
            AccessTokenLabel.TabIndex = 1;
            AccessTokenLabel.Text = "Access token";
            // 
            // AccessTokenTextBox
            // 
            AccessTokenTextBox.Location = new Point(6, 43);
            AccessTokenTextBox.Name = "AccessTokenTextBox";
            AccessTokenTextBox.ReadOnly = true;
            AccessTokenTextBox.Size = new Size(764, 27);
            AccessTokenTextBox.TabIndex = 0;
            // 
            // LogoutButton
            // 
            LogoutButton.Location = new Point(12, 304);
            LogoutButton.Name = "LogoutButton";
            LogoutButton.Size = new Size(94, 29);
            LogoutButton.TabIndex = 1;
            LogoutButton.Text = "Logout";
            LogoutButton.UseVisualStyleBackColor = true;
            LogoutButton.Click += LogoutButton_Click;
            // 
            // RefreshTokenButton
            // 
            RefreshTokenButton.Location = new Point(112, 304);
            RefreshTokenButton.Name = "RefreshTokenButton";
            RefreshTokenButton.Size = new Size(109, 29);
            RefreshTokenButton.TabIndex = 2;
            RefreshTokenButton.Text = "Refresh token";
            RefreshTokenButton.UseVisualStyleBackColor = true;
            RefreshTokenButton.Click += RefreshTokenButton_Click;
            // 
            // ConnectPeerButton
            // 
            ConnectPeerButton.Location = new Point(112, 370);
            ConnectPeerButton.Name = "ConnectPeerButton";
            ConnectPeerButton.Size = new Size(106, 29);
            ConnectPeerButton.TabIndex = 3;
            ConnectPeerButton.Text = "Connect peer";
            ConnectPeerButton.UseVisualStyleBackColor = true;
            ConnectPeerButton.Click += ConnectPeerButton_Click;
            // 
            // RelaysTextBox
            // 
            RelaysTextBox.Location = new Point(482, 305);
            RelaysTextBox.Multiline = true;
            RelaysTextBox.Name = "RelaysTextBox";
            RelaysTextBox.ReadOnly = true;
            RelaysTextBox.Size = new Size(306, 425);
            RelaysTextBox.TabIndex = 4;
            // 
            // RefreshRelaysButton
            // 
            RefreshRelaysButton.Location = new Point(382, 698);
            RefreshRelaysButton.Name = "RefreshRelaysButton";
            RefreshRelaysButton.Size = new Size(94, 29);
            RefreshRelaysButton.TabIndex = 5;
            RefreshRelaysButton.Text = "Refresh";
            RefreshRelaysButton.UseVisualStyleBackColor = true;
            RefreshRelaysButton.Click += RefreshRelaysButton_Click;
            // 
            // HostnameTextBox
            // 
            HostnameTextBox.Location = new Point(12, 370);
            HostnameTextBox.Name = "HostnameTextBox";
            HostnameTextBox.Size = new Size(94, 27);
            HostnameTextBox.TabIndex = 6;
            // 
            // PaidUntilTextBox
            // 
            PaidUntilTextBox.Location = new Point(12, 405);
            PaidUntilTextBox.Name = "PaidUntilTextBox";
            PaidUntilTextBox.ReadOnly = true;
            PaidUntilTextBox.Size = new Size(206, 27);
            PaidUntilTextBox.TabIndex = 7;
            // 
            // PaidUntilButton
            // 
            PaidUntilButton.Location = new Point(224, 405);
            PaidUntilButton.Name = "PaidUntilButton";
            PaidUntilButton.Size = new Size(106, 29);
            PaidUntilButton.TabIndex = 8;
            PaidUntilButton.Text = "Refresh";
            PaidUntilButton.UseVisualStyleBackColor = true;
            PaidUntilButton.Click += PaidUntilButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 739);
            Controls.Add(PaidUntilButton);
            Controls.Add(PaidUntilTextBox);
            Controls.Add(HostnameTextBox);
            Controls.Add(RefreshRelaysButton);
            Controls.Add(RelaysTextBox);
            Controls.Add(ConnectPeerButton);
            Controls.Add(RefreshTokenButton);
            Controls.Add(LogoutButton);
            Controls.Add(SessionGroupBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "MainForm";
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
            SessionGroupBox.ResumeLayout(false);
            SessionGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox SessionGroupBox;
        private Label AccessTokenLabel;
        private TextBox AccessTokenTextBox;
        private Label RefreshTokenLabel;
        private TextBox RefreshTokenTextBox;
        private Label DeviceNameLabel;
        private TextBox DeviceNameTextBox;
        private Label IPV4AddressLabel;
        private TextBox IPV4AddressTextBox;
        private Label PrivateKeyLabel;
        private TextBox PrivateKeyTextBox;
        private Button LogoutButton;
        private Button RefreshTokenButton;
        private Button ConnectPeerButton;
        private TextBox RelaysTextBox;
        private Button RefreshRelaysButton;
        private TextBox HostnameTextBox;
        private TextBox PaidUntilTextBox;
        private Button PaidUntilButton;
    }
}