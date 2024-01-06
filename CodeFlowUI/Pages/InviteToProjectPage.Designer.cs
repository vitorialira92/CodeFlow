

using CodeFlowBackend.DTO;
using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using System.Xml;

namespace CodeFlowUI.Pages
{
    partial class InviteToProjectPage
    {
        private System.ComponentModel.IContainer components = null;

        private long projectId;
        private long userId;

        private Label pageTitleLabel;
        private Label projectCodeLabel;
        private Label warningLabel;
        private Label developerLabel;

        private Button homepageButton;
        private RoundedButton inviteButton;

        private RoundedTextBox developerTextBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent(long projectId, long userId)
        {
            this.projectId = projectId;
            this.userId = userId;

            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1280, 832);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CodeFlow";
            FormClosed += InviteToProject_FormClosed;

            InitScreen();

        }

        private void InitScreen()
        {
            InitLabels();
            InitButtons();
            InitTextBox();
        }

        private void InitTextBox()
        {
            developerTextBox = new RoundedTextBox("", 572, 60);
            developerTextBox.Location = new Point(354, 394);
            developerTextBox.TextBox.Font = new Font("Ubuntu", 10);
            developerTextBox.Leave += new EventHandler(developerTextBox_Leave);

            this.Controls.Add(developerTextBox);
        }

        private void developerTextBox_Leave(object sender, EventArgs e)
        {
            
        }

        private void InitButtons()
        {
            this.homepageButton = new System.Windows.Forms.Button();
            this.homepageButton.Location = new Point(54, 46);
            this.homepageButton.Size = new Size(40, 43);
            this.homepageButton.FlatStyle = FlatStyle.Flat;
            this.homepageButton.FlatAppearance.BorderSize = 0;
            this.homepageButton.Cursor = Cursors.Hand;
            this.homepageButton.Image = Image.FromFile(@"Resources\home.png");
            this.homepageButton.Click += new EventHandler((object sender, EventArgs e) =>
            {
                this.Hide();
                new SpecificProjectPage(new ProjectPageDTO(this.projectId, this.userId, true)).Show();
            });

            this.Controls.Add(homepageButton);


            this.inviteButton = new RoundedButton("INVITE", 224, 57, Colors.CallToActionButton, 32);
            this.inviteButton.Location = new Point(528, 529);
            this.inviteButton.Cursor = Cursors.Hand;
            this.inviteButton.Click += invite_Click;

            this.Controls.Add(this.inviteButton);

        }

        private void invite_Click(object sender, EventArgs e)
        {
            if (CheckField())
            {
                if (ProjectService.InviteToProject(this.projectId, this.developerTextBox.TextBox.Text))
                {
                    MessageBox.Show($"Developer {this.developerTextBox.TextBox.Text} invited succesfully");
                    this.Hide();
                    new SpecificProjectPage(new ProjectPageDTO(this.projectId, this.userId, true)).Show();
                }
                else
                    MessageBox.Show("There was an error. This user might not be a developer. Try again.");
                    
            }
            else
                MessageBox.Show("Enter a valid developer's username");
        }

        private bool CheckField()
        {
            return !UserService.IsUsernameAvailable(this.developerTextBox.TextBox.Text);
        }

        private void InitLabels()
        {
            this.pageTitleLabel = new Label();
            this.pageTitleLabel.Text = "Invite to project";
            this.pageTitleLabel.Location = new Point(428, 39);
            this.pageTitleLabel.AutoSize = true;
            this.pageTitleLabel.ForeColor = Color.DarkBlue;
            this.pageTitleLabel.Font = new Font("Ubuntu", 32);

            this.Controls.Add(pageTitleLabel);

            InitDiviser();

            this.projectCodeLabel = new Label();
            this.projectCodeLabel.Text = $"PROJECT CODE: {ProjectService.GetProjectEnterCodeById(projectId)}";
            this.projectCodeLabel.ForeColor = Colors.DarkBlue;
            this.projectCodeLabel.Font = new Font("Ubuntu", 16, FontStyle.Bold);
            this.projectCodeLabel.AutoSize = true;
            this.projectCodeLabel.Location = new Point(450, 246);

            this.Controls.Add(this.projectCodeLabel);

            this.warningLabel = new Label();
            this.warningLabel.Text = "The developer will get an e-mail with this code.";
            this.warningLabel.ForeColor = Colors.DarkBlue;
            this.warningLabel.Font = new Font("Ubuntu", 8, FontStyle.Regular);
            this.warningLabel.AutoSize = true;
            this.warningLabel.Location = new Point(470, 287);

            this.Controls.Add(this.warningLabel);

            this.developerLabel = new Label();
            this.developerLabel.AutoSize = true;
            this.developerLabel.Text = "Developer’s username";
            this.developerLabel.Location = new Point(382, 377);
            this.developerLabel.Font = new Font("Ubuntu", 12);
            this.developerLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(developerLabel);

        }

        

        private void InitDiviser()
        {
            PictureBox diviser = new PictureBox();
            diviser.Image = Image.FromFile(@"Resources\small-vertical-line.png");
            diviser.Location = new Point(108, 41);
            diviser.Width = 2;
            diviser.Height = 56;

            this.Controls.Add(diviser);
        }
    }
}