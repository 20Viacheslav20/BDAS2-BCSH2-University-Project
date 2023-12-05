using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;

namespace Repositories.Repositories
{
    public class LogRepository : ILogsRepository
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "LOGY";

        public LogRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Logs> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE} ORDER BY IDLOGU DESC";

                List<Logs> logs = new List<Logs>();

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

        public Logs GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"SELECT * FROM {TABLE} WHERE IDLOGU = : logId";

                command.Parameters.Add("logId", OracleDbType.Int32).Value = id;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    return CreateLogFromReader(reader);
                }             

            }
        }

        private Logs CreateLogFromReader(OracleDataReader reader)
        {
            Logs log = new()
            {
                Id = int.Parse(reader["IDLOGU"].ToString()),
                Table = reader["TABULKA"].ToString(),
                Operation = reader["OPERACE"].ToString(),
                Time = DateTime.Parse(reader["CAS"].ToString()), 
                User = reader["UZIVATEL"].ToString(),
                Changes = reader["ZMENY"].ToString()
            };
            return log;
        }
    }
}
