using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using System.Text.RegularExpressions;
using CodeFlowBackend.DTO;
using System.Text;

namespace CodeFlowUI
{
    partial class RegisterPage
    {

        private System.ComponentModel.IContainer components = null;

        //forms components
        private RoundedTextBox firstNameTextBox;
        private RoundedTextBox lastNameTextBox;
        private RoundedTextBox emailTextBox;
        private RoundedTextBox usernameTextBox;
        private Label usernameLabel;
        private PasswordTextBox passwordTextBox;
        private Label passwordLabel;
        private GroupBox userRoleGroupBox;
        private RadioButton techLeaderRadioButton;
        private RadioButton developerRadioButton;
        private ComboBox specificToUserRoleComboBox;
        private PictureBox usernameCheckPictureBox;
        private Label userRoleLabel;


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
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1280, 832);
            Name = "RegisterPage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CodeFlow";
            FormClosed += RegisterPage_FormClosed;
            InitScreen();
        }

        private void InitScreen()
        {
            InitLogo();
            InitButtons();
            InitForm();
        }

        private void InitForm()
        {
            //first name
            firstNameTextBox = new RoundedTextBox("First name", 260, 60);
            firstNameTextBox.Location = new Point(354, 191);
            firstNameTextBox.TextBox.Font = new Font("Ubuntu", 10);
            this.Controls.Add(firstNameTextBox);

            //last name
            lastNameTextBox = new RoundedTextBox("Last name", 260, 60);
            lastNameTextBox.Location = new Point(666, 191);
            lastNameTextBox.TextBox.Font = new Font("Ubuntu", 10);
            this.Controls.Add(lastNameTextBox);

            //email
            emailTextBox = new RoundedTextBox("E-mail", 572, 60);
            emailTextBox.Location = new Point(354, 271);
            emailTextBox.TextBox.Font = new Font("Ubuntu", 10);
            this.Controls.Add(emailTextBox);

            //username
            usernameTextBox = new RoundedTextBox("Username", 528, 60);
            usernameTextBox.Location = new Point(354, 351);
            usernameTextBox.TextBox.Font = new Font("Ubuntu", 10);
            usernameTextBox.TextBox.Leave += new EventHandler((sender, e) =>
            {
                if (!usernameTextBox.TextBox.Text.Equals("Username"))
                {
                    bool isUsernameAvailable = UserService.IsUsernameAvailable(usernameTextBox.TextBox.Text);
                    
                    SetUsernameCheckPictureBox(isUsernameAvailable);
                    
                    if (!isUsernameAvailable) this.usernameLabel.Visible = true;
                }
                else
                {
                    this.usernameCheckPictureBox.Visible = false;
                }
                
                
            });
            this.Controls.Add(usernameTextBox);

            //username label
            usernameLabel = new Label();
            usernameLabel.Text = "Username already in use.";
            usernameLabel.ForeColor = Colors.ErrorColor;
            usernameLabel.Size = new Size(150, 18);
            usernameLabel.Font = new Font("Ubuntu", 6);
            usernameLabel.BackColor = Color.Transparent;
            usernameLabel.Location = new Point(380, 414);
            usernameLabel.Visible = false;
            this.Controls.Add(usernameLabel);


            //username check image
            usernameCheckPictureBox = new PictureBox();
            usernameCheckPictureBox.BackColor = Color.Transparent;
            usernameCheckPictureBox.Location = new Point(890, 363);
            usernameCheckPictureBox.Visible = false;
            this.Controls.Add(usernameCheckPictureBox);

            //password
            passwordTextBox = new PasswordTextBox(572, 60);
            passwordTextBox.Location = new Point(354, 431);
            passwordTextBox.TextBox.Font = new Font("Ubuntu", 10);
            this.Controls.Add(passwordTextBox);
            passwordTextBox.BringToFront();

            //password label
            passwordLabel = new Label();
            passwordLabel.Text = "Password must have at least 8 characters, 1 letter and 1 number.";
            passwordLabel.ForeColor = Colors.DarkBlue;
            passwordLabel.Size = new Size(360, 18);
            passwordLabel.Font = new Font("Ubuntu", 6);
            passwordLabel.BackColor = Color.Transparent;
            passwordLabel.Location = new Point(380, 499);
            this.Controls.Add(passwordLabel);

            //user role related question label
            userRoleLabel = new Label();
            userRoleLabel.Font = new Font("Ubuntu", 8);
            userRoleLabel.ForeColor = Colors.DarkBlue;
            userRoleLabel.Location = new Point(354, 619);
            userRoleLabel.Size = new Size(125, 20);
            userRoleLabel.Visible = false;
            this.Controls.Add(userRoleLabel);
            

            //user role related question
            specificToUserRoleComboBox = new ComboBox();

            specificToUserRoleComboBox.Location = new Point(514, 615);
            specificToUserRoleComboBox.Size = new Size(203, 27);
            specificToUserRoleComboBox.Visible = false;
            specificToUserRoleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(specificToUserRoleComboBox);

            InitRadioButtons();

        }

        private void InitRadioButtons()
        {
            userRoleGroupBox = new GroupBox();
            userRoleGroupBox.Text = "You are:";
            userRoleGroupBox.Location = new Point(354, 527);
            userRoleGroupBox.Size = new Size(572, 72);
            userRoleGroupBox.Font = new Font("Ubuntu", 8);
            userRoleGroupBox.ForeColor = Colors.DarkBlue;

            techLeaderRadioButton = new RadioButton();
            techLeaderRadioButton.Text = "Tech Leader";
            techLeaderRadioButton.Location = new Point(92, 24);
            techLeaderRadioButton.Size = new Size(154, 24);
            techLeaderRadioButton.Font = new Font("Ubuntu", 8);
            techLeaderRadioButton.ForeColor = Colors.DarkBlue;

            developerRadioButton = new RadioButton();
            developerRadioButton.Text = "Developer";
            developerRadioButton.Location = new Point(312, 24); 
            developerRadioButton.Size = new Size(154, 24);
            developerRadioButton.Font = new Font("Ubuntu", 8);
            developerRadioButton.ForeColor = Colors.DarkBlue;

            techLeaderRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            developerRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            userRoleGroupBox.Controls.Add(techLeaderRadioButton);
            userRoleGroupBox.Controls.Add(developerRadioButton);

            this.Controls.Add(userRoleGroupBox);
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb.Checked)
            {
                if (rb.Text.Equals("Tech Leader"))
                    AddSpecializationComboBox();
                else
                    AddExperienceLevelComboBox();
            }
        }

        private void SetUsernameCheckPictureBox(bool isAvailable)
        {
            if (isAvailable)
                this.usernameCheckPictureBox.Image = Image.FromFile(@"Resources\username-available.png");
            else
                this.usernameCheckPictureBox.Image = Image.FromFile(@"Resources\username-not-available.png");

            this.usernameCheckPictureBox.Visible = true;
        }

        private void InitButtons()
        {
            //button to return to login page
            Button backToLoginButton = new Button();
            backToLoginButton.Location = new Point(156, 77);
            backToLoginButton.Size = new Size(110, 24);

            backToLoginButton.Image = Image.FromFile(@"Resources\back-login.png");
            backToLoginButton.BackgroundImageLayout = ImageLayout.Stretch;
            backToLoginButton.BackColor = Color.Transparent;

            backToLoginButton.FlatStyle = FlatStyle.Flat;
            backToLoginButton.FlatAppearance.BorderSize = 0;

            backToLoginButton.Click += BackToLoginButton_Click;

            this.Controls.Add(backToLoginButton);

            //register button
            RoundedButton registerButton = new RoundedButton("REGISTER", 224, 57, Colors.CallToActionButton, 32);
            registerButton.Location = new Point(528, 731);
            registerButton.Click += RegisterButton_Click;

            this.Controls.Add(registerButton);

        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if(CheckFields()) //check fields avisa o usuário do erro 
            {
                string firstName = firstNameTextBox.TextBox.Text;
                string lastName = lastNameTextBox.TextBox.Text;
                string email = emailTextBox.TextBox.Text;
                string username = usernameTextBox.TextBox.Text;
                string password = passwordTextBox.TextBox.Text;
                bool isTechLeader = techLeaderRadioButton.Checked;
                int specificToUserRole = specificToUserRoleComboBox.SelectedIndex;

                RegisterDTO newUser = new RegisterDTO(firstName, lastName, email, username, password, isTechLeader, specificToUserRole);

                long userId = UserService.AddUser(newUser);

                if (userId != -1)
                {
                    MessageBox.Show("Account created succesfully!", "Welcome");
                    HomePage homePage = new HomePage(userId);
                    homePage.Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("An error occured, please try again!", "Error");
            }
        }

        private bool CheckFields()
        {
            List<string> wrongFields = new List<string>();

            string firstName = firstNameTextBox.TextBox.Text;

            if (firstName.Length < 2 || firstName.Contains(" ") || firstName.Equals("First name"))
                wrongFields.Add("first name");

            string lastName = lastNameTextBox.TextBox.Text;

            if (lastName.Length < 2 || lastName.Equals("Last name"))
                wrongFields.Add("last name");

            string email = emailTextBox.TextBox.Text;

            if (!email.Contains("@") || !email.Contains(".") || !UserService.IsEmailAvailable(email))
                wrongFields.Add("e-mail");

            string username = usernameTextBox.TextBox.Text;

            if (!UserService.IsUsernameAvailable(username) || username.Length < 3 || username.Equals("Username"))
            {
                wrongFields.Add("username");
                if(!username.Equals("Username"))
                    this.usernameLabel.Visible = true;
            }
            else
                this.usernameLabel.Visible = false;

            string password = passwordTextBox.TextBox.Text;

            if(password.Length < 8 || password.Contains(" ") || !Regex.IsMatch(password, @"(?=.*[a-zA-Z])(?=.*[0-9])"))
            {
                wrongFields.Add("password");
                this.passwordLabel.ForeColor = Colors.ErrorColor;
            }
            else
                this.passwordLabel.ForeColor = Colors.DarkBlue;

            if (!techLeaderRadioButton.Checked && !developerRadioButton.Checked)
                wrongFields.Add("role");
            else if (specificToUserRoleComboBox.SelectedIndex == -1)
                wrongFields.Add(userRoleLabel.Text.ToLower().Substring(0, userRoleLabel.Text.Length - 1));

            bool validInputs = wrongFields.Count == 0;

            if (!validInputs)
                ShowMessageForWrongFields(wrongFields);
            

            return validInputs;
        }

        private void ShowMessageForWrongFields(List<string> wrongFields)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("You entered wrong inputs for: ");

            foreach (var field in wrongFields)
            {
                sb.Append(field + ", ");
            }

            string message = sb.ToString();

            if (message[message.Length - 2] == ',')
            {
                message = message.Substring(0, message.Length - 2);
            }



            MessageBox.Show(message, "Attention");

        }
        private void AddExperienceLevelComboBox()
        {
            this.userRoleLabel.Text = "Experience level:";
            this.userRoleLabel.Visible = true;

            this.specificToUserRoleComboBox.Items.Clear();

            this.specificToUserRoleComboBox.Items.Add("Intern");
            this.specificToUserRoleComboBox.Items.Add("Junior");
            this.specificToUserRoleComboBox.Items.Add("Mid level");
            this.specificToUserRoleComboBox.Items.Add("Senior");
            this.specificToUserRoleComboBox.SelectedIndex = -1;


            this.specificToUserRoleComboBox.Visible = true;
        }

        private void AddSpecializationComboBox()
        {
            this.userRoleLabel.Text = "Specialization:";
            this.userRoleLabel.Visible = true;

            this.specificToUserRoleComboBox.Items.Clear();

            this.specificToUserRoleComboBox.Items.Add("Frontend");
            this.specificToUserRoleComboBox.Items.Add("Backend");
            this.specificToUserRoleComboBox.Items.Add("FullStack");
            this.specificToUserRoleComboBox.SelectedIndex = -1;

            this.specificToUserRoleComboBox.Visible = true;

        }

        private void BackToLoginButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage login = new LoginPage();
            login.Show();
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


        #endregion
    }
}