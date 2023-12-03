using Models.Models.Product;

namespace Repositories.IRepositories
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product GetById(int id);
        void Create(Product product);
        void Edit(Product product);
        void Delete(int id);
        List<StoragedProduct> GetProductsFromStorage(int storageId);


    }
}
