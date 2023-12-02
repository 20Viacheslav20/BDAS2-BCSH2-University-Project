using BDAS2_BCSH2_University_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_BCSH2_University_Project.Repositories.IRepositories
{
    public interface IStorageRepository
    {
        List<Storage> GetAll();
        Storage GetById(int id);
        void Create(Storage entity);
        void Edit(Storage entity);
        void Delete(int id);
    }
}
