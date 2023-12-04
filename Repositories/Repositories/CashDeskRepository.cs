using Models.Models;
using Models.Models.CashDesk;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                command.CommandText = $@"INSERT INTO {TABLE} (CISLO, JESAMOOBSLUZNA)
                                          VALUES(:cashDeskCount, :cashDeskIsSelf)";

                command.Parameters.Add("cashDeskCount", OracleDbType.Int32).Value = cashDesk.Count;
                //TODO
                command.Parameters.Add("cashDeskIsSelf", OracleDbType.Int32).Value = 1/*cashDesk.isSelf*/;

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
                if (dbCashDesk.Count != cashDesk.Count)
                {
                    query += "CISLO = :cashDeskCount, ";
                    command.Parameters.Add("cashDeskCount", OracleDbType.Int32).Value = cashDesk.Count;
                          
                }

                //if (dbCashDesk.isSelf != cashDesk.isSelf)
                //{
                //    query += "JESAMOOBSLUZNA = :cashDeskIsSelf, ";
                //    command.Parameters.Add("cashDeskIsSelf", OracleDbType.Boolean).Value = cashDesk.isSelf;

                  
                //}
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

                command.CommandText = @$"SELECT * FROM {TABLE}";

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

        private CashDesk CreateCashDeskFromReader(OracleDataReader reader)
        {
            CashDesk cashDesk = new()
            {
                Id = int.Parse(reader["IDPOKLADNY"].ToString()),
                Count = int.Parse(reader["CISLO"].ToString()),
                isSelf = Convert.ToBoolean(int.Parse(reader["JESAMOOBSLUZNA"].ToString()))

            };
            return cashDesk;
        }

        public CashDesk GetById(int id)
        {
            using(OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

               return GetByIdWithOracleCommand(command, id);

            }
        }

        private CashDesk GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT * FROM {TABLE} where IDPOKLADNY = :cashDeskId";

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
    }
}
