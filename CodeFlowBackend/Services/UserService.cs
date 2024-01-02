using CodeFlowBackend.DTO;
using CodeFlowBackend.Model.User;
using CodeFlowBackend.Model.User.Enum;
using CodeFlowBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Services
{
    public static class UserService
    {
        private static long AddDeveloper(RegisterDTO newUser)
        {
            Developer developer = new Developer(newUser.Username, newUser.FirstName, 
                newUser.LastName, newUser.Email, newUser.Password, (ExperienceLevelDeveloper) newUser.SpecificToUserRole );

            if (UserRepository.AddDeveloper(developer))
                return UserRepository.GetUserIdByEmail(newUser.Email);
            
             return 0;
        }

        private static long AddTechLeader(RegisterDTO newUser)
        {
            TechLeader techLeader = new TechLeader((Specializations)newUser.SpecificToUserRole, newUser.Username, newUser.FirstName,
                newUser.LastName, newUser.Email, newUser.Password);

            if (UserRepository.AddTechLeader(techLeader))
                return UserRepository.GetUserIdByEmail(newUser.Email);

            return 0;
        }

        public static LoginResponseDTO Login(LoginRequestDTO loginRequest)
        {
            (long? userId, bool? isTechLeader) response = UserRepository.Login(loginRequest.Username, loginRequest.Password);
            return new LoginResponseDTO(response.userId, response.isTechLeader);
        }


        public static bool IsUsernameAvailable(string username)
        {
            return UserRepository.GetIdByUsername(username) == 0;
        }

        public static long AddUser(RegisterDTO newUser)
        {
            long newUserId = 0;

            if (newUser.IsTechLeader)
                newUserId = AddTechLeader(newUser);
            else
                newUserId = AddDeveloper(newUser);

            return newUserId;
        }

        public static bool IsEmailAvailable(string email)
        {
            return UserRepository.GetUserIdByEmail(email) == 0;
        }

        public static string GetUserFirstNameByID(long userId)
        {
            return UserRepository.GetUserFirstNameById(userId);
        }

        public static UserDTO GetUserById(long userId)
        {
            long userRole = UserRepository.GetUserRoleById(userId);

            switch (userRole)
            {
                case 1:
                    TechLeader techleader = UserRepository.GetTechLeaderById(userId);
                    return new UserDTO(techleader.Id, techleader.FirstName, techleader.LastName, 
                        techleader.Email, techleader.Username, techleader.DateJoined, true, (int)techleader.Specialization);
                case 2:
                    Developer dev = UserRepository.GetDeveloperById(userId);
                    return new UserDTO(dev.Id, dev.FirstName, dev.LastName, dev.Email, dev.Username, dev.DateJoined, true, (int)dev.ExperienceLevel);
                default: return null;
            }
        }

        public static int GetUsersProjectCountById(long id)
        {
            return UserRepository.GetUsersProjectCountById(id);
        }
        
        public static int GetUsersDoneTasksCountById(long id)
        {
            return UserRepository.GetUsersDoneTasksCountById(id);
        }

        public static bool Update(UserDTO updatedUser)
        {
            if (updatedUser.IsTechLeader)
                return UserRepository.UpdateTechLeader(updatedUser.Id, updatedUser.FirstName, updatedUser.LastName, updatedUser.Email);
            else
                return UserRepository.UpdateDeveloper(updatedUser.Id, updatedUser.FirstName, updatedUser.LastName, updatedUser.Email, updatedUser.SpecificToUserRole);
        }

        public static void EnterProject(long userId, string projectCode)
        {
            UserRepository.EnterProject(userId, projectCode);
        }

        internal static string GetUsersUsernameById(string userId)
        {
            return UserRepository.GetUsersUsernameById(userId);
        }
    }

}
