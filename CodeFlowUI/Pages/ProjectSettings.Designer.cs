using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using Microsoft.VisualBasic.ApplicationServices;
using System.Threading.Channels;
using System.Xml;
using System.Xml.Linq;

namespace CodeFlowUI.Pages
{
    partial class ProjectSettings
    {
        private System.ComponentModel.IContainer components = null;

        private long projectId;
        private long userId;
        private ProjectOverviewDTO projectOverviewDTO;
        List<string> membersUsername;
        List<long> membersIdCopy;

        private Label pageTitleLabel;
        private Label projectNameLabel;
        private Label projectDescriptionLabel;
        private Label projectDueDateLabel;
        private Label projectMembersLabel;

        private RoundedTextBox nameTextBox;
        private RoundedTextBox descriptionTextBox;

        private Panel membersPanel;

        private DateTimePicker dueDatePicker;

        private Button homepageButton;
        private Button cancelProjectButton;
        private RoundedButton saveButton;

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
            this.Icon = new Icon(@"Resources\icon.ico");

            this.projectId = projectId;
            this.userId = userId;

            this.projectOverviewDTO = ProjectService.GetProjectOverviewById(projectId);

            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1280, 832);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CodeFlow";
            FormClosed += ProjectSettingsPage_FormClosed;

            InitScreen();
        }

        private void InitScreen()
        {
            InitLabels();
            InitButtons();
            InitTextBoxes();
            InitPanel();
        }

        private void InitPanel()
        {
            this.membersPanel = new Panel();
            this.membersPanel.Size = new Size(669, 110);
            this.membersPanel.Location = new Point(381, 402);
            this.membersPanel.BackColor = Color.White;
            this.membersPanel.AutoScroll = true;

            membersUsername = new List<string>();
            membersIdCopy = new List<long> (this.projectOverviewDTO.membersId);
            LoadMembers();

            this.Controls.Add(this.membersPanel);
        }

        private void CleanMembersPanel()
        {
            foreach (Control control in membersPanel.Controls.OfType<Control>().ToList())
            {
                membersPanel.Controls.Remove(control);
                control.Dispose();
            }
        }

        private void LoadMembers()
        {
            CleanMembersPanel();

            int xText = 22, yText = 16;
            int xTrash = 285, yTrash = 17;

            foreach (var memberId in projectOverviewDTO.membersId)
            {
                Label memberLabel = new Label();
                memberLabel.Text = UserService.GetUsersUsernameById(memberId);
                if(memberId == userId)
                    memberLabel.Text += " (Tech Leader)";
                memberLabel.Location = new Point(xText, yText);
                memberLabel.AutoSize = true;
                memberLabel.Font = new Font("Ubuntu", 12);

                this.membersPanel.Controls.Add(memberLabel);
                yText += memberLabel.Height + 8;
                
                if(memberId != userId)
                {
                    PictureBox deleteButton = new PictureBox();
                    deleteButton.Image = Image.FromFile(@"Resources\trash-can.png");
                    deleteButton.Location = new Point(xTrash, yTrash);
                    deleteButton.Size = new Size(22, 22);
                    deleteButton.Cursor = Cursors.Hand;
                    deleteButton.Click += new EventHandler((object sender, EventArgs e) =>
                    {
                        this.membersIdCopy.Remove(memberId);
                        this.membersUsername.Remove(memberLabel.Text);
                        LoadMembers();
                    });

                    this.membersPanel.Controls.Add(deleteButton);
                }

                yTrash += 20 + 9;

            }
        }

        private void InitTextBoxes()
        {
            nameTextBox = new RoundedTextBoxEditable(projectOverviewDTO.name, 572, 60);
            nameTextBox.Location = new Point(229, 209);
            nameTextBox.TextBox.Font = new Font("Ubuntu", 10);
            this.Controls.Add(nameTextBox);

            descriptionTextBox = new RoundedTextBoxEditable(projectOverviewDTO.description, 821, 60);
            descriptionTextBox.Location = new Point(229, 314);
            descriptionTextBox.TextBox.Font = new Font("Ubuntu", 10);
            descriptionTextBox.TextBox.Multiline = true;
            descriptionTextBox.TextBox.WordWrap = true;
            descriptionTextBox.TextBox.ScrollBars = ScrollBars.Vertical;
            descriptionTextBox.TextBox.Enabled = true;
            this.Controls.Add(descriptionTextBox);

            dueDatePicker = new DateTimePicker();
            dueDatePicker.Location = new Point(844, 209);
            dueDatePicker.Value = this.projectOverviewDTO.dueDate;
            dueDatePicker.MinDate = this.projectOverviewDTO.dueDate;

            this.Controls.Add(dueDatePicker);
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

            this.saveButton = new RoundedButton("SAVE", 224, 57, Colors.CallToActionButton, 32);
            this.saveButton.Location = new Point(528, 585);
            this.saveButton.Cursor = Cursors.Hand;
            this.saveButton.Click += saveButton_Click;

            this.Controls.Add(this.saveButton);

            this.cancelProjectButton = new System.Windows.Forms.Button();
            this.cancelProjectButton.Image = Image.FromFile(@"Resources\cancel-project-button.png");
            this.cancelProjectButton.Location = new Point(516, 761);
            this.cancelProjectButton.Size = new Size(247, 24);
            this.cancelProjectButton.BackColor = Color.Transparent;
            this.cancelProjectButton.FlatAppearance.BorderSize = 0;
            this.cancelProjectButton.FlatStyle = FlatStyle.Flat;
            this.cancelProjectButton.Cursor = Cursors.Hand;
            this.cancelProjectButton.Click += new EventHandler(cancelProjectButton_Click);
            this.Controls.Add(cancelProjectButton);
        }

        private void cancelProjectButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel this project?", "Careful", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                if (ProjectService.CancelProject(projectId))
                {
                    MessageBox.Show("Project canceled.");
                    new HomePage(new LoginResponseDTO(this.userId, true)).Show();
                    this.Hide();
                }
                    
                else
                    MessageBox.Show("There was a problem, try again.");
            }
                
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (WasThereAChange())
            {
                bool changes = true;

                if (this.nameTextBox.TextBox.Text != this.projectOverviewDTO.name)
                    changes = ChangeProjectName(this.nameTextBox.TextBox.Text);

                if (this.descriptionTextBox.TextBox.Text != this.projectOverviewDTO.description)
                    changes = changes && ChangeProjectDescription(this.descriptionTextBox.TextBox.Text);

                if (this.dueDatePicker.Value != this.projectOverviewDTO.dueDate)
                    changes = changes && ChangeProjectDueDate(this.dueDatePicker.Value);

                if (this.membersIdCopy.Count != this.projectOverviewDTO.membersId.Count)
                    changes = changes && ChangeProjectMembers(this.membersIdCopy);

                if (changes)
                    MessageBox.Show("Project updated succesfully!");
                else
                    MessageBox.Show("There was a problem updating.");
            }
            else
                MessageBox.Show("No changes.");
        }

        private bool ChangeProjectMembers(List<long> newMembersId)
        {
            bool changed = true;

            foreach (var member in this.projectOverviewDTO.membersId)
            {
                if (!newMembersId.Contains(member) && member != userId)
                {
                    changed = changed && ProjectService.RemoveMemberFromProject(projectId, member);
                }
                    
            }

            return changed;
        }

        private bool ChangeProjectDueDate(DateTime dueDate)
        {
            if (ProjectService.UpdateProjectDueDate(projectId, dueDate))
            {
                if (dueDate < DateTime.Today)
                    ProjectService.UpdateProjectStatus(projectId, ProjectStatus.Late);
                return true;
            }
                
            return false;
        }

        private bool ChangeProjectDescription(string description)
        {
            if (ProjectService.UpdateProjectDescription(projectId, description))
                return true;
            return false;
        }

        private bool ChangeProjectName(string newName)
        {
            if (ProjectService.UpdateProjectName(projectId, newName))
                return true;
            return false;
        }

        private bool WasThereAChange()
        {
            if (this.nameTextBox.TextBox.Text != this.projectOverviewDTO.name)
                return true;

            if (this.descriptionTextBox.TextBox.Text != this.projectOverviewDTO.description)
                return true;

            if (this.dueDatePicker.Value != this.projectOverviewDTO.dueDate)
                return true;

            if (this.membersIdCopy.Count != this.projectOverviewDTO.membersId.Count)
                return true;

            return false;
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

            InitDivisers();

            this.projectNameLabel = new Label();
            this.projectNameLabel.AutoSize = true;
            this.projectNameLabel.Text = "Name";
            this.projectNameLabel.Location = new Point(257, 192);
            this.projectNameLabel.Font = new Font("Ubuntu", 12);
            this.projectNameLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(projectNameLabel);

            this.projectDueDateLabel = new Label();
            this.projectDueDateLabel.AutoSize = true;
            this.projectDueDateLabel.Text = "Due date";
            this.projectDueDateLabel.Location = new Point(872, 192);
            this.projectDueDateLabel.Font = new Font("Ubuntu", 12);
            this.projectDueDateLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(projectDueDateLabel);

            this.projectDescriptionLabel = new Label();
            this.projectDescriptionLabel.AutoSize = true;
            this.projectDescriptionLabel.Text = "Description";
            this.projectDescriptionLabel.Location = new Point(257, 297);
            this.projectDescriptionLabel.Font = new Font("Ubuntu", 12);
            this.projectDescriptionLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(projectDescriptionLabel);

            this.projectMembersLabel = new Label();
            this.projectMembersLabel.AutoSize = true;
            this.projectMembersLabel.Text = "Members";
            this.projectMembersLabel.Location = new Point(257, 402);
            this.projectMembersLabel.Font = new Font("Ubuntu", 12);
            this.projectMembersLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(projectMembersLabel);

        }

        private void InitDivisers()
        {
            PictureBox diviser = new PictureBox();
            diviser.Image = Image.FromFile(@"Resources\small-vertical-line.png");
            diviser.Location = new Point(108, 41);
            diviser.Width = 2;
            diviser.Height = 56;

            this.Controls.Add(diviser);

            PictureBox bigHorizontalLine = new PictureBox();
            bigHorizontalLine.Location = new Point(355, 715);
            bigHorizontalLine.Image = Image.FromFile(@"Resources\big-horizontal-line.png");
            bigHorizontalLine.Size = new Size(570, 2);
            this.Controls.Add(bigHorizontalLine);
        }
    }
}