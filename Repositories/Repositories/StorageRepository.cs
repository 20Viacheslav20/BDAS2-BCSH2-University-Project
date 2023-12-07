using Models.Models.Storage;
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

        public void Create(Storage storage)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO {TABLE} (POCETPOLICEK, PRODEJNY_IDPRODEJNY)" +
                    $"VALUES(:storageShelves, :storageShopId)";

                command.Parameters.Add("storageShelves", OracleDbType.Int32).Value = storage.NumberOfShelves;
                command.Parameters.Add("storageShopId", OracleDbType.Int32).Value = storage.ShopId;

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDSKLADU = :storageId";

                command.Parameters.Add("storageId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Storage storage)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Storage dbStorage = GetByIdWithOracleCommand(command, storage.Id);

                if (dbStorage == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbStorage.NumberOfShelves != storage.NumberOfShelves)
                {
                    query += "POCETPOLICEK = :shopNumberOfShelves, ";
                    command.Parameters.Add("shopNumberOfShelves", OracleDbType.Int32).Value = storage.NumberOfShelves;
                }

                if (dbStorage.ShopId != storage.ShopId)
                {
                    query += "PRODEJNY_IDPRODEJNY = :shopId, ";
                    command.Parameters.Add("shopId", OracleDbType.Int32).Value = storage.ShopId;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDSKLADU = :storageId";
                    command.Parameters.Add("storageId", OracleDbType.Int32).Value = storage.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Storage> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"SELECT sk.IDSKLADU IDSKLADU, sk.POCETPOLICEK POCETPOLICEK, 
                                        sk.PRODEJNY_IDPRODEJNY PRODEJNY_IDPRODEJNY, pr.kontaktnicislo KONTAKTNICISLO 
                                        FROM {TABLE} sk                           
                                        JOIN prodejny pr ON pr.idprodejny = sk.prodejny_idprodejny";

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
            command.CommandText = @$"SELECT sk.IDSKLADU IDSKLADU, sk.POCETPOLICEK POCETPOLICEK,
                                    sk.PRODEJNY_IDPRODEJNY PRODEJNY_IDPRODEJNY, pr.kontaktnicislo KONTAKTNICISLO 
                                    FROM {TABLE} sk 
                                    JOIN prodejny pr ON pr.idprodejny = sk.prodejny_idprodejny 
                                    WHERE IDSKLADU = :storageId";

            command.Parameters.Add("storageId", OracleDbType.Int32).Value = id;

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

        public void AddProduct(Order order)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "insert_produkt";

                command.Parameters.Add("zbozi_idzbozi", OracleDbType.Varchar2).Value = order.ProductId;
                command.Parameters.Add("pocetzbozi", OracleDbType.Int32).Value = order.ProductCount;
                command.Parameters.Add("sklady_idskladu", OracleDbType.Int32).Value = order.StorageId;

                command.ExecuteNonQuery();
            }
        }

        private Storage CreateStorageFromReader(OracleDataReader reader)
        {
            Storage Storage = new()
            {
                Id = int.Parse(reader["IDSKLADU"].ToString()),
                NumberOfShelves = int.Parse(reader["POCETPOLICEK"].ToString()),
                ShopId = int.Parse(reader["PRODEJNY_IDPRODEJNY"].ToString()),
                Shop = new()
                {
                    Id = int.Parse(reader["PRODEJNY_IDPRODEJNY"].ToString()),
                    Contact = reader["KONTAKTNICISLO"].ToString(),
                }
            };
            return Storage;
        }
    }
}
