using BDAS2_BCSH2_University_Project.Models.Login;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_BCSH2_University_Project.Interfaces
{
    public interface IAuthorizationUserController
    {
        IActionResult Login(string returnUrl = null);
        Task<IActionResult> Login(LoginModel loginModel, string returnUrl);
        Task<IActionResult> Logout();

        // TODO add simulate method
        // TODO show all and one registered users
        // TODO write this method

        IActionResult Registrate();
        IActionResult Registrate(RegistrateModel registrateModel);
    }
}
