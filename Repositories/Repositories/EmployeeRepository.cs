using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
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
                        employers.Add(CreateEmployeeFromReader(reader));
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
                   $"WHERE z.IDZAMESTNANCE = :employeeId";


            command.Parameters.Add("employeeId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                return CreateEmployeeFromReader(reader);
            }
        }

        public void Create(Employee employee)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO {TABLE} (JMENO, PRIJMENI, RODNECISLO, TELEFONNICISLO)" +
                    "VALUES (:employeeName, :employeeSurname, :employeeBornNumber, :employeePhoneNumber)";

                command.Parameters.Add("employeeName" , OracleDbType.Varchar2 ).Value = employee.Name;
                command.Parameters.Add("employeeSurname", OracleDbType.Varchar2).Value = employee.Surname;
                command.Parameters.Add("employeeBornNumber", OracleDbType.Int32).Value = employee.BornNumber;
                command.Parameters.Add("employeePnoneNumber", OracleDbType.Int32).Value = employee.PhoneNumber;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Employee employee)
        {
            using(OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Employee dbEmployer = GetByIdWithOracleCommand(command,employee.Id);

                if (dbEmployer == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbEmployer.Name != employee.Name)
                {
                    query += "JMENO = :employeeName, ";
                    command.Parameters.Add("employeeName", OracleDbType.Varchar2).Value = employee.Name;
                }
                if (dbEmployer.Surname != employee.Surname)
                {
                    query += "PRIJMENI = :employeeSurname, ";
                    command.Parameters.Add("employeeSurname", OracleDbType.Varchar2).Value = employee.Surname;
                }

                if (dbEmployer.BornNumber != employee.BornNumber)
                {
                    query += "RODNECISLO = :employeeBornNumber, ";
                    command.Parameters.Add("employeeBornNumber", OracleDbType.Varchar2).Value = employee.BornNumber;
                }

                if ( dbEmployer.PhoneNumber != employee.PhoneNumber)
                {
                    query += "TELEFONNICISLO = :employeePhoneNumber ";
                    command.Parameters.Add("employeePhoneNumber", OracleDbType.Varchar2).Value = employee.PhoneNumber;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDZAMESTNANCE = :employeeId";
                    command.Parameters.Add("employeeId", OracleDbType.Int32).Value = employee.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDZAMESTNANCE = :employeeId";
                command.Parameters.Add("employeeId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        private Employee CreateEmployeeFromReader(OracleDataReader reader)
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

       
        public List<Employee> GetEmployeesWithoutAuth()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT z.IDZAMESTNANCE IDZAMESTNANCE, z.JMENO JMENO, z.PRIJMENI PRIJMENI
                    FROM zamestnanci z 
                    LEFT JOIN autuzivatele au ON z.idzamestnance = au.zamestnanci_idzamestnance
                    WHERE au.zamestnanci_idzamestnance IS NULL";

                List<Employee> employers = new List<Employee>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employers.Add(CreateSimpleEmployee(reader));
                    }
                    return employers;
                }
            }
        }
        private Employee CreateSimpleEmployee(OracleDataReader reader)
        {
            Employee employee = new()
            {
                Id = int.Parse(reader["IDZAMESTNANCE"].ToString()),
                Name = reader["JMENO"].ToString(),
                Surname = reader["PRIJMENI"].ToString(),
                
            };
            return employee;
        }
        public void GetEmployer(int id)
        {
            throw new NotImplementedException();
        }
    }
}
