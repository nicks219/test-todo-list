using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    // закомментированные методы не удаляю, возможно пригодятся
    public class SqlRepository : IRepository
    {
        private readonly TodoContext.TodoContext _context;
        private Func<EntryEntity, bool> predicate = (e) => { return true; };
        private Func<EntryEntity, int> keySelector = (e) => { return e.EntryId; };

        public SqlRepository(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<TodoContext.TodoContext>();
        }

        public IQueryable<EntryEntity> GetEntries(int currentPage, int pageSize)
        {
            var result = _context
                .Entries
                .Include(e => e.Initiator.UserStatus)
                .Include(e => e.Executor.UserStatus)
                .Include(p => p.TaskStatus)
                .ToList();
             
            var linqWork = result
                .OrderBy(e => keySelector(e))
                .Where(e => predicate(e))     
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .AsQueryable();
            return linqWork;
        }

        public EntryEntity GetEntry(int id)
        {
            return _context
                .Entries
                .Include(e => e.Initiator.UserStatus)
                .Include(e => e.Executor.UserStatus)
                .Include(p => p.TaskStatus)
                .Where(e => e.EntryId == id)
                .First();
        }

        public IQueryable<EntryEntity> GetEntries(int currentPage, int pageSize, int problemStatusFilter)
        {
            var result = _context
                .Entries
                .Include(e => e.Initiator.UserStatus)
                .Include(e => e.Executor.UserStatus)
                .Include(p => p.TaskStatus)
                .ToList();

            predicate = (e) => { return e.TaskStatus.ProblemStatusId == problemStatusFilter; };
             
            var linqWork = result
                .OrderBy(e => keySelector(e))
                .Where(e => predicate(e))   
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .AsQueryable();

            return linqWork;
        }

        public UserEntity GetUser(int id)
        {
            return _context.Users.Find(id);
        }

        public UserStatusEntity GetUserStatus(UserStatus userStatus)
        {
            return _context.UserStatus.Find((int)userStatus);
        }

        public ProblemStatusEntity GetProblemStatus(ProblemStatus problemStatus)
        {
            return _context.ProblemStatus.Find((int)problemStatus);
        }

        public bool StatusExist()
        {
            return _context.Set<UserStatusEntity>().Count() != 0;
        }

        public int Create(IEntity entity)
        {
            switch (entity.GetType().Name)
            {
                case "UserEntity":
                    _context.Users.Add((UserEntity)entity);
                    break;
                case "EntryEntity":
                    _context.Entries.Add((EntryEntity)entity);
                    break;
                case "UserStatusEntity":                  
                    _context.UserStatus.Add((UserStatusEntity)entity);                                       
                    break;
                case "ProblemStatusEntity":
                    _context.ProblemStatus.Add((ProblemStatusEntity)entity);
                    break;
                default:
                    throw new NotImplementedException("onCREATE ERROR");
            }

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

        public int GetEntriesCount()
        {
            return _context.Entries.Count();
        }

        public int Update(EntryEntity entry)
        {
            _context.Entries.Update(entry);
            var result = _context.SaveChanges();
            return result;
        }

        public IQueryable<ProblemStatusEntity> GetAllProblemStatuses()
        {
            return _context.ProblemStatus
                .Select(s => s)
                .AsNoTracking();
        }
    }
}