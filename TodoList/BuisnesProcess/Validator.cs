using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using TodoContextLib.TodoContext;

namespace TodoList.BuisnesProcess
{
    public class Validator : IValidator
    {
        private static readonly int TITLE_MIN_LENGTH = 5;
        private static readonly int REPORT_MIN_LENGTH = 5;

        public List<ActionStatus> GetValidActions(UserEntity user, EntryEntity entry)
        {
            return new List<ActionStatus>();
        }

        public List<ProblemStatus> GetValidStatuses(UserEntity user, EntryEntity entry)
        {
            return new List<ProblemStatus>();
        }

        public bool IsModelValid(EntryEntity entry)
        {
            // TODO: на поле Report повешен костыль в EntryDto, реально оно не обрабатывается
            if (string.IsNullOrEmpty(entry.Title) || string.IsNullOrEmpty(entry.Report)) return false;

            if (string.IsNullOrEmpty(entry.Description)) return false;

            if (entry.Title.Length < TITLE_MIN_LENGTH) return false;

            if (entry.Report.Length < REPORT_MIN_LENGTH) return false;

            if (entry.Initiator == null || entry.Executor == null) return false;

            // NB: по умолчанию DateTime имеет значение DateTime.MinValue, он никгда не равен null
            if (entry.Deadline == System.DateTime.MinValue) entry.Deadline = System.DateTime.Now;
            //if (entry.Deadline == System.DateTime.MinValue) return false;

            return true;
        }

        public bool IsInRole (HttpContext httpContext, string role)
        {
            return httpContext.User.IsInRole(role);
        }

        public bool IsAuthenticated(HttpContext httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated;
        }

    }
}
