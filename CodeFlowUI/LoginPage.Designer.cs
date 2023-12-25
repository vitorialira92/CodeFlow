using CodeFlowBackend.DTO;
using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using Microsoft.VisualBasic.ApplicationServices;

namespace CodeFlowUI
{
    partial class LoginPage
    {

        private System.ComponentModel.IContainer components = null;

        private RoundedButton login;
        private RoundedButton register;

        private RoundedTextBox usernameTextBox;
        private PasswordTextBox passwordTextBox;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Interface Design

        private void InitializeComponent()
        { 
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1280, 832);
            Name = "LoginPage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CodeFlow";
            FormClosed += LoginPage_FormClosed;
            InitScreen();
        }

        private void InitScreen()
        {
            InitLogo();
            InitTextBox();
            InitButtons();
        }

        private void InitButtons()
        {
            login = new RoundedButton("LOGIN", 224, 57, Colors.CallToActionButton, 32);
            login.Location = new System.Drawing.Point(528, 564);
            login.Click += new EventHandler(loginButton_Click);
            this.Controls.Add(login);

            register = new RoundedButton("REGISTER", 224, 57, Colors.SecondaryButton, 32);
            register.Location = new System.Drawing.Point(528, 643);
            register.Click += new EventHandler(registerButton_Click);
            this.Controls.Add(register);
        }

        private void Register_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InitTextBox()
        {
            usernameTextBox = new RoundedTextBox("Username", 680,60);
            usernameTextBox.Location = new Point(299, 356);
            this.Controls.Add(usernameTextBox);

            passwordTextBox = new PasswordTextBox(680, 60);
            passwordTextBox.Location = new System.Drawing.Point(299, 444);
            this.Controls.Add(passwordTextBox);
        }

        private void InitLogo()
        {
            this.Icon = new Icon(@"Resources\icon.ico");

            PictureBox logo = new Logo();
            logo.Location = new Point(335, 129);
            logo.Width = 610;
            logo.Height = 199;

            this.Controls.Add(logo);
        }

        #endregion

        private void loginButton_Click(object sender, EventArgs e)
        {
            long userId = UserService.Login(usernameTextBox.GetText(), passwordTextBox.GetText());

            if (userId != -1)
            {
                HomePage homePage = new HomePage(userId);
                homePage.Show();
                this.Hide();
            }
            else
            {
                Label errorMessage = new Label();
                errorMessage.Text = "Username or password incorrect.";
                errorMessage.Location = new Point(325, 516);
                errorMessage.ForeColor = Colors.ErrorColor;
                errorMessage.Font = new Font("Ubuntu", 8);
                errorMessage.Size = new Size(680, 40);
                this.Controls.Add(errorMessage);
            }
            
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            RegisterPage registerPage = new RegisterPage();
            registerPage.Show();
            this.Hide();
        }
    }
}