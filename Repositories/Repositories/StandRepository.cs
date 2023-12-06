using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;


namespace Repositories.Repositories
{
    public class StandRepository : IStandRepository
    {
        private readonly OracleConnection _oracleConnection;

        private readonly IProductRepository _productRepository;

        private const string TABLE = "PULTY";

        public StandRepository(OracleConnection oracleConnection, IProductRepository productRepository)
        {
            _oracleConnection = oracleConnection;
            _productRepository = productRepository;
        }

        public void Create(Stand stand)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $@"INSERT INTO {TABLE} (CISLO, POCETPOLICEK)
                                          VALUES(:standNumber, :standShelves)";

                command.Parameters.Add("standNumber", OracleDbType.Int32).Value = stand.Number;
                command.Parameters.Add("standShelves", OracleDbType.Int32).Value = stand.CountOfShelves;

                command.ExecuteNonQuery();

            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDPULTU = :standId";
                command.Parameters.Add("standId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Stand stand)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                Stand dbStand = GetByIdWithOracleCommand(command, stand.Id);

                if (dbStand == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbStand.Number != stand.Number)
                {
                    query += "CISLO = :standNumber, ";
                    command.Parameters.Add("standNumber", OracleDbType.Int32).Value = stand.Number;

                }

                if (dbStand.CountOfShelves != stand.CountOfShelves)
                {
                    query += "POCETPOLICEK = :standShelves, ";
                    command.Parameters.Add("standShelves", OracleDbType.Int32).Value = stand.CountOfShelves;

                    
                }
                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDPULTU = :standId";
                    command.Parameters.Add("standId", OracleDbType.Int32).Value = stand.Id;

                    command.ExecuteNonQuery();
                }
            }
        }


        public List<Stand> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"SELECT * FROM {TABLE}";

                List<Stand> stands = new List<Stand>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stands.Add(CreateStandFromReader(reader));
                    }
                    return stands;
                }
            }
        }

        public Stand GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);

            }
        }
        private Stand GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT * FROM {TABLE} where IDPULTU = :standId";

            command.Parameters.Add("standId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                Stand stand = CreateStandFromReader(reader);

                stand.Products = _productRepository.GetProductOnStands(id);
                return stand ;
            }
        }

        private Stand CreateStandFromReader(OracleDataReader reader)
        {
            Stand stand = new()
            {
                Id = int.Parse(reader["IDPULTU"].ToString()),
                Number = int.Parse(reader["CISLO"].ToString()),
                CountOfShelves = int.Parse(reader["POCETPOLICEK"].ToString())
            };
            return stand;
        }

    }
}
