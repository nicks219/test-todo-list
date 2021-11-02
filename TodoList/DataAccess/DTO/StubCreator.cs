using System;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess.DTO
{
    public static class StubCreator
    {
        public static readonly string LorenIpsum =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
            "incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud " +
            "exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure " +
            "dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit " +
            "anim id est laborum.";

        public static bool CreateStubs(this IRepository repo)
        {
            // должно создаваться один раз
            if (!repo.StatusExist())
            {
                foreach (var status in Enum.GetNames(typeof(UserStatus)))
                {
                    //var us = (UserStatus)Enum.Parse(typeof(UserStatus), status);
                    //repo.CreateUserStatus(new UserStatusEntity()
                    repo.Create(new UserStatusEntity()
                        { UserStatusName = status });
                }

                foreach (var status in Enum.GetNames(typeof(ProblemStatus)))
                {
                    //var ps = (ProblemStatus)Enum.Parse(typeof(ProblemStatus), status);
                    //repo.CreateProblemStatus(new ProblemStatusEntity()
                    repo.Create(new ProblemStatusEntity()
                        { ProblemStatusName = status });
                }

                //var result1 = repo.CreateUser(new UserEntity()
                repo.Create(new UserEntity()
                {
                    Name = "John",
                    UserStatus = repo.GetUserStatus(UserStatus.Initiator)
                });
                //var result2 = repo.CreateUser(new UserEntity()
                repo.Create(new UserEntity()
                {
                    Name = "Slame",
                    UserStatus = repo.GetUserStatus(UserStatus.Executor)
                });
            }

            if (!repo.StatusExist())
            {
                var user1 = repo.GetUser(1);
                var user2 = repo.GetUser(2);
                var status1 = repo.GetProblemStatus(ProblemStatus.Closed);

                //repo.CreateEntry(entry);
                int count = 2;
                while (count-- != 0)
                {
                    var date = DateTime.Now;
                    var entry1 = new EntryEntity()
                    {
                        Initiator = user1,
                        Executor = user2,
                        Deadline = date,
                        CompletionDate = date,
                        StartDate = date,
                        Title = "Next Task",
                        Description = LorenIpsum,
                        TaskStatus = status1
                    };
                    repo.Create(entry1);
                }
            }

            return true;
        }
    }
}