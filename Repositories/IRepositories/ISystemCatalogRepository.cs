using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ISystemCatalogRepository
    {
        List<SystemCatalog> GetAll();
        SystemCatalog GetById(int id);
        void Create(SystemCatalog systemCatalog);
        void Edit(SystemCatalog systemCatalog);
        void Delete(int id);
    }
}
