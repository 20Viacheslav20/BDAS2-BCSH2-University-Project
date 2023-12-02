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

                command.CommandText = $"SELECT * FROM {TABLE}";

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
        
        private Logs CreateLogFromReader(OracleDataReader reader)
        {
            Logs log = new()
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
