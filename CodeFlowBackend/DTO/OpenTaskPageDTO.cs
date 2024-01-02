using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class OpenTaskPageDTO
    {
        public long UserId { get; private set; }
        public long TaskId { get; private set; }
        public bool isUserTechLeader { get; private set; }

        public OpenTaskPageDTO(long userId, long taskId, bool isUserTechLeader)
        {
            UserId = userId;
            TaskId = taskId;
            this.isUserTechLeader = isUserTechLeader;
        }
    }
}
