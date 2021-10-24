namespace TodoList.BuisnesProcess
{
    /// Имя TaskStatus конфликтует с Threading
    public enum ProblemStatus
    {
        // id в sql идут от 1, enum идут от 0
        NEW = 1,
        IN_PROGRESS = 2,
        COMPLITED = 3,
        CLOSED = 4,
        CANCELLED = 5,
        ALL = 6
    }
}