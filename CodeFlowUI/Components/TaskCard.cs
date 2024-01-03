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
using System.Formats.Asn1;

namespace CodeFlowUI.Components
{
    internal class TaskCard : Panel
    {
        public TaskCard(string taskName, DateTime dueDate,(int done, int total) checklist, string assignee, TasksStatus status, Tag? tag)
        {
            Width = 260;
            Height = 112;
            BorderStyle = BorderStyle.None;
            AddTaskName(taskName, status);
            AddDueDate(dueDate.Date);
            AddChecklist(checklist);
            if (dueDate < DateTime.Today)
                AddLateLabel();
            AddAssignee(assignee);
            if (tag != null)
                AddTag(tag);
        }

        private void AddTag(Tag tag)
        {
            Panel statusCircle = new Panel();
            statusCircle.Size = new Size(9, 9);
            statusCircle.BackColor = Colors.ProjectCardBackgroundColor;

            statusCircle.Paint += (sender, e) =>
            {
                SolidBrush brush = new SolidBrush(tag.Color);

                e.Graphics.FillEllipse(brush, 0, 0, statusCircle.Width, statusCircle.Height);

            };
            statusCircle.Location = new Point(175, 92);

            Controls.Add(statusCircle);

            Label tagLabel = new Label();
            tagLabel.Text = tag.Name;
            tagLabel.ForeColor = Colors.DarkBlue;
            tagLabel.Font = new Font("Ubuntu Light", 8);
            tagLabel.Location = new Point(191, 89);
            tagLabel.AutoSize = true;
            tagLabel.BackColor = Color.Transparent;
            Controls.Add(tagLabel);
        }

        private void AddChecklist((int done, int total) checklist)
        {
            PictureBox checklistPicture = new PictureBox();
            checklistPicture.Image = Image.FromFile(@"Resources\checklist.png");
            checklistPicture.Size = new Size(20,20);
            checklistPicture.Location = new Point(20, 61);

            Controls.Add(checklistPicture);

            Label statsLabel = new Label();
            statsLabel.Text = $"{checklist.done}/{checklist.done}";
            statsLabel.ForeColor = Colors.DarkBlue;
            statsLabel.AutoSize = true;
            statsLabel.Location = new Point(53,61);
            statsLabel.BackColor = Color.Transparent;
            statsLabel.Font = new Font("Ubuntu Light", 8);

            Controls.Add(statsLabel);

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
            Label lateLabel = new Label();
            lateLabel.Text = "late";
            lateLabel.ForeColor = Colors.ErrorColor;
            lateLabel.Font = new Font("Ubuntu Light", 8);
            lateLabel.Location = new Point(222, 39);
            lateLabel.AutoSize = true;
            lateLabel.BackColor = Color.Transparent;
            Controls.Add(lateLabel);
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
