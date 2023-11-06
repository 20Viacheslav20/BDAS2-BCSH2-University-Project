using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using BDAS2_BCSH2_University_Project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class ShopController : Controller, IMainController<Shop>
    {
        private readonly IMainRepository<Shop> _ShopRepository;

        public ShopController(IMainRepository<Shop> ShopRepository)
        {
            _ShopRepository = ShopRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Shop model)
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
        public IActionResult Edit(int id, Shop model)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Shop> shops = _ShopRepository.GetAll();
            return View(shops);
        }
    }
}