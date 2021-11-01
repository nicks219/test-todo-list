using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    public class Filters// : IFilter
    {
        private Func<EntryEntity, bool> predicate;
        private Func<EntryEntity, int> keySelector;

        public Filters() 
        {
            // правила фильтрации по умолчанию
            predicate = (e) => { return true; };
            keySelector = (e) => { return e.EntryId; };
        }

        public void SetPredicate(Func<EntryEntity, bool> predicate)
        {
            this.predicate = predicate;
        }

        public Func<EntryEntity, bool> GetPredicate()
        {
            return predicate;
        }

        public void SetKeySelector(Func<EntryEntity, int> keySelector)
        {
            this.keySelector = keySelector;
        }

        public Func<EntryEntity, int> GetKeySelector()
        {
            return keySelector;
        }
    }
}
