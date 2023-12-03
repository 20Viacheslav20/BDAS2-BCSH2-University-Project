using Microsoft.AspNetCore.Mvc;

namespace BDAS2_BCSH2_University_Project.IControllers
{
    public interface ILogsController
    {
        IActionResult Index();

        IActionResult Details(int? id);
    }
}
