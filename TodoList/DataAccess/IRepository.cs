using System;
using System.Linq;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    // закомментированные методы не удаляю, возможно пригодятся
    public interface IRepository : IAsyncDisposable, IDisposable
    {
        EntryEntity GetEntry(int id);

        UserEntity GetUser(int id);

        UserStatusEntity GetUserStatus(UserStatus userStatus);

        ProblemStatusEntity GetProblemStatus(ProblemStatus problemStatus);

        // GetAll~ кандидат на "общий" метод с IEntity
        //IQueryable<EntryEntity> GetAllEntries();

        IQueryable<ProblemStatusEntity> GetAllProblemStatuses();

        IQueryable<EntryEntity> GetEntries(int currentPage, int pageSize);

        IQueryable<EntryEntity> GetEntries(int currentPage, int pageSize, int filter);

        int Create(IEntity entity);

        int Update(EntryEntity entry);

        int GetEntriesCount();

        bool StatusExist();
    }
}