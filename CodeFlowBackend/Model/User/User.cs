using CodeFlowBackend.Model.User.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Model.User
{
    internal class User
    {
        public long Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        internal string _password { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set;}
        public DateTime DateJoined { get; }
        public UserRole Role { get; private set; }
        public User(string username, string firstName, string lastName, string email, string password, UserRole role)
        {
            this.Username = username;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this._password = password;
            this.DateJoined = DateTime.Now;
            this.Role = role;
        }
        public User(long Id, string username, string firstName, string lastName, string email, string password, UserRole role, DateTime dateJoined)
        {
            this.Id = Id;
            this.Username = username;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email= email;
            this._password = password;
            this.DateJoined = dateJoined;
            this.Role = role;
        }

        public User(long Id, string username, string firstName, string lastName, string email, UserRole role)
        {
            this.Id = Id;
            this.Username = username;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.DateJoined = DateTime.Now;
            this.Role = role;
        }

    }
    
}
