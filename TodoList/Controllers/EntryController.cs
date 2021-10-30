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

        [HttpGet("[action]")]
        public ActionResult<EntryDto> OnGetEntry(int id = 1)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var result = new EntityModel(scope).GetEntry(id);
            return EntryDto.ConvertToDto(result);
        }

        [HttpGet("[action]")]
        public ActionResult<List<EntryDto>> OnGetPage(int page = 0, int filter = 5)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var result = new EntityModel(scope).GetEntries(page, filter, out page);
                return EntryDto.ConvertToDto(result, page);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnGetPage]");
                return EntryDto.Error("[EntryController: OnGetPage]");
            }
        }

        [HttpGet("[action]")]
        public ActionResult<List<ProblemStatusDto>> OnGetProblemStatuses()
        {
            //return new List<ProblemStatusDto>();
            using var scope = _serviceScopeFactory.CreateScope();
            var result = new EntityModel(scope).GetProblemStatuses();
            return ProblemStatusDto.ConvertToDto(result);
        }

        /// Creates stubs
        [HttpPost]
        public bool? OnPostCreateStub()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                return new EntityModel(scope).CreateStub();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnPostCreateStub]");
                return null;
            }
        }

        [HttpPost("[action]")]
        public bool? OnPostCreate([FromBody] EntryEntity model)
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
                return null;
            }
        }

        [HttpPut]
        public bool? OnPutUpdate([FromBody] EntryEntity model)
        {
            try
            {
                //if (model.IsModelValid())
                if (_rules.IsModelValid(model))
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    return new EntityModel(scope).UpdateEntry(model);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnPutUpdate]");
                return null;
            }
        }
    }
}