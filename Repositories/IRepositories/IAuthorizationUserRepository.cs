using Models.Models;
using Models.Models.Login;

namespace Repositories.IRepositories
{
    public interface IAuthorizationUserRepository
    {
        List<UserRole> Authenticate(LoginModel loginModel);
        List<UserRole> GetRolesForUser(int userId);
        void Register(AutorisedUser registrateModel);
        List<AutorisedUser> GetAutorisedUsers();
        AutorisedUser GetAutorisedUser(int id);
        SimulatedUser Simulate(int userId);
        void Edit(AutorisedUser autorisedUser);
        void Delete(int id);
        Image GetUserImage(int userId);
        void SaveImageForUser(Image image, int userId);
        List<Image> GetAllImages();
    }
}
