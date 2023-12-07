

using Models.Models;

namespace Repositories.IRepositories
{
    public interface ISaleRepository
    {
        List<Sale> GetAll();
        Sale GetById(int id);
        void Create(Sale sale);
        void Edit(Sale sale);
        void Delete(int id);

        List<Sale> GetNotUsedSales();
    }
}
