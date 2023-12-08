using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface IEmployeeController
    {
        IActionResult Index(string searchText);
        IActionResult Details(int? id);
        IActionResult Save(int? id);
        IActionResult Save(int? id, Employee model);
        IActionResult Delete(int? id);
    }
}
