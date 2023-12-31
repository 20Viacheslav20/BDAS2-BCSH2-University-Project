﻿using BDAS2_BCSH2_University_Project.IControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.Login;
using Repositories.IRepositories;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class SystemCatalogController : Controller, ISystemCatalogController
    {
        private readonly ISystemCatalogRepository _systemCatalogRepository;

        public SystemCatalogController(ISystemCatalogRepository systemCatalogRepository)
        {
            _systemCatalogRepository = systemCatalogRepository;
        }

        [HttpGet]
        public IActionResult Index(string searchText)
        {
            List<SystemCatalog> systemCatalogs = new List<SystemCatalog>();
            if (!string.IsNullOrEmpty(searchText))
            {
                systemCatalogs = _systemCatalogRepository.SearchSystemCatalog(searchText);
                return View(systemCatalogs);
            }
            systemCatalogs = _systemCatalogRepository.GetAll();
            return View(systemCatalogs);
        }
    }
}
