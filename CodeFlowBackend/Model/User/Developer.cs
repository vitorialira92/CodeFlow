using CodeFlowBackend.Model.User.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Model.User
{
    internal class Developer : User
    {
        public Developer(string username, string firstName, string lastName, string email, string password, ExperienceLevelDeveloper experienceLevel) 
            : base(username, firstName, lastName, email, password, UserRole.Developer)
        {
            this.ExperienceLevel = experienceLevel;
        }

        public int DeveloperId { get; private set; }
        public ExperienceLevelDeveloper ExperienceLevel { get; private set; }

    }
}
