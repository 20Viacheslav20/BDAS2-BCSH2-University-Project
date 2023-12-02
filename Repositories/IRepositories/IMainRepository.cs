namespace Repositories.IRepositories
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
