using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;

namespace Repositories.Repositories
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
                if (_oracleConnection.State == ConnectionState.Closed)
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

                return GetByIdWithOracleCommand(command, id);
            }
        }

        private Position GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT * FROM {TABLE} WHERE IDPOZICE = :entityId";

            command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }

                Position position = CreatePositionFromReader(reader);
                
                position.EmployeeCount = GetEmployeeCount(position.Id);
                
                return position;
            }
        }

        public void Create(Position entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "AddPosition";

                command.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = entity.Name;
                command.Parameters.Add("p_mzda", OracleDbType.Int32).Value = entity.Salary;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Position entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Position dbPosition = GetByIdWithOracleCommand(command, entity.Id);

                if (dbPosition == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbPosition.Name != entity.Name)
                {
                    query += "NAZEV = :entityName, ";
                    command.Parameters.Add("entityName", OracleDbType.Varchar2).Value = entity.Name;
                }

                if (dbPosition.Salary != dbPosition.Salary)
                {
                    query += "MZDA = :entitySalary, ";
                    command.Parameters.Add("entitySalary", OracleDbType.Int32).Value = entity.Salary;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDPOZICE = :entityId";
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

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDPOZICE = :entityId";

                command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        private Position CreatePositionFromReader(OracleDataReader reader)
        {
            Position Position = new()
            {
                Id = int.Parse(reader["IDPOZICE"].ToString()),
                Name = reader["NAZEV"].ToString(),
                Salary = int.Parse(reader["MZDA"].ToString())
            };
            return Position;
        }

        private int GetEmployeeCount(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = $"SELECT POCET_ZAMESTNANCU_NA_POZICI(:p_idpozice) FROM DUAL";
                command.Parameters.Add("p_idpozice", OracleDbType.Int32).Value = id;

                int employeeCount = Convert.ToInt32(command.ExecuteScalar());

                return employeeCount;
            }
        }

    }
}
