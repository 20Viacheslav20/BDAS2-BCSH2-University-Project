using Models.Models;

namespace Repositories.IRepositories
{
    public interface IStorageRepository
    {
        List<Storage> GetAll();
        Storage GetById(int id);
        void Create(Storage entity);
        void Edit(Storage entity);
        void Delete(int id);
    }
}
