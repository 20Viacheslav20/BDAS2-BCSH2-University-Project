using Models.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category GetById(int id);
        void Create(Category category);
        void Edit(Category category);
        void Delete(int id);

        void IncreasePrice(IncreasePrice increasePrice);
    }
}
