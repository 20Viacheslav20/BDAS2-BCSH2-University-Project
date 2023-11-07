using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class ShopRepository : IMainRepository<Shop>
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "PRODEJNY";

        public ShopRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Shop> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE}";

                List<Shop> shops = new List<Shop>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        shops.Add(CreateShopFromReader(reader));
                    }
                    return shops;
                }

            }
        }

        public Shop GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE} WHERE IDPRODEJNY = {id}";

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return CreateShopFromReader(reader);
                    }
                    else
                    {
                        return null;
                    }
                }

            }
        }

        public void Create(Shop entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Shop entity)
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

        private Shop CreateShopFromReader(OracleDataReader reader)
        {
            Shop Shop = new Shop()
            {
                Id = int.Parse(reader["IDPRODEJNY"].ToString()),
                Contact = reader["KONTAKTNICISLO"].ToString(),
                Square = double.Parse(reader["PLOCHA"].ToString())
            };
            return Shop;
        }
    }
}
