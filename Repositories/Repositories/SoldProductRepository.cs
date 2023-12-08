using Models.Models;
using Models.Models.Product;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class SoldProductRepository : ISoldProductRepository
    {
        private readonly OracleConnection _oracleConnection;
        
        private const string TABLE = "PRODANE_ZBOZI";

        public SoldProductRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<SoldProduct> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                RunView(command);

                command.CommandText = $"SELECT * FROM v_prodeje";

                List<SoldProduct> soldProducts = new List<SoldProduct>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        soldProducts.Add(CreateSoldProductsFromReader(reader));
                    }
                    return soldProducts;
                }
            }
        }

        public void Create(SoldProduct soldProduct)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"INSERT INTO {TABLE} (POCETZBOZI, PRODEJE_IDPRODEJE, ZBOZI_IDZBOZI, PRODEJNICENA)
                                        VALUES (:soldProductCount, :soldProductSaleId, :soldProductProductId, :soldProductPrice)";

                command.Parameters.Add("soldProductCount", OracleDbType.Int32).Value = soldProduct.ProductsCount;
                command.Parameters.Add("soldProductSaleId", OracleDbType.Int32).Value = soldProduct.SaleId;
                command.Parameters.Add("soldProductProductId", OracleDbType.Int32).Value = soldProduct.ProductId;
                command.Parameters.Add("soldProductPrice", OracleDbType.Int32).Value = soldProduct.SoldPrice;


                command.ExecuteNonQuery();
            }
        }

        private void RunView(OracleCommand command)
        {
            command.CommandText = @$"CREATE OR REPLACE VIEW v_prodeje AS
                                    SELECT pr.idprodeje, pr.datumprodeje, pr.celkovacena, p.platba_type,
                                    z.nazev AS nazev_zbozi, pz.zbozi_idzbozi ZBOZIID, pz.pocetzbozi
                                    FROM prodeje pr
                                    JOIN platby p ON pr.platba_idplatby = p.idplatby
                                    JOIN prodane_zbozi pz ON pr.idprodeje = pz.prodeje_idprodeje
                                    JOIN zbozi z ON pz.zbozi_idzbozi = z.idzbozi";

            command.ExecuteNonQuery();
        }

        private SoldProduct CreateSoldProductsFromReader(OracleDataReader reader)
        {
            SoldProduct soldProduct = new()
            {
                ProductsCount = int.Parse(reader["POCETZBOZI"].ToString()),
                SaleId = int.Parse(reader["idprodeje"].ToString()),
                SoldDate = DateTime.Parse(reader["datumprodeje"].ToString()),
                ProductId = int.Parse(reader["ZBOZIID"].ToString()),
                SoldPrice = int.Parse(reader["celkovacena"].ToString()),
                ProductName = reader["nazev_zbozi"].ToString(),
                PaymentType = reader["platba_type"].ToString()
            };
            return soldProduct;
        }


    }
}
