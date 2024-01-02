using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Model.Project;
using CodeFlowBackend.Model.Tasks;
using CodeFlowBackend.Model.User;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Repositories
{
    internal class ProjectRepository : DBConnection
    {
        private ProjectRepository() { }

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

        internal static List<ProjectTask> GetAllTasksByProjectId(long projectId)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();

            try
            {
                Open();
                string query = @"select id, name, description, due_date, assignee, status from assignment where project_id = @id;";
                _command = new SQLiteCommand(query, _connection);
                _command.Parameters.AddWithValue("@id", projectId);

                var reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    long id = (long)reader["id"];
                    string name = reader["name"].ToString();
                    string description = reader["description"].ToString();
                    string assignee = reader["assignee"].ToString();
                    DateTime dueDate = DateTime.Parse(reader["due_date"].ToString());
                    TasksStatus status = (TasksStatus)(long)reader["status"];
                    tasks.Add(new ProjectTask(id, name, description, dueDate, assignee, status));
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
    }
}
