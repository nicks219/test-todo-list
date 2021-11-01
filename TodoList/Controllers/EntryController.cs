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
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var result = new EntityModel(scope).GetEntry(id);
                return EntryDto.ConvertToDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnGetEntry]");
                return EntryDto.Error("[EntryController: OnGetEntry]")[0];
            }
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
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var result = new EntityModel(scope).GetProblemStatuses();
                return ProblemStatusDto.ConvertToDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnGetProblemStatuses]");
                return null;
            }
        }

        // Для Create
        [HttpGet("[action]")]
        public ActionResult<List<UserDto>> OnGetUsers()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var result = new EntityModel(scope).GetUsers();
                return UserDto.ConvertToDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnGetProblemStatuses]");
                return null;
            }
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

        // TODO: РАБОТАЕМ НАД CREATE
        [HttpPost("[action]")]
        public ActionResult<EntryDto> OnPostCreate([FromBody] EntryDto dto)
        {
            try
            {
                // найди куда IRules прикрутить - вовнутрь прокинь
                //if (_rules.IsModelValid(model))
                using var scope = _serviceScopeFactory.CreateScope();
                var result = new EntityModel(scope).CreateEntry(dto);
                return EntryDto.ConvertToDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnPostCreate]");
                return new EntryDto();
            }
        }

        [HttpPut]
        public ActionResult<EntryDto> OnPutUpdate([FromBody] EntryDto dto)
        {
            try
            {
                // найди куда IRules прикрутить - вовнутрь прокинь
                using var scope = _serviceScopeFactory.CreateScope();
                var result = new EntityModel(scope).UpdateEntry(dto);
                return EntryDto.ConvertToDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnPutUpdate]");
                return new EntryDto();
            }
        }
    }
}