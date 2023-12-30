using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.DTO
{
    public class LoginRequestDTO
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public LoginRequestDTO(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}

