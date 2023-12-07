using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using Models.Models.CashDesks;
using Repositories.IRepositories;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class CashDeskController : Controller, IMainController<CashDesk>
    {
        private readonly ICashDeskRepository _cashDeskRepository;
        private readonly IShopRepository _shopRepository;

        public CashDeskController(ICashDeskRepository cashDeskrepository, IShopRepository shopRepository)
        {
            _cashDeskRepository = cashDeskrepository;
            _shopRepository = shopRepository;
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
                _cashDeskRepository.Delete(id.GetValueOrDefault());
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
            CashDesk cashDesk = _cashDeskRepository.GetById(id.GetValueOrDefault());
            if (cashDesk == null)
            {
                return NotFound();
            }

            return View(cashDesk);
        }

        public IActionResult Save(int? id)
        {
            GetAllShops();
            if (id == null)
            {
                return View(new CashDesk());
            }

            CashDesk cashDesk = _cashDeskRepository.GetById(id.GetValueOrDefault());

            if (cashDesk == null)
            {
                return NotFound();
            }

            return View(cashDesk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(int? id, CashDesk model)
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
                        _cashDeskRepository.Create(model);
                    }
                    else
                    {
                        _cashDeskRepository.Edit(model);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            GetAllShops();
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            List<CashDesk> cashDesks = _cashDeskRepository.GetAll();
            return View(cashDesks);
        }

        [NonAction]
        private void GetAllShops()
        {
            List<Shop> shops = _shopRepository.GetAll();
            ViewBag.Shops = new SelectList(shops, nameof(Shop.Id), nameof(Shop.Contact));
        }
    }
}
