using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Model.Project;
using CodeFlowBackend.Model.Tasks;
using CodeFlowBackend.Repositories;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeFlowBackend.Services
{
    public static class ProjectService
    {
        public static void CreateNewTag(string name, long projectId)
        {
            Random random = new Random();

            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            Color randomColor = Color.FromArgb(red, green, blue);

            string hexColor = $"#{randomColor.R:X2}{randomColor.G:X2}{randomColor.B:X2}";

            ProjectRepository.CreateNewTag(name, hexColor, projectId);
        }

        public static bool IsProjectNameAvailableForThisUser(string name, long techleaderId)
        {
            return ProjectRepository.IsProjectNameAvailableForThisUser(name, techleaderId);
        }

        public static bool CreateProject(CreateProjectDTO createProjectDTO)
        {
            return ProjectRepository.CreateProject(createProjectDTO.TechLeaderId, createProjectDTO.Name, createProjectDTO.Description, createProjectDTO.DueDate);
        }

        public static bool CreateTask(CreateTaskDTO createTaskDTO)
        {
            return ProjectRepository.CreateTask(createTaskDTO.ProjectId, createTaskDTO.Name, createTaskDTO.Description,
                (createTaskDTO.tag == null) ? null : createTaskDTO.tag.Id, createTaskDTO.Checklist, UserService.GetUserIdByUsername(createTaskDTO.AssigneeUsername), createTaskDTO.DueDate);
        }

        public static List<ProjectBasicInfoDTO> GetAllProjectsBasicInfoByUserId(long userId)
        {
            List<Project> projects = ProjectRepository.GetAllActiveProjectsByUserId(userId);

            List<ProjectBasicInfoDTO> projectBasicInfoDTOs = new List<ProjectBasicInfoDTO>();

            foreach(var project in projects)
            {
                projectBasicInfoDTOs.Add(new ProjectBasicInfoDTO(project.Id, project.Name, project.Description, project.Status, project.DueDate));
            }

            return projectBasicInfoDTOs;
        }

        public static List<Tag> GetAllTagsByProjectId(long projectId)
        {
            return ProjectRepository.GetAllTagsByProjectId(projectId);
        }

        public static List<TaskCardDTO> GetAllTasksByProjectId(long projectId)
        {
            List<ProjectTask> tasks = ProjectRepository.GetAllTasksByProjectId(projectId);

            List<TaskCardDTO> tasksDTO = new List<TaskCardDTO>();

            foreach(var task in tasks)
            {
                tasksDTO.Add(new TaskCardDTO(
                        task.Id, task.Name, task.DueDate, task.Assignee, 
                        ProjectRepository.GetChecklistRateByTaskId(task.Id) ,task.Status, task.tag
                    ));
            }

            return tasksDTO;
        }

        public static string GetProjectNameById(long projectId)
        {
            return ProjectRepository.GetProjectNameById(projectId);
        }

        public static ProjectStatus GetProjectStatusById(long projectId)
        {
            return (ProjectStatus) ProjectRepository.GetProjectStatusById(projectId);
        }

        public static bool IsUserOnProject(string username, long projectId)
        {
            return ProjectRepository.IsUserOnProject(username, projectId);
        }

        internal static long GetProjectIdByItsCode(string projectCode)
        {
            return ProjectRepository.GetProjectIdByItsCode();
        }
    }
}
