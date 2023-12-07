using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ICashDeskRepository
    {
        List<CashDesk> GetAll();
        CashDesk GetById(int id);
        void Create(CashDesk cashDesk);
        void Edit(CashDesk cashDesk);
        void Delete(int id);
        List<CashDesk> GetCashDesksForShop(int shopId);
    }
}
