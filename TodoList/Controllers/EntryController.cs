using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
        public ActionResult<List<EntryDto>> OnGetAll()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var result = new EntityModel(scope).GetAllEntries();
            return EntryDto.ConvertToDto(result);
        }

        [HttpGet("[action]")]
        public ActionResult<List<EntryDto>> OnGetPage(int page = 0)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var result = new EntityModel(scope).GetEntriesPage(page, out page);
            return EntryDto.ConvertToDto(result, page);
        }

        /// Creates stubs
        [HttpPost]
        public bool OnPostCreateStub()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                return new EntityModel(scope).CreateStubs();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnPost]");
                return false;
            }
        }

        [HttpPost("[action]")]
        public bool OnPostCreate([FromBody] EntryEntity model)
        {
            try
            {
                //if (model.IsModelValid())
                if (_rules.IsModelValid(model))
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    return new EntityModel(scope).CreateEntry(model);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnPostCreate]");
                return false;
            }
        }
    }
}