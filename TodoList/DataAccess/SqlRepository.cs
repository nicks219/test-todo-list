﻿using Microsoft.EntityFrameworkCore;
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
        private Func<EntryEntity, bool> predicate;// = (e) => { return true; };
        private Func<EntryEntity, int> keySelector;// = (e) => { return e.EntryId; };

        private readonly Filters<Func<EntryEntity, bool>> filterPredicate = new();
        private readonly Filters<Func<EntryEntity, int>> filterKeySelector = new();

        public SqlRepository(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<TodoContext.TodoContext>();
            // по умолчанию
            predicate = filterPredicate.Get((e) => { return true; });
            keySelector = filterKeySelector.Get((e) => { return e.EntryId; });
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
                .FirstOrDefault(e => e.EntryId == id);
        }

        public int GetEntriesCount()
        {
            return _context.Entries.Count();
        }

        public int GetEntriesCount(int filter)
        {
            predicate = filterPredicate.Get((e) => { return e.TaskStatus.ProblemStatusId == filter; });

            var sqlQuerry = _context
                .Entries
                .Include(p => p.TaskStatus)
                .ToList();

            var linqQuerryResult = sqlQuerry
                .OrderBy(e => keySelector(e))
                .Where(e => predicate(e))
                .Count();

            return linqQuerryResult;
        }

        public IQueryable<EntryEntity> GetEntries(int currentPage, int pageSize, int problemStatusFilter)
        {
            // TODO: позже надо сделать их настройку в отдельном методе
            //keySelector = (e) => { return e.EntryId; };
            //predicate = (e) => { return e.TaskStatus.ProblemStatusId == problemStatusFilter; };
            predicate = filterPredicate.Get((e) => { return e.TaskStatus.ProblemStatusId == problemStatusFilter; });

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