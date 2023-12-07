using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;

namespace Repositories.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly OracleConnection _oracleConnection;
        private readonly IAddressRepository _addressRepository;
        private readonly IStandRepository _standRepository;
        private readonly ICashDeskRepository _cashDeskRepository;
        private const string TABLE = "PRODEJNY";

        public ShopRepository(OracleConnection oracleConnection, IAddressRepository addressRepository, IStandRepository standRepository, ICashDeskRepository cashDeskRepository)
        {
            _oracleConnection = oracleConnection;
            _addressRepository = addressRepository;
            _standRepository = standRepository;
            _cashDeskRepository = cashDeskRepository;

        }

        public List<Shop> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT p.idprodejny IDPRODEJNY, 
                                        p.kontaktnicislo KONTAKTNICISLO, p.plocha PLOCHA
                                        FROM {TABLE} p";

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
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                Shop shop = GetByIdWithOracleCommand(command, id);
                shop.Stands = _standRepository.GetStandsForShop(id);
                shop.CashDesks = _cashDeskRepository.GetCashDesksForShop(id);
                return shop;
            }
        }
        private Shop GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = @$"SELECT p.idprodejny IDPRODEJNY, p.kontaktnicislo KONTAKTNICISLO, 
                                    p.plocha PLOCHA 
                                    FROM {TABLE} p
                                    WHERE IDPRODEJNY = :entityId";

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

                command.Parameters.Clear();

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

                if (entity.AddressId != 0)
                {
                    _addressRepository.SetAddressToShop(entity.Id, entity.AddressId);
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
            Shop shop = new()
            {
                Id = int.Parse(reader["IDPRODEJNY"].ToString()),
                Contact = reader["KONTAKTNICISLO"].ToString(),
                Square = double.Parse(reader["PLOCHA"].ToString()),
            };
            shop.Address = _addressRepository.GetShopAddress(shop.Id);
            return shop;
        }

        public List<Shop> GetShopsForStorage(int storageId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();


                command.CommandText = @$"SELECT p.idprodejny IDPRODEJNY, p.kontaktnicislo KONTAKTNICISLO, p.plocha PLOCHA
                                        FROM {TABLE} p
                                        LEFT JOIN sklady s ON p.idprodejny = s.prodejny_idprodejny
                                        WHERE s.idskladu IS NULL
                                           OR s.idskladu = :storageId";

                command.Parameters.Add(":storageId", OracleDbType.Int32).Value = storageId;

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
    }
}
