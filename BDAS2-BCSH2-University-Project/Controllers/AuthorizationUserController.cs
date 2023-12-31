﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories.IRepositories;
using Models.Models.Product;
using System.Security.Claims;
using Models.Models.Login;
using Models.Models;
using BDAS2_BCSH2_University_Project.IControllers;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class AuthorizationUserController : Controller, IAuthorizationUserController
    {
        private readonly IAuthorizationUserRepository _authorizationUserRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public AuthorizationUserController(IAuthorizationUserRepository authorizationUserRepository, IEmployeeRepository employeeRepository)
        {
            _authorizationUserRepository = authorizationUserRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Index()
        {
            List<AutorisedUser> users = _authorizationUserRepository.GetAutorisedUsers();
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AutorisedUser autorisedUser = _authorizationUserRepository.GetAutorisedUser(id.GetValueOrDefault());
            if (autorisedUser == null)
            {
                return NotFound();
            }

            autorisedUser.Image = _authorizationUserRepository.GetUserImage(autorisedUser.Id);

            return View(autorisedUser);
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
                    if (roles.Count != 0)
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
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Simulate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                SimulatedUser simulatedUser = _authorizationUserRepository.Simulate(id.GetValueOrDefault());
                if (simulatedUser != null)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, $"Simulation ({simulatedUser.Login})"),
                    };

                    foreach (UserRole role in simulatedUser.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.ToStringValue()));
                    }

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    TempData["Success"] = $"You simulated {simulatedUser.Login}";

                    return RedirectToAction(nameof(Index));
                }
            } catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> StopSimulation()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Success"] = "Simulation stopped successfully.";

            return RedirectToAction(nameof(Index), nameof(Product));
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
            GetEployees();
            return View(new AutorisedUser());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Registrate(AutorisedUser registrateModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _authorizationUserRepository.Register(registrateModel);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            GetEployees();
            return View(registrateModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                _authorizationUserRepository.Delete(id.GetValueOrDefault());
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return View(new AutorisedUser());
            }

            AutorisedUser autorisedUser = _authorizationUserRepository.GetAutorisedUser(id.GetValueOrDefault());

            if (autorisedUser == null)
            {
                return NotFound();
            }

            return View(autorisedUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Edit(int? id, AutorisedUser model, IFormFile file)
        {
            if (id != null)
            {
                if (id != model.Id)
                {
                    return NotFound();
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _authorizationUserRepository.Edit(model);

                    SaveImage(file, id.GetValueOrDefault());

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult AllImages()
        {
            List<Image> images = _authorizationUserRepository.GetAllImages();
            return View(images);
        }


        [NonAction]
        private void GetEployees()
        {
            List<Employee> employees = _employeeRepository.GetEmployeesWithoutAuth();
            ViewBag.Employees = new SelectList(employees, nameof(Employee.Id), nameof(Employee.Name));
        }

        [NonAction]
        private void SaveImage(IFormFile file, int userId)
        {
            if (file != null && file.Length > 0)
            {
                Image image = new()
                {
                    Name = file.FileName,
                    Extension = Path.GetExtension(file.FileName),
                };

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    image.Data = memoryStream.ToArray();
                }
                _authorizationUserRepository.SaveImageForUser(image, userId);
            }
        }

    }

}
