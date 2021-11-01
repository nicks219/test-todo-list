using System;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    // Не удалось использовать Filters через DI
    public interface IFilter
    {
        //public const int NoFilter = 6;

        public void SetPredicate(Func<EntryEntity, bool> predicate);

        public Func<EntryEntity, bool> GetPredicate();

        public void SetKeySelector(Func<EntryEntity, int> keySelector);

        public Func<EntryEntity, int> GetKeySelector();
    }
}
