﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TodoList.BuisnesProcess;
using TodoList.DataAccess.DTO;
using TodoList.Models;

namespace TodoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntryController : ControllerBase
    {
        private readonly ILogger<EntryController> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IValidator _validator;

        public EntryController(ILogger<EntryController> logger, IServiceScopeFactory serviceScopeFactory, IValidator validator)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _validator = validator;
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
                // TODO: верни что-то более информативное
                return null;
            }
        }

        [HttpPost]
        public bool? OnPostCreateStub()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                return new EntityModel(scope).CreateEntryStub();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnPostCreateStub]");
                // TODO: верни что-то более информативное
                return null;
            }
        }

        //[Authorize]
        [HttpPost("[action]")]
        public ActionResult<EntryDto> OnPostCreate([FromBody] EntryDto dto)
        {
            //var isAuthenticated = _validator.IsAuthenticated(HttpContext);
            if (!_validator.IsInRole(HttpContext, "Initiator"))
            {
                return EntryDto.Error("[EntryController: Login please]")[0];
            }

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var result = new EntityModel(scope, _validator).CreateEntry(dto);
                return EntryDto.ConvertToDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnPostCreate]");
                return EntryDto.Error("[EntryController: OnPostCreate]")[0];
            }
        }

        [HttpPut]
        public ActionResult<EntryDto> OnPutUpdate([FromBody] EntryDto dto)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var result = new EntityModel(scope, _validator).UpdateEntry(dto);
                return EntryDto.ConvertToDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EntryController: OnPutUpdate]");
                return EntryDto.Error("[EntryController: OnPutUpdate]")[0];
            }
        }
    }
}