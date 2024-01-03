using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Model.Project;
using CodeFlowBackend.Model.Tasks;
using CodeFlowBackend.Model.User;
using CodeFlowBackend.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        a.assignee, a.status, t.name as tagname, t.color from assignment a 
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
                    Tag tag = new Tag(reader["tagname"].ToString(), ColorTranslator.FromHtml(reader["color"].ToString()));
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
    }
}
