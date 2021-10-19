using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using TodoList.DataAccess;
using TodoList.DataAccess.DTO;
using TodoList.DataAccess.TodoContext;

namespace TodoList.Models
{
    public class EntityModel
    {
        private readonly IServiceScope _serviceScope;

        public EntityModel(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
        }

        public List<EntryEntity> GetEntries()
        {
            using var repo = _serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            var result = repo.GetAllEntries().ToList();
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
            var result = repo.CreateEntry(entry);
            return result != 0;
        }
    }
}