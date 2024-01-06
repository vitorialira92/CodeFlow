using CodeFlowBackend.DTO;
using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using CodeFlowUI.Util;
using System.Text;

namespace CodeFlowUI.Pages
{
    partial class ProfilePage
    {
        private System.ComponentModel.IContainer components = null;
        private UserDTO user;


        //forms components
        private Button backToHomePageButton;
        private Button changePasswordButton;
        private RoundedTextBox firstNameTextBox;
        private RoundedTextBox lastNameTextBox;
        private RoundedTextBox emailTextBox;
        private RoundedTextBox usernameTextBox;
        private ComboBox specificToUserRoleComboBox;
        private RoundedButton logoutButton;
        private RoundedButton saveButton;

        private Label usernameLabel;
        private Label userRoleLabel;
        private Label firstNameLabel;
        private Label lastNameLabel;
        private Label emailLabel;
        private Label specificToUserRoleLabel;
        private Label dateJoinedLabel;
        private Label daysWithUsLabel;
        private Label projectsCountLabel;
        private Label tasksCountLabel;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Interface code

        private void InitializeComponent(long userId)
        {
           
            this.user = UserService.GetUserById(userId);

            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1280, 832);
            this.Name = "ProfilePage";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CodeFlow";
            FormClosed += ProfilePage_FormClosed;

            InitScreen();
        }


        private void InitScreen()
        {
            InitLogo();
            InitButtons();
            InitLabels();
            InitTextBoxes();
        }

        private void InitTextBoxes()
        {

            //first name
            firstNameTextBox = new RoundedTextBoxEditable(user.FirstName, 260, 60);
            firstNameTextBox.Location = new Point(354, 191);
            firstNameTextBox.TextBox.Font = new Font("Ubuntu", 10);
            firstNameTextBox.Leave += new EventHandler((object sender, EventArgs e) =>
            {
                if (!firstNameTextBox.TextBox.Text.Equals(user.FirstName))
                    saveButton.Enabled = true;
            });
            this.Controls.Add(firstNameTextBox);

            //last name
            lastNameTextBox = new RoundedTextBoxEditable(user.LastName, 260, 60);
            lastNameTextBox.Location = new Point(666, 191);
            lastNameTextBox.TextBox.Font = new Font("Ubuntu", 10);

            lastNameTextBox.Leave += new EventHandler((object sender, EventArgs e) =>
             {
                 if (!lastNameTextBox.TextBox.Text.Equals(user.LastName))
                     saveButton.Enabled = true;
             });

            this.Controls.Add(lastNameTextBox);

            //email
            emailTextBox = new RoundedTextBoxEditable(user.Email, 572, 60);
            emailTextBox.Location = new Point(354, 292);
            emailTextBox.TextBox.Font = new Font("Ubuntu", 10);
            emailTextBox.Leave += new EventHandler((object sender, EventArgs e) =>
            {
                if (!emailTextBox.TextBox.Text.Equals(user.Email))
                    saveButton.Enabled = true;
            });
            this.Controls.Add(emailTextBox);

            //username
            usernameTextBox = new RoundedTextBox(user.Username, 572, 60);
            usernameTextBox.Location = new Point(354, 394);
            usernameTextBox.TextBox.Font = new Font("Ubuntu", 10);
            this.usernameTextBox.Enabled = false;
            this.Controls.Add(usernameTextBox);

            //combo box
            specificToUserRoleComboBox = new ComboBox();

            specificToUserRoleComboBox.Location = new Point(618, 474);
            specificToUserRoleComboBox.Size = new Size(203, 27);
            specificToUserRoleComboBox.Visible = false;
            specificToUserRoleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.Controls.Add(specificToUserRoleComboBox);

            if (user.IsTechLeader)
                AddSpecializationComboBox();
            else
                AddExperienceLevelComboBox();

        }

