using CodeFlowBackend.DTO;
using CodeFlowUI.Styles;
using System.Xml;
using CodeFlowBackend.Model.Tasks;
using CodeFlowUI.Components;
using CodeFlowBackend.Services;
using CodeFlowBackend.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;

namespace CodeFlowUI.Pages
{
    partial class SpecificTaskPage
    {
        private System.ComponentModel.IContainer components = null;

        private OpenTaskPageDTO openTaskPageDTO;

        List<ChecklistItem> checklistCopy;

        private List<Tag> allTags;
        private Tag selectedTag;

        private ProjectTask projectTask;

        private Label pageTitleLabel;
        private Label statusLabel;
        private Label descriptionLabel;
        private Label assigneeLabel;
        private Label dueDateLabel;
        private Label tagLabel;
        private Label checklistLabel;

        private Panel tagColorPanel;
        private Panel statusColorPanel;

        private CheckedListBox checkList;

        private System.Windows.Forms.ComboBox tagComboBox;
        private System.Windows.Forms.ComboBox statusComboBox;
        private DateTimePicker dueDatePicker;
        private RoundedTextBox descriptionTextBox;
        private RoundedTextBox assigneeTextBox;
        private Panel checklistPanel;
        private System.Windows.Forms.Button homepageButton;
        private RoundedButton saveButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        

        private void InitializeComponent(OpenTaskPageDTO openTaskPageDTO)
        {
            this.Icon = new Icon(@"Resources\icon.ico");

            this.openTaskPageDTO = openTaskPageDTO;

            this.projectTask = ProjectService.GetTaskById(this.openTaskPageDTO.TaskId);

            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 832);
            this.Text = "CodeFlow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            FormClosed += SpecificTaskPage_FormClosed;

            InitScreen();
        }

        private void InitScreen()
        {
            InitLabels();
            InitTextBoxes();
            InitPanel();
            InitComboBoxes();
            InitButtons();

        }

        private void InitComboBoxes()
        {
            this.tagComboBox = new System.Windows.Forms.ComboBox();
            this.tagComboBox.Location = new Point(737, 478);
            this.tagComboBox.Size = new Size(264, 60);
            this.tagComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.Controls.Add(this.tagComboBox);

            if (!this.openTaskPageDTO.isUserTechLeader)
                this.tagComboBox.Enabled = false;

            this.tagColorPanel = new Panel();
            this.tagColorPanel.Size = new Size(15, 29);
            this.tagColorPanel.Location = new Point(707, 478);

            this.Controls.Add(tagColorPanel);

            LoadTagOptions();

            this.statusComboBox = new System.Windows.Forms.ComboBox();
            this.statusComboBox.Location = new Point(737, 386);
            this.statusComboBox.Size = new Size(264, 60);
            this.statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.Controls.Add(this.statusComboBox);

            if (!this.openTaskPageDTO.isUserTechLeader)
                this.statusComboBox.Enabled = false;

            this.statusColorPanel = new Panel();
            this.statusColorPanel.Size = new Size(15, 29);
            this.statusColorPanel.Location = new Point(707, 389);

            this.Controls.Add(statusColorPanel);

            LoadStatusOptions();
        }

        private void LoadStatusOptions()
        {
            this.statusComboBox.Items.Add("Todo");
            this.statusComboBox.Items.Add("In progress");
            this.statusComboBox.Items.Add("Review");
            this.statusComboBox.Items.Add("Done");

            ChangeStatusColor(this.projectTask.Status);

            this.statusComboBox.SelectedIndex = (int) this.projectTask.Status - 1;

            this.statusComboBox.SelectedIndexChanged += new EventHandler(statusComboBox_SelectedIndexChanged);
        }

