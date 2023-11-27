namespace BDAS2_BCSH2_University_Project.Models.Login
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
    }
}