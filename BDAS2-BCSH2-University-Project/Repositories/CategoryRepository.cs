using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class CategoryRepository : IMainRepository<Category>
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "KATEGORIJE";

        public CategoryRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Category> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE}";

                List<Category> categories = new List<Category>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(CreateCategoryFromReader(reader));
                    }
                    return categories;
                }

            }
        }


        public Category GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Create(Category entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Category entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        private Category CreateCategoryFromReader(OracleDataReader reader)
        {
            Category category = new Category()
            {
                Id = int.Parse(reader["IDKATEGORIJE"].ToString()),
                Name = reader["NAZEV"].ToString()
            };
            return category;
        }
    }
}
