using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models.Login;
using Models.Models.Product;
using Models.Models.Storage;
using Repositories.IRepositories;
using Repositories.Repositories;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class SoldProductController : Controller, ISoldProductController
    {
        private readonly ISoldProductRepository _soldProductRepository;

        public SoldProductController(ISoldProductRepository soldProductRepository)
        {
            _soldProductRepository = soldProductRepository;
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Create(int? id)
        {
            return View(new SoldProduct());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Create(int? id, SoldProduct model)
        {
            throw new NotImplementedException();
        }
        

        [HttpGet]
        public IActionResult Index()
        {
            List<SoldProduct> soldProducts = _soldProductRepository.GetAll();
            return View(soldProducts);
        }
    }
}
