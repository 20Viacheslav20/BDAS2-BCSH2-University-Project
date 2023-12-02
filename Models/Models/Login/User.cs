namespace Models.Models.Login
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
    }
}