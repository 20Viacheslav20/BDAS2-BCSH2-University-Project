using Models.Models;
using Models.Models.Product;

namespace Repositories.IRepositories
{
    public interface ILogsRepository
    {
        List<Logs> GetAll();
        Logs GetById(int id);
    }
}
