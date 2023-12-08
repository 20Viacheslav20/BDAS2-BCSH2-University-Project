using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface ISystemCatalogController
    {
        IActionResult Index(string searchText);
    }
}
