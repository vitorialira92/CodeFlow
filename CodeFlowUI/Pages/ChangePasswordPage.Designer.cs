

using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using CodeFlowUI.Util;
using System.DirectoryServices.ActiveDirectory;
using System.Xml;

namespace CodeFlowUI.Pages
{
    partial class ChangePasswordPage
    {

        private System.ComponentModel.IContainer components = null;
        private long userId;

        private Label newPasswordLabel;
        private Label repeatNewPasswordLabel;
        private Label currentPasswordLabel;

        private PasswordTextBox newPasswordTextBox;
        private PasswordTextBox repeatNewPasswordTextBox;
        private PasswordTextBox currentPasswordTextBox;

        private Button homepageButton;
        private RoundedButton changePasswordButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent(long userId)
        {
            this.userId = userId;
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 832);
            this.Text = "CodeFlow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            FormClosed += ChangePassword_FormClosed;

            InitScreen();
        }

        private void ChangePassword_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void InitScreen()
        {
            InitLogo();
            InitLabels();
            InitTextBoxes();
            InitButtons();
        }

        private void InitButtons()
        {
            this.homepageButton = new System.Windows.Forms.Button();
            this.homepageButton.Image = Image.FromFile(@"Resources\back-to-homepage.png");
            this.homepageButton.Location = new Point(156, 77);
            this.homepageButton.Size = new Size(154, 24);
            this.homepageButton.BackColor = Color.Transparent;
            this.homepageButton.FlatAppearance.BorderSize = 0;
            this.homepageButton.FlatStyle = FlatStyle.Flat;
            this.homepageButton.Cursor = Cursors.Hand;
            this.homepageButton.Click += new EventHandler((object sender, EventArgs e) =>
            {
                new HomePage(new CodeFlowBackend.DTO.LoginResponseDTO(userId, UserService.IsUserTechLeader(userId))).Show();
                this.Hide();
            });

            this.Controls.Add(this.homepageButton);

            this.changePasswordButton = new RoundedButton("SAVE", 224, 57, Colors.CallToActionButton, 32);
            this.changePasswordButton.Location = new Point(528, 672);
            this.changePasswordButton.Cursor = Cursors.Hand;
            this.changePasswordButton.Click += changePassword_Click;

            this.Controls.Add(this.changePasswordButton);
        }

        private void changePassword_Click(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                string currentPassword = this.currentPasswordTextBox.TextBox.Text;
                string newPassword = this.newPasswordTextBox.TextBox.Text;

                if (UserService.ChangePassword(userId, currentPassword, newPassword))
                {
                    MessageBox.Show("Updated succesfully");
                    this.Hide();
                    new ProfilePage(this.userId).Show();
                }
                else
                    MessageBox.Show("Your current password is wrong");
            }
            else
                MessageBox.Show("Passwords must match and must follow the rule stated");
        }

        private bool CheckFields()
        {
            string currentPassword = this.currentPasswordTextBox.TextBox.Text;
            string newPassword = this.newPasswordTextBox.TextBox.Text;
            string repeatPassword = this.repeatNewPasswordTextBox.TextBox.Text;
            if (!newPassword.Equals(repeatPassword))
                return false;
            if (!ValidationUtils.IsPasswordValid(newPassword))
                return false;
            return true;
        }

        private void InitLabels()
        {
            this.newPasswordLabel = new Label();
            this.newPasswordLabel.AutoSize = true;
            this.newPasswordLabel.Text = "New password";
            this.newPasswordLabel.Location = new Point(382, 278);
            this.newPasswordLabel.Font = new Font("Ubuntu", 12);
            this.newPasswordLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(newPasswordLabel);

            Label passwordLabel = new Label();
            passwordLabel.Text = "Password must have at least 8 characters, 1 letter and 1 number.";
            passwordLabel.ForeColor = Colors.DarkBlue;
            passwordLabel.AutoSize = true;
            passwordLabel.Font = new Font("Ubuntu", 6);
            passwordLabel.BackColor = Color.Transparent;
            passwordLabel.Location = new Point(382, 355);
            this.Controls.Add(passwordLabel);


            this.repeatNewPasswordLabel = new Label();
            this.repeatNewPasswordLabel.AutoSize = true;
            this.repeatNewPasswordLabel.Text = "Repeat your new password";
            this.repeatNewPasswordLabel.Location = new Point(382, 378);
            this.repeatNewPasswordLabel.Font = new Font("Ubuntu", 12);
            this.repeatNewPasswordLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(repeatNewPasswordLabel);

            this.currentPasswordLabel = new Label();
            this.currentPasswordLabel.AutoSize = true;
            this.currentPasswordLabel.Text = "Current password";
            this.currentPasswordLabel.Location = new Point(382, 478);
            this.currentPasswordLabel.Font = new Font("Ubuntu", 12);
            this.currentPasswordLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(currentPasswordLabel);
        }

        private void InitTextBoxes()
        {
            newPasswordTextBox = new PasswordTextBox("", 572, 60);
            newPasswordTextBox.Location = new Point(354, 295);
            newPasswordTextBox.TextBox.Font = new Font("Ubuntu", 10);
            newPasswordTextBox.TextBox.MaxLength = 30;
            this.Controls.Add(newPasswordTextBox);

            repeatNewPasswordTextBox = new PasswordTextBox("", 572, 60);
            repeatNewPasswordTextBox.Location = new Point(354, 395);
            repeatNewPasswordTextBox.TextBox.Font = new Font("Ubuntu", 10);
            repeatNewPasswordTextBox.TextBox.MaxLength = 30;
            this.Controls.Add(repeatNewPasswordTextBox);
            
            currentPasswordTextBox = new PasswordTextBox("", 572, 60);
            currentPasswordTextBox.Location = new Point(354, 495);
            currentPasswordTextBox.TextBox.Font = new Font("Ubuntu", 10);
            currentPasswordTextBox.TextBox.MaxLength = 30;
            this.Controls.Add(currentPasswordTextBox);
        }

        private void InitLogo()
        {
            this.Icon = new Icon(@"Resources\icon.ico");

            Logo logo = new Logo();
            logo.Location = new Point(525, 63);
            logo.Width = 230;
            logo.Height = 75;

            this.Controls.Add(logo);
        }
    }
}