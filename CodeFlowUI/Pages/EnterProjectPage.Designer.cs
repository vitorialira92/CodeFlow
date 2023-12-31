using CodeFlowBackend.Exceptions;
using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using Microsoft.VisualBasic.ApplicationServices;

namespace CodeFlowUI.Pages
{
    partial class EnterProjectPage
    {
        private System.ComponentModel.IContainer components = null;

        private RoundedTextBox codeTextBox;
        private RoundedButton enterButton;
        private Button backToHomePageButton;
        private Label typeLabel;
        private Label warningLabel;

        private long userId;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent(long userId)
        {
            this.userId = userId;

            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1280, 832);
            this.Name = "EnterProjectPage";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CodeFlow";
            //FormClosed += ProfilePage_FormClosed;

            InitScreen();
        }

        private void InitScreen()
        {
            InitLogo();
            InitButtons();
            InitLabels();
            InitTextBox();
        }

        private void InitTextBox()
        {
            this.codeTextBox = new RoundedTextBox("XXXXXX", 198, 80);
            this.codeTextBox.Location = new Point(541, 380);
            this.codeTextBox.TextBox.Font = new Font("Ubuntu", 16, FontStyle.Bold);
            this.codeTextBox.TextBox.MaxLength = 6;
            this.codeTextBox.TextBox.TextChanged += new EventHandler((object sender, EventArgs e) =>
            {
                this.codeTextBox.TextBox.Text = this.codeTextBox.TextBox.Text.ToUpper();
                this.codeTextBox.TextBox.SelectionStart = this.codeTextBox.TextBox.Text.Length;
            });


            this.Controls.Add(this.codeTextBox);
        }

        private void InitLabels()
        {
            this.typeLabel = new Label();
            this.typeLabel.Text = "Type the project access code:";
            this.typeLabel.ForeColor = Colors.DarkBlue;
            this.typeLabel.Font = new Font("Ubuntu", 16, FontStyle.Bold);
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new Point(425, 241);

            this.Controls.Add(this.typeLabel);

            this.warningLabel = new Label();
            this.warningLabel.Text = "If you were invited by the Tech Leader, you recieved it in your e-mail.";
            this.warningLabel.ForeColor = Colors.DarkBlue;
            this.warningLabel.Font = new Font("Ubuntu", 8, FontStyle.Regular);
            this.warningLabel.Size = new Size(405, 40);
            this.warningLabel.Location = new Point(425, 292);

            this.Controls.Add(this.warningLabel);
        }

        private void InitButtons()
        {
            this.backToHomePageButton = new System.Windows.Forms.Button();
            this.backToHomePageButton.Image = Image.FromFile(@"Resources\back-to-homepage.png");
            this.backToHomePageButton.Location = new Point(156, 77);
            this.backToHomePageButton.Size = new Size(154, 24);
            this.backToHomePageButton.BackColor = Color.Transparent;
            this.backToHomePageButton.FlatAppearance.BorderSize = 0;
            this.backToHomePageButton.FlatStyle = FlatStyle.Flat;
            this.backToHomePageButton.Cursor = Cursors.Hand;
            this.backToHomePageButton.Click += new EventHandler((object sender, EventArgs e) =>
            {
                new HomePage(new CodeFlowBackend.DTO.LoginResponseDTO(userId, false)).Show();
                this.Hide();
            });

            this.Controls.Add(this.backToHomePageButton);

            this.enterButton = new RoundedButton("ENTER", 224,57, Colors.CallToActionButton, 32);
            this.enterButton.BackColor = Color.Transparent;
            this.enterButton.Cursor = Cursors.Hand;
            this.enterButton.Location = new Point(528, 531);
            this.enterButton.Click += new EventHandler(enterButton_Click);

            this.Controls.Add(enterButton);
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            try
            {
                UserService.EnterProject(userId, this.codeTextBox.TextBox.Text);
                MessageBox.Show("You entered the project succesfully");
                new HomePage(new CodeFlowBackend.DTO.LoginResponseDTO(userId, false)).Show();
                this.Hide();
            }
            catch(EnterProjectException ex)
            {
                MessageBox.Show($"There was an error entering the project: {ex.Message}");
            }
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

        #endregion
    }
}