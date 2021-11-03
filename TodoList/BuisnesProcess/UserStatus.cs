namespace TodoList.BuisnesProcess
{
    // TODO: роли admin/user более традиционны
    public enum UserStatus
    {
        // NB: id в sql идут от 1
        Admin = 1,
        Initiator = 2,
        Executor = 3
    }
}