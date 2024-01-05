

using CodeFlowBackend.DTO;
using CodeFlowBackend.Services;
using CodeFlowUI.Styles;

namespace CodeFlowUI.Pages
{
    using CodeFlowBackend.Model;
    using CodeFlowBackend.Model.User;
    using CodeFlowUI.Components;

    partial class HomePage
    {
        
        private System.ComponentModel.IContainer components = null;
        private Panel greetingPanel;
        private ProjectContainer projectContainer;
        private Button addProjectButton;
        private Button profileButton;
        private Label myProjectsPanel;
        private Label projectsStats;
        private List<ProjectCard> projectsCards;


        private List<ProjectBasicInfoDTO> projectBasicInfoDTOs;
        private LoginResponseDTO loginResponseDTO;
        private string userFirstName;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Rendering page

        private void InitializeComponent(LoginResponseDTO responseDTO)
        {
            this.Icon = new Icon(@"Resources\icon.ico");

            this.loginResponseDTO = responseDTO;
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 832);
            this.Text = "CodeFlow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            FormClosed += HomePage_FormClosed;
            InitData();
            InitScreen();
        }


        private void InitData()
        {
            this.projectBasicInfoDTOs = ProjectService.GetAllProjectsBasicInfoByUserId(loginResponseDTO.UserId!.Value);
            this.userFirstName = UserService.GetUserFirstNameByID(loginResponseDTO.UserId!.Value);
            CheckProjectStatus(); //sets project to late if needed
        }

        private void CheckProjectStatus()
        {
            for(int i = 0; i < projectBasicInfoDTOs.Count; i++)
            {
                var project = projectBasicInfoDTOs[i];
                if (project.dueDate < DateTime.Now && project.status != CodeFlowBackend.Model.ProjectStatus.Late)
                {
                    ProjectService.UpdateStatus(project.id, CodeFlowBackend.Model.ProjectStatus.Late);
                    ProjectBasicInfoDTO updatedProject = new ProjectBasicInfoDTO(project.id, project.name, project.description, ProjectStatus.Late, project.dueDate);
                    project = updatedProject;
                }
                    
            }
        }

        private void InitScreen()
        {
            InitProjects();
            InitGreeting();
            InitButtons();
        }

        private void InitProjects()
        {
            this.projectContainer = new ProjectContainer();
            this.projectContainer.Location = new Point(32, 120);
            this.Controls.Add(this.projectContainer);

            this.myProjectsPanel = new Label();
            this.myProjectsPanel.Text = "My projects";
            this.myProjectsPanel.Font = new Font("Ubuntu", 12);
            this.myProjectsPanel.Location = new Point(40, 32);
            this.myProjectsPanel.Size = new Size(150, 30);
            this.myProjectsPanel.ForeColor = Colors.DarkBlue;
            this.projectContainer.Controls.Add(myProjectsPanel);

            this.projectsCards = new List<ProjectCard>();
            int x = 0, y = 0;
            int count = 0;

            int doneCount = 0, ongoingCount = 0, lateCount = 0, openCount = 0, canceledCount = 0;

            Panel projectsCardsPanel = new Panel();
            projectsCardsPanel.BackColor = Color.Transparent;
            projectsCardsPanel.Size = new Size(1166, 482);
            projectsCardsPanel.Location = new Point(40, 92);
            projectsCardsPanel.AutoScroll = true;

            foreach (var project in projectBasicInfoDTOs)
            {
                count++;
                ProjectCard projectCard = new ProjectCard(project.name, project.description, project.dueDate, project.status);
                projectCard.Location = new Point(x, y);

                if (count % 4 == 0)
                {
                    x = 0;
                    y = y + 140 + 32;
                }
                else
                {
                    x = x + 32 + 260;
                }

                projectCard.Click += new EventHandler((object sender, EventArgs e) => {
                    new SpecificProjectPage(
                    new ProjectPageDTO(project.id, this.loginResponseDTO.UserId!.Value, this.loginResponseDTO.isTechLeader!.Value)).Show(); this.Hide();
                });
                projectCard.Cursor = Cursors.Hand;

                this.projectsCards.Add(projectCard);
                projectsCardsPanel.Controls.Add(projectCard);

                switch (project.status)
                {
                    case CodeFlowBackend.Model.ProjectStatus.Canceled: canceledCount++; break;
                    case CodeFlowBackend.Model.ProjectStatus.Done: doneCount++; break;
                    case CodeFlowBackend.Model.ProjectStatus.Late: lateCount++; break;
                    case CodeFlowBackend.Model.ProjectStatus.OnGoing: ongoingCount++; break;
                    default: openCount++; break;
                }

                this.projectContainer.Controls.Add(projectsCardsPanel);
            }

            this.addProjectButton = new Button();
            this.addProjectButton.Image = Image.FromFile(@"Resources\button-add.png");
            this.addProjectButton.Size = new Size(79, 84);
            this.addProjectButton.Location = new Point(1119, 511);
            this.addProjectButton.BackgroundImageLayout = ImageLayout.Zoom;
            this.addProjectButton.FlatStyle = FlatStyle.Flat;
            this.addProjectButton.FlatAppearance.BorderSize = 0;
            this.addProjectButton.BackColor = Color.Transparent;
            this.addProjectButton.Cursor = Cursors.Hand;
            this.addProjectButton.Click += new EventHandler((object sender, EventArgs e) =>
            {
                if (loginResponseDTO.isTechLeader!.Value)
                    new CreateProjectPage(loginResponseDTO.UserId!.Value).Show();
                else
                    new EnterProjectPage(loginResponseDTO.UserId!.Value).Show();
                this.Hide();
            });
            
            this.projectContainer.Controls.Add(addProjectButton);
            this.addProjectButton.BringToFront();


            this.projectsStats = new Label();
            this.projectsStats.Text = $"{ongoingCount} on going | {doneCount} done " +
                $"| {lateCount} late | {openCount} open | {canceledCount} canceled";
            this.projectsStats.Font = new Font("Ubuntu", 8);
            this.projectsStats.ForeColor = Colors.DarkBlue;
            this.projectsStats.AutoSize = true;
            this.projectContainer.Controls.Add(projectsStats);
            
            this.projectsStats.Location = new Point(1216 - this.projectsStats.Width - 40, 32);
            this.projectContainer.Invalidate();

        }

        private void InitGreeting()
        {
            Label greetingLabel = new Label();
            greetingLabel.Text = $"Hello, {this.userFirstName}";
            greetingLabel.Font = new Font("Ubuntu Bold", 40);
            greetingLabel.Location = new Point(32,32);
            greetingLabel.Size = new Size(1156, 74);
            greetingLabel.ForeColor = Colors.DarkBlue;
            this.Controls.Add(greetingLabel);
        }

        private void InitButtons()
        {
            this.profileButton = new Button();
            this.profileButton.Image = Image.FromFile(@"Resources\button-profile.png");
            this.profileButton.Size = new Size(65, 65);
            this.profileButton.Location = new Point(1188, 32);
            this.profileButton.FlatStyle = FlatStyle.Flat;
            this.profileButton.FlatAppearance.BorderSize = 0;
            this.profileButton.Cursor = Cursors.Hand;
            this.profileButton.Click += new EventHandler((object sender, EventArgs e) => {
                    new ProfilePage(this.loginResponseDTO.UserId!.Value).Show();
                    this.Hide();
                });

            this.Controls.Add(profileButton);
        }

        #endregion
    }
}