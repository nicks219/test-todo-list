namespace TodoList.BuisnesProcess
{
    // TODO: напротив админа должен быть юзер, а не инициатор, т.е. постоянная "роль" в системе
    public enum UserStatus
    {
        // id в sql идут от 1, enum идут от 0
        ADMIN = 1,
        INITIATOR = 2,
        EXECUTOR = 3
    }
}