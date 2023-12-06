using Models.Models.Product;
using Models.Models.Storage;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;


namespace Repositories.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "ZBOZI";

        public ProductRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Product> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"SELECT z.idzbozi IDZBOZI, z.nazev NAZEV,
                    z.aktualnicena AKTUALNICENA, z.cenazeclubcartou CENAZECLUBCARTOU,
                    k.nazev KATEGORIJE, k.idkategorije IDKATEGORIJE,
                    z.hmotnost HMOTNOST FROM {TABLE} z 
                    JOIN KATEGORIJE k ON z.kategorije_idkategorije = k.idkategorije";

                List<Product> products = new List<Product>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(CreateProductFromReader(reader));
                    }
                    return products;
                }
            }
        }

        public Product GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        private Product GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = @$"SELECT z.idzbozi IDZBOZI, z.nazev NAZEV,
                    z.aktualnicena AKTUALNICENA, z.cenazeclubcartou CENAZECLUBCARTOU, 
                    k.nazev KATEGORIJE, k.idkategorije IDKATEGORIJE, 
                    z.hmotnost HMOTNOST FROM {TABLE} z
                    JOIN KATEGORIJE k ON z.kategorije_idkategorije = k.idkategorije 
                    WHERE z.IDZBOZI = :productId";

            command.Parameters.Add("productId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }

                Product product = CreateProductFromReader(reader);

                product.Aviability = GetAviability(product.Id);
                return product;
            }
        }

        public void Create(Product product)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO {TABLE} (NAZEV, AKTUALNICENA, CENAZECLUBCARTOU, HMOTNOST, KATEGORIJE_idKategorije) " +
                              "VALUES (:productName, :productActualPrice, :productClubCardPrice, :productHmotnost, :productCategoryId)";

                command.Parameters.Add("productName", OracleDbType.Varchar2).Value = product.Name;
                command.Parameters.Add("productActualPrice", OracleDbType.Int32).Value = product.ActualPrice;
                command.Parameters.Add("productClubCardPrice", OracleDbType.Int32).Value = product.ClubCardPrice;
                command.Parameters.Add("productHmotnost", OracleDbType.Decimal).Value = product.Weight;
                command.Parameters.Add("productCategoryId", OracleDbType.Decimal).Value = product.CategoryId;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Product product)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Product dbProduct = GetByIdWithOracleCommand(command, product.Id);

                if (dbProduct == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbProduct.Name != product.Name)
                {
                    query += "NAZEV = :productName, ";
                    command.Parameters.Add("productName", OracleDbType.Varchar2).Value = product.Name;
                }

                if (dbProduct.ActualPrice != product.ActualPrice)
                {
                    query += "AKTUALNICENA = :productActualPrice, ";
                    command.Parameters.Add("productActualPrice", OracleDbType.Int32).Value = product.ActualPrice;
                }

                if (dbProduct.ClubCardPrice != product.ClubCardPrice)
                {
                    query += "CENAZECLUBCARTOU = :productClubCardPrice, ";
                    command.Parameters.Add("productClubCardPrice", OracleDbType.Int32).Value = product.ClubCardPrice;
                }

                if (dbProduct.Weight != product.Weight)
                {
                    query += "HMOTNOST = :productHmotnost ";
                    command.Parameters.Add("productHmotnost", OracleDbType.Decimal).Value = product.Weight;
                }

                if (dbProduct.Category.Id != product.CategoryId)
                {
                    query += "KATEGORIJE_IDKATEGORIJE = :productKategorijeId ";
                    command.Parameters.Add("productKategorijeId", OracleDbType.Int32).Value = product.CategoryId;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDZBOZI = :productId";
                    command.Parameters.Add("productId", OracleDbType.Int32).Value = product.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDZBOZI = :productId";
                command.Parameters.Add("productId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        private Product CreateProductFromReader(OracleDataReader reader)
        {
            string price = reader["CENAZECLUBCARTOU"].ToString();
            Product product = new()
            {
                Id = int.Parse(reader["IDZBOZI"].ToString()),
                Name = reader["NAZEV"].ToString(),
                ActualPrice = double.Parse(reader["AKTUALNICENA"].ToString()),
                ClubCardPrice = !string.IsNullOrEmpty(price) ? int.Parse(price) : null,
                CategoryId = int.Parse(reader["IDKATEGORIJE"].ToString()),
                Category = new()
                {
                    Id = int.Parse(reader["IDKATEGORIJE"].ToString()),
                    Name = reader["KATEGORIJE"].ToString()
                },
                Weight = decimal.Parse(reader["HMOTNOST"].ToString())
            };
            return product;
        }

        private StoragedProduct CreateStoragedProductFromReader(OracleDataReader reader)
        {
            StoragedProduct product = new()
            {
                Id = int.Parse(reader["IDZBOZI"].ToString()),
                Name = reader["NAZEV"].ToString(),
                Count = int.Parse(reader["POCETZBOZI"].ToString()),
            };
            return product;
        }


        public List<StoragedProduct> GetProductsFromStorage(int storageId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT z.IDZBOZI IDZBOZI, z.nazev NAZEV, zns.pocetZbozi POCETZBOZI 
                                        FROM SKLADY s
                                        JOIN ZBOZI_NA_SKLADE zns ON s.idSkladu = zns.SKLADY_idSkladu
                                        JOIN ZBOZI z ON zns.ZBOZI_idZbozi = z.idZbozi
                                        WHERE zns.SKLADY_idSkladu =:idSkladu ";

                command.Parameters.Add("idSkladu", OracleDbType.Int32).Value = storageId;

                List<StoragedProduct> storagedProducts = new List<StoragedProduct>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        storagedProducts.Add(CreateStoragedProductFromReader(reader));
                    }
                    return storagedProducts;
                }
            }
        }

        public List<StoragedProduct>GetProductOnStands(int standId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT z.IDZBOZI IDZBOZI, z.nazev NAZEV, znp.pocetZbozi POCETZBOZI  
                                        FROM PULTY p
                                        JOIN ZBOZI_NA_PULTE znp ON p.IDPULTU = znp.PULTY_IDPULTU
                                        JOIN ZBOZI z ON znp.ZBOZI_IDZBOZI = z.IDZBOZI
                                        WHERE p.IDPULTU = :idPultu;
 
                ";

                command.Parameters.Add("idPultu", OracleDbType.Int32).Value = standId;

                List<StoragedProduct> productsOnStand = new List<StoragedProduct>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productsOnStand.Add(CreateProductOnStandFromReader(reader));
                    }
                    return productsOnStand;
                }
            }
        }

        private StoragedProduct CreateProductOnStandFromReader(OracleDataReader reader)
        {
            StoragedProduct product = new()
            {
                Id = int.Parse(reader["IDZBOZI"].ToString()),
                Name = reader["NAZEV"].ToString(),
                Count = int.Parse(reader["POCETZBOZI"].ToString()),
            };
            return product;
        }


        private string GetAviability(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = $"SELECT GET_DOSTUPNOST_ZBOZI(:p_idzbozi) FROM DUAL";
                command.Parameters.Add("p_idzbozi", OracleDbType.Int32).Value = id;

                string aviability = command.ExecuteScalar().ToString();

                return aviability;
            }
        }

    }
}
