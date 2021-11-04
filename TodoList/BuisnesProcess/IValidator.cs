using DataAccess.TodoContext;
using System.Collections.Generic;

namespace TodoList.BuisnesProcess
{
    public interface IValidator
    {
        bool IsModelValid(EntryEntity entry);

        List<ActionStatus> GetValidActions(UserEntity user, EntryEntity entry);

        List<ProblemStatus> GetValidStatuses(UserEntity user, EntryEntity entry);
    }
}
