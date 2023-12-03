using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;

namespace Repositories.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "ADRESY";

        public AddressRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Address> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE}";

                List<Address> addresses = new List<Address>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        addresses.Add(CreateAddressFromReader(reader));
                    }
                    return addresses;
                }
            }
        }

        public void Create(Address address)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"INSERT INTO {TABLE}(MESTO, ULICE)
                                        VALUES (:addressCity, :addressStreet)";

                command.Parameters.Add("addressCity", OracleDbType.Varchar2).Value = address.City;
                command.Parameters.Add("addressStreet", OracleDbType.Varchar2).Value = address.City;


                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDADRESY = :addressId";
                command.Parameters.Add("addressId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Address address)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Address dbAddress = GetByIdWithOracleCommand(command, address.Id);

                if (dbAddress == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbAddress.City != address.City)
                {
                    query += "MESTO = :addressCity, ";
                    command.Parameters.Add("addressCity", OracleDbType.Varchar2).Value = address.City;
                }
                if (dbAddress.Street != address.Street)
                {
                    query += "ULICE = :addressStreet, ";
                    command.Parameters.Add("addressStreet", OracleDbType.Varchar2).Value = address.Street;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDADRESY = :addressId";
                    command.Parameters.Add("addressId", OracleDbType.Int32).Value = address.Id;

                    command.ExecuteNonQuery();
                }
            }
        }


        public Address GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        public Address GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = @$"SELECT * 
                                     FROM {TABLE}
                                     WHERE IDADRESY = :addressId";


            command.Parameters.Add("addressId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                return CreateAddressFromReader(reader);
            }
        }

        public List<Address> GetAddressesForEmployee(int employeeId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT * FROM {TABLE} 
                                        WHERE 
                                            (zamestnanci_idzamestnance is null 
                                            or IDADRESY = (select MAX(IDADRESY) FROM  ADRESY WHERE zamestnanci_idzamestnance = :employeeId)) 
                                            and prodejny_idprodejny is null";

                command.Parameters.Clear();
                command.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;

                List<Address> addresses = new List<Address>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        addresses.Add(CreateAddressFromReader(reader));
                    }
                    return addresses;
                }
            }
        }

        public List<Address> GetAddressesForShop(int shopId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT * FROM {TABLE} 
                                        WHERE (
                                            prodejny_idprodejny is null 
                                            or IDADRESY = (select MAX(IDADRESY) FROM ADRESY WHERE PRODEJNY_IDPRODEJNY = :shopId)) 
                                            and zamestnanci_idzamestnance is null";

                command.Parameters.Clear();
                command.Parameters.Add("shopId", OracleDbType.Int32).Value = shopId;

                List<Address> addresses = new List<Address>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        addresses.Add(CreateAddressFromReader(reader));
                    }
                    return addresses;
                }
            }
        }

        public Address GetEmployeeAddress(int employeeId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {

                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT * 
                                     FROM {TABLE}
                                     WHERE ZAMESTNANCI_IDZAMESTNANCE = :employeeId";


                command.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    return CreateAddressFromReader(reader);
                }
            }
        }

        public void SetAddressToEmployee(int employeeId, int addressId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.Parameters.Clear();

                command.CommandText = @$"UPDATE {TABLE} SET ZAMESTNANCI_IDZAMESTNANCE = :employeeId WHERE IDADRESY = :addressId";

                command.Parameters.Add("employeeId", OracleDbType.Int32).Value = employeeId;
                command.Parameters.Add("addressId", OracleDbType.Int32).Value = addressId;


                command.ExecuteNonQuery();
            }
        }

        public Address GetShopAddress(int shopId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT * FROM {TABLE}
                                       WHERE PRODEJNY_IDPRODEJNY = :shopId";


                command.Parameters.Add("shopId", OracleDbType.Int32).Value = shopId;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    return CreateAddressFromReader(reader);
                }
            }
        }

        public void SetAddressToShop(int shopId, int addressId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.Parameters.Clear();

                command.CommandText = @$"UPDATE {TABLE} SET PRODEJNY_IDPRODEJNY = :shopId WHERE IDADRESY = :addressId";

                command.Parameters.Add("shopId", OracleDbType.Int32).Value = shopId;
                command.Parameters.Add("addressId", OracleDbType.Int32).Value = addressId;

                command.ExecuteNonQuery();
            }
        }

        private Address CreateAddressFromReader(OracleDataReader reader)
        {
            Address address = new()
            {
                Id = int.Parse(reader["IDADRESY"].ToString()),
                City = reader["MESTO"].ToString(),
                Street = reader["ULICE"].ToString(),
            };
            return address;
        }

    }
}
