using Microsoft.AspNetCore.Mvc;

namespace BDAS2_BCSH2_University_Project.Interfaces
{
    public interface IMainController<T>
    {
        IActionResult Index();
        IActionResult Details(int? id);
        IActionResult Edit(int? id);
        IActionResult Edit(int id, T model);
        IActionResult Create();
        IActionResult Create(T model);

        IActionResult Delete(int id);

    }
}
