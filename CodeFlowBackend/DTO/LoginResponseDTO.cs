using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class LoginResponseDTO
    {
        
        public long? UserId { get; private set; }
        public bool? isTechLeader { get; private set; }

        public LoginResponseDTO(long? userId, bool? isTechLeader)
        {
            UserId = userId;
            this.isTechLeader = isTechLeader;
        }
    }
}
