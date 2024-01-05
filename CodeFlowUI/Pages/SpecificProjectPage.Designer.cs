using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Model.Tasks;
using CodeFlowBackend.Services;
using CodeFlowUI.Components;
using CodeFlowUI.Styles;
using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection.PortableExecutable;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CodeFlowUI.Pages
{
    partial class SpecificProjectPage
    {
        private System.ComponentModel.IContainer components = null;

        private ProjectPageDTO projectPageDTO;
        private List<TaskCardDTO> taskCardsDTO;

        private Panel todoContainer;
        private Panel inProgressContainer;
        private Panel reviewContainer;
        private Panel doneContainer;
        private ProjectContainer projectContainer;
        private RoundedButton addTaskButton;
        private System.Windows.Forms.Button homeButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Button addDevelopersButton;

        private Label projectNameLabel;
        private Label todoLabel;
        private Label inProgressLabel;
        private Label reviewLabel;
        private Label doneLabel;
        private Label statsLabel;



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
            this.projectPageDTO = projectPageDTO;
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1280, 832);
            this.Name = "SpecificProjectPage";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CodeFlow";
            FormClosed += SpecificProjectPage_FormClosed;

            InitScreen();
        }

        private void InitScreen()
        {
            InitLogo();
            InitContainers();
            InitLabels();
            InitButtons();
            if (this.projectPageDTO.IsUserTechLeader)
                InitTasksTechLeader();
            else
                InitTasksDeveloper();
        }

        private void InitTasksDeveloper()
        {
            int x = 9, yTodo = 8, yInProgress = 8, yReview = 8, yDone = 8;

            int todoCount = 0, inProgressCount = 0, reviewCount = 0, doneCount = 0;

            this.taskCardsDTO = ProjectService.GetAllTasksByProjectIdAndByUserId(this.projectPageDTO.ProjectId, this.projectPageDTO.UserId);
            
            List<string> tags = new List<string>();

            List<TaskCardDTO> taskCardsCopy = new List<TaskCardDTO>(this.taskCardsDTO);

            foreach (var task in taskCardsDTO)
            {
                if (!tags.Contains(task.Tag.Name))
                {
                    tags.Add(task.Tag.Name);
                    List<TaskCardDTO> list = new List<TaskCardDTO>(ProjectService.GetAllTasksByProjectIdAndTagId(this.projectPageDTO.ProjectId, task.Tag.Id));
                    foreach (var item in list)
                    {
                        if (!taskCardsCopy.Contains(item))
                            taskCardsCopy.Add(item);
                    }

                }
            }

            this.taskCardsDTO = new List<TaskCardDTO>(taskCardsCopy);

            foreach (var task in taskCardsDTO)
            {
                TaskCard taskCard = new TaskCard(task.Name, task.DueDate, task.Checklist, task.Assignee, task.Status, task.Tag);

                int y;


                switch (task.Status)
                {
                    case TasksStatus.Review:
                        y = yReview;
                        yReview += 112 + 9;
                        reviewCount++;
                        this.reviewContainer.Controls.Add(taskCard);
                        break;
                    case TasksStatus.Todo:
                        y = yTodo;
                        yTodo += 112 + 9;
                        todoCount++;
                        this.todoContainer.Controls.Add(taskCard);
                        break;
                    case TasksStatus.InProgress:
                        y = yInProgress;
                        yInProgress += 112 + 9;
                        inProgressCount++;
                        this.inProgressContainer.Controls.Add(taskCard);
                        break;
                    default:
                        y = yDone;
                        yDone += 112 + 9;
                        doneCount++;
                        this.doneContainer.Controls.Add(taskCard);
                        break;
                }

                taskCard.Location = new Point(x, y);
                taskCard.Click += new EventHandler((object sender, EventArgs e) =>
                {
                    this.Hide();
                    new SpecificTaskPage(new OpenTaskPageDTO(this.projectPageDTO.UserId, task.Id, this.projectPageDTO.IsUserTechLeader)).Show();
                });
            }

            InitStats(todoCount, inProgressCount, reviewCount, doneCount);
        }

        private void InitTasksTechLeader()
        {

            int x = 9, yTodo = 8, yInProgress = 8, yReview = 8, yDone = 8;
            
            int todoCount = 0, inProgressCount = 0, reviewCount = 0, doneCount = 0;

            this.taskCardsDTO = ProjectService.GetAllTasksByProjectId(this.projectPageDTO.ProjectId);

            foreach(var task in taskCardsDTO)
            {
                TaskCard taskCard = new TaskCard(task.Name, task.DueDate, task.Checklist, task.Assignee, task.Status, task.Tag);

                int y;
                

                switch (task.Status)
                {
                    case TasksStatus.Review:
                        y = yReview; 
                        yReview += 112 + 9;
                        reviewCount++;
                        this.reviewContainer.Controls.Add(taskCard); 
                        break;
                    case TasksStatus.Todo:
                        y = yTodo; 
                        yTodo += 112 + 9;
                        todoCount++;
                        this.todoContainer.Controls.Add(taskCard); 
                        break;
                    case TasksStatus.InProgress: 
                        y = yInProgress; 
                        yInProgress += 112 + 9;
                        inProgressCount++;
                        this.inProgressContainer.Controls.Add(taskCard); 
                        break;
                    default: 
                        y = yDone; 
                        yDone += 112 + 9;
                        doneCount++;
                        this.doneContainer.Controls.Add(taskCard); 
                        break;
                }
                
                taskCard.Location = new Point(x, y);
                taskCard.Click += new EventHandler((object sender, EventArgs e) =>
                {
                    this.Hide();
                    new SpecificTaskPage(new OpenTaskPageDTO(this.projectPageDTO.UserId, task.Id, this.projectPageDTO.IsUserTechLeader)).Show();
                });
            }

            InitStats(todoCount, inProgressCount, reviewCount, doneCount);
        }

        private void InitStats(int todoCount, int inProgressCount, int reviewCount, int doneCount)
        {
            string fullStats = $"{todoCount} todo | {inProgressCount} in progress | {reviewCount} in review | {doneCount} done";

            Label fullStatsLabel = new Label();
            fullStatsLabel.Text = fullStats;
            fullStatsLabel.Location = new Point(36,748);
            fullStatsLabel.AutoSize = true;
            fullStatsLabel.Font = new Font("Ubuntu", 12);

            this.Controls.Add(fullStatsLabel);

            int sum = todoCount + inProgressCount + reviewCount + doneCount;
            if(sum!=0)
            {
                double donePercent = (double)100 * doneCount / sum;

                Label donePercentLabel = new Label();
                donePercentLabel.Text = $"{donePercent}% done";
                donePercentLabel.Location = new Point(1139, 743);
                donePercentLabel.AutoSize = true;
                donePercentLabel.Font = new Font("Ubuntu", 12);
                if (donePercent > 50)
                    donePercentLabel.ForeColor = Colors.DoneTask;

                this.Controls.Add(donePercentLabel);
            }
            
        }

        private void InitLabels()
        {
            AddProjectStatus(ProjectService.GetProjectStatusById(this.projectPageDTO.ProjectId));

            this.projectNameLabel = new Label();
            this.projectNameLabel.Text = ProjectService.GetProjectNameById(this.projectPageDTO.ProjectId);
            this.projectNameLabel.Location = new Point(160, 39);
            this.projectNameLabel.AutoSize = true;
            this.projectNameLabel.ForeColor = Color.DarkBlue;
            this.projectNameLabel.Font = new Font("Ubuntu", 32);

            this.Controls.Add(this.projectNameLabel);

            this.todoLabel = new Label();
            this.todoLabel.Text = "Todo";
            this.todoLabel.Location = new Point(135,34);
            this.todoLabel.AutoSize = true;
            this.todoLabel.ForeColor = Color.DarkBlue;
            this.todoLabel.Font = new Font("Ubuntu", 24);

            this.projectContainer.Controls.Add(this.todoLabel);

            this.inProgressLabel = new Label();
            this.inProgressLabel.Text = "In Progress";
            this.inProgressLabel.Location = new Point(368, 34);
            this.inProgressLabel.AutoSize = true;
            this.inProgressLabel.ForeColor = Color.DarkBlue;
            this.inProgressLabel.Font = new Font("Ubuntu", 24);

            this.projectContainer.Controls.Add(this.inProgressLabel);

            this.reviewLabel = new Label();
            this.reviewLabel.Text = "Review";
            this.reviewLabel.Location = new Point(693, 34);
            this.reviewLabel.AutoSize = true;
            this.reviewLabel.ForeColor = Color.DarkBlue;
            this.reviewLabel.Font = new Font("Ubuntu", 24);

            this.projectContainer.Controls.Add(this.reviewLabel);
            
            this.doneLabel = new Label();
            this.doneLabel.Text = "Done";
            this.doneLabel.Location = new Point(1001, 34);
            this.doneLabel.AutoSize = true;
            this.doneLabel.ForeColor = Color.DarkBlue;
            this.doneLabel.Font = new Font("Ubuntu", 24);

            this.projectContainer.Controls.Add(this.doneLabel);

        }

        private void AddProjectStatus(ProjectStatus status)
        {
            Panel statusCircle = new Panel();
            statusCircle.Size = new Size(28, 28);
            statusCircle.BackColor = Color.White;

            statusCircle.Paint += (sender, e) =>
            {
                SolidBrush brush = new SolidBrush(GetStatusColor(status));

                e.Graphics.FillEllipse(brush, 0, 0, statusCircle.Width, statusCircle.Height);

            };
            statusCircle.Location = new Point(120, 54);

            this.Controls.Add(statusCircle);
        }

        private Color GetStatusColor(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.Late: return Colors.LateProject;
                case ProjectStatus.Open: return Colors.OpenProject;
                case ProjectStatus.OnGoing: return Colors.OngoingProject;
                case ProjectStatus.Canceled: return Colors.CanceledProject;
                default: return Colors.DoneProject;
            }
        }

        private void InitButtons()
        {
            this.addTaskButton = new RoundedButton("+ ADD TASK", 263, 45, Colors.CallToActionButton, 12);
            this.addTaskButton.Location = new Point(40,533);
            this.addTaskButton.Cursor = Cursors.Hand;
            this.addTaskButton.Click += new EventHandler(addTask_Click);

            this.projectContainer.Controls.Add(addTaskButton);

            this.homeButton = new System.Windows.Forms.Button();
            this.homeButton.Location = new Point(54,46);
            this.homeButton.Size = new Size(40,43);
            this.homeButton.FlatStyle = FlatStyle.Flat;
            this.homeButton.FlatAppearance.BorderSize = 0;
            this.homeButton.Cursor = Cursors.Hand;
            this.homeButton.Image = Image.FromFile(@"Resources\home.png");
            this.homeButton.Click += new EventHandler( (object sender, EventArgs e) =>
            {
                this.Hide();
                new HomePage(new LoginResponseDTO(this.projectPageDTO.UserId, this.projectPageDTO.IsUserTechLeader)).Show();
            });

            this.Controls.Add(homeButton);

            if(this.projectPageDTO.IsUserTechLeader)
            {
                this.settingsButton = new System.Windows.Forms.Button();
                this.settingsButton.Location = new Point(1150, 49);
                this.settingsButton.Size = new Size(40, 40);
                this.settingsButton.FlatStyle = FlatStyle.Flat;
                this.settingsButton.FlatAppearance.BorderSize = 0;
                this.settingsButton.Cursor = Cursors.Hand;
                this.settingsButton.Image = Image.FromFile(@"Resources\setting.png");
                this.settingsButton.Click += new EventHandler((object sender, EventArgs e) =>
                {
                    this.Hide();
                    new ProjectSettings(this.projectPageDTO.ProjectId, this.projectPageDTO.UserId).Show();
                });

                this.Controls.Add(settingsButton);

                this.addDevelopersButton = new System.Windows.Forms.Button();
                this.addDevelopersButton.Location = new Point(1200, 46);
                this.addDevelopersButton.Size = new Size(48, 47);
                this.addDevelopersButton.FlatStyle = FlatStyle.Flat;
                this.addDevelopersButton.FlatAppearance.BorderSize = 0;
                this.addDevelopersButton.Cursor = Cursors.Hand;
                this.addDevelopersButton.Image = Image.FromFile(@"Resources\invite-to-project-button.png");
                this.addDevelopersButton.Click += new EventHandler((object sender, EventArgs e) =>
                {
                    this.Hide();
                    new InviteToProjectPage(this.projectPageDTO.ProjectId, this.projectPageDTO.UserId).Show();
                });

                this.Controls.Add(addDevelopersButton);
            }

        }

        private void addTask_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CreateTaskPage(this.projectPageDTO).Show();
        }

        private void InitLogo()
        {
            this.Icon = new Icon(@"Resources\icon.ico");

        }

        private void InitContainers()
        {
            this.projectContainer = new ProjectContainer();
            this.projectContainer.Location = new Point(32, 118);
            this.Controls.Add(projectContainer);

            this.todoContainer = new Panel();
            this.todoContainer.Size = new Size(281, 440);
            this.todoContainer.Location = new Point(31, 86);
            this.todoContainer.BackColor = Color.White;
            this.todoContainer.AutoScroll = true;

            this.projectContainer.Controls.Add(todoContainer);

            this.inProgressContainer = new Panel();
            this.inProgressContainer.Size = new Size(281, 492);
            this.inProgressContainer.Location = new Point(321, 86);
            this.inProgressContainer.BackColor = Color.White;
            this.inProgressContainer.AutoScroll = true;

            this.projectContainer.Controls.Add(inProgressContainer);

            this.reviewContainer = new Panel();
            this.reviewContainer.Size = new Size(281, 492);
            this.reviewContainer.Location = new Point(612, 86);
            this.reviewContainer.BackColor = Color.White;
            this.reviewContainer.AutoScroll = true;

            this.projectContainer.Controls.Add(reviewContainer);

            this.reviewContainer = new Panel();
            this.reviewContainer.Size = new Size(281, 492);
            this.reviewContainer.Location = new Point(903, 86);
            this.reviewContainer.BackColor = Color.White;
            this.reviewContainer.AutoScroll = true;

            this.projectContainer.Controls.Add(reviewContainer);

            InitDivisers();
        }

        private void InitDivisers()
        {
            PictureBox diviser1 = new PictureBox();
            diviser1.Image = Image.FromFile(@"Resources\big-vertical-line.png");
            diviser1.Location = new Point(317, 94);
            diviser1.Width = 2;
            diviser1.Height = 484;

            this.projectContainer.Controls.Add(diviser1);

            PictureBox diviser2 = new PictureBox();
            diviser2.Image = Image.FromFile(@"Resources\big-vertical-line.png");
            diviser2.Location = new Point(608, 94);
            diviser2.Width = 2;
            diviser2.Height = 484;

            this.projectContainer.Controls.Add(diviser2);

            PictureBox diviser3 = new PictureBox();
            diviser3.Image = Image.FromFile(@"Resources\big-vertical-line.png");
            diviser3.Location = new Point(899, 94);
            diviser3.Width = 2;
            diviser3.Height = 484;

            this.projectContainer.Controls.Add(diviser3);

            PictureBox diviser4 = new PictureBox();
            diviser4.Image = Image.FromFile(@"Resources\small-vertical-line.png");
            diviser4.Location = new Point(108, 41);
            diviser4.Width = 2;
            diviser4.Height = 56;

            this.Controls.Add(diviser4);
        }
    }
}