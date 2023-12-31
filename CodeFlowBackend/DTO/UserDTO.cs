using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class UserDTO
    {
        public long Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Username { get; private set; }
        public bool IsTechLeader { get; private set; }
        public int SpecificToUserRole { get; private set; }

        public DateTime JoinedDate { get; private set; }
        public UserDTO(long id, string firstName, string lastName, string email, string username, DateTime joinedDate, bool isTechLeader, int specificToUserRole)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            JoinedDate = joinedDate;
            IsTechLeader = isTechLeader;
            SpecificToUserRole = specificToUserRole;
        }
    }
}
