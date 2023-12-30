using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Model.Project;
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
    }
}
