using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Model.Project;
using CodeFlowBackend.Model.Tasks;
using CodeFlowBackend.Model.User;
using CodeFlowBackend.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Net.Mime.MediaTypeNames;

namespace CodeFlowBackend.Repositories
{
    internal class ProjectRepository : DBConnection
    {
        private ProjectRepository() { }

        internal static void CreateNewTag(string name, string hexColor, long projectId)
        {
            try
            {
                Open();

                string query = @"insert into tag(project_id, name, color) values (@projectId, @name, @color);
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@name", name);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.Parameters.AddWithValue("@color", hexColor);

                _command.ExecuteNonQuery();
                
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Close();
            }
        }

        internal static bool CreateProject(long techLeaderId, string name, string description, DateTime dueDate)
        {
            try
            {
                Open();

                string query = @"insert into project (techleader_id, name, description, due_date, status) 
                        values (@techleaderId, @name, @description, @due_date, 1);
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@techleaderId", techLeaderId);
                _command.Parameters.AddWithValue("@name", name);
                _command.Parameters.AddWithValue("@description", description);
                _command.Parameters.AddWithValue("@due_date", dueDate);
                
                bool createProject = _command.ExecuteNonQuery() > 0;
                bool addProjectMember = false, createEnterCode = false;

                query = @"SELECT id from project where name = @name and techleader_id = @techleaderId;";

                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@techleaderId", techLeaderId);
                _command.Parameters.AddWithValue("@name", name);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                {
                    long projectId = (long)reader[0];

                    query = @"insert into project_members (project_id, member_id, enter_date) 
                            values (@project_id, @member_id, @enter_date);";

                    _command = new SQLiteCommand(query, _connection);
                    _command.Parameters.AddWithValue("@project_id", projectId);
                    _command.Parameters.AddWithValue("@member_id", techLeaderId);
                    _command.Parameters.AddWithValue("@enter_date", DateTime.Now);

                    addProjectMember = _command.ExecuteNonQuery() > 0;

                    string enterCode = GerenateProjectEnterCode();

                    Open();

                    query = @"insert into project_entercode (project_id, entercode) values (@projectId, @entercode);";

                    _command = new SQLiteCommand(query, _connection);
                    _command.Parameters.AddWithValue("@projectId", projectId);
                    _command.Parameters.AddWithValue("@entercode", enterCode);

                    createEnterCode = _command.ExecuteNonQuery() > 0;
                }


                return createProject && addProjectMember && createEnterCode;

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

        private static string GerenateProjectEnterCode()
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                builder.Append(chars[random.Next(chars.Length)]);
            }

            if (GetProjectIdByItsCode(builder.ToString()) != 0)
                return GerenateProjectEnterCode();

            return builder.ToString();
        
        }

