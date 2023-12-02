using Microsoft.AspNetCore.Mvc;
using Models.Models.Login;

namespace BDAS2_BCSH2_University_Project.Interfaces
{
    public interface IAuthorizationUserController
    {
        IActionResult Index();
        IActionResult Login(string returnUrl = null);
        Task<IActionResult> Login(LoginModel loginModel, string returnUrl);
        Task<IActionResult> Logout();

        // TODO add simulate method
        //IActionResult Simulate();
        IActionResult Details(int? id);
        IActionResult Registrate();
        IActionResult Registrate(AutorisedUser registrateModel);
        IActionResult Delete(int? id);
        IActionResult Edit(int? id);
        IActionResult Edit(int? id, AutorisedUser model);
    }
}
