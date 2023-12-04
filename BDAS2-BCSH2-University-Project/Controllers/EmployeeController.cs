using Models.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Models.Models.Login;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories.Repositories;
using static System.Formats.Asn1.AsnWriter;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class EmployeeController : Controller, IMainController<Employee>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMainRepository<Position> _positionRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IAddressRepository _addressRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IMainRepository<Position> positionRepository, 
                                        IShopRepository shopRepository, IAddressRepository addressRepository)
        {
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
            _shopRepository = shopRepository;
            _addressRepository = addressRepository;
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
            GetData(id.GetValueOrDefault());
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
            GetData(id.GetValueOrDefault());
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            List<Employee> employers = _employeeRepository.GetAll();
            return View(employers);
        }

        [NonAction]
        private void GetData(int id)
        {
            GetAllShops();
            GetAllPosition();
            GetAddresses(id);
        }

        [NonAction]
        private void GetAddresses(int id)
        {
            List<Address> addresses = _addressRepository.GetAddressesForEmployee(id);
            ViewBag.Addresses = new SelectList(addresses, nameof(Address.Id), nameof(Address.StringAddress));
        }

        [NonAction]
        private void GetAllShops()
        {
            List<Shop> shops = _shopRepository.GetAll();
            ViewBag.Shops = new SelectList(shops, nameof(Shop.Id), nameof(Shop.StringAddress));
        }

        [NonAction]
        private void GetAllPosition()
        {
            List<Position> positions = _positionRepository.GetAll();
            ViewBag.Positions = new SelectList(positions, nameof(Position.Id), nameof(Position.Name));
        }

    }
}
