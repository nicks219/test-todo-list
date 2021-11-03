namespace TodoList.BuisnesProcess
{
    // NB: Имя TaskStatus конфликтует с Threading
    public enum ProblemStatus
    {
        // NB: id в sql идут от 1
        New = 1,
        InProgress = 2,
        Complited = 3,
        Closed = 4,
        Cancelled = 5,
        All = 6
    }
}