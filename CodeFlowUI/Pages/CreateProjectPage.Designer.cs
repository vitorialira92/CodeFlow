using CodeFlowBackend.DTO;
using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using CodeFlowUI.Util;

namespace CodeFlowUI.Pages
{
    partial class CreateProjectPage
    {
        private System.ComponentModel.IContainer components = null;
        private long userId;

        private Label pageTitleLabel;
        private Label nameLabel;
        private Label descriptionLabel;
        private Label dueDateLabel;

        private DateTimePicker dueDatePicker;
        private RoundedTextBox nameTextBox;
        private RoundedTextBox descriptionTextBox;

        private Button homepageButton;
        private RoundedButton createProjectButton;

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

            this.Icon = new Icon(@"Resources\icon.ico");

            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 832);
            this.Text = "CodeFlow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            FormClosed += CreateProject_FormClosed;

            InitScreen();
        }

        private void InitScreen()
        {
            InitLabels();
            InitTextBoxes();
            InitButtons();

        }

        private void InitTextBoxes()
        {
            nameTextBox = new RoundedTextBox("", 572, 60);
            nameTextBox.Location = new Point(229, 291);
            nameTextBox.TextBox.Font = new Font("Ubuntu", 10);
            nameTextBox.TextBox.MaxLength = 30;
            this.Controls.Add(nameTextBox);

            dueDatePicker = new DateTimePicker();
            dueDatePicker.Location = new Point(844, 291);
            dueDatePicker.MinDate = DateTime.Today;

            this.Controls.Add(dueDatePicker);

            descriptionTextBox = new RoundedTextBox("", 821, 60);
            descriptionTextBox.Location = new Point(229, 396);
            descriptionTextBox.TextBox.Font = new Font("Ubuntu", 10);
            descriptionTextBox.TextBox.MaxLength = 150;
            this.Controls.Add(descriptionTextBox);

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
                new HomePage(new LoginResponseDTO(this.userId, true)).Show();
            });

            this.Controls.Add(homepageButton);

            this.createProjectButton = new RoundedButton("CREATE", 224, 57, Colors.CallToActionButton, 32);
            this.createProjectButton.Location = new Point(528, 672);
            this.createProjectButton.Cursor = Cursors.Hand;
            this.createProjectButton.Click += createProject_Click;

            this.Controls.Add(this.createProjectButton);
        }

        private void createProject_Click(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                if(ProjectService.CreateProject(new CreateProjectDTO(this.userId, this.nameTextBox.TextBox.Text, this.descriptionTextBox.TextBox.Text, dueDatePicker.Value)))
                {
                    MessageBox.Show("Project created succesfully");
                    new HomePage(new LoginResponseDTO(this.userId, true)).Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("There was an error creating this project");
            }
            else
                MessageBox.Show("Please fill all fields correctly");
        }

        private bool CheckFields()
        {
            if(!ProjectService.IsProjectNameAvailableForThisUser(this.nameTextBox.TextBox.Text, this.userId))
            {
                MessageBox.Show("This project name is not available for you");
                return false;
            }
            if (!ValidationUtils.IsInputAValidName(this.nameTextBox.TextBox.Text, null, true))
                return false;
            if (!ValidationUtils.IsInputAValidName(this.descriptionTextBox.TextBox.Text, null, true))
                return false;
            
            return true;
        }

        private void InitLabels()
        {
            this.pageTitleLabel = new Label();
            this.pageTitleLabel.Text = "Create project";
            this.pageTitleLabel.Location = new Point(120, 39);
            this.pageTitleLabel.AutoSize = true;
            this.pageTitleLabel.ForeColor = Color.DarkBlue;
            this.pageTitleLabel.Font = new Font("Ubuntu", 32);

            this.Controls.Add(pageTitleLabel);

            InitDiviser();

            this.nameLabel = new Label();
            this.nameLabel.AutoSize = true;
            this.nameLabel.Text = "Name*";
            this.nameLabel.Location = new Point(257, 274);
            this.nameLabel.Font = new Font("Ubuntu", 12);
            this.nameLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(nameLabel);

            this.dueDateLabel = new Label();
            this.dueDateLabel.AutoSize = true;
            this.dueDateLabel.Text = "Due date*";
            this.dueDateLabel.Location = new Point(872, 274);
            this.dueDateLabel.Font = new Font("Ubuntu", 12);
            this.dueDateLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(dueDateLabel);

            this.descriptionLabel = new Label();
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Text = "Description*";
            this.descriptionLabel.Location = new Point(257, 379);
            this.descriptionLabel.Font = new Font("Ubuntu", 12);
            this.descriptionLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(descriptionLabel);
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