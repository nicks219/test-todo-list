using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess.DTO
{
    public static class EntryRules
    {
        private static readonly int TITLE_MIN_LENGTH = 5;
        private static readonly int REPORT_MIN_LENGTH = 5;

        /// Entry model validation rules
        public static bool IsValid(this EntryEntity entry)
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
