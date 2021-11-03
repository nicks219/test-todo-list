using System;
using System.Linq;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.TodoContext;
using TodoList.Dto;

namespace TodoList.DataAccess
{
    public interface IRepository : IAsyncDisposable, IDisposable
    {
        EntryEntity GetEntry(int id);

        UserEntity GetUser(int id);

        UserEntity GetUser(LoginDto login);

        IQueryable<UserEntity> GetAllUsers();

        UserStatusEntity GetUserStatus(UserStatus userStatus);

        ProblemStatusEntity GetProblemStatus(ProblemStatus problemStatus);

        IQueryable<ProblemStatusEntity> GetAllProblemStatuses();

        IQueryable<EntryEntity> GetEntries(int currentPage, int pageSize);

        int Create(IEntity entity);

        int Update(EntryEntity entry);

        int GetEntriesCount();

        bool StatusExist();

        void SetFilters(Filter filters);
    }
}