using Models.Models;

namespace Repositories.IRepositories
{
    public interface ILogsRepository
    {
        List<Logs> GetAll();
        Logs GetById(int id);
        
        void DeleteOldLogs(int dayCount);
    }
}
