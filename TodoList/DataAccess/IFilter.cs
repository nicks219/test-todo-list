using System;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    // NB: Не удалось использовать Filters через DI
    public interface IFilter
    {
        public void SetPredicate(Func<EntryEntity, bool> predicate);

        public Func<EntryEntity, bool> GetPredicate();

        public void SetKeySelector(Func<EntryEntity, int> keySelector);

        public Func<EntryEntity, int> GetKeySelector();
    }
}
