using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class CreateProjectDTO
    {
        public long TechLeaderId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }

        public CreateProjectDTO(long techLeaderId, string name, string description, DateTime dueDate)
        {
            TechLeaderId = techLeaderId;
            Name = name;
            Description = description;
            DueDate = dueDate;
        }
    }
}
