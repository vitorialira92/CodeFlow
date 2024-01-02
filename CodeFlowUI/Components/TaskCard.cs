using CodeFlowBackend.Model;
using CodeFlowUI.Styles;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using CodeFlowBackend.Model.Tasks;

namespace CodeFlowUI.Components
{
    internal class TaskCard : Panel
    {
        public TaskCard(string taskName, DateTime dueDate,(int done, int total) checklist, string assignee, TasksStatus status)
        {
            Width = 260;
            Height = 112;
            BorderStyle = BorderStyle.None;
            AddTaskName(taskName, status);
            AddDueDate(dueDate.Date);
            AddChecklist();
            if (dueDate < DateTime.Now)
                AddLateLabel();
            AddAssignee(assignee);
        }

        private void AddChecklist()
        {
            PictureBox checklist = new PictureBox();
            checklist.Image = Image.FromFile(@"Resources\checklist.png");
            checklist.Size = new Size(20,20);
            checklist.Location = new Point(20, 61);

            Controls.Add(checklist);
        }
    

        private void AddAssignee(string assignee)
        {
            Label assigneeLabel = new Label();
            assigneeLabel.Text = assignee;
            assigneeLabel.ForeColor = Colors.DarkBlue;
            assigneeLabel.Font = new Font("Ubuntu Light", 8);
            assigneeLabel.Location = new Point(20, 89);
            assigneeLabel.AutoSize = true;
            assigneeLabel.BackColor = Color.Transparent;
            Controls.Add(assigneeLabel);
        }

        private void AddLateLabel()
        {
            Label dueDateLabel = new Label();
            dueDateLabel.Text = "late";
            dueDateLabel.ForeColor = Colors.ErrorColor;
            dueDateLabel.Font = new Font("Ubuntu Light", 8);
            dueDateLabel.Location = new Point(219, 89);
            dueDateLabel.AutoSize = true;
            dueDateLabel.BackColor = Color.Transparent;
            Controls.Add(dueDateLabel);
        }

        private void AddStatus(TasksStatus status)
        {
            Panel statusCircle = new Panel();
            statusCircle.Size = new Size(9, 9);
            statusCircle.BackColor = Colors.ProjectCardBackgroundColor;

            statusCircle.Paint += (sender, e) =>
            {
                SolidBrush brush = new SolidBrush(GetStatusColor(status));

                e.Graphics.FillEllipse(brush, 0, 0, statusCircle.Width, statusCircle.Height);

            };
            statusCircle.Location = new Point(20, 19);

            Controls.Add(statusCircle);
        }
        private void AddDueDate(DateTime dueDate)
        {
            Label dueDateLabel = new Label();
            dueDateLabel.Text = $"Due {dueDate.ToString("MM/dd/yy")}";
            dueDateLabel.ForeColor = Colors.DarkBlue;
            dueDateLabel.Font = new Font("Ubuntu Light", 8);
            dueDateLabel.Location = new Point(20, 39);
            dueDateLabel.AutoSize = true;
            dueDateLabel.BackColor = Color.Transparent;
            Controls.Add(dueDateLabel);
        }
        private void AddTaskName(string taskName, TasksStatus status)
        {
            AddStatus(status);

            Label AddTaskName = new Label();
            AddTaskName.Text = taskName;
            AddTaskName.Font = new Font("Ubuntu", 14, FontStyle.Regular);
            AddTaskName.ForeColor = Colors.DarkBlue;
            AddTaskName.Location = new Point(32, 12);
            AddTaskName.AutoSize = true;
            AddTaskName.BackColor = Color.Transparent;
            Controls.Add(AddTaskName);
        }
        private Color GetStatusColor(TasksStatus status)
        {
            switch (status)
            {
                case TasksStatus.Review: return Colors.ReviewTask;
                case TasksStatus.Todo: return Colors.TodoTask;
                case TasksStatus.InProgress: return Colors.InProgressTask;
                default: return Colors.DoneTask;
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
