using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Model.Project
{
    internal class Project
    {
        public long Id { get; private set; }    
        public string Name { get; private set; }
        public string Description { get; private set; }
        public long TechLeaderId { get; private set; }  
        public DateTime DueDate { get; private set; }
        public ProjectStatus Status { get; private set; }

        public Project(long id, string name, string description, long techLeaderId, DateTime dueDate, ProjectStatus status)
        {
            Id = id;
            Name = name;
            Description = description;
            TechLeaderId = techLeaderId;
            DueDate = dueDate;
            Status = status;
        }

    }
}
