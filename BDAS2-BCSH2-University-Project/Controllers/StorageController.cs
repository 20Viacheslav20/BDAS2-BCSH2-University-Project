using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.Login;
using Repositories.IRepositories;
using System.Data;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class StorageController : Controller, IMainController<Storage>
    {
        private readonly IStorageRepository _storageRepository;

        public StorageController(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
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
                _storageRepository.Delete(id.GetValueOrDefault());
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
            Storage storage = _storageRepository.GetById(id.GetValueOrDefault());
            if (storage == null)
            {
                return NotFound();
            }

            return View(storage);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            List<Storage> storages = _storageRepository.GetAll();
            return View(storages);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Save(int? id)
        {
            if (id == null)
            {
                return View(new Storage());
            }

            Storage storage = _storageRepository.GetById(id.GetValueOrDefault());

            if (storage == null)
            {
                return NotFound();
            }

            return View(storage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Save(int? id, Storage model)
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
                        _storageRepository.Create(model);
                    }
                    else
                    {
                        _storageRepository.Edit(model);
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
