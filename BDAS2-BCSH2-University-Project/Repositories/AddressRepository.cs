using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class AddressRepository : IMainRepository<Address>
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "ADRESY";

        public AddressRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Address> GetAll()
        {
            using(OracleCommand command = _oracleConnection.CreateCommand())
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

        public void Create(Address entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO{TABLE}(MESTO,ULICE)" +
                    "VALUES (:entityCity, :entityStreet)";

                command.Parameters.Add("entityCity", OracleDbType.Varchar2).Value = entity.City;
                command.Parameters.Add("entityStreet", OracleDbType.Varchar2).Value = entity.City;
                

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDADRESY = :entityId";
                command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Address entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Address dbAddress = GetByIdWithOracleCommand(command, entity.Id);

                if (dbAddress == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbAddress.City != entity.City)
                {
                    query += "MESTO = :entityCity, ";
                    command.Parameters.Add("entityCity", OracleDbType.Varchar2).Value = entity.City;
                }
                if (dbAddress.Street != entity.Street)
                {
                    query += "ULICE = :entityStreet, ";
                    command.Parameters.Add("entityStreet", OracleDbType.Varchar2).Value = entity.Street;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDADRESY = :entityId";
                    command.Parameters.Add("entityId", OracleDbType.Int32).Value = entity.Id;

                    command.ExecuteNonQuery();
                }
            }
        }


        public Address GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                return GetByIdWithOracleCommand(command,id);
            }
        }

        public Address GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = @$"SELECT * 
                                     FROM {TABLE}
                                     WHERE IDADRESY = :entityId";


            command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                return CreateAddressFromReader(reader);
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
