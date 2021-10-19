namespace TodoList.BuisnesProcess
{
    /// Имя TaskStatus конфликтует с Threading
    public enum ProblemStatus
    {
        NEW,
        IN_PROGRESS,
        COMPLITED,
        CLOSED,
        CANCELLED,
        ALL
    }
}