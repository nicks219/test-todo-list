namespace TodoList.DataAccess.TodoContext
{
    public class UserEntity : IEntity
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public UserStatusEntity UserStatus { get; set; }

        // Many-to-many на данный момент не нужно, но понадобится в дальнейшем
        //public ICollection<UserEntryEntity> UserEntryBind { get; set; }
    }
}
