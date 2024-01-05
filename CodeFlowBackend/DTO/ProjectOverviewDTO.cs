using CodeFlowBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class ProjectOverviewDTO
    {
        public readonly long id;
        public readonly string name;
        public readonly string description;
        public readonly DateTime dueDate;
        public List<long> membersId;

        public ProjectOverviewDTO(long id, string name, string description, DateTime dueDate, List<long> membersId)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.dueDate = dueDate;
            this.membersId = membersId;
        }
    }
}
