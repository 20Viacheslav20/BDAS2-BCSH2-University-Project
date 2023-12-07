using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using Models.Models.Login;
using Models.Models.Product;
using Repositories.IRepositories;


namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class SoldProductController : Controller, ISoldProductController
    {
        private readonly ISoldProductRepository _soldProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISaleRepository _saleRepository;

        public SoldProductController(ISoldProductRepository soldProductRepository, IProductRepository productRepository, ISaleRepository saleRepository)
        {
            _soldProductRepository = soldProductRepository;
            _productRepository = productRepository;
            _saleRepository = saleRepository;
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Create(int? id)
        {
            GetData();
            return View(new SoldProduct());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Create(int? id, SoldProduct model)
        {
            if (id != null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null)
                    {
                        _soldProductRepository.Create(model);
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            GetData();
            return View(model);
        }
        

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            List<SoldProduct> soldProducts = _soldProductRepository.GetAll();
            return View(soldProducts);
        }

        [NonAction]
        private void GetData()
        {
            GetSales();
            GetAllProducts();
        }

        [NonAction]
        private void GetSales()
        {
            List<Sale> sales = _saleRepository.GetNotUsedSales();
            ViewBag.Sales = new SelectList(sales, nameof(Sale.Id), nameof(Sale.Displayinfo));
        }

        [NonAction]
        private void GetAllProducts()
        {
            List<Product> products = _productRepository.GetAll();
            ViewBag.Products = new SelectList(products, nameof(Product.Id), nameof(Product.Name));
        }
    }
}
