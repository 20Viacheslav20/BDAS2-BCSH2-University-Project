using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using BDAS2_BCSH2_University_Project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class CategoryController : Controller, IMainController<Category>
    {
        private readonly IMainRepository<Category> _categoryRepository;

        public CategoryController(IMainRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category model)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Category> categories = _categoryRepository.GetAll();
            return View(categories);
        }
    }
}