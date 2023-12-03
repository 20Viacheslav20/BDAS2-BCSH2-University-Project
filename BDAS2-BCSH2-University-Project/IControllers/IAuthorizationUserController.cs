using Microsoft.AspNetCore.Mvc;
using Models.Models.Login;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface IAuthorizationUserController
    {
        IActionResult Index();
        IActionResult Login(string returnUrl = null);
        Task<IActionResult> Login(LoginModel loginModel, string returnUrl);
        Task<IActionResult> Logout();
        Task<IActionResult> Simulate(int? id);
        Task<IActionResult> StopSimulation();
        IActionResult Details(int? id);
        IActionResult Registrate();
        IActionResult Registrate(AutorisedUser registrateModel);
        IActionResult Delete(int? id);
        IActionResult Edit(int? id);
        IActionResult Edit(int? id, AutorisedUser model, IFormFile file);
        IActionResult AllImages();
    }
}
