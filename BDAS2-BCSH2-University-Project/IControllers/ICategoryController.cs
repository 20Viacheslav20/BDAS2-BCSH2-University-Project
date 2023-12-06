using Microsoft.AspNetCore.Mvc;
using Models.Models.Categor;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface ICategoryController
    {
        IActionResult Index();
        IActionResult Details(int? id);
        IActionResult Save(int? id);
        IActionResult Save(int? id, Category model);
        IActionResult Delete(int? id);
        IActionResult IncreasePrice(int? id);
        IActionResult IncreasePrice(IncreasePrice increasePrice);
    }
}
