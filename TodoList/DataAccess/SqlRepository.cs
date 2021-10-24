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

        public SqlRepository(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<TodoContext.TodoContext>();
        }

        public IQueryable<EntryEntity> GetAllEntries()
        {
            var result = _context
                .Entries
                .Include(e => e.Initiator.UserStatus)
                .Include(e => e.Executor.UserStatus)
                .Include(p => p.TaskStatus)
                .AsNoTracking();
            return result;
        }

        //public IQueryable<EntryEntity> FindByStatus(ProblemStatus problemStatus)
        //{
        //    var result = _context
        //        .Entries
        //        .Include(e => e.Initiator.UserStatus)
        //        .Include(e => e.Executor.UserStatus)
        //        .Include(p => p.TaskStatus)
        //        .Where(p => p.TaskStatus.ProblemStatusId == (int)problemStatus)
        //        .AsNoTracking();
        //    return result;
        //}

        //public IQueryable<EntryEntity> FindByUser(int userId)
        //{
        //    throw new NotImplementedException();
        //}

        public IQueryable<EntryEntity> GetEntriesPage(int currentPage, int pageSize)
        {
            var result = _context
                .Entries
                .Include(e => e.Initiator.UserStatus)
                .Include(e => e.Executor.UserStatus)
                .Include(p => p.TaskStatus)
                .OrderBy(e => e.Title)///   TODO: должен быть заменяемым
                .Where(e => true)     ///   TODO: должен быть заменяемым
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .AsNoTracking();
            return result;
        }

        public UserEntity GetUser(int id)
        {
            return _context.Users.Find(id);
        }

        public EntryEntity GetEntry(int id)
        {
            return _context.Entries.Find(id);
        }

        public UserStatusEntity GetUserStatus(UserStatus userStatus)
        {
            return _context.UserStatus.Find((int)userStatus);
        }

        public ProblemStatusEntity GetProblemStatus(ProblemStatus problemStatus)
        {
            return _context.ProblemStatus.Find((int)problemStatus);
        }

        //public int CreateEntry(EntryEntity entry)
        //{
        //    _context.Entries.Add(entry);
        //    var result = _context.SaveChanges();
        //    return result;
        //}

        //public int CreateUser(UserEntity user)
        //{
        //    _context.Users.Add(user);
        //    var result = _context.SaveChanges();
        //    return result;
        //}

        //public int CreateUserStatus(UserStatusEntity userStatus)
        //{
        //    _context.UserStatus.Add(userStatus);
        //    var result = _context.SaveChanges();
        //    return result;
        //}

        //public int CreateProblemStatus(ProblemStatusEntity problemStatus)
        //{
        //    //String typeName = problemStatus.GetType().FullName;
        //    //Type entityType = Type.GetType(typeName);

        //    _context.ProblemStatus.Add(problemStatus);
        //    var result = _context.SaveChanges();
        //    return result;
        //}

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
    }
}