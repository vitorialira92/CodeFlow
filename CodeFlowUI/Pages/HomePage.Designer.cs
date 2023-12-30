

using CodeFlowBackend.DTO;
using CodeFlowBackend.Services;
using CodeFlowUI.Styles;

namespace CodeFlowUI.Pages
{
    using CodeFlowBackend.Model.User;
    using CodeFlowUI.Components;

    partial class HomePage
    {
        
        private System.ComponentModel.IContainer components = null;
        private Panel greetingPanel;
        private ProjectContainer projectContainer;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent(LoginResponseDTO responseDTO)
        {
            //this.user = UserService.GetUserBasicInfo(userId);
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 832);
            this.Text = "CodeFlow";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            InitScreen();
        }

        private void InitScreen()
        {

            this.projectContainer = new ProjectContainer();
            this.projectContainer.Location = new Point(32,120);
            Controls.Add(this.projectContainer);
            ProjectCard projectCard = new ProjectCard("project name", "description", DateTime.Today, CodeFlowBackend.Model.ProjectStatus.Done);
            projectCard.Location = new Point(40,92);
            //Controls.Add(projectCard);
            this.projectContainer.Controls.Add(projectCard);
            projectCard.Click += new EventHandler(projectCard_onClick);
            InitGreeting();
            InitButtons();
        }

        private void projectCard_onClick(object sender, EventArgs e)
        {
            
        }

        private void InitGreeting()
        {
            Label greetingLabel = new Label();
            //greetingLabel.Text = $"Hello, {user.firstName}";
            greetingLabel.Font = new Font("Ubuntu Bold", 40);
            greetingLabel.Location = new Point(32,32);
            greetingLabel.Size = new Size(1156, 74);
            greetingLabel.ForeColor = Colors.DarkBlue;
            this.Controls.Add(greetingLabel);
        }

        private void InitButtons()
        {
            
        }

        #endregion
    }
}