        private void InitLabels()
        {
            this.firstNameLabel = new Label();
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Text = "First name";
            this.firstNameLabel.Location = new Point(380, 174);
            this.firstNameLabel.Font = new Font("Ubuntu", 12);
            this.firstNameLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(firstNameLabel);

            this.lastNameLabel = new Label();
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Text = "Last name";
            this.lastNameLabel.Location = new Point(692, 174);
            this.lastNameLabel.Font = new Font("Ubuntu", 12);
            this.lastNameLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(lastNameLabel);

            this.emailLabel = new Label();
            this.emailLabel.AutoSize = true;
            this.emailLabel.Text = "E-mail";
            this.emailLabel.Location = new Point(380, 275);
            this.emailLabel.Font = new Font("Ubuntu", 12);
            this.emailLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(emailLabel);

            this.usernameLabel = new Label();
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Text = "Username";
            this.usernameLabel.Location = new Point(380, 377);
            this.usernameLabel.Font = new Font("Ubuntu", 12);
            this.usernameLabel.ForeColor = Colors.DarkBlue;
            this.Controls.Add(usernameLabel);

            this.specificToUserRoleLabel = new Label();
            this.specificToUserRoleLabel.AutoSize = true;
            this.specificToUserRoleLabel.Text = (user.IsTechLeader) ? "Specialization:" : "Experience level:";
            this.specificToUserRoleLabel.Location = new Point(422, 478);
            this.specificToUserRoleLabel.Font = new Font("Ubuntu", 12);
            this.specificToUserRoleLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(specificToUserRoleLabel);


            this.userRoleLabel = new Label();
            this.userRoleLabel.AutoSize = true;
            this.userRoleLabel.Text = (user.IsTechLeader) ? "Tech Leader" : "Developer";
            this.userRoleLabel.Location = new Point(994, 166);
            this.userRoleLabel.Font = new Font("Ubuntu", 24, FontStyle.Italic | FontStyle.Bold);
            this.userRoleLabel.ForeColor = Colors.DarkBlue;
            this.Controls.Add(userRoleLabel);

            AddDetails();

            this.dateJoinedLabel = new Label();
            this.dateJoinedLabel.AutoSize = true;
            this.dateJoinedLabel.Text = $"Joined {user.JoinedDate.ToString("MM/dd/yy")}";
            this.dateJoinedLabel.Location = new Point(1002, 242);
            this.dateJoinedLabel.Font = new Font("Ubuntu", 10);
            this.dateJoinedLabel.ForeColor = Colors.DarkBlue;
            this.Controls.Add(dateJoinedLabel);

            this.daysWithUsLabel = new Label();
            this.daysWithUsLabel.AutoSize = true;
            this.daysWithUsLabel.Text = $"{(DateTime.Today - user.JoinedDate).Days} days with us";
            this.daysWithUsLabel.Location = new Point(1002, 273);
            this.daysWithUsLabel.Font = new Font("Ubuntu", 10);
            this.daysWithUsLabel.ForeColor = Colors.DarkBlue;
            this.Controls.Add(daysWithUsLabel);

            this.projectsCountLabel = new Label();
            this.projectsCountLabel.AutoSize = true;
            this.projectsCountLabel.Text = $"{UserService.GetUsersProjectCountById(user.Id)} projects";
            this.projectsCountLabel.Location = new Point(1002, 304);
            this.projectsCountLabel.Font = new Font("Ubuntu", 10);
            this.projectsCountLabel.ForeColor = Colors.DarkBlue;
            this.Controls.Add(projectsCountLabel);

            this.tasksCountLabel = new Label();
            this.tasksCountLabel.AutoSize = true;
            this.tasksCountLabel.Text = $"{UserService.GetUsersDoneTasksCountById(user.Id)} tasks done";
            this.tasksCountLabel.Location = new Point(1002, 335);
            this.tasksCountLabel.Font = new Font("Ubuntu", 10);
            this.tasksCountLabel.ForeColor = Colors.DarkBlue;
            this.Controls.Add(tasksCountLabel);

        }

        private void AddDetails()
        {
            PictureBox verticalLine = new PictureBox();
            verticalLine.Location = new Point(976, 166);
            verticalLine.Image = Image.FromFile(@"Resources\line.png");
            verticalLine.Size = new Size(2, 231);
            this.Controls.Add(verticalLine);

            PictureBox horizontalLine = new PictureBox();
            horizontalLine.Location = new Point(1002, 220);
            horizontalLine.Image = Image.FromFile(@"Resources\line-horizontal.png");
            horizontalLine.Size = new Size(177, 2);
            this.Controls.Add(horizontalLine);
            
            PictureBox bigHorizontalLine = new PictureBox();
            bigHorizontalLine.Location = new Point(355, 715);
            bigHorizontalLine.Image = Image.FromFile(@"Resources\big-horizontal-line.png");
            bigHorizontalLine.Size = new Size(570, 2);
            this.Controls.Add(bigHorizontalLine);
            
        }