        internal static bool CreateTask(long projectId, string name, string description, long? tagId, List<string> checklist, long assingeedId, DateTime dueDate)
        {
            try
            {
                Open();

                string query = @"insert into assignment (project_id, name, description, due_date, assignee, status, tag_id) 
                        values (@projectId, @name, @description, @due_date, @assignee, 1, @tag_id);
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.Parameters.AddWithValue("@name", name);
                _command.Parameters.AddWithValue("@description", description);
                _command.Parameters.AddWithValue("@due_date", dueDate);
                _command.Parameters.AddWithValue("@assignee", assingeedId);
                _command.Parameters.AddWithValue("@tag_id", tagId);
                
                int createTask = _command.ExecuteNonQuery();
                bool addChecklist;

                if (checklist != null && checklist.Count > 0)
                {
                    addChecklist = true;

                    query = @"SELECT id from assignment where name = @name and project_id = @projectId;";

                    _command = new SQLiteCommand(query, _connection);
                    _command.Parameters.AddWithValue("@projectId", projectId);
                    _command.Parameters.AddWithValue("@name", name);

                    var reader = _command.ExecuteReader();

                    if (reader.Read())
                    {
                        long id = (long)reader[0];

                        foreach (var check in checklist)
                        {
                            query = @"insert into checklist (assignment_id, name, isDone) 
                            values (@assignmentId, @checklistname, 0);";

                            _command = new SQLiteCommand(query, _connection);
                            _command.Parameters.AddWithValue("@assignmentId", id);
                            _command.Parameters.AddWithValue("@checklistname", check);

                            addChecklist = addChecklist && _command.ExecuteNonQuery() > 0;
                        }
                    }
                }
                else
                    addChecklist = true;

                
                return createTask > 0 && addChecklist;

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

        internal static List<Project> GetAllActiveProjectsByUserId(long userId)
        {
            List<Project> projects = new List<Project>();

            try
            {
                Open();

                string query = @"SELECT p.id, p.techleader_id, p.name, p.description, p.due_date, p.status
                FROM project p
                JOIN project_members pm ON p.id = pm.project_id
                WHERE pm.member_id = @userId AND pm.left_date IS NULL;
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@userId", userId);

                var reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    long id = (long)reader["id"];
                    long techLeaderId = (long)reader["techleader_id"];
                    string name = reader["name"].ToString();
                    string description = reader["description"].ToString();
                    DateTime dueDate = DateTime.Parse(reader["due_date"].ToString());
                    ProjectStatus status = (ProjectStatus)(long)reader["status"];
                    projects.Add(new Project(id, name, description, techLeaderId, dueDate, status));
                }

                return projects;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return projects;
            }
            finally
            {
                Close();
            }
            
        }

        internal static List<Tag> GetAllTagsByProjectId(long projectId)
        {
            List<Tag> tags = new List<Tag>();

            try
            {
                Open();
                string query = @"select id, name, color from tag where project_id = @id;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", projectId);

                var reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    long id = (long)reader["id"];
                    string name = reader["name"].ToString();
                    string color = reader["color"].ToString();
                   
                    Tag tag = new Tag(id, name, ColorTranslator.FromHtml(color));
                    tags.Add(tag);
                }

                return tags;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return tags;
            }
            finally
            {
                Close();
            }
        }

        internal static List<ProjectTask> GetAllTasksByProjectId(long projectId)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();

            try
            {
                Open();
                string query = @"select a.id, a.name, a.description, a.due_date, 
                        a.assignee, a.status, t.id as tagid, t.name as tagname, t.color from assignment a 
                        left join tag t on a.project_id = t.project_id where a.project_id = @id;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", projectId);

                var reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    long id = (long)reader["id"];
                    string name = reader["name"].ToString();
                    string description = reader["description"].ToString();
                    string assignee = UserService.GetUsersUsernameById((long)reader["assignee"]);
                    DateTime dueDate = DateTime.Parse(reader["due_date"].ToString());
                    TasksStatus status = (TasksStatus)(long)reader["status"];
                    Tag tag = new Tag((long)reader["tagid"], reader["tagname"].ToString(), ColorTranslator.FromHtml(reader["color"].ToString()));
                    tasks.Add(new ProjectTask(id, name, description, dueDate, assignee, status, tag));
                }

