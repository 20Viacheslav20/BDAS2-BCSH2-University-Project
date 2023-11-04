using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class ProductRepository : IMainRepository<Product>
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "ZBOZI";

        // SELECT z.nazev, k.nazev FROM ZBOZI z JOIN kategorije k on (z.kategorije_idkategorije = k.idkategorije);

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

            }
             throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        private Product CreateProductFromReader(OracleDataReader reader)
        {
            string price = reader["CENAZECLUBCARTOU"].ToString();

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
