using BDAS2_BCSH2_University_Project.Models.Login;

namespace BDAS2_BCSH2_University_Project.Interfaces
{
    public interface IAuthorizationUserRepository
    {
        List<UserRole> Authenticate(LoginModel loginModel);
        List<UserRole> GetRolesForUser(int userId);
        void Register(AutorisedUser registrateModel);

        List<AutorisedUser> GetAutorisedUsers();
        AutorisedUser GetAutorisedUser(int id);

        // TODO write this method
        //void Simulate();
        void Edit(AutorisedUser autorisedUser);
        void Delete(int id);
    }
}
