﻿using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models.Categor;
using Models.Models.Login;
using Models.Models.Product;
using Repositories.IRepositories;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class ProductController : Controller, IProductController
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
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
                _productRepository.Delete(id.GetValueOrDefault());
            } catch (Exception e)
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
            Product product = _productRepository.GetById(id.GetValueOrDefault());
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Save(int? id)
        {
            GetCategories();
            if (id == null)
            {
                return View(new Product());
            }

            Product product = _productRepository.GetById(id.GetValueOrDefault());
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Save(int? id, Product model)
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
                        _productRepository.Create(model);
                    }
                    else
                    {
                        _productRepository.Edit(model);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            GetCategories();
            return View(model);
        }

        [HttpGet]
        public IActionResult Index(string searchText)
        {
            List<Product> products = new List<Product>();
            if (!string.IsNullOrEmpty(searchText))
            {
                products = _productRepository.SearchProduct(searchText);
                return View(products);
            }
            products = _productRepository.GetAll();
            return View(products);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Stats()
        {
            List<ProductStats> productStats = _productRepository.GetProductStats();
            return View(productStats);
        }

        [NonAction]
        private void GetCategories()
        {
            List<Category> categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, nameof(Category.Id), nameof(Category.Name));
        }
    }
}
