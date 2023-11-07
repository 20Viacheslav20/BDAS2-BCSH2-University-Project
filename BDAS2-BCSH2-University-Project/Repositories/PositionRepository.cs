using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class PositionRepository : IMainRepository<Position>
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "POZICE";

        public PositionRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Position> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE}";

                List<Position> positions = new List<Position>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        positions.Add(CreatePositionFromReader(reader));
                    }
                    return positions;
                }

            }
        }

        public Position GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE} WHERE IDPOZICE = {id}";

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return CreatePositionFromReader(reader);
                    }
                    else
                    {
                        return null;
                    }
                }

            }
        }

        public void Create(Position entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Position entity)
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

        private Position CreatePositionFromReader(OracleDataReader reader)
        {
            Position Position = new Position()
            {
                Id = int.Parse(reader["IDPOZICE"].ToString()),
                Name = reader["NAZEV"].ToString(),
                Salary = double.Parse(reader["MZDA"].ToString())
            };
            return Position;
        }
    }
}
