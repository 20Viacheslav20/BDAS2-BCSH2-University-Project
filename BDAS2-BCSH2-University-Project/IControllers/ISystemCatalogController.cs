using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface ISystemCatalogController
    {
        IActionResult Index();
        IActionResult Details(int? id);
        IActionResult Save(int? id);
        IActionResult Save(int? id, SystemCatalog model);
        IActionResult Delete(int? id);
    }
}
