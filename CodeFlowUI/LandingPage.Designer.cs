using CodeFlowUI.Styles;

namespace CodeFlowUI
{
    partial class LandingPage
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

        #region Windows Form Designer

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            InitializeComponentAsync();
        }


        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private async Task InitializeComponentAsync()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 832);
            this.Text = "CodeFlow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            InitLogo();

            await Task.Delay(2000);
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            this.Hide();
        }

        #endregion

        private void InitLogo()
        {

            PictureBox logo = new Logo();
            logo.Location = new Point(160, 260);
            logo.Width = 960;
            logo.Height = 312;

            this.Controls.Add(logo);

            Label signature = new Label();
            signature.Location = new Point(482, 572);
            signature.Width = 350;
            signature.Height = 38;
            signature.Text = "by Vitória Tenório";
            signature.ForeColor = ColorTranslator.FromHtml("#7DA5FA");
            signature.Font = new Font("Codec Warm Trial", 28, FontStyle.Regular);

            this.Controls.Add(signature);
        }

    }
}
