using CodeFlowBackend.Model.User.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Model.User
{
    internal class TechLeader : User
    {
        public int TechLeaderId { get; private set; }
        public Specializations Specialization { get; private set; }

        public TechLeader(long userId, Specializations specialization, 
            string username, string firstName, string lastName, string email, string password, DateTime dateJoined) : base(userId, username, firstName, lastName, 
                email, password, UserRole.TechLeader, dateJoined)
        {
            Specialization = specialization;
        }

        public TechLeader(Specializations specialization, 
            string username, string firstName, string lastName,string email, string password) : base(username, firstName, lastName,
                email, password, UserRole.TechLeader)
        {
            Specialization = specialization;
        }

    }
}
