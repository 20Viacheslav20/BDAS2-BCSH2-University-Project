using Models.Models;

namespace Repositories.IRepositories
{
    public interface ILogsRepository
    {
        List<Logs> GetAll();
    }
}
