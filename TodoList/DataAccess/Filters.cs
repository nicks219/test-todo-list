using System;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    public class Filters
    {
        private Func<EntryEntity, bool> _predicate;
        private Func<EntryEntity, int> _keySelector;

        public Filters()
        {
            // NB: правила фильтрации по умолчанию
            _predicate = (e) => { return true; };
            _keySelector = (e) => { return e.EntryId; };
        }

        public void SetPredicate(Func<EntryEntity, bool> predicate)
        {
            _predicate = predicate;
        }

        public Func<EntryEntity, bool> GetPredicate()
        {
            return _predicate;
        }

        public void SetKeySelector(Func<EntryEntity, int> keySelector)
        {
            _keySelector = keySelector;
        }

        public Func<EntryEntity, int> GetKeySelector()
        {
            return _keySelector;
        }
    }
}
