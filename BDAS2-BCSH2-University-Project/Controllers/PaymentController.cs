using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.Login;
using Models.Models.Product;
using Repositories.IRepositories;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class PaymentController : Controller, IMainController<Payment>
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
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
                _paymentRepository.Delete(id.GetValueOrDefault(), "");
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Payment payment = _paymentRepository.GetPayment(id.GetValueOrDefault());
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Save(int? id)
        {
            if (id == null)
            {
                return View(new Payment());
            }

            Payment payment = _paymentRepository.GetPayment(id.GetValueOrDefault());
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Save(int? id, Payment model)
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
                        _paymentRepository.Create(model);
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
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Index()
        {
            List<Payment> payments = _paymentRepository.GetAllPayments();
            return View(payments);
        }
    }
}