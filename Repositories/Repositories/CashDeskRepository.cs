using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;
using Models.Models.CashDesks;

namespace Repositories.Repositories
{
    public class CashDeskRepository : ICashDeskRepository
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "POKLADNY";

        public CashDeskRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public void Create(CashDesk cashDesk)
        {
            using(OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $@"INSERT INTO {TABLE} (CISLO, JESAMOOBSLUZNA, PRODEJNY_idProdejny)
                                          VALUES(:cashDeskCount, :cashDeskIsSelf, :shopId)";

                command.Parameters.Add(":cashDeskCount", OracleDbType.Int32).Value = cashDesk.Number;
                command.Parameters.Add(":cashDeskIsSelf", OracleDbType.Int16).Value = cashDesk.IsSelf ? 1 : 0;
                command.Parameters.Add(":shopId", OracleDbType.Int32).Value = cashDesk.ShopId;


                command.ExecuteNonQuery();

            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDPOKLADNY = :cashDeskId";
                command.Parameters.Add("cashDeskId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(CashDesk cashDesk)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                CashDesk dbCashDesk = GetByIdWithOracleCommand(command, cashDesk.Id);

                if (dbCashDesk == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbCashDesk.Number != cashDesk.Number)
                {
                    query += "CISLO = :cashDeskCount, ";
                    command.Parameters.Add("cashDeskCount", OracleDbType.Int32).Value = cashDesk.Number;
                          
                }

                if (dbCashDesk.IsSelf != cashDesk.IsSelf)
                {
                    query += "JESAMOOBSLUZNA = :cashDeskIsSelf, ";
                    command.Parameters.Add("cashDeskIsSelf", OracleDbType.Int16).Value = cashDesk.IsSelf ? 1 : 0;

                }

                if (dbCashDesk.Id != cashDesk.Id)
                {
                    query += "PRODEJNY_idProdejny = :shopId, ";
                    command.Parameters.Add(":shopId", OracleDbType.Int32).Value = cashDesk.ShopId;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDPOKLADNY = :cashDeskId";
                    command.Parameters.Add("cashDeskId", OracleDbType.Int32).Value = cashDesk.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<CashDesk> GetAll()
        {
            using(OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"SELECT p.idpokladny IDPOKLADNY, p.cislo CISLO, 
                                    p.jesamoobsluzna JESAMOOBSLUZNA, p.prodejny_idprodejny PRODEJNY_IDPRODEJNY, 
                                    pr.kontaktnicislo KONTAKTNICISLO
                                    FROM {TABLE} p 
                                    JOIN PRODEJNY pr ON pr.idprodejny = p.prodejny_idprodejny";

                List<CashDesk> cashDesks = new List<CashDesk>();

                using(OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cashDesks.Add(CreateCashDeskFromReader(reader));
                    }
                    return cashDesks;
                }
            }
        }

        public CashDesk GetById(int id)
        {
            using(OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        private CashDesk GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = @$"SELECT p.idpokladny IDPOKLADNY, p.cislo CISLO, 
                                    p.jesamoobsluzna JESAMOOBSLUZNA, p.prodejny_idprodejny PRODEJNY_IDPRODEJNY, 
                                    pr.kontaktnicislo KONTAKTNICISLO
                                    FROM {TABLE} p 
                                    JOIN PRODEJNY pr ON pr.idprodejny = p.prodejny_idprodejny 
                                    WHERE IDPOKLADNY = :cashDeskId";

            command.Parameters.Add("cashDeskId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                return CreateCashDeskFromReader(reader);
            }
        }

        public List<ShopCashDesk> GetCashDesksForShop(int shopId)
        {
            using(OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = $@"SELECT * FROM {TABLE} WHERE PRODEJNY_idprodejny = :shopId";

                command.Parameters.Add("shopId", OracleDbType.Int32).Value = shopId;

                List<ShopCashDesk> cashDesks = new List<ShopCashDesk>();

                using(OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cashDesks.Add(CreateShopCashDeskFromReader(reader));
                    }
                    return cashDesks;
                }
            }
        }

        private ShopCashDesk CreateShopCashDeskFromReader(OracleDataReader reader)
        {
            ShopCashDesk cashDesk = new()
            {
                Id = int.Parse(reader["IDPOKLADNY"].ToString()),
                Number = int.Parse(reader["CISLO"].ToString()),
                IsSelf = Convert.ToBoolean(int.Parse(reader["JESAMOOBSLUZNA"].ToString())),
                ShopId = int.Parse(reader["PRODEJNY_IDPRODEJNY"].ToString()),
            };
            return cashDesk;
        }

        private CashDesk CreateCashDeskFromReader(OracleDataReader reader)
        {
            CashDesk cashDesk = new()
            {
                Id = int.Parse(reader["IDPOKLADNY"].ToString()),
                Number = int.Parse(reader["CISLO"].ToString()),
                IsSelf = Convert.ToBoolean(int.Parse(reader["JESAMOOBSLUZNA"].ToString())),
                ShopId = int.Parse(reader["PRODEJNY_IDPRODEJNY"].ToString()),
                Shop = new()
                {
                    Id = int.Parse(reader["PRODEJNY_IDPRODEJNY"].ToString()),
                    Contact = reader["KONTAKTNICISLO"].ToString(),
                }
            };
            return cashDesk;
        }
    }
}
