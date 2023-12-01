using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class LogRepository : IMainRepository<Log>
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "LOGY";

        public LogRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public void Create(Log entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO {TABLE} (TABULKA, OPERACE, CAS, UZIVATEL)" +
                                      $"VALUES(:entityTable, :entityOperation, entityTime, entityUser)";
                command.Parameters.Add("entityTable", OracleDbType.Varchar2).Value = entity.Table;
                command.Parameters.Add("entityOperation", OracleDbType.Varchar2).Value = entity.Operation;
                command.Parameters.Add("entityTime", OracleDbType.Date).Value = entity.Time;
                command.Parameters.Add("entityUser", OracleDbType.Varchar2).Value = entity.User;

                command.ExecuteNonQuery();

            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDLOGU = :entityId";

                command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Log entity)
        {
           using(OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Log dbLog = GetByIdWithOracleCommand(command, entity.Id); 

                if (dbLog == null)
                {
                    return;
                }
                command.Parameters.Clear();

                string query = "";
                if (dbLog.Table != entity.Table)
                {
                    query += "TABULKA = :entityTable";
                    command.Parameters.Add("entityTable", OracleDbType.Varchar2).Value = entity.Table;
                }

                if (dbLog.Operation != entity.Operation)
                {
                    query += "OPERACE = :entityOperation";
                    command.Parameters.Add("entityOperation", OracleDbType.Varchar2).Value = entity.Operation;
                }

                if (dbLog.Time != entity.Time)
                {
                    query += "CAS = :entityTime";
                    command.Parameters.Add("entityTime", OracleDbType.Date).Value = entity.Time;
                }

                if (dbLog.User != entity.User)
                {
                    query += "UZIVATEL = :entityUser";
                    command.Parameters.Add("entityUser", OracleDbType.Varchar2).Value = entity.User;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDLOGU = :entityId";
                    command.Parameters.Add("entityId", OracleDbType.Int32).Value = entity.Id;

                    command.ExecuteNonQuery();
                }


            }
        }

        public List<Log> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE}";

                List<Log> logs = new List<Log>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logs.Add(CreateLogFromReader(reader));
                    }
                    return logs;
                }
            }
        }

        public Log GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        private Log GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT * FROM {TABLE} WHERE IDLOGU = :entityId";

            command.Parameters.Add("entityId", OracleDbType.Int32 ).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                return CreateLogFromReader(reader);
            }
        }
        private Log CreateLogFromReader(OracleDataReader reader)
        {
            Log log = new()
            {
                Id = int.Parse(reader["IDLOGU"].ToString()),
                Table = reader["TABULKA"].ToString(),
                Operation = reader["OPERACE"].ToString(),
                // Time = reader["CAS"].ToString(), TODO
                User = reader["UZIVATEL"].ToString()
            };
            return log;
        }
    }
}