using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;
using System.Xml.Linq;

namespace Repositories.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly OracleConnection _oracleConnection;
        private readonly IAddressRepository _addressRepository;

        private const string TABLE = "ZAMESTNANCI";

        public EmployeeRepository(OracleConnection oracleConnection, IAddressRepository addressRepository)
        {
            _oracleConnection = oracleConnection;
            _addressRepository = addressRepository;
        }

        public List<Employee> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"SELECT z.IDZAMESTNANCE IDZAMESTNANCE, 
                    z.JMENO JMENO, z.PRIJMENI PRIJMENI, z.RODNECISLO RODNECISLO, 
                    z.PRODEJNY_IDPRODEJNY IDPRODEJNY,
                    z.TELEFONNICISLO TELEFONNICISLO, poz.Nazev POZICE, poz.IDPOZICE IDPOZICE
                    FROM {TABLE} z
                    JOIN POZICE poz ON poz.IDPOZICE = z.POZICE_IDPOZICE";

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
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                Employee employee = GetByIdWithOracleCommand(command, id);
                employee.Subordinates = GetUserSubordinates(id);
                employee.Address = _addressRepository.GetEmployeeAddress(id);
                return employee; 
            }
        }

        public Employee GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = @$"SELECT z.IDZAMESTNANCE IDZAMESTNANCE,
                   z.JMENO JMENO, z.PRIJMENI PRIJMENI, z.RODNECISLO RODNECISLO,
                   z.TELEFONNICISLO TELEFONNICISLO, poz.Nazev POZICE, poz.IDPOZICE IDPOZICE,
                   z.PRODEJNY_IDPRODEJNY IDPRODEJNY
                   FROM {TABLE} z
                   JOIN POZICE poz ON poz.IDPOZICE = z.POZICE_IDPOZICE
                   WHERE z.IDZAMESTNANCE = :employeeId";


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

        private List<Employee> GetUserSubordinates(int employeeId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {

                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT z.IDZAMESTNANCE IDZAMESTNANCE, z.jmeno JMENO, z.prijmeni PRIJMENI, 
                                        z.telefonniCislo TELEFONNICISLO, z.rodneCislo RODNECISLO, poz.nazev POZICE, poz.IDPOZICE IDPOZICE,
                                        z.PRODEJNY_IDPRODEJNY IDPRODEJNY
                                        FROM ZAMESTNANCI z
                                        JOIN pozice poz on poz.idpozice = z.pozice_idpozice
                                        START WITH z.ZAMESTNANCI_IDZAMESTNANCE = :employeeId
                                        CONNECT BY PRIOR z.IDZAMESTNANCE = z.ZAMESTNANCI_IDZAMESTNANCE";

                command.Parameters.Clear();
                command.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;

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

        public void Create(Employee employee)
        {
            try
            {
                using (OracleCommand command = _oracleConnection.CreateCommand())
                {
                    _oracleConnection.Open();

                    command.CommandText = $"INSERT INTO {TABLE} (JMENO, PRIJMENI, RODNECISLO, TELEFONNICISLO, PRODEJNY_IDPRODEJNY, POZICE_IDPOZICE)" +
                        "VALUES (:employeeName, :employeeSurname, :employeeBornNumber, :employeePhoneNumber, :employeeShopId, :employeerPositionId)";

                    command.Parameters.Add("employeeName", OracleDbType.Varchar2).Value = employee.Name;
                    command.Parameters.Add("employeeSurname", OracleDbType.Varchar2).Value = employee.Surname;
                    command.Parameters.Add("employeeBornNumber", OracleDbType.Varchar2).Value = employee.BornNumber;
                    command.Parameters.Add("employeePnoneNumber", OracleDbType.Varchar2).Value = employee.PhoneNumber;
                    command.Parameters.Add("employeeShopId", OracleDbType.Int32).Value = employee.ShopId;
                    command.Parameters.Add("employeerPositionId", OracleDbType.Int32).Value = employee.PositionId;

                    command.ExecuteNonQuery();
                }
            }catch (OracleException ex)
            {
                throw new Exception("Born Number is incorrect!");


            }
        }

        public void Edit(Employee employee)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Employee dbEmployer = GetByIdWithOracleCommand(command, employee.Id);

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

                if (dbEmployer.PhoneNumber != employee.PhoneNumber)
                {
                    query += "TELEFONNICISLO = :employeePhoneNumber, ";
                    command.Parameters.Add("employeePhoneNumber", OracleDbType.Varchar2).Value = employee.PhoneNumber;
                }

                if (dbEmployer.ShopId != employee.ShopId)
                {
                    query += "PRODEJNY_IDPRODEJNY = :employeeShopId, ";
                    command.Parameters.Add("employeeShopId", OracleDbType.Int32).Value = employee.ShopId;
                }

                if (dbEmployer.Position.Id  != employee.PositionId)
                {
                    query += "POZICE_IDPOZICE = :smployeerPositionId ";
                    command.Parameters.Add("smployeerPositionId", OracleDbType.Int32).Value = employee.PositionId;
                }

                if (employee.AddressId != 0)
                {
                    _addressRepository.SetAddressToEmployee(employee.Id, employee.AddressId);
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
                ShopId = int.Parse(reader["IDPRODEJNY"].ToString()),
                Position = new()
                {
                    Id = int.Parse(reader["IDPOZICE"].ToString()),
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
    }
}
