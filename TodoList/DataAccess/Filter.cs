using System;
using TodoContextLib.TodoContext;

namespace TodoList.DataAccess
{
    /// <summary>
    /// настраиваемый фильтр для Repository
    /// </summary>
    public class Filter
    {
        private Func<EntryEntity, bool> _predicate;
        private Func<EntryEntity, int> _keySelector;

        /// <summary>
        /// создаются правила фильтрации по умолчанию
        /// </summary>
        public Filter()
        {
            _predicate = (e) => { return true; };
            _keySelector = (e) => { return e.EntryId; };
        }

        /// <summary>
        /// установка правил фильтрации
        /// </summary>
        /// <param name="predicate">делегат с правилами</param>
        public void SetPredicate(Func<EntryEntity, bool> predicate)
        {
            _predicate = predicate;
        }

        /// <summary>
        /// запрос правил фильтрации
        /// </summary>
        /// <returns><делегат с правилами/returns>
        public Func<EntryEntity, bool> GetPredicate()
        {
            return _predicate;
        }

        /// <summary>
        /// установка правил сортировки
        /// </summary>
        /// <param name="keySelector">делегат с правилами</param>
        public void SetKeySelector(Func<EntryEntity, int> keySelector)
        {
            _keySelector = keySelector;
        }

        /// <summary>
        /// запрос правил сортировки
        /// </summary>
        /// <returns>делегат с правилами</returns>
        public Func<EntryEntity, int> GetKeySelector()
        {
            return _keySelector;
        }
    }
}
