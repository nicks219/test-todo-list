using System.Collections.Generic;
using TodoList.DataAccess.TodoContext;

namespace TodoList.BuisnesProcess
{
    public class Rules : IRules
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
            if (string.IsNullOrEmpty(entry.Title) || string.IsNullOrEmpty(entry.Report)) return false;

            if (string.IsNullOrEmpty(entry.Description)) return false;

            if (entry.Title.Length < TITLE_MIN_LENGTH) return false;

            if (entry.Report.Length < REPORT_MIN_LENGTH) return false;

            if (entry.Initiator == null || entry.Executor == null) return false;

            /// DateTime возможно лучше создавать на стороне сервера
            /// по умолчанию этот тип имеет значение DateTime.MinValue
            if (entry.Deadline == System.DateTime.MinValue) entry.Deadline = System.DateTime.Now;
            //if (entry.Deadline == System.DateTime.MinValue) return false;

            /// 
            return true;
        }
    }
}
