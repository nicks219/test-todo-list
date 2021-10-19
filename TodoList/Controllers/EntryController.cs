using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.DTO;
using TodoList.DataAccess.TodoContext;
using TodoList.Models;

namespace TodoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntryController : ControllerBase
    {
        /// логгирование пока не подключено
        private readonly ILogger<EntryController> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRules _rules;

        public EntryController(ILogger<EntryController> logger, IServiceScopeFactory serviceScopeFactory, IRules rules)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _rules = rules;
        }

        [HttpGet]
        public ActionResult<List<EntryDto>> OnGet()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var result = new EntityModel(scope).GetEntries();
            return EntryDto.ConvertToDto(result);
        }

        /// Creates stubs
        [HttpPost]
        public bool OnPost()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            return new EntityModel(scope).CreateStubs();
        }

        [HttpPost("[action]")]
        public bool OnPostCreate([FromBody] EntryEntity model)
        {
            //if (model.IsModelValid())
            if (_rules.IsModelValid(model))
            {
                using var scope = _serviceScopeFactory.CreateScope();
                return new EntityModel(scope).CreateEntry(model);
            }
            return false;
        }
    }
}