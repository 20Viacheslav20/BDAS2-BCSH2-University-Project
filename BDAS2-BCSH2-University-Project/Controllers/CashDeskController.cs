using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Repositories.IRepositories;
using Repositories.Repositories;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class CashDeskController : Controller, IMainController<CashDesk>
    {
        private readonly ICashDeskRepository _cashDeskRepository;

        public CashDeskController(ICashDeskRepository cashDeskrepository)
        {
            _cashDeskRepository = cashDeskrepository;
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

        [HttpGet]
        public IActionResult Index()
        {
            List<CashDesk> cashDesks = _cashDeskRepository.GetAll();
            return View(cashDesks);
        }

        public IActionResult Save(int? id)
        {
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
            return View(model);
        }
    }
}
