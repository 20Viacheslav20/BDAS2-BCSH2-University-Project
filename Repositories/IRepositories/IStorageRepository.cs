using Models.Models;

namespace Repositories.IRepositories
{
    public interface IStorageRepository
    {
        List<Storage> GetAll();
        Storage GetById(int id);
        void Create(Storage storage);
        void Edit(Storage storage);
        void Delete(int id);
    }
}
