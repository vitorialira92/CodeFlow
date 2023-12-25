﻿using CodeFlowBackend.DTO;
using CodeFlowBackend.Model.User;
using CodeFlowBackend.Model.User.Enum;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeFlowBackend.Repositories
{
    internal class UserRepository : DBConnection
    {
        private static UserRepository _instance;
        private UserRepository() { }

        public static UserRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserRepository();
                }
                return _instance;
            }
        }

        private static bool AddUser(User user)
        {
            try
            {
                Open();

                string query = "INSERT INTO user (username, email, first_name, " +
                    "last_name, date_joined, role, password) VALUES (@username, @email, @firstName, " +
                    "@lastName, @dateJoined, @role, @password);";

                _command = new SQLiteCommand(query, _connection);

                (string password, string salt) password_salt = HashUtil.GetHashedPassword(user._password);

                _command.Parameters.AddWithValue("@username", user.Username);
                _command.Parameters.AddWithValue("@email", user.Email);
                _command.Parameters.AddWithValue("@firstName", user.FirstName);
                _command.Parameters.AddWithValue("@lastName", user.LastName);
                _command.Parameters.AddWithValue("@dateJoined", user.DateJoined);
                _command.Parameters.AddWithValue("@role", (int) user.Role);
                _command.Parameters.AddWithValue("@password", password_salt.password);

                query = "INSERT INTO user_salt (user_id, salt) VALUES (@user_id, @salt);";

                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@user_id", GetIdByUsername(user.Username));
                _command.Parameters.AddWithValue("@salt", password_salt.salt);

                _command.ExecuteNonQuery();

                return _command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
                return false;
            }
            finally
            {
                Close();
            }
        }

        public static int GetIdByUsername(string username)
        {
            int id = 0;
            try
            {
                Open();
                string query = "SELECT id FROM user WHERE username = @username;";
                _command = new SQLiteCommand(query, _connection);

                _command.Parameters.AddWithValue("@username", username);
                var reader = _command.ExecuteReader();
                if (reader.Read())
                {
                    id = Convert.ToInt32(reader["id"]);
                }

                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
                return id;

            }
            finally { Close(); }
            
        }

        public static bool AddDeveloper(Developer user)
        {

            try
            {
                if (AddUser(user))
                {
                    int id = GetIdByUsername(user.Username);

                    Open();
                    string query = "INSERT INTO developer (user_id, experience_level)" +
                        " VALUES (@user_id, @experience_level);";

                    _command = new SQLiteCommand(query, _connection);

                    _command.Parameters.AddWithValue("@experience_level", (int) user.ExperienceLevel);
                    _command.Parameters.AddWithValue("@user_id", id);


                    return _command.ExecuteNonQuery() > 0;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
                return false;
            }
            finally
            {
                Close();
            }
        }

        public static bool AddTechLeader(TechLeader user)
        {

            try
            {

                if (AddUser(user))
                {
                    int id = GetIdByUsername(user.Username);

                    Open();
                    string query = "INSERT INTO techleader (user_id, specialization)" +
                        " VALUES (@user_id, @specialization);";

                    _command = new SQLiteCommand(query, _connection);

                    _command.Parameters.AddWithValue("@specialization", (int)user.Specialization);
                    _command.Parameters.AddWithValue("@user_id", id);


                    return _command.ExecuteNonQuery() > 0;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
                return false;
            }
            finally
            {
                Close();
            }
        }
            
        public static long Login(string username, string password)
        {
            long userId = -1;

            Open();

            string query = @"SELECT u.id, u.password, u.role, s.salt 
                 FROM user u
                 INNER JOIN user_salt s ON u.id = s.user_id 
                 WHERE u.username = @username;";
            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@username", username);

            var reader = _command.ExecuteReader();

            if (reader.Read()) 
            {
                long id = (long)reader["id"];
                string hashedPassword = reader["password"].ToString();

                string salt = reader["salt"].ToString();

                if (HashUtil.CheckPassword(password, hashedPassword, salt))
                    userId = id;
            }

            Close();

            return userId;
            
        }

        internal static User GetUserById(long userId)
        {
            User user = null;
            Open();

            string query = @"SELECT * 
                 FROM user
                 WHERE id = @id;";
            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@id", userId);

            var reader = _command.ExecuteReader();

            if (reader.Read())
            {
                long id = (long)reader["id"];
                string username = reader["username"].ToString();
                string firstName = reader["username"].ToString();
                string lastName = reader["username"].ToString();
                string email = reader["username"].ToString();
                long role = (long)reader["role"];
                string salt = reader["salt"].ToString();

                user = new User(id, username, firstName, lastName, email, (UserRole) role);

            }

            Close();

            return user;
        }

        internal static long GetUserIdByEmail(string email)
        {
            long userId = 0;
            Open();

            string query = @"SELECT id 
                 FROM user
                 WHERE email = @email;";
            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@email", email);

            var reader = _command.ExecuteReader();

            if (reader.Read())
                userId = (long)reader["id"];

            Close();

            return userId;
        }
    }
}