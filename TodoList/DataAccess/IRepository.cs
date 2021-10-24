using System;
using System.Linq;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    // закомментированные методы не удаляю, возможно пригодятся
    public interface IRepository : IAsyncDisposable, IDisposable
    {
        IQueryable<EntryEntity> GetAllEntries();

        //IQueryable<EntryEntity> FindByStatus(ProblemStatus problemStatus);

        //IQueryable<EntryEntity> FindByUser(int userId);

        IQueryable<EntryEntity> GetEntriesPage(int currentPage, int pageSize);

        EntryEntity GetEntry(int id);

        UserEntity GetUser(int id);

        UserStatusEntity GetUserStatus(UserStatus userStatus);

        ProblemStatusEntity GetProblemStatus(ProblemStatus problemStatus);

        //int CreateEntry(EntryEntity entry);

        //int CreateUser(UserEntity user);

        //int CreateUserStatus(UserStatusEntity user);

        //int CreateProblemStatus(ProblemStatusEntity problemStatus);

        int Create(IEntity entity);

        int GetEntriesCount();
    }
}