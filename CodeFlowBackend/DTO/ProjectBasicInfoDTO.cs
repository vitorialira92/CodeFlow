using CodeFlowBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class ProjectBasicInfoDTO
    {
        public readonly long id;
        public readonly string name;
        public readonly string description;
        public readonly ProjectStatus status;
        public readonly DateTime dueDate;

        public ProjectBasicInfoDTO(long id, string name, string description, ProjectStatus status, DateTime dueDate)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.status = status;
            this.dueDate = dueDate;
        }
    }
}
