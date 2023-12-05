using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using Models.Models.Login;
using Models.Models.Product;
using Models.Models.Storage;
using Repositories.IRepositories;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class StorageController : Controller, IStorageController
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IProductRepository _productRepository;

        public StorageController(IStorageRepository storageRepository, IShopRepository shopRepository, IProductRepository productRepository)
        {
            _storageRepository = storageRepository;
            _shopRepository = shopRepository;
            _productRepository = productRepository;
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
            GetAllShops(id.GetValueOrDefault());
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
            GetAllShops(id.GetValueOrDefault());
            return View(model);
        }

        [HttpGet]
        public IActionResult AddProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Order order = new() { StorageId = id.GetValueOrDefault() };
            GetAllProducts();
            return View(order);
        }

        [HttpPost]
        public IActionResult AddProduct(Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _storageRepository.AddProduct(order);
                   return RedirectToAction(nameof(Details), new{  id = order.StorageId });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            GetAllProducts();
            return View(order);
        }

        
        [NonAction]
        private void GetAllShops(int id)
        {
            List<Shop> shops = _shopRepository.GetShopsForStorage(id);
            ViewBag.Shops = new SelectList(shops, nameof(Shop.Id), nameof(Shop.Contact));
        }

        [NonAction]
        private void GetAllProducts()
        {
            List<Product> products = _productRepository.GetAll();
            ViewBag.Products = new SelectList(products, nameof(Product.Id), nameof(Product.Name));
        }
    }
}
