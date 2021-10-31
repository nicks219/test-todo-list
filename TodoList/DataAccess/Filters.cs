using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.DataAccess.TodoContext;

namespace TodoList.DataAccess
{
    public class Filters<T>
    {
        //public Func<EntryEntity, bool> predicate;
        //public Func<EntryEntity, int> keySelector;
        //public delegate bool pr (EntryEntity en);
        //public delegate int ks(EntryEntity en);

        public T lambda;

        public Filters(T lambda)
        {
            this.lambda  = lambda;
        }

        public Filters() { }

        public T Get()
        {
            return lambda;
        }

        public T Get(T lambda)
        {
            this.lambda = lambda;
            return lambda;
        }

        public void Set(T lambda)
        {
            this.lambda = lambda;
        }
    }
}
