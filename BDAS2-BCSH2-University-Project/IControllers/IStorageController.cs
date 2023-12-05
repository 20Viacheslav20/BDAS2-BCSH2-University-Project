using Microsoft.AspNetCore.Mvc;
using Models.Models.Storage;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface IStorageController
    {
        IActionResult Index();
        IActionResult Details(int? id);
        IActionResult Save(int? id);
        IActionResult Save(int? id, Storage model);
        IActionResult Delete(int? id);
        IActionResult AddProduct(int? id);
        IActionResult AddProduct(Order order);
    }
}
