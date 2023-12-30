using CodeFlowBackend.DTO;
using CodeFlowBackend.Model.User;
using CodeFlowBackend.Model.User.Enum;
using CodeFlowBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static UserBasicInfoDTO GetUserBasicInfo(long userId)
        {
            User user = UserRepository.GetUserById(userId);

            return new UserBasicInfoDTO(user.Id, user.FirstName, user.Role);
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
    }

}
