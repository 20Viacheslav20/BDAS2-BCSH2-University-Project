using BDAS2_BCSH2_University_Project.Models.Login;

namespace BDAS2_BCSH2_University_Project.Interfaces
{
    public interface IAuthorizationUserRepository
    {
        List<UserRole> Authenticate(LoginModel loginModel);
        User GetUser(int userId);
        List<UserRole> GetRolesForUser(int userId);
        void Register();
        
        // TODO show all registered users
        // TODO write this method
        //void Simulate();

    }
}
