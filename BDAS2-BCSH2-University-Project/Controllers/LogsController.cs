using Microsoft.AspNetCore.Mvc;
using BDAS2_BCSH2_University_Project.IControllers;
using Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Models.Models.Login;
using System.Data;
using Models.Models.Product;
using Repositories.Repositories;
using Models.Models.Logs;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class LogsController : Controller, ILogsController
    {
        private readonly ILogsRepository _logRepository;

        public LogsController(ILogsRepository logRepository)
        {
            _logRepository = logRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Logs> logs = _logRepository.GetAll();
            return View(logs);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Logs logs = _logRepository.GetById(id.GetValueOrDefault());
            if (logs == null)
            {
                return NotFound();
            }

            return View(logs);
        }

        [HttpGet]
        public IActionResult DeteleOldLogs(int? dayCount)
        {
            if (dayCount == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (dayCount > 0)
            {
                try
                {
                    // TODO call function from rep _logRepository.DeteleOldLogs(dayCount.GetValueOrDefault())
                }
                catch (Exception e)
                {
                    TempData["Error"] = e.Message;
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
