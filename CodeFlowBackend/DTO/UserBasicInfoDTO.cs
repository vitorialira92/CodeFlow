using CodeFlowBackend.Model.User.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class UserBasicInfoDTO
    {
        public readonly long id;
        public readonly string firstName;
        public readonly UserRole role;

        public UserBasicInfoDTO(long id, string firstName, UserRole role)
        {
            this.id = id;
            this.firstName = firstName;
            this.role = role;
        }
    }
}
