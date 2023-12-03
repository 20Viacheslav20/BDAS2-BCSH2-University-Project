using Models.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Models.Models.Login;
using System.Data;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class CategoryController : Controller, IMainController<Category>
    {
        private readonly IMainRepository<Category> _categoryRepository;

        public CategoryController(IMainRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
                _categoryRepository.Delete(id.GetValueOrDefault());
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
            Category category = _categoryRepository.GetById(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Save(int? id)
        {
            if (id == null)
            {
                return View(new Category());
            }

            Category category = _categoryRepository.GetById(id.GetValueOrDefault());

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Save(int? id, Category model)
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
                        _categoryRepository.Create(model);
                    }
                    else
                    {
                        _categoryRepository.Edit(model);
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
            List<Category> categories = _categoryRepository.GetAll();
            return View(categories);
        }
    }
}