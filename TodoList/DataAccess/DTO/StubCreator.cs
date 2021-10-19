using System;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess.DTO
{
    public static class StubCreator
    {
        public static bool CreateStubs(this IRepository repo)
        {
            var result1 = repo.CreateUser(new UserEntity() { Name = "John", UserStatus = BuisnesProcess.UserStatus.INITIATOR });
            var result2 = repo.CreateUser(new UserEntity() { Name = "Slame", UserStatus = BuisnesProcess.UserStatus.EXECUTOR });
            var user1 = repo.GetUser(1);
            var user2 = repo.GetUser(2);

            var date = DateTime.Now;
            var entry = new EntryEntity()
            {
                Initiator = user1,
                Executor = user2,
                Deadline = date,
                CompletionDate = date,
                StartDate = date,
                Title = "First Task"
            };
            repo.CreateEntry(entry);
            return result1 + result2 == 2;
        }
    }
}
