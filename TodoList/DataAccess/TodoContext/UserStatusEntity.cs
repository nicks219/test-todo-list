namespace TodoList.DataAccess.TodoContext
{
    public class UserStatusEntity : IEntity
    {
        public int UserStatusId { get; set; }

        //public UserStatus UserStatus { get; set; }

        public string UserStatusName { get; set; }
    }
}