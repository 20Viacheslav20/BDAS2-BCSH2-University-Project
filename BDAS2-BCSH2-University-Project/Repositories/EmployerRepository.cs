using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class EmployerRepository : IMainRepository<Employer>
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "ZAMESTNANEC";

        public EmployerRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Employer> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE}";

                List<Employer> employers = new List<Employer>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employers.Add(CreateEmployerFromReader(reader));
                    }
                    return employers;
                }
            }
        }

        public Employer GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        public Employer GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT * FROM {TABLE} WHERE IDZAMESTNANCE = :entityId";

            command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                return CreateEmployerFromReader(reader);
            }
        }

        public void Create(Employer entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO{TABLE}(JMENO,PRIJMENI,RODNECISLO,TELEFONNICISLO)" +
                    "VALUES (:entityName, :entitySurname, :entityBornNumber, :entityPhoneNumber)";

                command.Parameters.Add("entityName" , OracleDbType.Varchar2 ).Value = entity.Name;
                command.Parameters.Add("entitySurname", OracleDbType.Varchar2).Value = entity.Surname;
                command.Parameters.Add("entityBornNumber", OracleDbType.Int32).Value=entity.BornNumber;
                command.Parameters.Add("entityPnoneNumber", OracleDbType.Int32).Value=entity.PhoneNumber;

                command.ExecuteNonQuery();
            }

        }

        public void Edit(Employer entity)
        {
            using(OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Employer dbEmployer = GetByIdWithOracleCommand(command,entity.Id);

                if (dbEmployer == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbEmployer.Name != entity.Name)
                {
                    query += "JMENO = :entityName, ";
                    command.Parameters.Add("entityName", OracleDbType.Varchar2).Value = entity.Name;
                }
                if (dbEmployer.Surname != entity.Surname)
                {
                    query += "PRIJMENI = :entitySurname, ";
                    command.Parameters.Add("entitySurname", OracleDbType.Varchar2).Value = entity.Surname;
                }

                if (dbEmployer.BornNumber != entity.BornNumber)
                {
                    query += "RODNECISLO = :entityBornNumber, ";
                    command.Parameters.Add("entityBornNumber", OracleDbType.Int32).Value = entity.BornNumber;
                }

                if ( dbEmployer.PhoneNumber != entity.PhoneNumber)
                {
                    query += "TELEFONNICISLO = :entityPhoneNumber ";
                    command.Parameters.Add("entityPhoneNumber", OracleDbType.Int32).Value = entity.PhoneNumber;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDZAMESTNANCE = :entityId";
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

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDZAMESTNANCE = :entityId";
                command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        private Employer CreateEmployerFromReader(OracleDataReader reader)
        {
            
            Employer employer = new()
            {
                Id = int.Parse(reader["IDZAMESTANCE"].ToString()),
                Name = reader["JMENO"].ToString(),
                Surname = reader["PRIJMENI"].ToString(),
                BornNumber = reader["RODNECISLO"].ToString(),
                PhoneNumber= int.Parse(reader["TELEFONNICISLO"].ToString()),
               
            };
            return employer;
        }
    }
}
