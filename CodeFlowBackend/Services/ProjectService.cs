using CodeFlowBackend.DTO;
using CodeFlowBackend.Model.Project;
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

    }
}
