using System;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess.DTO
{
    public static class StubCreator
    {
        public readonly static string lorenIpsum =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
            "incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud " +
            "exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure " +
            "dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit " +
            "anim id est laborum.";

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
                Title = "First Task",
                Description = lorenIpsum
            };
            repo.CreateEntry(entry);
            return result1 + result2 == 2;
        }
    }
}
