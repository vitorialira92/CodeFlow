using CodeFlowBackend.DTO;
using CodeFlowBackend.Exceptions;
using CodeFlowBackend.Model.Project;
using CodeFlowBackend.Model.User;
using CodeFlowBackend.Model.User.Enum;
using CodeFlowBackend.Services;
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
        private UserRepository() { }

        private static bool AddUser(User user)
        {
            try
            {
                Open();

                string query = "INSERT INTO user (username, email, first_name, " +
                    "last_name, date_joined, role, password) VALUES (@username, @email, @firstName, " +
                    "@lastName, @dateJoined, @role, @password);";

                _command = new SQLiteCommand(query, _connection);

                (string password, string salt) password_salt = HashUtil.GetHashedAndSalt(user._password);

                _command.Parameters.AddWithValue("@username", user.Username);
                _command.Parameters.AddWithValue("@email", user.Email);
                _command.Parameters.AddWithValue("@firstName", user.FirstName);
                _command.Parameters.AddWithValue("@lastName", user.LastName);
                _command.Parameters.AddWithValue("@dateJoined", user.DateJoined);
                _command.Parameters.AddWithValue("@role", (int)user.Role);
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

        public static long GetIdByUsername(string username)
        {
            long id = 0;
            try
            {
                Open();
                string query = "SELECT id FROM user WHERE username = @username;";
                _command = new SQLiteCommand(query, _connection);

                _command.Parameters.AddWithValue("@username", username);
                var reader = _command.ExecuteReader();
                if (reader.Read())
                {
                    id = (long)(reader["id"]);
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
                    long id = GetIdByUsername(user.Username);

                    Open();
                    string query = "INSERT INTO developer (user_id, experience_level)" +
                        " VALUES (@user_id, @experience_level);";

                    _command = new SQLiteCommand(query, _connection);

                    _command.Parameters.AddWithValue("@experience_level", (int)user.ExperienceLevel);
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
                    long id = GetIdByUsername(user.Username);

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


        internal static (long? id, bool? isTechLeader) Login(string username, string password)
        {
            try
            {
                Open();

                string query = @"SELECT s.salt FROM user_salt s INNER JOIN user u ON u.id = s.user_id where u.username=@username;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@username", username);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                {
                    string salt = reader["salt"].ToString();
                    string hashedPassword = HashUtil.GetHashedWithGivenSalt(password, salt);
                    if (hashedPassword == "")
                        return (null, null);

                    query = @"SELECT id, role
                     FROM User 
                     WHERE username=@username AND password=@password;";
                    _command = new SQLiteCommand(query, _connection);
                    _command.Parameters.AddWithValue("@username", username);
                    _command.Parameters.AddWithValue("@password", hashedPassword);


                    reader = _command.ExecuteReader();

                    if (reader.Read())
                    {
                        long id = (long)reader["id"];
                        long role = (long)reader["role"];
                        return (id, role == 1);
                    }
                }
                return (null, null);
            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Erro ao abrir o banco de dados: {e.Message}\n");
                return (null, null);
            }
            finally
            {
                Close();
            }
        }

        internal static Developer GetDeveloperById(long userId)
        {
            try
            {
                Open();

                string query = @"SELECT id, username, email, first_name, last_name, date_joined, role
                 FROM user
                 WHERE id = @id;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", userId);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                {
                    long id = (long)reader["id"];
                    string username = reader["username"].ToString();
                    string firstName = reader["first_name"].ToString();
                    string lastName = reader["last_name"].ToString();
                    string email = reader["email"].ToString();
                    long role = (long)reader["role"];
                    DateTime joinedDate = (DateTime)reader["date_joined"];

                    if (role != 2) return null;

                    query = @"SELECT (experience_level) from developer WHERE user_id=@id;";

                    _command = new SQLiteCommand(query, _connection);
                    _command.Parameters.AddWithValue("@id", userId);
                    reader = _command.ExecuteReader();

                    if (reader.Read())
                        return new Developer(id, username, firstName, lastName, email, "", joinedDate, (ExperienceLevelDeveloper)(long)reader["experience_level"]);


                }
                return null;
            }


            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                Close();
            }
        }

        internal static TechLeader GetTechLeaderById(long userId)
        {
            try
            {
                Open();

                string query = @"SELECT id, username, email, first_name, last_name, date_joined, role
                 FROM user
                 WHERE id = @id;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", userId);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                {
                    long id = (long)reader["id"];
                    string username = reader["username"].ToString();
                    string firstName = reader["first_name"].ToString();
                    string lastName = reader["last_name"].ToString();
                    string email = reader["email"].ToString();
                    long role = (long)reader["role"];
                    DateTime joinedDate = (DateTime)reader["date_joined"];

                    if (role != 1) return null;

                    query = @"SELECT (specialization) from techleader WHERE user_id=@id;";

                    _command = new SQLiteCommand(query, _connection);
                    _command.Parameters.AddWithValue("@id", userId);
                    reader = _command.ExecuteReader();

                    if (reader.Read())
                        return new TechLeader(id, (Specializations)(long)reader["specialization"], username, firstName, lastName, email, "", joinedDate);

                }
                return null;
            }


            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                Close();
            }
        }

        internal static long GetUserIdByEmail(string email)
        {
            long userId = 0;
            try
            {
                Open();

                string query = @"SELECT id 
                 FROM user
                 WHERE email = @email;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@email", email);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                    userId = (long)reader["id"];
                return userId;

            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return userId;
            }
            finally { Close(); }


        }

        internal static string GetUserFirstNameById(long userId)
        {
            string userFirstName = "";
            Open();

            string query = @"SELECT first_name 
                 FROM user
                 WHERE id = @id;";
            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@id", userId);

            var reader = _command.ExecuteReader();

            if (reader.Read())
                userFirstName = reader["first_name"].ToString();

            Close();

            return userFirstName;
        }

        internal static long GetUserRoleById(long userId)
        {

            long role = 0;
            Open();

            string query = @"SELECT role 
                 FROM user
                 WHERE id = @id;";
            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@id", userId);

            var reader = _command.ExecuteReader();

            if (reader.Read())
                role = (long)reader["role"];

            Close();

            return role;
        }

        internal static int GetUsersProjectCountById(long id)
        {
            try
            {
                Open();

                string query = @"SELECT COUNT(project_id)
                 FROM project_members
                 WHERE member_id = @id;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", id);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                    return (int)(long)reader[0];
                return 0;
            }


            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
            finally
            {
                Close();
            }
        }

        internal static int GetUsersDoneTasksCountById(long id)
        {
            try
            {
                Open();

                string query = @"SELECT COUNT(id)
                 FROM assignment
                 WHERE assignee = @id AND status = 3;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", id);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                    return (int)(long)reader[0];
                return 0;
            }


            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
            finally
            {
                Close();
            }
        }

        internal static bool UpdateTechLeader(long id, string firstName, string lastName, string email)
        {
            try
            {
                Open();

                string query = @"UPDATE user
                    SET first_name = @firstName,
                        last_name = @lastName,
                        email = @email
                    WHERE id = @userId;";

                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@firstName", firstName);
                _command.Parameters.AddWithValue("@lastName", lastName);
                _command.Parameters.AddWithValue("@email", email);
                _command.Parameters.AddWithValue("@userId", id);
                _command.ExecuteNonQuery();

                return true;
            }


            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Close();
            }
        }

        internal static bool UpdateDeveloper(long id, string firstName, string lastName, string email, int experienceLevel)
        {
            try
            {
                if (!UpdateUser(id, firstName, lastName, email))
                    return false;

                Open();

                string query = @"UPDATE developer
                    SET experience_level = @experiencelevel
                    WHERE user_id = @userId;
                    ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@experiencelevel", experienceLevel);
                _command.Parameters.AddWithValue("@userId", id);

                _command.ExecuteNonQuery();
                return true;
            }


            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Close();
            }
        }

        private static bool UpdateUser(long id, string firstName, string lastName, string email)
        {

            try
            {
                Open();

                string query = @"UPDATE user
                    SET first_name = @firstName,
                        last_name = @lastName,
                        email = @email
                    WHERE id = @userId;
                    ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@firstName", firstName);
                _command.Parameters.AddWithValue("@lastName", lastName);
                _command.Parameters.AddWithValue("@email", email);
                _command.Parameters.AddWithValue("@userId", id);

                _command.ExecuteNonQuery();
                return true;
            }

            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Close();
            }
        }

        internal static void EnterProject(long userId, string projectCode)
        {
            long projectId = ProjectService.GetProjectIdByItsCode(projectCode);
            if (projectId == 0)
                throw new EnterProjectException("code invalid.");

            if(!IsUserInvitedToProject(userId, projectId))
                throw new EnterProjectException("user not invited to this project.");

            try
            {
                Open();

                string query = "UPDATE project_invite SET entered = 1 WHERE user_id = @userId AND project_id = @projectId";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@userId", userId);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.ExecuteNonQuery();

                query = "INSERT INTO project_members (project_id, member_id, enter_date) VALUES (@projectId, @userId, CURRENT_TIMESTAMP)";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@userId", userId);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.ExecuteNonQuery();


            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                Close();
            }
        }

        private static bool IsUserInvitedToProject(long userId, long projectId)
        {

            try
            {
                Open();

                string query = "SELECT COUNT(*) FROM project_invite WHERE user_id = @userId AND project_id = @projectId AND entered = 0";

                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@userId", userId);
                _command.Parameters.AddWithValue("@projectId", projectId);

                var reader = _command.ExecuteReader();

                long count = 0;

                if (reader.Read())
                    count = (long)reader[0];

                return count != 0;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return false;
            }
            finally
            {
                Close();
            }
        }

        
        internal static string GetUsersUsernameById(long userId)
        {
            string username = "";

            try
            {
                Open();

                string query = "SELECT username FROM user WHERE id = @id";

                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", userId);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                    username = reader[0].ToString();

                return username;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return username;
            }
            finally
            {
                Close();
            }
        }

        internal static bool ChangePassword(long userId, string currentPassword, string newPassword)
        {
            try
            {
                Open();

                string query = @"SELECT s.salt FROM user_salt s INNER JOIN user u ON u.id = s.user_id where u.id=@id;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", userId);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                {
                    string salt = reader["salt"].ToString();
                    string hashedPassword = HashUtil.GetHashedWithGivenSalt(currentPassword, salt);
                    if (hashedPassword == "")
                        return false;

                    query = @"SELECT count(*)
                     FROM user 
                     WHERE id=@id AND password=@password;";
                    _command = new SQLiteCommand(query, _connection);
                    _command.Parameters.AddWithValue("@id", userId);
                    _command.Parameters.AddWithValue("@password", hashedPassword);


                    reader = _command.ExecuteReader();

                    if(reader.Read())
                        if ((long)reader[0] != 0)
                        {
                            (string password, string salt) newPasswordAndSalt = HashUtil.GetHashedAndSalt(newPassword);

                            query = @"UPDATE user SET password = @newPassword WHERE id = @id;";

                            _command = new SQLiteCommand(query, _connection);

                            _command.Parameters.AddWithValue("@newPassword", newPasswordAndSalt.password);
                            _command.Parameters.AddWithValue("@id", userId);

                            _command.ExecuteNonQuery();

                            query = @"UPDATE user_salt SET salt = @salt WHERE user_id = @id;";

                            _command = new SQLiteCommand(query, _connection);

                            _command.Parameters.AddWithValue("@salt", newPasswordAndSalt.salt);
                            _command.Parameters.AddWithValue("@id", userId);

                            _command.ExecuteNonQuery(); 
                            return true;
                        }
                }
                return false;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Erro ao abrir o banco de dados: {e.Message}\n");
                return false;
            }
            finally
            {
                Close();
            }
        }
    }
}
