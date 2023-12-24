using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowUI.Styles
{
    using CodeFlowBackend.Model;
    internal class ProjectCard : Panel
    {
        public ProjectCard(string projectName, string description, DateTime dueDate, ProjectStatus status)
        {
            this.Width = 260;
            this.Height = 140;
            this.BorderStyle = BorderStyle.None;
            AddProjectName(projectName);
            AddDescription(description);
            AddDueDate(dueDate.Date);
            AddStatus(status);
        }

        private void AddStatus(ProjectStatus status)
        {
            Panel statusCircle = new Panel();
            statusCircle.Size = new Size(9, 9);
            statusCircle.BackColor = Colors.ProjectCardBackgroundColor; 

            statusCircle.Paint += (sender, e) =>
            {
                SolidBrush brush = new SolidBrush(GetStatusColor(status));
                
                    e.Graphics.FillEllipse(brush, 0, 0, statusCircle.Width, statusCircle.Height);
                
            };

            Label statusLabel = new Label();
            statusLabel.Text = GetStatusText(status);
            statusLabel.Font = new Font("Ubuntu Light", 8);
            statusLabel.AutoSize = true;
            statusLabel.BackColor = Color.Transparent;

            int xPositionStatusLabel = this.Width - 20 - statusLabel.Width;
            int xPositionStatusCircle = xPositionStatusLabel - 13;

            statusLabel.Location = new Point(xPositionStatusLabel, 113);
            statusCircle.Location = new Point(xPositionStatusCircle, 120);


            this.Controls.Add(statusLabel);

            this.Controls.Add(statusCircle);
        }

        private string GetStatusText(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.Late: return "late";
                case ProjectStatus.Open: return "open";
                case ProjectStatus.OnGoing: return "on going";
                case ProjectStatus.Done: return "done";
                default: return "canceled";
            }
        }

        private void AddDueDate(DateTime dueDate)
        {
            Label dueDateLabel = new Label();
            dueDateLabel.Text = $"Due {dueDate.ToString("dd/MM/yy")}";
            dueDateLabel.ForeColor = Colors.DarkBlue;
            dueDateLabel.Font = new Font("Ubuntu Light", 8);
            dueDateLabel.Location = new Point(20, 114);
            dueDateLabel.AutoSize = true;
            dueDateLabel.BackColor = Color.Transparent;
            this.Controls.Add(dueDateLabel);
        }

        private void AddDescription(string description)
        {
            Label descriptionLabel = new Label();
            descriptionLabel.Text = description;
            descriptionLabel.Font = new Font("Ubuntu Light", 8);
            descriptionLabel.Location = new Point(20, 43);
            descriptionLabel.AutoSize = true;
            descriptionLabel.BackColor = Color.Transparent;
            this.Controls.Add(descriptionLabel);
        }

        private void AddProjectName(string projectName)
        {
            Label projectNameLabel = new Label();
            projectNameLabel.Text = projectName;
            projectNameLabel.Font = new Font("Ubuntu", 14, FontStyle.Regular);
            projectNameLabel.ForeColor = Colors.DarkBlue;
            projectNameLabel.Location = new Point(20, 12);
            projectNameLabel.AutoSize = true;
            projectNameLabel.BackColor = Color.Transparent;
            this.Controls.Add(projectNameLabel);
        }
        private Color GetStatusColor(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.Late:return Colors.LateProject; 
                case ProjectStatus.Open: return Colors.OpenProject;
                case ProjectStatus.OnGoing: return Colors.OngoingProject;
                case ProjectStatus.Done: return Colors.DoneProject;
                default: return Colors.CanceledProject;  
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                var radius = 12;
                path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
                path.AddArc(new Rectangle(Width - radius - 1, 0, radius, radius), -90, 90);
                path.AddArc(new Rectangle(Width - radius - 1, Height - radius - 1, radius, radius), 0, 90);
                path.AddArc(new Rectangle(0, Height - radius - 1, radius, radius), 90, 90);
                path.CloseAllFigures();

                e.Graphics.FillPath(new SolidBrush(Colors.ProjectCardBackgroundColor), path);

                e.Graphics.DrawPath(new Pen(Colors.StrokeContainer, 1), path);
            }
        }
    }
}
