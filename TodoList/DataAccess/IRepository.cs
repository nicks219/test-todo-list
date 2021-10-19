using System;
using System.Linq;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    public interface IRepository : IAsyncDisposable, IDisposable
    {
        IQueryable<EntryEntity> GetAllEntries();

        EntryEntity GetEntry(int id);

        int CreateEntry(EntryEntity entry);

        UserEntity GetUser(int id);

        int CreateUser(UserEntity user);
    }
}