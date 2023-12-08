using Models.Models;

namespace Repositories.IRepositories
{
    public interface ISystemCatalogRepository
    {
        List<SystemCatalog> GetAll();

        List<SystemCatalog> SearchSystemCatalog(string search);
    }
}
