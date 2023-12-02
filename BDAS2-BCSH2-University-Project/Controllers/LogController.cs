using BDAS2_BCSH2_University_Project.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BDAS2_BCSH2_University_Project.Models;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class LogController : Controller, IMainController<Log>
    {
        private readonly IMainRepository<Log> _logRepository;

        public LogController(IMainRepository<Log> logRepository)
        {
            _logRepository = logRepository;
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
                _logRepository.Delete(id.GetValueOrDefault());
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
            Log log = _logRepository.GetById(id.GetValueOrDefault());
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Log> logs = _logRepository.GetAll();
            return View(logs);
        }

        public IActionResult Save(int? id)
        {
            if (id == null)
            {
                return View(new Log());
            }

            Log log = _logRepository.GetById(id.GetValueOrDefault());

            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        public IActionResult Save(int? id, Log model)
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
                        _logRepository.Create(model);
                    }
                    else
                    {
                        _logRepository.Edit(model);
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
