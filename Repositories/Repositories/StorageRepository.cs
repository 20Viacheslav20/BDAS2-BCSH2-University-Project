using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;

namespace Repositories.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly OracleConnection _oracleConnection;

        private readonly IProductRepository _productRepository;

        private const string TABLE = "SKLADY";

        public StorageRepository(OracleConnection oracleConnection, IProductRepository productRepository)
        {
            _oracleConnection = oracleConnection;

            _productRepository = productRepository;
        }

        public void Create(Storage entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO {TABLE} (POCETPOLICEK, PRODEJNY_IDPRODEJNY)" +
                    $"VALUES(:entityShelves, :storageShopId)";

                command.Parameters.Add("entityShelves", OracleDbType.Int32).Value = entity.NumberOfShelves;
                command.Parameters.Add("storageShopId", OracleDbType.Int32).Value = entity.ShopId;

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDSKLADU = :entityId";

                command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Storage entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Storage dbStorage = GetByIdWithOracleCommand(command, entity.Id);

                if (dbStorage == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbStorage.NumberOfShelves != entity.NumberOfShelves)
                {
                    query += "POCETPOLICEK = :shopNumberOfShelves, ";
                    command.Parameters.Add("shopNumberOfShelves", OracleDbType.Int32).Value = entity.NumberOfShelves;
                }

                if (dbStorage.ShopId != entity.ShopId)
                {
                    query += "PRODEJNY_IDPRODEJNY = :shopId, ";
                    command.Parameters.Add("shopId", OracleDbType.Int32).Value = entity.ShopId;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDSKLADU = :entityId";
                    command.Parameters.Add("entityId", OracleDbType.Int32).Value = entity.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Storage> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"SELECT * FROM {TABLE}";

                List<Storage> storages = new List<Storage>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        storages.Add(CreateStorageFromReader(reader));
                    }
                    return storages;
                }
            }
        }

        private Storage CreateStorageFromReader(OracleDataReader reader)
        {
            Storage Storage = new()
            {
                Id = int.Parse(reader["IDSKLADU"].ToString()),
                NumberOfShelves = int.Parse(reader["POCETPOLICEK"].ToString()),
                ShopId = int.Parse(reader["PRODEJNY_IDPRODEJNY"].ToString())
            };
            return Storage;
        }

        public Storage GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        private Storage GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT * FROM {TABLE} WHERE IDSKLADU = :entityId";

            command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                Storage storage = CreateStorageFromReader(reader);

                storage.Products = _productRepository.GetProductsFromStorage(id);
                return storage;
            }
        }

    
    }
}
