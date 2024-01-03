using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;

namespace CodeFlowUI.Pages
{
    partial class CreateTaskPage
    {
        private System.ComponentModel.IContainer components = null;

        List<string> checklist = new List<string>();
        List<Tag> allTags;
        Tag selectedTag;

        private ProjectPageDTO projectPageDTO;

        private Label pageTitleLabel;
        private Label nameLabel;
        private Label descriptionLabel;
        private Label assigneeLabel;
        private Label dueDateLabel;
        private Label tagLabel;
        private Label checklistLabel;

        private Panel tagColorPanel;

        private ComboBox tagComboBox;
        private DateTimePicker dueDatePicker;
        private RoundedTextBox nameTextBox;
        private RoundedTextBox descriptionTextBox;
        private RoundedTextBox assigneeTextBox;
        private RoundedTextBox checklistTextBox;
        private RoundedTextBox createTagTextBox;
        private Panel checklistPanel;
        private Button homepageButton;
        private RoundedButton addChecklistButton;
        private RoundedButton addTagButton;
        private RoundedButton createTaskButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent(ProjectPageDTO projectPageDTO)
        {
            this.Icon = new Icon(@"Resources\icon.ico");

            this.projectPageDTO = projectPageDTO;
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 832);
            this.Text = "CodeFlow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            FormClosed += CreateTask_FormClosed;

            InitScreen();
        }

        private void InitScreen()
        {
            InitLabels();
            InitTextBoxes();
            InitButtons();
            InitComboBox();
            InitPanel();
        }

        private void InitPanel()
        {
            this.checklistPanel = new Panel();
            this.checklistPanel.Size = new Size(332, 227);
            this.checklistPanel.Location = new Point(315, 511);
            this.checklistPanel.BackColor = Color.White;
            this.checklistPanel.AutoScroll = true;

            this.Controls.Add(this.checklistPanel);
        }

        private void InitComboBox()
        {
            this.tagComboBox = new ComboBox();
            this.tagComboBox.Location = new Point(679, 381);
            this.tagComboBox.Size = new Size(322, 60);
            this.tagComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.Controls.Add(this.tagComboBox);

            LoadTagOptions();

            this.tagColorPanel = new Panel();
            this.tagColorPanel.Size = new Size(15, 29);
            this.tagColorPanel.Location = new Point(658, 381);

            this.Controls.Add(tagColorPanel);
        }

        private void LoadTagOptions()
        {
            this.tagComboBox.Items.Clear();

            this.tagComboBox.Items.Add("None");

            allTags = ProjectService.GetAllTagsByProjectId(this.projectPageDTO.ProjectId);

            foreach (Tag tag in allTags)
            {
                this.tagComboBox.Items.Add(tag.Name);
            }

            this.tagComboBox.Items.Add("Create a new one");
            this.tagComboBox.SelectedIndex = 0;

            this.tagComboBox.SelectedIndexChanged += new EventHandler(tagComboBox_SelectedIndexChanged);

        }

