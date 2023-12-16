

using CodeFlowUI.Styles;

namespace CodeFlowUI
{
    partial class LoginPage
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 832);
            this.Text = "CodeFlow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
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
            CallToActionButton login = new CallToActionButton("LOGIN", 224,57);
            login.Location = new System.Drawing.Point(528, 564);
            this.Controls.Add(login);

            Button register = new GradientButton("REGISTER", 224, 57);
            register.Location = new System.Drawing.Point(528, 643);
            this.Controls.Add(register);
        }

        private void InitTextBox()
        {
            LightBlueTextBox emailTextBox = new LightBlueTextBox("Email", 681, 60);
            emailTextBox.Location = new System.Drawing.Point(299,356);
            this.Controls.Add(emailTextBox);

            PasswordBox passwordTextBox = new PasswordBox("Password", 681, 60);
            passwordTextBox.Location = new System.Drawing.Point(299, 444);
            this.Controls.Add(passwordTextBox);
        }

        private void InitLogo()
        {
            PictureBox logo = new Logo();
            logo.Location = new Point(335, 129);
            logo.Width = 610;
            logo.Height = 199;

            this.Controls.Add(logo);
        }

        #endregion
    }
}