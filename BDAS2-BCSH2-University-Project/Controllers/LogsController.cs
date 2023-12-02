using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Repositories.Repositories;
using BDAS2_BCSH2_University_Project.IControllers;
using Repositories.IRepositories;

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
        public IActionResult Index()
        {
            List<Logs> logs = _logRepository.GetAll();
            return View(logs);
        }

        
    }
}