        private void tagComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tagComboBox.SelectedItem.ToString() == "Create a new one")
            {
                ShowCreateNewTag();
                this.tagColorPanel.BackColor = Color.White;
                this.selectedTag = null;
            }
            else
            {
                ChangeTagColor(tagComboBox.SelectedItem.ToString());
                HideCreateNewTag();
            }
                
        }

        private void ChangeTagColor(string tagName)
        {
            var tag = allTags.Where(x => x.Name.Equals(tagName)).FirstOrDefault();
            this.selectedTag = tag;
            if(tag != null) 
                this.tagColorPanel.BackColor = tag.Color;
            else
                this.tagColorPanel.BackColor = Color.White;
        }

        private void HideCreateNewTag()
        {
            this.addTagButton.Visible = false;
            this.createTagTextBox.Visible = false;
        }

        private void ShowCreateNewTag()
        {
            this.addTagButton.Visible = true;
            this.createTagTextBox.Visible = true;
        }

        private void InitTextBoxes()
        {
            nameTextBox = new RoundedTextBox("", 572, 60);
            nameTextBox.Location = new Point(180, 171);
            nameTextBox.TextBox.Font = new Font("Ubuntu", 10);
            this.Controls.Add(nameTextBox);

            dueDatePicker = new DateTimePicker();
            dueDatePicker.Location = new Point(795, 191);
            dueDatePicker.MinDate = DateTime.Today;

            this.Controls.Add(dueDatePicker);

            descriptionTextBox = new RoundedTextBox("", 821, 60);
            descriptionTextBox.Location = new Point(180, 276);
            descriptionTextBox.TextBox.Font = new Font("Ubuntu", 10);
            this.Controls.Add(descriptionTextBox);

            assigneeTextBox = new RoundedTextBoxEditable(UserService.GetUsersUsernameById(this.projectPageDTO.UserId), 442, 60);
            assigneeTextBox.Location = new Point(180, 381);
            assigneeTextBox.TextBox.Font = new Font("Ubuntu", 10);
            assigneeTextBox.Leave += new EventHandler(assigneeTextBox_Leave);
            if (!this.projectPageDTO.IsUserTechLeader)
                this.assigneeTextBox.Enabled = false;
            this.Controls.Add(assigneeTextBox);
            
            createTagTextBox = new RoundedTextBox("Type the tag name", 322, 60);
            createTagTextBox.Location = new Point(679, 475);
            createTagTextBox.TextBox.Font = new Font("Ubuntu", 10);
            createTagTextBox.Visible = false;
            this.Controls.Add(createTagTextBox);
            
            checklistTextBox = new RoundedTextBox("", 221, 40);
            checklistTextBox.Location = new Point(318, 458);
            checklistTextBox.TextBox.Font = new Font("Ubuntu", 10);
            this.Controls.Add(checklistTextBox);
        }

        private void assigneeTextBox_Leave(object sender, EventArgs e)
        {
            if (UserService.IsUsernameAvailable(assigneeTextBox.TextBox.Text))
            {
                MessageBox.Show("This user doesn't exist");
            }
            else if (!ProjectService.IsUserOnProject(assigneeTextBox.TextBox.Text, this.projectPageDTO.ProjectId))
            {
                MessageBox.Show("This user is not on this project");
            }
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
                new HomePage(new LoginResponseDTO(this.projectPageDTO.UserId, this.projectPageDTO.IsUserTechLeader)).Show();
            });

            this.Controls.Add(homepageButton);

            this.createTaskButton = new RoundedButton("CREATE", 224,57, Colors.CallToActionButton, 32);
            this.createTaskButton.Location = new Point(777, 681);
            this.createTaskButton.Cursor = Cursors.Hand;
            this.createTaskButton.Click += createTask_Click;

            this.Controls.Add(this.createTaskButton);
            
            this.addTagButton = new RoundedButton("ADD", 97,40, Colors.CallToActionButton, 32);
            this.addTagButton.Location = new Point(904, 547);
            this.addTagButton.Cursor = Cursors.Hand;
            this.addTagButton.Visible = false;
            this.addTagButton.Click += AddTag_Click;

            this.Controls.Add(this.addTagButton);
            
            this.addChecklistButton = new RoundedButton("ADD", 97,40, Colors.CallToActionButton, 32);
            this.addChecklistButton.Location = new Point(550, 457);
            this.addChecklistButton.Cursor = Cursors.Hand;
            this.addChecklistButton.Click += AddChecklist_Click;

            this.Controls.Add(this.addChecklistButton);
        }

        private void AddChecklist_Click(object sender, EventArgs e)
        {
            if (this.checklist.Contains(this.checklistTextBox.TextBox.Text.ToLower()))
            {
                MessageBox.Show($"You already added {this.checklistTextBox.TextBox.Text.ToLower()}");
                this.checklistTextBox.TextBox.Text = "";
                return;
            }
            string newChecklist = this.checklistTextBox.TextBox.Text.ToLower();

            this.checklist.Add(newChecklist);

            LoadChecklist();

            this.checklistTextBox.TextBox.Text = "";
        }
        
        private void CleanChecklist()
        {
            foreach (Control control in checklistPanel.Controls.OfType<Control>().ToList())
            {
                checklistPanel.Controls.Remove(control);
                control.Dispose();
            }
        }

        private void LoadChecklist()
        {
            CleanChecklist();
            int xSquare = 22, ySquare = 17;
            int xText = 56, yText = 16;
            int xTrash = 285, yTrash = 17;

            foreach (var item in checklist)
            {

                PictureBox square = new PictureBox();
                square.Image = Image.FromFile(@"Resources\small-square.png");
                square.Location = new Point(xSquare, ySquare);
                square.Size = new Size(20, 20);

                this.checklistPanel.Controls.Add(square);
                ySquare += 20 + 11;

                Label checkLabel = new Label();
                checkLabel.Text = item;
                checkLabel.Location = new Point(xText, yText);
                checkLabel.AutoSize = true;
                checkLabel.Font = new Font("Ubuntu", 12);

                this.checklistPanel.Controls.Add(checkLabel);
                yText += checkLabel.Height + 8;

                PictureBox deleteButton = new PictureBox();
                deleteButton.Image = Image.FromFile(@"Resources\trash-can.png");
                deleteButton.Location = new Point(xTrash, yTrash);
                deleteButton.Size = new Size(22, 22);
                deleteButton.Cursor = Cursors.Hand;
                deleteButton.Click += new EventHandler((object sender, EventArgs e) =>
                {
                    this.checklist.Remove(item);
                    LoadChecklist();
                });

                this.checklistPanel.Controls.Add(deleteButton);
                yTrash += 20 + 9;



            }
        }

        private void AddTag_Click(object sender, EventArgs e)
        {
            if(allTags.Where(x=> x.Name.ToLower().Equals((this.createTagTextBox.TextBox.Text).ToLower())).FirstOrDefault() != null)
            {
                MessageBox.Show("A tag with this name already exists.");
                return;
            }

            if(!String.IsNullOrWhiteSpace(this.createTagTextBox.TextBox.Text))
            {
                HideCreateNewTag();
                ProjectService.CreateNewTag(this.createTagTextBox.TextBox.Text, this.projectPageDTO.ProjectId);
                LoadTagOptions();
                this.tagComboBox.SelectedIndex = this.tagComboBox.FindStringExact(this.createTagTextBox.TextBox.Text);
                this.createTagTextBox.TextBox.Text = "";
            }
        }

        private void createTask_Click(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                string name = this.nameTextBox.TextBox.Text;
                string description = this.descriptionTextBox.TextBox.Text;
                string assignee = this.assigneeTextBox.TextBox.Text;
                DateTime dueDate = this.dueDatePicker.Value;

                if (ProjectService.CreateTask(new CreateTaskDTO(this.projectPageDTO.ProjectId, name, description, selectedTag, checklist, assignee, dueDate)))
                {
                    MessageBox.Show("Task created succesfully");
                    new SpecificProjectPage(this.projectPageDTO).Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("There was an error creating this task");

            }
            else
                MessageBox.Show("Please fill all the mandatory fields correctly");
        }

        private bool CheckFields()
        {
            if (String.IsNullOrWhiteSpace(this.nameTextBox.TextBox.Text) || this.nameTextBox.TextBox.Text.Equals(""))
                return false;
            if (this.assigneeTextBox.TextBox.Text.Equals(""))
                return false;
            if (!ProjectService.IsUserOnProject(assigneeTextBox.TextBox.Text, this.projectPageDTO.ProjectId))
                return false;
            if (UserService.IsUsernameAvailable(assigneeTextBox.TextBox.Text)) 
                return false;
            if (String.IsNullOrWhiteSpace(this.assigneeTextBox.TextBox.Text))
                return false;
            if (String.IsNullOrWhiteSpace(this.descriptionTextBox.TextBox.Text))
                return false;

            return true;
        }

        private void InitLabels()
        {
            this.pageTitleLabel = new Label();
            this.pageTitleLabel.Text = "Create task";
            this.pageTitleLabel.Location = new Point(120, 39);
            this.pageTitleLabel.AutoSize = true;
            this.pageTitleLabel.ForeColor = Color.DarkBlue;
            this.pageTitleLabel.Font = new Font("Ubuntu", 32);

            this.Controls.Add(pageTitleLabel);

            InitDiviser();

            this.nameLabel = new Label();
            this.nameLabel.AutoSize = true;
            this.nameLabel.Text = "Name*";
            this.nameLabel.Location = new Point(208, 154);
            this.nameLabel.Font = new Font("Ubuntu", 12);
            this.nameLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(nameLabel);

            this.dueDateLabel = new Label();
            this.dueDateLabel.AutoSize = true;
            this.dueDateLabel.Text = "Due date*";
            this.dueDateLabel.Location = new Point(823, 154);
            this.dueDateLabel.Font = new Font("Ubuntu", 12);
            this.dueDateLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(dueDateLabel);

            this.descriptionLabel = new Label();
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Text = "Description*";
            this.descriptionLabel.Location = new Point(208, 259);
            this.descriptionLabel.Font = new Font("Ubuntu", 12);
            this.descriptionLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(descriptionLabel);
            
            this.assigneeLabel = new Label();
            this.assigneeLabel.AutoSize = true;
            this.assigneeLabel.Text = "Assignee*";
            this.assigneeLabel.Location = new Point(208, 364);
            this.assigneeLabel.Font = new Font("Ubuntu", 12);
            this.assigneeLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(assigneeLabel);

            this.tagLabel = new Label();
            this.tagLabel.AutoSize = true;
            this.tagLabel.Text = "Tag";
            this.tagLabel.Location = new Point(707, 364);
            this.tagLabel.Font = new Font("Ubuntu", 12);
            this.tagLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(tagLabel);
            
            this.checklistLabel = new Label();
            this.checklistLabel.AutoSize = true;
            this.checklistLabel.Text = "Checklist";
            this.checklistLabel.Location = new Point(208, 469);
            this.checklistLabel.Font = new Font("Ubuntu", 12);
            this.checklistLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(checklistLabel);
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