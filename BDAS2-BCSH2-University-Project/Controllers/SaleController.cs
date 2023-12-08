using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using Models.Models.Login;
using Repositories.IRepositories;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class SaleController : Controller, IMainController<Sale>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IPaymentRepository _paymentRepository;

        public SaleController(ISaleRepository saleRepository, IPaymentRepository paymentRepository)
        {
            _saleRepository = saleRepository;
            _paymentRepository = paymentRepository;
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
                _saleRepository.Delete(id.GetValueOrDefault());
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = _saleRepository.GetById(id.GetValueOrDefault());
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Save(int? id)
        {
            GetPayments(id.GetValueOrDefault());
            if (id == null)
            {
                return View(new Sale());
            }

            Sale sale = _saleRepository.GetById(id.GetValueOrDefault());
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Save(int? id, Sale model)
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
                        _saleRepository.Create(model);
                    }
                    else
                    {
                        _saleRepository.Edit(model);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            GetPayments(id.GetValueOrDefault());
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.ShiftLeader))]
        public IActionResult Index()
        {
            List<Sale> sales = _saleRepository.GetAll();
            return View(sales);
        }

        [NonAction]
        private void GetPayments(int saleId)
        {
            List<Payment> payments = _paymentRepository.GetPaymentsForSales(saleId);
            ViewBag.Payments = new SelectList(payments, nameof(Payment.Id), nameof(Payment.DisplayInfo));
        }
    }
}
