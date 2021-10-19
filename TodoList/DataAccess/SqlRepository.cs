using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    public class SqlRepository : IRepository
    {
        private readonly TodoContext.TodoContext _context;

        public SqlRepository(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<TodoContext.TodoContext>();
        }

        public IQueryable<EntryEntity> GetAllEntries()
        {
            var result = _context
                .Entries
                .Include(e => e.Initiator)
                .Include(e => e.Executor);
            return result;
        }

        public UserEntity GetUser(int id)
        {
            return _context.Users.Find(id);
        }

        public EntryEntity GetEntry(int id)
        {
            throw new NotImplementedException();
        }

        public int CreateEntry(EntryEntity entry)
        {
            _context.Entries.Add(entry);
            var result = _context.SaveChanges();
            return result;
        }

        public int CreateUser(UserEntity user)
        {
            _context.Users.Add(user);
            var result = _context.SaveChanges();
            return result;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync().ConfigureAwait(false);
        }
    }
}