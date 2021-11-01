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
    // TODO: c filtered страницы на фронты некорректно возвращается после update-> rtrn
    public class SqlRepository : IRepository
    {
        private readonly TodoContext.TodoContext _context;
        private Func<EntryEntity, bool> predicate;
        private Func<EntryEntity, int> keySelector;

        public SqlRepository(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<TodoContext.TodoContext>();
            //_context = serviceProvider.GetService<TodoContext.TodoContext>();
        }

        public void SetFilters(Filters filters)
        {
            predicate = filters.GetPredicate();
            keySelector = filters.GetKeySelector();
        }

        public IQueryable<EntryEntity> GetEntries(int currentPage, int pageSize)
        {
            var contextQuery = _context
                .Entries
                .Include(e => e.Initiator.UserStatus)
                .Include(e => e.Executor.UserStatus)
                .Include(p => p.TaskStatus)
                .ToList();

            var linqQuery = contextQuery
                .OrderBy(e => keySelector(e))
                .Where(e => predicate(e))
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .AsQueryable();

            return linqQuery;
        }

        public EntryEntity GetEntry(int id)
        {
            return _context
                .Entries
                .Include(e => e.Initiator.UserStatus)
                .Include(e => e.Executor.UserStatus)
                .Include(p => p.TaskStatus)
                .FirstOrDefault(e => e.EntryId == id);
        }

        public int GetEntriesCount()
        {
            var contextQuerry = _context
                .Entries
                .Include(p => p.TaskStatus)
                .ToList();

            var linqQuery = contextQuerry
                .OrderBy(e => keySelector(e))
                .Where(e => predicate(e))
                .Count();

            return linqQuery;
        }

        public UserEntity GetUser(int id)
        {
            var result = _context.Users.Find(id);
            return result;

            //var result = _context
            //    .Users
            //    //.Where(u => u.UserId == id)
            //    .Include(u => u.UserStatus)
            //    .FirstOrDefault(u => u.UserId == id);
            // return _context.Users.Find(id);
        }

        public IQueryable<UserEntity> GetAllUsers()
        {
            return _context
                .Users
                .Include(s => s.UserStatus)
                .Select(u => u)//????????????
                .AsQueryable();
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

        public int Update(EntryEntity entry)
        {

            //_context.Attach<UserEntity>(entry.Initiator);
            //_context.Attach<UserEntity>(entry.Executor);
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