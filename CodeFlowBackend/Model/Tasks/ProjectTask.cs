using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Model.Tasks
{
    public class ProjectTask
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime? DueDate { get; private set; }
        public string Assignee {  get; private set; }
        public TasksStatus Status { get; set; }
        public Tag Tag { get; set; }

        public List<ChecklistItem> Checklist { get; set; }

        public ProjectTask(long id, string name, string description, DateTime? dueDate, string assignee, TasksStatus status, Tag tag)
        {
            Id = id;
            Name = name;
            Description = description;
            DueDate = dueDate;
            Assignee = assignee;
            Status = status;
            Tag = tag;
        }

        public ProjectTask(long id, string name, string description, DateTime? dueDate, string assignee, TasksStatus status, Tag tag, List<ChecklistItem> checklist)
        {
            Id = id;
            Name = name;
            Description = description;
            DueDate = dueDate;
            Assignee = assignee;
            Status = status;
            Tag = tag;
            Checklist = checklist;
        }
    }
}
