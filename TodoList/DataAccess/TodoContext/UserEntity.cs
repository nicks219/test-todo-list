using TodoList.BuisnesProcess;

namespace TodoList.DataAccess.TodoContext
{
    public class UserEntity
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public UserStatus UserStatus { get; set; }

        /// Many-to-many на данный момент не нужно, но понадобится в дальнейшем
        //public ICollection<UserEntryEntity> UserEntryBind { get; set; }
    }
}
