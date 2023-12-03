using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.Login;
using Repositories.IRepositories;
using System.Data;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class ShopController : Controller, IMainController<Shop>
    {
        private readonly IMainRepository<Shop> _shopRepository;

        public ShopController(IMainRepository<Shop> shopRepository)
        {
            _shopRepository = shopRepository;
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
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            List<Shop> shops = _shopRepository.GetAll();
            return View(shops);
        }
    }
}