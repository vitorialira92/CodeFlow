using CodeFlowBackend.Model.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class TaskCardDTO
    {
        public long Id { get; private set; }    
        public string Name { get; private set; }
        public DateTime DueDate { get; private set; }
        public string Assignee { get; private set; }
        public (int done, int total) Checklist { get; private set; }
        public TasksStatus Status { get; private set; } 

        public TaskCardDTO(long id, string name, DateTime dueDate, string assignee, (int done, int total) checklist, TasksStatus status)
        {
            Id = id;
            Name = name;
            DueDate = dueDate;
            Assignee = assignee;
            Checklist = checklist;
            Status = status;
        }
    }
}