        private void statusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.openTaskPageDTO.isUserTechLeader && this.statusComboBox.SelectedIndex == 4)
            {
                this.statusComboBox.SelectedIndex = (int) this.projectTask.Status - 1;
            }

            ChangeStatusColor((TasksStatus) statusComboBox.SelectedIndex + 1);
        }

        private void ChangeStatusColor(TasksStatus status)
        {
            switch (status)
            {
                case TasksStatus.Todo: this.statusColorPanel.BackColor = Colors.TodoTask;break;
                case TasksStatus.InProgress: this.statusColorPanel.BackColor = Colors.InProgressTask; break;
                case TasksStatus.Review: this.statusColorPanel.BackColor = Colors.ReviewTask; break;
                case TasksStatus.Done: this.statusColorPanel.BackColor = Colors.DoneTask; break;
            }
        }

        private void LoadTagOptions()
        {
            this.tagComboBox.Items.Clear();

            this.tagComboBox.Items.Add("None");

            allTags = ProjectService.GetAllTagsByProjectId(this.openTaskPageDTO.ProjectId);

            int index = 0, count = 0;
            
            foreach (Tag tag in allTags)
            {
                count++;
                if (tag.Id == this.projectTask.Tag.Id)
                {
                    ChangeTagColor(tag.Name);
                    index = count;
                }
                    
                this.tagComboBox.Items.Add(tag.Name);
            }
            this.tagComboBox.SelectedIndex = count;

            this.tagComboBox.SelectedIndexChanged += new EventHandler(tagComboBox_SelectedIndexChanged);

        }

        private void tagComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTagColor(tagComboBox.SelectedItem.ToString());
        }

        private void ChangeTagColor(string tagName)
        {

            var tag = allTags.Where(x => x.Name.Equals(tagName)).FirstOrDefault();
            this.selectedTag = tag;
            if (tag != null)
                this.tagColorPanel.BackColor = tag.Color;
            else
                this.tagColorPanel.BackColor = Color.White;
        }

        private void InitPanel()
        {
            this.checklistPanel = new Panel();
            this.checklistPanel.Size = new Size(332, 227);
            this.checklistPanel.Location = new Point(188, 394);
            this.checklistPanel.BackColor = Color.White;
            this.checklistPanel.AutoScroll = true;

            this.Controls.Add(this.checklistPanel);

            LoadChecklist();
        }

        private void LoadChecklist()
        {
            checkList = new CheckedListBox();
            checkList.Location = new Point(22, 16);
            checkList.CheckOnClick = true;
            checkList.Size = new Size(332, 227);


            this.checklistCopy = new List<ChecklistItem>(this.projectTask.Checklist);

            for (int i = 0; i < this.projectTask.Checklist.Count; i++)
            {
                var item = this.projectTask.Checklist[i];
                checkList.Items.Add(item.Name, item.IsChecked);
            }

            checkList.ItemCheck += new ItemCheckEventHandler(CheckListItemCheck);
            this.checklistPanel.Controls.Add(checkList);
        }

        private void CheckListItemCheck(object sender, ItemCheckEventArgs e)
        {
            var item = this.checklistCopy[e.Index];

            item.IsChecked = e.NewValue == CheckState.Checked; ;
        }

        private void InitTextBoxes()
        {
            dueDatePicker = new DateTimePicker();
            dueDatePicker.Location = new Point(737, 276);
            dueDatePicker.MinDate = DateTime.Today;
            dueDatePicker.Value = this.projectTask.DueDate;
            if (!this.openTaskPageDTO.isUserTechLeader)
                this.dueDatePicker.Enabled = false;

            this.Controls.Add(dueDatePicker);

            descriptionTextBox = new RoundedTextBox("", 821, 60);
            descriptionTextBox.Location = new Point(188, 171);
            descriptionTextBox.TextBox.Text = this.projectTask.Description;
            descriptionTextBox.TextBox.Font = new Font("Ubuntu", 10);
            descriptionTextBox.TextBox.MaxLength = 150;
            this.Controls.Add(descriptionTextBox);

            assigneeTextBox = new RoundedTextBoxEditable(UserService.GetUsersUsernameById(this.openTaskPageDTO.UserId), 442, 60);
            assigneeTextBox.Location = new Point(188, 276);
            assigneeTextBox.TextBox.Font = new Font("Ubuntu", 10);
            assigneeTextBox.Leave += new EventHandler(assigneeTextBox_Leave);
            if (!this.openTaskPageDTO.isUserTechLeader)
                this.assigneeTextBox.TextBox.Enabled = false;
            this.Controls.Add(assigneeTextBox);
        }

        private void assigneeTextBox_Leave(object sender, EventArgs e)
        {

            if (UserService.IsUsernameAvailable(assigneeTextBox.TextBox.Text))
            {
                MessageBox.Show("This user doesn't exist");
            }
            else if (!ProjectService.IsUserOnProject(assigneeTextBox.TextBox.Text, this.openTaskPageDTO.ProjectId))
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
                new HomePage(new LoginResponseDTO(this.openTaskPageDTO.UserId, this.openTaskPageDTO.isUserTechLeader)).Show();
            });

            this.Controls.Add(homepageButton);

            this.saveButton = new RoundedButton("SAVE", 224, 57, Colors.CallToActionButton, 32);
            this.saveButton.Location = new Point(528, 672);
            this.saveButton.Cursor = Cursors.Hand;
            this.saveButton.Click += saveButton_Click;

            this.Controls.Add(this.saveButton);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (WasThereChanges())
            {
                bool update = true;
                if (this.descriptionTextBox.TextBox.Text != this.projectTask.Description)
                    update = update && ProjectService.UpdateTaskDescription(this.projectTask.Id, this.descriptionTextBox.TextBox.Text);

                if (this.assigneeTextBox.TextBox.Text != this.projectTask.Assignee)
                    update = update && ProjectService.UpdateTaskAssignee(this.projectTask.Id, this.assigneeTextBox.TextBox.Text);

                if (this.dueDatePicker.Value != this.projectTask.DueDate)
                    update = update && ProjectService.UpdateTaskDueDate(this.projectTask.Id, this.dueDatePicker.Value);

                if (this.tagComboBox.SelectedText != this.projectTask.Tag.Name)
                    update = update && ProjectService.UpdateTaskTag(this.projectTask.Id, this.allTags.ElementAt(this.tagComboBox.SelectedIndex-1));
                
                foreach(var item in this.projectTask.Checklist)
                {
                    if(!checklistCopy.Contains(item))
                        update = update && ProjectService.UpdateTaskChecklist(this.openTaskPageDTO.ProjectId, this.projectTask.Id, item);
                }

                if (this.statusComboBox.SelectedIndex + 1 != (int)this.projectTask.Status)
                {
                    update = update && ProjectService.UpdateTaskStatus(this.openTaskPageDTO.ProjectId, this.openTaskPageDTO.TaskId,
                        this.statusComboBox.SelectedIndex + 1);
                }


                    if (update)
                {
                    MessageBox.Show("Updated succesfully!");
                    this.Hide();
                    new SpecificProjectPage(new ProjectPageDTO(this.openTaskPageDTO.ProjectId, this.openTaskPageDTO.UserId, this.openTaskPageDTO.isUserTechLeader)).Show();
                }
                else
                    MessageBox.Show("There was an error updating.");
                    
            }
            else
                MessageBox.Show("No changes made.");
        }

        private bool WasThereChanges()
        {
            if (this.descriptionTextBox.TextBox.Text != this.projectTask.Description)
                return true;
            if (this.assigneeTextBox.TextBox.Text != this.projectTask.Assignee)
                return true;
            if (this.dueDatePicker.Value != this.projectTask.DueDate)
                return true;
            if (this.tagComboBox.SelectedText != this.projectTask.Tag.Name)
                return true;
            if(this.checklistCopy != this.projectTask.Checklist) 
                return true;
            if (this.statusComboBox.SelectedIndex + 1 != (int)this.projectTask.Status)
                return true;
            return false;
        }

        private void InitLabels()
        {
            this.pageTitleLabel = new Label();
            this.pageTitleLabel.Text = this.projectTask.Name;
            this.pageTitleLabel.Location = new Point(120, 39);
            this.pageTitleLabel.AutoSize = true;
            this.pageTitleLabel.ForeColor = Color.DarkBlue;
            this.pageTitleLabel.Font = new Font("Ubuntu", 32);

            this.Controls.Add(pageTitleLabel);

            InitDiviser();

            this.statusLabel = new Label();
            this.statusLabel.AutoSize = true;
            this.statusLabel.Text = "Status";
            this.statusLabel.Location = new Point(765, 369);
            this.statusLabel.Font = new Font("Ubuntu", 12);
            this.statusLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(statusLabel);

            this.dueDateLabel = new Label();
            this.dueDateLabel.AutoSize = true;
            this.dueDateLabel.Text = "Due date";
            this.dueDateLabel.Location = new Point(765, 259);
            this.dueDateLabel.Font = new Font("Ubuntu", 12);
            this.dueDateLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(dueDateLabel);

            this.descriptionLabel = new Label();
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Text = "Description";
            this.descriptionLabel.Location = new Point(216, 154);
            this.descriptionLabel.Font = new Font("Ubuntu", 12);
            this.descriptionLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(descriptionLabel);

            this.assigneeLabel = new Label();
            this.assigneeLabel.AutoSize = true;
            this.assigneeLabel.Text = "Assignee";
            this.assigneeLabel.Location = new Point(216, 259);
            this.assigneeLabel.Font = new Font("Ubuntu", 12);
            this.assigneeLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(assigneeLabel);

            this.tagLabel = new Label();
            this.tagLabel.AutoSize = true;
            this.tagLabel.Text = "Tag";
            this.tagLabel.Location = new Point(765, 461);
            this.tagLabel.Font = new Font("Ubuntu", 12);
            this.tagLabel.ForeColor = Colors.DarkBlue;

            this.Controls.Add(tagLabel);

            this.checklistLabel = new Label();
            this.checklistLabel.AutoSize = true;
            this.checklistLabel.Text = "Checklist";
            this.checklistLabel.Location = new Point(216, 369);
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