using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IStandRepository
    {
        List<Stand> GetAll();
        Stand GetById(int id);
        void Create(Stand stand);
        void Edit(Stand stand);
        void Delete(int id);
    }
}
