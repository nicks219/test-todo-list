using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.BuisnesProcess;
using TodoList.DataAccess;
using TodoList.DataAccess.DTO;
using TodoList.DataAccess.TodoContext;

namespace TodoList.Models
{
    public class EntityModel
    {
        private const int PageSize = 3;
        private const int MinPage = 0;
        private const int NoFilter = 6;
        private readonly IServiceScope _serviceScope;
        private readonly IRules _rules;

        public EntityModel(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
        }

        public EntityModel(IServiceScope serviceScope, IRules rules) : this(serviceScope)
        {
            this._rules = rules;
        }

        public EntryEntity GetEntry(int id)
        {
            // костыль: сделай проверку входных данных, при невалидном id бд ничего не выдаёт
            if (id == 0)
            {
                id = 1;
            }
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            var result = repo.GetEntry(id);
            return result;
        }

        public List<EntryEntity> GetEntries(int currentPage, int filter, out int correctedPage)
        {
            // фильтр через DI в таком виде работает некорректно
            Filters filters = new();
            if (filter != NoFilter)
            {
                filters.SetPredicate((e) => { return e.TaskStatus.ProblemStatusId == filter; });
            }

            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();

            repo.SetFilters(filters);

            currentPage = FixPageNumber(currentPage, repo.GetEntriesCount());

            var result = repo.GetEntries(currentPage, PageSize).ToList();

            correctedPage = currentPage;
            return result;
        }

        public bool CreateEntryStub()
        {
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            return repo.CreateEntryStub();
        }

        internal List<UserEntity> GetUsers()
        {
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            return repo.GetAllUsers().ToList();
        }

        internal List<ProblemStatusEntity> GetProblemStatuses()
        {
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            var result = repo.GetAllProblemStatuses().ToList();
            return result;
        }

        public EntryEntity CreateEntry(EntryDto dto)
        {
            if (dto.Initiator == null || dto.Executor == null || dto.TaskStatus == null) 
            {
                return EntryDto.ConvertFromDto(EntryDto.Error("[Create: Undefined dto, please reload your browser page]")[0]);
            }

            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            ProblemStatusEntity taskStatus = repo.GetProblemStatus((ProblemStatus)dto.TaskStatus.ProblemStatusId);
            UserEntity user1 = repo.GetUser(dto.Initiator.UserId);
            UserEntity user2 = repo.GetUser(dto.Executor.UserId);
            dto.TaskStatus = taskStatus;
            dto.Initiator = user1;
            dto.Executor = user2;

            EntryEntity model = EntryDto.ConvertFromDto(dto);

            if (_rules != null && _rules.IsModelValid(model))
            {
                model.EntryId = 0;
                repo.Create(model);
                var result = repo.GetEntry(model.EntryId);
                return result;
            }

            return EntryDto.ConvertFromDto(EntryDto.Error("[Create: Wrong arguments]")[0]);
        }

        internal EntryEntity UpdateEntry(EntryDto dto)
        {
            if (dto.Initiator == null || dto.Executor == null || dto.TaskStatus == null)
            {
                return EntryDto.ConvertFromDto(EntryDto.Error("[Update: Undefined dto, please reload your browser page]")[0]);
            }

            // TODO: ты обновляешь все связанные таблицы (dbo.ProblemStatus)
            // потому приходится вставлять в них new сущности (стр.99) и менять в них поля
            // TODO: обновляй только то, что необходимо (dbo.Entries)
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();

            ProblemStatusEntity taskStatus = repo.GetProblemStatus((ProblemStatus)dto.TaskStatus.ProblemStatusId);
            UserEntity user1 = repo.GetUser(dto.Initiator.UserId);
            UserEntity user2 = repo.GetUser(dto.Executor.UserId);
            dto.TaskStatus = taskStatus;
            dto.Initiator = user1;
            dto.Executor = user2;

            EntryEntity model = EntryDto.ConvertFromDto(dto);

            if (_rules != null && _rules.IsModelValid(model))
            {
                repo.Update(model);
                var result = repo.GetEntry(model.EntryId);
                return result;
            }

            return EntryDto.ConvertFromDto(EntryDto.Error("[Update: Wrong arguments]")[0]);
        }

        private static int FixPageNumber(int currentPage, int entriesCount)
        {
            int maxPage = Math.DivRem(entriesCount, PageSize, out int remainder);
            if (remainder > 0)
            {
                maxPage++;
            }

            if (currentPage >= maxPage)
            {
                currentPage = --maxPage;
            }

            if (currentPage < MinPage)
            {
                currentPage = MinPage;
            }

            return currentPage;
        }
    }
}