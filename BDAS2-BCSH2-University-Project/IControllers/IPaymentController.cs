using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface IPaymentController
    {
        IActionResult Index();
        IActionResult Details(int? id);
        IActionResult AddCashPayment();
        IActionResult AddCashPayment(int? id, Cash cashPayment);

        IActionResult AddCouponPayment();
        IActionResult AddCouponPayment(int? id, Coupon couponPayment);

        IActionResult AddCreditCardPayment();
        IActionResult AddCreditCardPayment(int? id, CreditCard creditCardPayment);

        IActionResult Delete(int? id, Payment payment);
    }
}
