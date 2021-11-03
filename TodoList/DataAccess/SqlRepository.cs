using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RandomSongSearchEngine.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    public class SqlRepository : IRepository
    {
        private readonly TodoContext.TodoContext _context;
        private Func<EntryEntity, bool> _predicate;
        private Func<EntryEntity, int> _keySelector;

        public SqlRepository(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<TodoContext.TodoContext>();
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

        public IQueryable<EntryEntity> GetEntries(int currentPage, int pageSize)
        {
            var contextQuery = _context
                .Entries
                .Include(e => e.Initiator.UserStatus)
                .Include(e => e.Executor.UserStatus)
                .Include(p => p.TaskStatus)
                .ToList();
            // NB: возвращаю IQueryable с надеждой на будущие изменения
            var linqQuery = contextQuery
                .OrderBy(e => _keySelector(e))
                .Where(e => _predicate(e))
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .AsQueryable();

            return linqQuery;
        }

        public int GetEntriesCount()
        {
            var contextQuerry = _context
                .Entries
                .Include(p => p.TaskStatus)
                .ToList();
            // NB: возвращаю IQueryable с надеждой на будущие изменения
            var linqQuery = contextQuerry
                .OrderBy(e => _keySelector(e))
                .Where(e => _predicate(e))
                .Count();

            return linqQuery;
        }

        public UserEntity GetUser(int id)
        {
            var result = _context.Users.Find(id);
            return result;
        }

        public UserEntity GetUser(LoginDto login)
        {
            return _context.Users
                .FirstOrDefault(u => u.Name == login.UserName);
        }

        public IQueryable<UserEntity> GetAllUsers()
        {
            return _context
                .Users
                .Include(s => s.UserStatus)
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

        public IQueryable<ProblemStatusEntity> GetAllProblemStatuses()
        {
            return _context.ProblemStatus
                .Select(s => s)
                .AsNoTracking();
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

        public int Update(EntryEntity entry)
        {
            _context.Entries.Update(entry);
            var result = _context.SaveChanges();
            return result;
        }

        public bool StatusExist()
        {
            return _context.Set<UserStatusEntity>().Any();
        }

        public void SetFilters(Filters filters)
        {
            _predicate = filters.GetPredicate();
            _keySelector = filters.GetKeySelector();
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