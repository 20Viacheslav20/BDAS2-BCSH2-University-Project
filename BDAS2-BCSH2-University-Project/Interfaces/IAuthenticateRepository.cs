using BDAS2_BCSH2_University_Project.Models.Login;

namespace BDAS2_BCSH2_University_Project.Interfaces
{
    public interface IAuthenticateRepository
    {
        List<Role> Authenticate(LoginModel loginModel);
        User GetUser(int userId);
        List<Role> GetRolesForUser(int userId);

        // TODO write this method
        //void Register();

    }
}
