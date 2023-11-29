using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using BDAS2_BCSH2_University_Project.Models.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class AuthorizationUserController : Controller, IAuthorizationUserController
    {
        private readonly IAuthorizationUserRepository _authorizationUserRepository;
        public AuthorizationUserController(IAuthorizationUserRepository authorizationUserRepository)
        {
            _authorizationUserRepository = authorizationUserRepository;
        }
    
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<UserRole> roles = _authorizationUserRepository.Authenticate(loginModel);
                    if (roles != null)
                    {
                        List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, loginModel.Login),
                        };

                        foreach (UserRole role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToStringValue()));
                        }

                        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction(nameof(Index), nameof(Product));
                    }
                } catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }          
            }
            return View(loginModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index), nameof(Product));
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Registrate()
        {
            return View(new RegistrateModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Registrate(RegistrateModel registrateModel)
        {
            throw new NotImplementedException();
        }
    }
}
