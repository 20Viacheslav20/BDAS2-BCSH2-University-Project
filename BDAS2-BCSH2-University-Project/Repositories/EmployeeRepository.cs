using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class EmployeeRepository : IMainRepository<Employee>
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "ZAMESTNANCI";

        public EmployeeRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Employee> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT z.IDZAMESTNANCE IDZAMESTNANCE, " +
                    $"z.JMENO JMENO, z.PRIJMENI PRIJMENI, z.RODNECISLO RODNECISLO, " +
                    $"z.TELEFONNICISLO TELEFONNICISLO, poz.Nazev POZICE " +
                    $"FROM {TABLE} z " +
                    $"JOIN POZICE poz ON poz.IDPOZICE = z.POZICE_IDPOZICE";

                List<Employee> employers = new List<Employee>();

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

        public Employee GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        public Employee GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT z.IDZAMESTNANCE IDZAMESTNANCE, " +
                   $"z.JMENO JMENO, z.PRIJMENI PRIJMENI, z.RODNECISLO RODNECISLO, " +
                   $"z.TELEFONNICISLO TELEFONNICISLO, poz.Nazev POZICE " +
                   $"FROM {TABLE} z " +
                   $"JOIN POZICE poz ON poz.IDPOZICE = z.POZICE_IDPOZICE " +
                   $"WHERE z.IDZAMESTNANCE = :entityId";


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

        public void Create(Employee entity)
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

        public void Edit(Employee entity)
        {
            using(OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Employee dbEmployer = GetByIdWithOracleCommand(command,entity.Id);

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
                    command.Parameters.Add("entityBornNumber", OracleDbType.Varchar2).Value = entity.BornNumber;
                }

                if ( dbEmployer.PhoneNumber != entity.PhoneNumber)
                {
                    query += "TELEFONNICISLO = :entityPhoneNumber ";
                    command.Parameters.Add("entityPhoneNumber", OracleDbType.Varchar2).Value = entity.PhoneNumber;
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

        private Employee CreateEmployerFromReader(OracleDataReader reader)
        {

            Employee employer = new()
            {
                Id = int.Parse(reader["IDZAMESTNANCE"].ToString()),
                Name = reader["JMENO"].ToString(),
                Surname = reader["PRIJMENI"].ToString(),
                BornNumber = reader["RODNECISLO"].ToString(),
                PhoneNumber = reader["TELEFONNICISLO"].ToString(),
                Position = new()
                {
                    Name = reader["POZICE"].ToString(),
                }
               
            };
            return employer;
        }
    }
}
