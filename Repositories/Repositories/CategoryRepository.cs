using BDAS2_BCSH2_University_Project.Interfaces;
using Models.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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
                if (_oracleConnection.State == ConnectionState.Closed)
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
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        private Category GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT * FROM {TABLE} where IDKATEGORIJE = :entityId";

            command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                return CreateCategoryFromReader(reader);
            }
        }

        public void Create(Category entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO {TABLE} (NAZEV) VALUES (:entityName)";

                command.Parameters.Add("entityName", OracleDbType.Varchar2).Value = entity.Name;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Category entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Category dbCategory = GetByIdWithOracleCommand(command, entity.Id);

                if (dbCategory == null)
                    return;

                command.Parameters.Clear();

                if (dbCategory.Name != entity.Name)
                {
                    command.CommandText = $"UPDATE {TABLE} SET NAZEV = :entityName WHERE IDKATEGORIJE = :entityId";
                    command.Parameters.Add("entityName", OracleDbType.Varchar2).Value = entity.Name;
                    command.Parameters.Add("entityId", OracleDbType.Int32).Value = entity.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDKATEGORIJE = :entityId";
                command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }
        private Category CreateCategoryFromReader(OracleDataReader reader)
        {
            Category category = new()
            {
                Id = int.Parse(reader["IDKATEGORIJE"].ToString()),
                Name = reader["NAZEV"].ToString()
            };
            return category;
        }
    }
}
