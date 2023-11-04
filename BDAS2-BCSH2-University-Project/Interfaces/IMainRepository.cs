
namespace BDAS2_BCSH2_University_Project.Interfaces
{
    public interface IMainRepository<T>
    {
        List<T> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Edit(T entity);
        void Delete(int id);
    }
}