        private void InitLogo()
        {
            this.Icon = new Icon(@"Resources\icon.ico");

            PictureBox logo = new Logo();
            logo.Location = new Point(525, 63);
            logo.Width = 230;
            logo.Height = 75;

            this.Controls.Add(logo);
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
                new HomePage(new CodeFlowBackend.DTO.LoginResponseDTO(user.Id, user.IsTechLeader)).Show();
                this.Hide();
            });

            this.Controls.Add(this.backToHomePageButton);

            logoutButton = new RoundedButton("logout", 150, 35, Colors.CallToActionButton, 32);
            logoutButton.Location = new Point(1038, 66);
            logoutButton.Cursor = Cursors.Hand;
            logoutButton.Font = new Font("Ubuntu", 12);
            logoutButton.Click += new EventHandler((object sender, EventArgs e) =>
            {
                this.Hide();
                new LoginPage().Show();
            });

            this.Controls.Add(logoutButton); 
            
            saveButton = new RoundedButton("SAVE", 224, 57, Colors.CallToActionButton, 40);
            saveButton.Location = new Point(528, 612);
            saveButton.Cursor = Cursors.Hand;
            saveButton.Enabled = false;
            saveButton.Click += saveButton_Click;

            this.Controls.Add(saveButton);

            this.changePasswordButton = new System.Windows.Forms.Button();
            this.changePasswordButton.Image = Image.FromFile(@"Resources\change-password-button.png");
            this.changePasswordButton.Location = new Point(663, 544);
            this.changePasswordButton.Size = new Size(263, 24);
            this.changePasswordButton.BackColor = Color.Transparent;
            this.changePasswordButton.FlatAppearance.BorderSize = 0;
            this.changePasswordButton.FlatStyle = FlatStyle.Flat;
            this.changePasswordButton.Cursor = Cursors.Hand;
            this.changePasswordButton.Click += new EventHandler((object sender, EventArgs e) =>
            {
                new ChangePasswordPage(user.Id).Show();
                this.Hide();
            });
            this.Controls.Add(changePasswordButton);
    
        }

        private void AddExperienceLevelComboBox()
        {
            this.specificToUserRoleLabel.Text = "Experience level:";
            this.specificToUserRoleLabel.Visible = true;

            this.specificToUserRoleComboBox.Items.Clear();

            this.specificToUserRoleComboBox.Items.Add("Intern");
            this.specificToUserRoleComboBox.Items.Add("Junior");
            this.specificToUserRoleComboBox.Items.Add("Mid level");
            this.specificToUserRoleComboBox.Items.Add("Senior");
            this.specificToUserRoleComboBox.SelectedIndex = user.SpecificToUserRole - 1;

            this.specificToUserRoleComboBox.Leave += new EventHandler((object sender, EventArgs e) =>
            {
                if (!specificToUserRoleComboBox.SelectedIndex.Equals(user.SpecificToUserRole - 1))
                    saveButton.Enabled = true;
            });

            this.specificToUserRoleComboBox.Visible = true;
        }

        private void AddSpecializationComboBox()
        {
            this.specificToUserRoleLabel.Text = "Specialization:";
            this.specificToUserRoleLabel.Visible = true;

            this.specificToUserRoleComboBox.Items.Clear();

            this.specificToUserRoleComboBox.Items.Add("Frontend");
            this.specificToUserRoleComboBox.Items.Add("Backend");
            this.specificToUserRoleComboBox.Items.Add("FullStack");
            this.specificToUserRoleComboBox.SelectedIndex = user.SpecificToUserRole - 1;
            this.specificToUserRoleComboBox.Enabled = false;

            this.specificToUserRoleComboBox.Visible = true;

        }
        #endregion

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (WereThereChanges())
            {
                string firstName = firstNameTextBox.TextBox.Text, 
                    lastName = lastNameTextBox.TextBox.Text, 
                        email = emailTextBox.TextBox.Text;
                int specificToUser = specificToUserRoleComboBox.SelectedIndex + 1;

                List<string> wrongFields = new List<string>();

                if (!ValidationUtils.IsEmailValid(email))
                    wrongFields.Add("e-mail");

                if (!ValidationUtils.IsInputAValidName(firstName, null, false))
                    wrongFields.Add("first name");

                if (!ValidationUtils.IsInputAValidName(lastName, null, true))
                    wrongFields.Add("last name");

                if (wrongFields.Count == 0)
                {
                    UserDTO updatedUser = new UserDTO(this.user.Id, firstName,
                    lastName, email, this.user.Username, this.user.JoinedDate, this.user.IsTechLeader, specificToUser);

                    if (UserService.Update(updatedUser))
                    {
                        this.user = updatedUser;

                        MessageBox.Show("Updated succesfully!");
                    }
                    else
                        MessageBox.Show("There was a problem updating");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("The following fields are wrong: ");

                    foreach(var str in wrongFields)
                    {
                        sb.Append(str +  ", ");
                    }

                    MessageBox.Show(sb.ToString().Substring(0, sb.ToString().Length - 2));
                }

                

            }
            
        }

        private bool WereThereChanges()
        {
            return !((this.specificToUserRoleComboBox.SelectedIndex == user.SpecificToUserRole -1) 
                && (this.emailTextBox.TextBox.Text.Equals(user.Email)) && (this.firstNameTextBox.TextBox.Text.Equals(user.FirstName)) &&
                    (this.lastNameTextBox.TextBox.Text.Equals(user.LastName)));
        }
    }
}