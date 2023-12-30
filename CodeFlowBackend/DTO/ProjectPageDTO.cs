using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class ProjectPageDTO
    {
        public long ProjectId { get; private set; }
        public long UserId { get; private set; }
        public bool IsUserTechLeader {  get; private set; }

        public ProjectPageDTO(long projectId, long userId, bool isUserTechLeader)
        {
            ProjectId = projectId;
            UserId = userId;
            IsUserTechLeader = isUserTechLeader;
        }
    }
}
