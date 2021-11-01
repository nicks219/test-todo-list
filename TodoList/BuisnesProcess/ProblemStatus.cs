namespace TodoList.BuisnesProcess
{
    /// Имя TaskStatus конфликтует с Threading
    public enum ProblemStatus
    {
        // id в sql идут от 1, enum идут от 0
        New = 1,
        InProgress = 2,
        Complited = 3,
        Closed = 4,
        Cancelled = 5,
        All = 6
    }
}