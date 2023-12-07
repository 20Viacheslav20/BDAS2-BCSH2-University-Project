using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Repositories.IRepositories;

namespace BDAS2_BCSH2_University_Project.Controllers
{
    public class SystemCatalogController : Controller, ISystemCatalogRepository
    {
        public void Create(SystemCatalog systemCatalog)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(SystemCatalog systemCatalog)
        {
            throw new NotImplementedException();
        }

        public List<SystemCatalog> GetAll()
        {
            throw new NotImplementedException();
        }

        public SystemCatalog GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