                return tasks;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return tasks;
            }
            finally
            {
                Close();
            }
        }

        internal static (int done, int total) GetChecklistRateByTaskId(long id)
        {
            (int done, int total) checklist = (0, 0);
            try
            {
                Open();
                string query = @"SELECT 
                    COUNT(*) AS TotalChecklists,
                    SUM(CASE WHEN isDone = 1 THEN 1 ELSE 0 END) AS ChecklistsDone FROM  checklist WHERE  assignment_id = @id;";
                _command = new SQLiteCommand(query, _connection);

                _command.Parameters.AddWithValue("@id", id);
                var reader = _command.ExecuteReader();
                if (reader.Read())
                {
                    checklist.total =(int) (long) reader[0];
                    checklist.done = (int)(long)reader[1];
                }

                return checklist;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
                return checklist;

            }
            finally { Close(); }
        }

        internal static string GetProjectNameById(long projectId)
        {
            string name = "";
            try
            {
                Open();
                string query = "SELECT name FROM project WHERE id = @id;";
                _command = new SQLiteCommand(query, _connection);

                _command.Parameters.AddWithValue("@id", projectId);
                var reader = _command.ExecuteReader();
                if (reader.Read())
                {
                    name = reader[0].ToString();
                }

                return name;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
                return name;

            }
            finally { Close(); }
        }

        internal static long GetProjectStatusById(long projectId)
        {
            long status = 0;
            try
            {
                Open();
                string query = "SELECT status FROM project WHERE id = @id;";
                _command = new SQLiteCommand(query, _connection);

                _command.Parameters.AddWithValue("@id", projectId);
                var reader = _command.ExecuteReader();
                if (reader.Read())
                {
                    status =(long) reader[0];
                }

                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
                return status;

            }
            finally { Close(); }
        }

        internal static bool IsProjectNameAvailableForThisUser(string name, long techleaderId)
        {
            try
            {
                Open();

                string query = "SELECT count(*) FROM project WHERE name = @name and techleader_id = @techleaderId;";

                _command = new SQLiteCommand(query, _connection);

                _command.Parameters.AddWithValue("@name", name);
                _command.Parameters.AddWithValue("@techleaderId", techleaderId);

                int count = Convert.ToInt32(_command.ExecuteScalar());
                return count == 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
                return false;

            }
            finally { Close(); }
        }

        internal static bool IsUserOnProject(string username, long projectId)
        {

            try
            {
                Open();
                string query = @"SELECT u.first_name 
                            FROM user u
                            JOIN project_members pm ON u.id = pm.member_id
                            WHERE u.username = @username AND pm.project_id = @projectId
                            ";
                _command = new SQLiteCommand(query, _connection);

                _command.Parameters.AddWithValue("@username", username.ToLower());
                _command.Parameters.AddWithValue("@projectId", projectId);
                var reader = _command.ExecuteReader();
                if (reader.Read())
                {
                    string name = reader[0].ToString();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
                return false;

            }
            finally { Close(); }
        }

        internal static long GetProjectIdByItsCode(string projectCode)
        {
            long projectId = 0;

            try
            {
                Open();

                string query = "SELECT project_id FROM project_entercode WHERE entercode = @enterCode";

                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@enterCode", projectCode);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                    projectId = (long)reader[0];

                return projectId;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return projectId;
            }
            finally
            {
                Close();
            }
        }

        internal static string GetProjectEnterCodeById(long projectId)
        {
            string enterCode = "";

            try
            {
                Open();

                string query = "SELECT entercode FROM project_entercode WHERE project_id = @projectId";

                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@projectId", projectId);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                    enterCode = reader[0].ToString();

                return enterCode;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return enterCode;
            }
            finally
            {
                Close();
            }
        }

        internal static bool InviteToProject(long projectId, long userId)
        {
            try
            {
                Open();

                string query = @"insert into project_invite (user_id, project_id, entered) values (@userId, @projectId, 0);
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@userId", userId);
                _command.Parameters.AddWithValue("@projectId", projectId);

                return _command.ExecuteNonQuery() > 0;

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

        internal static ProjectOverviewDTO GetProjectOverviewById(long projectId)
        {
            ProjectOverviewDTO response = null;
            try
            {
                Open();

                string query = "SELECT name, description, due_date FROM project WHERE id = @id;";

                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", projectId);

                var reader = _command.ExecuteReader();

                if (reader.Read())
                {
                    string name = reader["name"].ToString();
                    string description = reader["description"].ToString();
                    DateTime dueDate = DateTime.Parse(reader["due_date"].ToString());

                    List<long> membersId = new List<long>();

                    query = "SELECT member_id FROM project_members WHERE project_id = @id and left_date IS NULL;";

                    _command = new SQLiteCommand(query, _connection);
                    _command.Parameters.AddWithValue("@id", projectId);

                    reader = _command.ExecuteReader();
                    while(reader.Read())
                    {
                        membersId.Add((long)reader["member_id"]);
                    }

                    response = new ProjectOverviewDTO(projectId, name, description, dueDate, membersId);
                }
                    

                return response;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return response;
            }
            finally
            {
                Close();
            }
        }

        internal static bool RemoveMemberFromProject(long projectId, long memberId)
        {
            try
            {
                Open();

                string query = @"UPDATE project_members SET left_date = @leftDate WHERE member_id = @memberId AND project_id = @projectId;
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@memberId", memberId);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.Parameters.AddWithValue("@leftDate", DateTime.Today);

                bool removed = _command.ExecuteNonQuery() > 0;

                query = @"UPDATE assignment SET assignee = NULL WHERE assignee = @memberId AND project_id = @projectId;
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@memberId", memberId);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.ExecuteNonQuery();

                return removed;
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

        internal static bool UpdateProjectDueDate(long projectId, DateTime dueDate)
        {
            try
            {
                Open();

                string query = @"UPDATE project SET due_date = @dueDate WHERE id = @projectId;
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.Parameters.AddWithValue("@dueDate", dueDate);

                return _command.ExecuteNonQuery() > 0;

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

        internal static bool UpdateStatus(long projectId, int status)
        {
            try
            {
                Open();

                string query = @"UPDATE project SET status = @status WHERE id = @projectId;
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.Parameters.AddWithValue("@status", status);

                return _command.ExecuteNonQuery() > 0;

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

        internal static bool UpdateProjectDescription(long projectId, string description)
        {
            try
            {
                Open();

                string query = @"UPDATE project SET description = @description WHERE id = @projectId;
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.Parameters.AddWithValue("@description", description);

                return _command.ExecuteNonQuery() > 0;

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

        internal static bool UpdateProjectName(long projectId, string name)
        {
            try
            {
                Open();

                string query = @"UPDATE project SET name = @name WHERE id = @projectId;
                ";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@projectId", projectId);
                _command.Parameters.AddWithValue("@name", name);

                return _command.ExecuteNonQuery() > 0;

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

        internal static List<ProjectTask> GetAllTasksByProjectIdAndByUserId(long projectId, long userId)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();

            try
            {
                Open();
                string query = @"select a.id, a.name, a.description, a.due_date, 
                        a.assignee, a.status, t.id as tagid, t.name as tagname, t.color from assignment a 
                        left join tag t on a.project_id = t.project_id where a.project_id = @id and a.assignee = @userId;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", projectId);
                _command.Parameters.AddWithValue("@userId", userId);

                var reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    long id = (long)reader["id"];
                    string name = reader["name"].ToString();
                    string description = reader["description"].ToString();
                    string assignee = UserService.GetUsersUsernameById((long)reader["assignee"]);
                    DateTime dueDate = DateTime.Parse(reader["due_date"].ToString());
                    TasksStatus status = (TasksStatus)(long)reader["status"];
                    Tag tag = new Tag((long)reader["tagid"], reader["tagname"].ToString(), ColorTranslator.FromHtml(reader["color"].ToString()));
                    tasks.Add(new ProjectTask(id, name, description, dueDate, assignee, status, tag));
                }

                return tasks;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return tasks;
            }
            finally
            {
                Close();
            }
        }

        internal static List<ProjectTask> GetAllTasksByProjectIdAndByTagId(long projectId, long tagId)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();

            try
            {
                Open();
                string query = @"select a.id, a.name, a.description, a.due_date, 
                        a.assignee, a.status, t.id as tagid, t.name as tagname, t.color from assignment a 
                        left join tag t on a.project_id = t.project_id where a.project_id = @id and t.id = @tagId;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", projectId);
                _command.Parameters.AddWithValue("@tagId", tagId);

                var reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    long id = (long)reader["id"];
                    string name = reader["name"].ToString();
                    string description = reader["description"].ToString();
                    string assignee = UserService.GetUsersUsernameById((long)reader["assignee"]);
                    DateTime dueDate = DateTime.Parse(reader["due_date"].ToString());
                    TasksStatus status = (TasksStatus)(long)reader["status"];
                    Tag tag = new Tag((long)reader["tagid"], reader["tagname"].ToString(), ColorTranslator.FromHtml(reader["color"].ToString()));
                    tasks.Add(new ProjectTask(id, name, description, dueDate, assignee, status, tag));
                }

                return tasks;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                return tasks;
            }
            finally
            {
                Close();
            }
        }
    }
}
