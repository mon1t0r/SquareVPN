namespace VPNClient_Windows_Test.Forms
{
    partial class AuthForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            UserIdTextBox = new TextBox();
            AuthLabel = new Label();
            LoginButton = new Button();
            RemoveDevicesListTextBox = new TextBox();
            RemoveDeviceTextBox = new TextBox();
            SuspendLayout();
            // 
            // UserIdTextBox
            // 
            UserIdTextBox.Location = new Point(12, 87);
            UserIdTextBox.MaxLength = 16;
            UserIdTextBox.Name = "UserIdTextBox";
            UserIdTextBox.Size = new Size(367, 27);
            UserIdTextBox.TabIndex = 0;
            // 
            // AuthLabel
            // 
            AuthLabel.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            AuthLabel.Location = new Point(12, 9);
            AuthLabel.Name = "AuthLabel";
            AuthLabel.Size = new Size(367, 75);
            AuthLabel.TabIndex = 1;
            AuthLabel.Text = "Auth";
            AuthLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LoginButton
            // 
            LoginButton.Location = new Point(12, 120);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(367, 29);
            LoginButton.TabIndex = 2;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += LoginButton_Click;
            // 
            // RemoveDevicesListTextBox
            // 
            RemoveDevicesListTextBox.Location = new Point(12, 155);
            RemoveDevicesListTextBox.Multiline = true;
            RemoveDevicesListTextBox.Name = "RemoveDevicesListTextBox";
            RemoveDevicesListTextBox.ReadOnly = true;
            RemoveDevicesListTextBox.Size = new Size(367, 272);
            RemoveDevicesListTextBox.TabIndex = 3;
            // 
            // RemoveDeviceTextBox
            // 
            RemoveDeviceTextBox.Location = new Point(12, 433);
            RemoveDeviceTextBox.Name = "RemoveDeviceTextBox";
            RemoveDeviceTextBox.Size = new Size(367, 27);
            RemoveDeviceTextBox.TabIndex = 4;
            // 
            // AuthForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(391, 473);
            Controls.Add(RemoveDeviceTextBox);
            Controls.Add(RemoveDevicesListTextBox);
            Controls.Add(LoginButton);
            Controls.Add(AuthLabel);
            Controls.Add(UserIdTextBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "AuthForm";
            Text = "AuthForm";
            FormClosed += AuthForm_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox UserIdTextBox;
        private Label AuthLabel;
        private Button LoginButton;
        private TextBox RemoveDevicesListTextBox;
        private TextBox RemoveDeviceTextBox;
    }
}