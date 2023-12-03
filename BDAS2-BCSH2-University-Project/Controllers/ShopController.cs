using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using Models.Models.Login;
using Repositories.IRepositories;
using System.Data;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class ShopController : Controller, IMainController<Shop>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IAddressRepository _addressRepository;

        public ShopController(IShopRepository shopRepository, IAddressRepository addressRepository)
        {
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
                _shopRepository.Delete(id.GetValueOrDefault());
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Index));
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
            Shop shop = _shopRepository.GetById(id.GetValueOrDefault());
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Save(int? id)
        {
            GetAddresses(id.GetValueOrDefault());
            if (id == null)
            {
                return View(new Shop());
            }

            Shop shop = _shopRepository.GetById(id.GetValueOrDefault());

            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Save(int? id, Shop model)
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
                        _shopRepository.Create(model);
                    }
                    else
                    {
                        _shopRepository.Edit(model);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            GetAddresses(id.GetValueOrDefault());
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            List<Shop> shops = _shopRepository.GetAll();
            return View(shops);
        }

        [NonAction]
        private void GetAddresses(int id)
        {
            List<Address> addresses = _addressRepository.GetAddressesForShop(id);
            ViewBag.Addresses = new SelectList(addresses, nameof(Address.Id), nameof(Address.StringAddress));
        }
    }
}