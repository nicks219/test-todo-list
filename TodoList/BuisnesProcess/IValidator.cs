using DataAccess.TodoContext;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace TodoList.BuisnesProcess
{
    public interface IValidator
    {
        bool IsModelValid(EntryEntity entry);

        bool IsInRole(HttpContext httpContext, string role);

        bool IsAuthenticated(HttpContext httpContext);

        List<ActionStatus> GetValidActions(UserEntity user, EntryEntity entry);

        List<ProblemStatus> GetValidStatuses(UserEntity user, EntryEntity entry);
    }
}
