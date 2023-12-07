using Models.Models;
using Models.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ISoldProductRepository
    {
        List<SoldProduct> GetAll();
        void Create(SoldProduct soldProduct);

    }
}
