namespace RandomSongSearchEngine.Dto
{
    public class LoginDto
    {
        public string UserName { get; }

        public LoginDto(string userName)
        {
            UserName = userName;
        }
    }
}
