using Models.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Models.Models.Login;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class AddressController : Controller, IMainController<Address>
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                _addressRepository.Delete(id.GetValueOrDefault());
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Address address = _addressRepository.GetById(id.GetValueOrDefault());
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        [HttpGet]
        public IActionResult Index()
        {
           List<Address> addresses = _addressRepository.GetAll();
            return View(addresses);
        }

        [HttpGet]
        public IActionResult Save(int? id)
        {

            if (id == null)
            {
                return View(new Address());
            }

            Address address = _addressRepository.GetById(id.GetValueOrDefault());

            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        [HttpPost]
        public IActionResult Save(int? id, Address model)
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
                        _addressRepository.Create(model);
                    }
                    else
                    {
                        _addressRepository.Edit(model);
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
    }
}
