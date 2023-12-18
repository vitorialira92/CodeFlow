using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Model.User
{
    internal abstract class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        private string _password;
        public string FirstName { get; private set; }
        public string LastName { get; private set;}
        public DateTime DateJoined { get; }
        public DateTime LastLogin {  get; private set; }
        public bool IsLoggedIn { get; private set; }    
        public User(string username, string firstName, string lastName, string password)
        {
            this.Id = CalculateId();
            this.Username = username;
            this.FirstName = firstName;
            this.LastName = lastName;
            this._password = GetHashedPassword(password);
            this.DateJoined = DateTime.Now;
        }

        private int CalculateId()
        {
            return DateTime.Now.GetHashCode();
        }

        private string GetHashedPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 2500);

            byte[] hash = pbkdf2.GetBytes(30);
            byte[] hashBytes = new byte[hash.Length + salt.Length];

            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

            return Convert.ToBase64String(hashBytes);
        }

        public bool CheckPassword(string password)
        {
            byte[] hashBytes = Convert.FromBase64String(_password);

            byte[] salt = new byte[16];

            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 2500);

            byte[] hash = pbkdf2.GetBytes(30);

            return hash.Equals(hashBytes);
        }

        public void Login()
        {
            this.IsLoggedIn = true;
        }

        public void LogOut()
        {
            this.IsLoggedIn = false;
            this.LastLogin = DateTime.Now;
        }

        public abstract override string ToString();
    }
    
}
