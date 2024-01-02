using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Model.Project;
using CodeFlowBackend.Model.Tasks;
using CodeFlowBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Services
{
    public static class ProjectService
    {
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

        public static List<TaskCardDTO> GetAllTasksByProjectId(long projectId)
        {
            List<ProjectTask> tasks = ProjectRepository.GetAllTasksByProjectId(projectId);

            List<TaskCardDTO> tasksDTO = new List<TaskCardDTO>();

            foreach(var task in tasks)
            {
                tasksDTO.Add(new TaskCardDTO(
                        task.Id, task.Name, task.DueDate, UserService.GetUsersUsernameById(task.Assignee), ProjectRepository.GetChecklistRateByTaskId(task.Id) ,task.Status
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
    }
}
