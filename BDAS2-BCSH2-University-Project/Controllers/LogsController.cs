using Microsoft.AspNetCore.Mvc;
using Models.Models;
using BDAS2_BCSH2_University_Project.IControllers;
using Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Models.Models.Login;
using System.Data;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class LogsController : Controller, ILogsController
    {
        private readonly ILogsRepository _logRepository;

        public LogsController(ILogsRepository logRepository)
        {
            _logRepository = logRepository;
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Index()
        {
            List<Logs> logs = _logRepository.GetAll();
            return View(logs);
        }

        
    }
}
