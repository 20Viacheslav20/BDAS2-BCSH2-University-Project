using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class ProductRepository : IMainRepository<Product>
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "ZBOZI";

        public ProductRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Product> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE}";

                List<Product> products = new List<Product>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(CreateProductFromReader(reader));
                    }
                    return products;
                }
            }
        }

        public Product GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        private Product GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT * FROM {TABLE} WHERE IDZBOZI = {id}";

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return CreateProductFromReader(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public void Create(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Product entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                
                Product dbproduct = GetByIdWithOracleCommand(command, entity.Id);
                if (dbproduct != null)
                {
                    string query = "";
                    if (dbproduct.Name != entity.Name)
                    {
                        query += "NAZEV = :entityName, ";
                        command.Parameters.Add("entityName", OracleDbType.Varchar2).Value = entity.Name;
                    }

                    if (dbproduct.ActualPrice != entity.ActualPrice)
                    {
                        query += "AKTUALNICENA = :entityActualPrice, ";
                        command.Parameters.Add("entityActualPrice", OracleDbType.Int32).Value = entity.ActualPrice;
                    }

                    if (dbproduct.ClubCardPrice != entity.ClubCardPrice)
                    {
                        query += "CENAZECLUBCARTOU = :entityClubCardPrice ";
                        command.Parameters.Add("entityClubCardPrice", OracleDbType.Int32).Value = entity.ClubCardPrice;
                    }

                    // TODO Weight

                    if (!string.IsNullOrEmpty(query))
                    {
                        query = query.TrimEnd(',', ' ');

                        command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDZBOZI = {entity.Id}";

                        command.ExecuteNonQuery();
                    }

                }

            }

        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        private Product CreateProductFromReader(OracleDataReader reader)
        {
            string price = reader["CENAZECLUBCARTOU"].ToString();
            //var a = reader.GetValue(1);
            Product product = new Product()
            {
                Id = int.Parse(reader["IDZBOZI"].ToString()),
                Name = reader["NAZEV"].ToString(),
                ActualPrice = int.Parse(reader["AKTUALNICENA"].ToString()),
                ClubCardPrice = !string.IsNullOrEmpty(price) ? int.Parse(price) : null,
                CaregoryId = int.Parse(reader["KATEGORIJE_IDKATEGORIJE"].ToString()),
                Weight = decimal.Parse(reader["HMOTNOST"].ToString())
            };
            return product;
        }
    }
}
