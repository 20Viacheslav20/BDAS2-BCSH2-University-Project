using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using Models.Models.Login;
using Models.Models.Stands;
using Repositories.IRepositories;


namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class StandController : Controller, IMainController<Stand>
    {
        private readonly IStandRepository _standRepository;
        private readonly IShopRepository _shopRepository;

        public StandController(IStandRepository standRepository, IShopRepository shopRepository)
        {
            _standRepository = standRepository;
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
                _standRepository.Delete(id.GetValueOrDefault());
            }
            catch (Exception e)
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
            Stand stand = _standRepository.GetById(id.GetValueOrDefault());
            if (stand == null)
            {
                return NotFound();
            }

            return View(stand);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            List<Stand> stands = _standRepository.GetAll();
            return View(stands);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Save(int? id)
        {
            GetAllShops();
            if (id == null)
            {
                return View(new Stand());
            }

            Stand stand = _standRepository.GetById(id.GetValueOrDefault());

            if (stand == null)
            {
                return NotFound();
            }

            return View(stand); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Save(int? id, Stand model)
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
                        _standRepository.Create(model);
                    }
                    else
                    {
                        _standRepository.Edit(model);
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

        [NonAction]
        private void GetAllShops()
        {
            List<Shop> shops = _shopRepository.GetAll();
            ViewBag.Shops = new SelectList(shops, nameof(Shop.Id), nameof(Shop.Contact));
        }
    }
}
