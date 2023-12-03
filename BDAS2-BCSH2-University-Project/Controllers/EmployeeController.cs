using Models.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Models.Models.Login;
using System.Data;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class EmployeeController : Controller, IMainController<Employee>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
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
                _employeeRepository.Delete(id.GetValueOrDefault());
            }
            catch(Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employer = _employeeRepository.GetById(id.GetValueOrDefault());
            if (employer == null)
            {
                return NotFound();
            }
            return View(employer);

        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Save(int? id)
        {
            if (id == null)
            {
                return View(new Employee());
            }

            Employee employer = _employeeRepository.GetById(id.GetValueOrDefault());

            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Save(int? id, Employee model)
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
                    if (id == null)
                    {
                        _employeeRepository.Create(model);
                    }
                    else
                    {
                        _employeeRepository.Edit(model);
                    }

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
        [Authorize]
        public IActionResult Index()
        {
            List<Employee> employers = _employeeRepository.GetAll();
            return View(employers);
        }

    }
}
