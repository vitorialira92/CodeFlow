using CodeFlowUI.Styles;

namespace CodeFlowUI
{
    partial class LandingPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Design

        private void InitializeComponent()
        {
            InitializeComponentAsync();
        }

        private async Task InitializeComponentAsync()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 832);
            this.Text = "CodeFlow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            InitLogo();

            await Task.Delay(1500);
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            this.Hide();
        }

        #endregion

        private void InitLogo()
        {
            this.Icon = new Icon(@"Resources\icon.ico");

            PictureBox logo = new Logo();
            logo.Location = new Point(160, 260);
            logo.Width = 960;
            logo.Height = 312;

            this.Controls.Add(logo);

            Label signature = new Label();
            signature.Location = new Point(530, 572);
            signature.Width = 350;
            signature.Height = 38;
            signature.Text = "by Vitória Tenório";
            signature.ForeColor = ColorTranslator.FromHtml("#7DA5FA");
            signature.Font = new Font("Codec Warm Trial", 16, FontStyle.Regular);

            this.Controls.Add(signature);
        }

    }
}
