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

            PictureBox pictureBox = new PictureBox();
            pictureBox.Location = new Point(160, 260);
            pictureBox.Width = 960;
            pictureBox.Height = 312;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = Image.FromFile(@"Resources\logo-nome.png");

            this.Controls.Add(pictureBox);

            Label label = new Label();
            label.Location = new Point(482, 572);
            label.Width = 350;
            label.Height = 38;
            label.Text = "by Vitória Tenório";
            label.ForeColor = ColorTranslator.FromHtml("#7DA5FA");
            label.Font = new Font("Codec Warm Trial", 28, FontStyle.Regular);

            this.Controls.Add(label);
        }

    }
}
