using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.DataAccess;
using TodoList.DataAccess.DTO;
using TodoList.DataAccess.TodoContext;

namespace TodoList.Models
{
    public class EntityModel
    {
        private const int PageSize = 3;
        private const int MinPage = 0;
        private readonly IServiceScope _serviceScope;

        public EntityModel(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
        }

        public List<EntryEntity> GetAllEntries()
        {
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            var result = repo.GetAllEntries().ToList();
            return result;
        }

        public List<EntryEntity> GetEntriesPage(int currentPage, out int correctedPage)
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

            var result = repo.GetEntriesPage(currentPage, PageSize).ToList();
            correctedPage = currentPage;
            return result;
        }

        public bool CreateStubs()
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
    }
}