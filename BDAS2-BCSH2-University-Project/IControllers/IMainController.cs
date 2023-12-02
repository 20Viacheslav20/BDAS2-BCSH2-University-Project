using Microsoft.AspNetCore.Mvc;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface IMainController<T>
    {
        IActionResult Index();
        IActionResult Details(int? id);
        IActionResult Save(int? id);
        IActionResult Save(int? id, T model);
        IActionResult Delete(int? id);

    }
}
