using Models.Models;


namespace Repositories.IRepositories
{
    public interface IShopRepository
    {
        List<Shop> GetAll();
        Shop GetById(int id);
        void Create(Shop shop);
        void Edit(Shop shop);
        void Delete(int id);
        List<Shop> GetShopsForStorage(int storageId);
    }
}
