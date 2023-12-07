using Models.Models.CashDesks;

namespace Repositories.IRepositories
{
    public interface ICashDeskRepository
    {
        List<CashDesk> GetAll();
        CashDesk GetById(int id);
        void Create(CashDesk cashDesk);
        void Edit(CashDesk cashDesk);
        void Delete(int id);
        List<ShopCashDesk> GetCashDesksForShop(int shopId);
    }
}
