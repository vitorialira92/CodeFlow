using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class RegisterDTO
    {
        public RegisterDTO(string firstName, string lastName, string email, string username, string password, bool isTechLeader, int specificToUserRole)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email.ToLower();
            Username = username.ToLower();
            Password = password;
            IsTechLeader = isTechLeader;
            SpecificToUserRole = specificToUserRole;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool IsTechLeader { get; private set; }
        public int SpecificToUserRole { get; private set; }

       
    }
}
