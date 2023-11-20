using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;
using System.IO;

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

                command.CommandText = $"SELECT p.idprodejny IDPRODEJNY, p.kontaktnicislo KONTAKTNICISLO, " +
                    $"p.plocha PLOCHA, " +
                    $"ad.mesto MESTO, ad.ulice ULICE " +
                    $"FROM {TABLE} p " +
                    $"JOIN ADRESY ad ON ad.prodejny_idprodejny = p.idprodejny";

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

                return GetByIdWithOracleCommand(command, id);
            }
        }

        private Shop GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT p.idprodejny IDPRODEJNY, p.kontaktnicislo KONTAKTNICISLO, " +
                                    $"p.plocha PLOCHA, " +
                                    $"ad.mesto MESTO, ad.ulice ULICE " +
                                    $"FROM {TABLE} p " +
                                    $"JOIN ADRESY ad on ad.prodejny_idprodejny = p.idprodejny " +
                                    $"WHERE IDPRODEJNY = :entityId";

            command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                return CreateShopFromReader(reader);
            }
        }

        public void Create(Shop entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO {TABLE} (KONTAKTNICISLO, PLOCHA) VALUES (:entityContact, :entitySquare)";

                command.Parameters.Add("entityContact", OracleDbType.Varchar2).Value = entity.Contact;
                command.Parameters.Add("entitySquare", OracleDbType.Varchar2).Value = entity.Square;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Shop entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Shop dbShop = GetByIdWithOracleCommand(command, entity.Id);

                if (dbShop == null)
                    return;

                string query = "";

                if (dbShop.Contact != entity.Contact)
                {
                    query += "KONTAKTNICISLO = :entityContact, ";
                    command.Parameters.Add("entityContact", OracleDbType.Varchar2).Value = entity.Contact;
                }

                if (dbShop.Square != entity.Square)
                {
                    query += "PLOCHA = :entitySquare, ";
                    command.Parameters.Add("entitySquare", OracleDbType.Int32).Value = entity.Square;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDPRODEJNY = :entityId";
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

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDPRODEJNY = :entityId";
                command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        private Shop CreateShopFromReader(OracleDataReader reader)
        {
            Shop Shop = new()
            {
                Id = int.Parse(reader["IDPRODEJNY"].ToString()),
                Contact = reader["KONTAKTNICISLO"].ToString(),
                Square = double.Parse(reader["PLOCHA"].ToString()),
                Address = new()
                {
                    City = reader["MESTO"].ToString(),
                    Street = reader["ULICE"].ToString(),
                }
            };
            return Shop;
        }
    }
}
