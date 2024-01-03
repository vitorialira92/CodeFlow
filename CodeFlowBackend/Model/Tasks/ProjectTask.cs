using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Model.Tasks
{
    internal class ProjectTask
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public string Assignee {  get; private set; }
        public TasksStatus Status { get; private set; }
        public Tag tag { get; private set; }

        public ProjectTask(long id, string name, string description, DateTime dueDate, string assignee, TasksStatus status, Tag tag)
        {
            Id = id;
            Name = name;
            Description = description;
            DueDate = dueDate;
            Assignee = assignee;
            Status = status;
            this.tag = tag;
        }
    }
}
