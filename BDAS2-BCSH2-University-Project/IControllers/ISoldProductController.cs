using Microsoft.AspNetCore.Mvc;
using Models.Models.Product;


namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface ISoldProductController
    {
        IActionResult Index();
        IActionResult Create(int? id);

        IActionResult Create(int? id, SoldProduct model);

    }
}
