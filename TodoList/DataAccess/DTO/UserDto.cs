using DataAccess.TodoContext;
using System.Collections.Generic;
using System.Linq;

namespace TodoList.DataAccess.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public UserStatusEntity UserStatusId { get; set; }

        public UserDto(UserEntity user)
        {
            UserId = user.UserId;
            Name = user.Name;
            UserStatusId = user.UserStatus;
        }

        public static List<UserDto> ConvertToDto(List<UserEntity> users)
        {
            return users
                .Select(u => new UserDto(u))
                .ToList();
        }
    }
}
