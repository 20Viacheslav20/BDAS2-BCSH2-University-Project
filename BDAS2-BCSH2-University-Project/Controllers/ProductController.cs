using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class ProductController : Controller, IMainController<Product>
    {
        private readonly IMainRepository<Product> _productRepository;

        public ProductController(IMainRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
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
            if (id == null)
            {
                return NotFound();
            }

            Product product = _productRepository.GetById(id.GetValueOrDefault());

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _productRepository.Edit(model);
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<Product> products = _productRepository.GetAll();
            return View(products);
        }

    }
}
