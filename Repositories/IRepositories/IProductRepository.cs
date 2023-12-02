using Models.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
