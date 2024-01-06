using CodeFlowBackend.DTO;
using CodeFlowBackend.Model;
using CodeFlowBackend.Model.Project;
using CodeFlowBackend.Model.Tasks;
using CodeFlowBackend.Model.User;
using CodeFlowBackend.Repositories;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Data.Entity.Infrastructure.Design.Executor;

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
                        ProjectRepository.GetChecklistRateByTaskId(task.Id) ,task.Status, task.Tag
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
            return ProjectRepository.GetProjectIdByItsCode(projectCode);
        }

        public static string GetProjectEnterCodeById(long projectId)
        {
            return ProjectRepository.GetProjectEnterCodeById(projectId);
        }

        public static bool InviteToProject(long projectId, string username)
        {
            long userId = UserService.GetUserIdByUsername(username);

            if (userId == 0)
                return false;

            if (UserService.IsUserTechLeader(userId))
                return false;

            return ProjectRepository.InviteToProject(projectId, userId);
        }

        public static ProjectOverviewDTO GetProjectOverviewById(long projectId)
        {
            return ProjectRepository.GetProjectOverviewById(projectId);
        }

        public static bool RemoveMemberFromProject(long projectId, long memberId)
        {
            return ProjectRepository.RemoveMemberFromProject(projectId, memberId);
        }

        public static bool UpdateProjectDueDate(long projectId, DateTime dueDate)
        {
            return ProjectRepository.UpdateProjectDueDate(projectId, dueDate);
        }

        public static bool UpdateProjectStatus(long projectId, ProjectStatus status)
        {
            int statusNumber = (int)status;
            return ProjectRepository.UpdateProjectStatus(projectId, statusNumber);
        }

        public static bool UpdateProjectDescription(long projectId, string description)
        {
            return ProjectRepository.UpdateProjectDescription(projectId, description);
        }

        public static bool UpdateProjectName(long projectId, string newName)
        {
            return ProjectRepository.UpdateProjectName(projectId, newName);
        }

        public static bool CancelProject(long projectId)
        {
            return ProjectRepository.UpdateProjectStatus(projectId, 3);
        }

        public static List<TaskCardDTO> GetAllTasksByProjectIdAndByUserId(long projectId, long userId)
        {
            List<ProjectTask> tasks = ProjectRepository.GetAllTasksByProjectIdAndByUserId(projectId, userId);

            List<TaskCardDTO> tasksDTO = new List<TaskCardDTO>();

            foreach (var task in tasks)
            {
                tasksDTO.Add(new TaskCardDTO(
                        task.Id, task.Name, task.DueDate, task.Assignee,
                        ProjectRepository.GetChecklistRateByTaskId(task.Id), task.Status, task.Tag
                    ));
            }

            return tasksDTO;

        }

        public static List<TaskCardDTO> GetAllTasksByProjectIdAndTagId(long projectId, long tagId)
        {
            List<ProjectTask> tasks = ProjectRepository.GetAllTasksByProjectIdAndByTagId(projectId, tagId);

            List<TaskCardDTO> tasksDTO = new List<TaskCardDTO>();

            foreach (var task in tasks)
            {
                tasksDTO.Add(new TaskCardDTO(
                        task.Id, task.Name, task.DueDate, task.Assignee,
                        ProjectRepository.GetChecklistRateByTaskId(task.Id), task.Status, task.Tag
                    ));
            }

            return tasksDTO;
        }

        public static ProjectTask GetTaskById(long taskId)
        {
            return ProjectRepository.GetTaskById(taskId);
        }

        public static bool UpdateTaskDescription(long id, string description)
        {
            return ProjectRepository.UpdateTaskDescription(id, description);
        }

        public static bool UpdateTaskAssignee(long id, string username)
        {
            return ProjectRepository.UpdateTaskAssignee(id, UserService.GetUserIdByUsername(username));
        }

        public static bool UpdateTaskDueDate(long id, DateTime dueDate)
        {
            return ProjectRepository.UpdateTaskDueDate(id, dueDate);
        }

        public static bool UpdateTaskTag(long id, Tag tag)
        {
            return ProjectRepository.UpdateTaskTag(id, tag);
        }

        public static bool UpdateTaskChecklist(long projectId, long userId, long taskId, long checklistId, bool isDone)
        {
            bool update = ProjectRepository.UpdateTaskChecklist(checklistId, isDone);
            (int done, int total) check = ProjectRepository.GetChecklistRateByTaskId(taskId);
            if(check.done != 0 && check.done == check.total)
            {
                if(UserService.IsUserTechLeader(userId))
                    UpdateTaskStatus(projectId, taskId, 4);
                else
                    UpdateTaskStatus(projectId, taskId, 3);
            }
            ProjectTask task = ProjectRepository.GetTaskById(taskId);
            if((task.Status == TasksStatus.Done || task.Status == TasksStatus.Review) && check.done != check.total)
            {
                if(check.done == 0)
                    UpdateTaskStatus(projectId, taskId, 1);
                else
                    UpdateTaskStatus(projectId, taskId, 2);
            }
            else if (task.Status == TasksStatus.InProgress && check.done != check.total && check.done == 0)
            {
                UpdateTaskStatus(projectId, taskId, 1);
            }
            else if (task.Status == TasksStatus.Todo && check.done != check.total && check.done != 0)
            {
                UpdateTaskStatus(projectId, taskId, 2);
            }
            check = ProjectRepository.GetTaskRateByProjectId(projectId);
            long status = ProjectRepository.GetProjectStatusById(projectId);
            /* Open = 1,
        OnGoing = 2,
        Canceled = 3,
        Done = 4,
        Late = 5*/

            if (check.done != check.total && check.done != 0 && status != 2 && status != 3)
                ProjectRepository.UpdateProjectStatus(projectId, 2);
            else if (check.done == check.total && check.done != 0 && status != 4 && status != 3)
                ProjectRepository.UpdateProjectStatus(projectId, 4);
            else if (check.done != check.total && check.done != 0 && status == 4)
                ProjectRepository.UpdateProjectStatus(projectId, 2);
            else if (check.done == 0 && status == 4)
                ProjectRepository.UpdateProjectStatus(projectId, 1);

            return update;
        }


        public static bool UpdateTaskStatus(long projectId, long taskId, int status)
        {
            bool update = ProjectRepository.UpdateTaskStatus(taskId, status);

            ProjectStatus projectStatus = GetProjectStatusById(projectId);
            (int done, int total) check = ProjectRepository.GetTaskRateByProjectId(projectId);

            if (projectStatus == ProjectStatus.Open && status > 1)
                ProjectRepository.UpdateProjectStatus(projectId, 2);
            else if (projectStatus != ProjectStatus.Done && (check.done != 0 && check.done == check.total))
                ProjectRepository.UpdateProjectStatus(projectId, 4);
            return update;
        }

        public static bool RemoveTaskTag(long taskId)
        {
            return ProjectRepository.RemoveTaskTag(taskId);
        }
    }
}
