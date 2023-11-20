using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class ProductRepository : IMainRepository<Product>
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

                command.CommandText = $"SELECT z.idzbozi IDZBOZI, z.nazev NAZEV, " +
                    $"z.aktualnicena AKTUALNICENA, z.cenazeclubcartou CENAZECLUBCARTOU," +
                    $"z.hmotnost HMOTNOST, k.nazev KATEGORIJE FROM {TABLE} z " +
                    $"JOIN KATEGORIJE k ON z.kategorije_idkategorije = k.idkategorije";

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
                _oracleConnection.Open();

                return GetByIdWithOracleCommand(command, id);
            }
        }

        private Product GetByIdWithOracleCommand(OracleCommand command, int id)
        {
            command.CommandText = $"SELECT z.idzbozi IDZBOZI, z.nazev NAZEV, " +
                $"z.aktualnicena AKTUALNICENA, z.cenazeclubcartou CENAZECLUBCARTOU," +
                $"z.hmotnost HMOTNOST, k.nazev KATEGORIJE FROM {TABLE} z " +
                $"JOIN KATEGORIJE k ON z.kategorije_idkategorije = k.idkategorije " +
                $"WHERE IDZBOZI = :entityId";

            command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;  
                }
                return CreateProductFromReader(reader);
            }
        }

        public void Create(Product entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"INSERT INTO {TABLE} (NAZEV, AKTUALNICENA, CENAZECLUBCARTOU, HMOTNOST, KATEGORIJE_idKategorije) " +
                              "VALUES (:entityName, :entityActualPrice, :entityClubCardPrice, :entityHmotnost, :entityCategoryId)";

                command.Parameters.Add("entityName", OracleDbType.Varchar2).Value = entity.Name;
                command.Parameters.Add("entityActualPrice", OracleDbType.Int32).Value = entity.ActualPrice;
                command.Parameters.Add("entityClubCardPrice", OracleDbType.Int32).Value = entity.ClubCardPrice;
                command.Parameters.Add("entityHmotnost", OracleDbType.Decimal).Value = entity.Weight;
                command.Parameters.Add("entityCategoryId", OracleDbType.Decimal).Value = 1;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Product entity)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Product dbProduct = GetByIdWithOracleCommand(command, entity.Id);

                if (dbProduct == null)
                    return;

                command.Parameters.Clear();

                string query = "";
                if (dbProduct.Name != entity.Name)
                {
                    query += "NAZEV = :entityName, ";
                    command.Parameters.Add("entityName", OracleDbType.Varchar2).Value = entity.Name;
                }

                if (dbProduct.ActualPrice != entity.ActualPrice)
                {
                    query += "AKTUALNICENA = :entityActualPrice, ";
                    command.Parameters.Add("entityActualPrice", OracleDbType.Int32).Value = entity.ActualPrice;
                }

                if (dbProduct.ClubCardPrice != entity.ClubCardPrice)
                {
                    query += "CENAZECLUBCARTOU = :entityClubCardPrice, ";
                    command.Parameters.Add("entityClubCardPrice", OracleDbType.Int32).Value = entity.ClubCardPrice;
                }

                if (dbProduct.Weight != entity.Weight)
                {
                    query += "HMOTNOST = :entityHmotnost ";
                    command.Parameters.Add("entityHmotnost", OracleDbType.Decimal).Value = entity.Weight;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDZBOZI = :entityId";
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

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDZBOZI = :entityId";
                command.Parameters.Add("entityId", OracleDbType.Int32).Value = id;

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
                ActualPrice = int.Parse(reader["AKTUALNICENA"].ToString()),
                ClubCardPrice = !string.IsNullOrEmpty(price) ? int.Parse(price) : null,
                // TODO 
                //CategoryId = int.Parse(reader["KATEGORIJE_IDKATEGORIJE"].ToString()),
                Category = reader["KATEGORIJE"].ToString(),
                Weight = decimal.Parse(reader["HMOTNOST"].ToString())
            };
            return product;
        }
    }
}
