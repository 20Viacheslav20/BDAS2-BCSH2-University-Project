using Microsoft.AspNetCore.Mvc;
using Models.Models.Product;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface IProductController
    {
        //IActionResult Index();
        IActionResult Details(int? id);
        IActionResult Save(int? id);
        IActionResult Save(int? id, Product model);
        IActionResult Delete(int? id);
        IActionResult Stats();
        IActionResult Index(string searchText);
    }
}
