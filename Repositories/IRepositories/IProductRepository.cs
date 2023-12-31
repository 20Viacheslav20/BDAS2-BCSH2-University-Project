﻿using Models.Models.Product;

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
        List<StoragedProduct> GetProductOnStands(int standId);
        List<ProductStats> GetProductStats();
        List<Product> SearchProduct(string search);
    }
}
