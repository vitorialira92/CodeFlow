using CodeFlowBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class CreateTaskDTO
    {
        public long ProjectId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Tag tag { get; private set; }
        public List<string> Checklist { get; private set; }
        public string AssigneeUsername { get; private set; }
        public DateTime? DueDate { get;private set; }

        public CreateTaskDTO(long projectId, string name, string description, Tag? tag, List<string>? checklist, string assigneeUsername, DateTime? dueDate)
        {
            ProjectId = projectId;
            Name = name;
            Description = description;
            this.tag = tag;
            Checklist = checklist;
            AssigneeUsername = assigneeUsername;
            DueDate = dueDate;
        }
    }
}
