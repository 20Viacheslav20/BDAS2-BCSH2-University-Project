using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;


namespace Repositories.Repositories
{
    public class SystemCatalogRepository : ISystemCatalogRepository
    {
        private readonly OracleConnection _oracleConnection;
        private const string TABLE = "ALL_OBJECTS";  

        public SystemCatalogRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<SystemCatalog> GetAll()
        {
            using(OracleCommand command =_oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $@"SELECT * FROM {TABLE} WHERE OWNER = 'ST67020'";

                List<SystemCatalog> systemCatalogs = new List<SystemCatalog>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        systemCatalogs.Add(CreateSystemCatalogFromReader(reader));
                    }
                    return systemCatalogs;
                }
            }
        }

        public List<SystemCatalog> SearchSystemCatalog(string search)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $@"SELECT OBJECT_ID, OBJECT_NAME, OWNER, OBJECT_TYPE
                                        FROM ALL_OBJECTS
                                        WHERE OWNER = 'ST67020' 
                                        AND (LOWER(OBJECT_NAME) LIKE LOWER('{search}%') 
                                        OR LOWER(OBJECT_TYPE) LIKE LOWER('{search}%'))";

                List<SystemCatalog> systemCatalogs = new List<SystemCatalog>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        systemCatalogs.Add(CreateSystemCatalogFromReader(reader));
                    }
                    return systemCatalogs;
                }
            }
        }

        private SystemCatalog CreateSystemCatalogFromReader(OracleDataReader reader)
        {
            SystemCatalog systemCatalog = new()
            {
                Id = int.Parse(reader["OBJECT_ID"].ToString()),
                Name = reader["OBJECT_NAME"].ToString(),
                Owner = reader["OWNER"].ToString(),
                Type = reader["OBJECT_TYPE"].ToString(),
            };
            return systemCatalog;
        }


    }
}
