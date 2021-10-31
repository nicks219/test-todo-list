using Microsoft.AspNetCore.Mvc;
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

        public EntityModel(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
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
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            int entriesCount = repo.GetEntriesCount();

            // TODO вынеси в одтельный метод
            int MaxPage = Math.DivRem(entriesCount, PageSize, out int remainder);
            if (remainder > 0)
            {
                MaxPage++;
            }
            if (currentPage >= MaxPage)
            {
                currentPage = --MaxPage;
            }

            if (currentPage < MinPage)
            {
                currentPage = MinPage;
            }

            var result = new List<EntryEntity>();

            if (filter == NoFilter)
            {
                result = repo.GetEntries(currentPage, PageSize).ToList();
            }
            else
            {
                result = repo.GetEntries(currentPage, PageSize, filter).ToList();
            }

            correctedPage = currentPage;
            return result;
        }

        public bool CreateStub()
        {
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            return repo.CreateStubs();
        }

        public bool CreateEntry(EntryEntity entry)
        {
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            var result = repo.Create(entry);
            return result != 0;
        }

        internal List<ProblemStatusEntity> GetProblemStatuses()
        {
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            return repo.GetAllProblemStatuses().ToList();
        }

        internal EntryEntity UpdateEntry(EntryDto dto)
        {
            // TODO: ты обновляешь все связанные таблицы (dbo.ProblemStatus)
            // потому приходится вставлять в них new сущности (стр.99) и менять в них поля
            // TODO: обновляй только то, что необходимо (dbo.Entries)
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();

            var taskStatus = repo.GetProblemStatus((ProblemStatus)dto.TaskStatus.ProblemStatusId);
            dto.TaskStatus = taskStatus;

            EntryEntity model = EntryDto.ConvertFromDto(dto);

            var result2 = repo.Update(model);

            var result = repo.GetEntry(model.EntryId);
            return result; // != 0
        }
    }
}