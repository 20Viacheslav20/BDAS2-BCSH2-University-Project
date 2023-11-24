using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using BDAS2_BCSH2_University_Project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class EmployerController : Controller, IMainController<Employer>
    {
        private readonly IMainController<Employer> _employerRepository;

        public EmployerController(IMainController<Employer> employerRepository)
        {
            _employerRepository = employerRepository;
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
                _employerRepository.Delete(id.GetValueOrDefault());
            }
            catch(Exception e)
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
            Employer employer = _employerRepository.GetById(id.GetValueOrDefault());
            if (employer == null)
            {
                return NotFound();
            }
            return View(employer);

        }
        [HttpGet]
        public IActionResult Save(int? id)
        {
            if (id == null)
            {
                return View(new Employer());
            }

            Employer employer = _employerRepository.GetById(id.GetValueOrDefault());

            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(int? id, Employer model)
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
                        _employerRepository.Create(model);
                    }
                    else
                    {
                        _employerRepository.Edit(model);
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
        public IActionResult Index()
        {
            List<Employer> employers = _employerRepository.GetAll();
            return View(employers);
        }

    }
}
