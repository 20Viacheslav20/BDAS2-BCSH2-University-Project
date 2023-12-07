using Models.Models;
using Models.Models.Product;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly OracleConnection _oracleConnection;
        private readonly IPaymentRepository _paymentRepository;

        private const string TABLE = "PRODEJE";

        public SaleRepository(OracleConnection oracleConnection, IPaymentRepository paymentRepository)
        {
            _oracleConnection = oracleConnection;
            _paymentRepository = paymentRepository;
        }

        public List<Sale> GetAll()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"SELECT IDPRODEJE, DATUMPRODEJE, 
                                        CELKOVACENA, PLATBA_IDPLATBY 
                                        FROM {TABLE}";

                List<Sale> sales = new List<Sale>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sales.Add(CreateSaleFromReader(reader));
                    }
                    return sales;
                }
            }
        }

        public void Create(Sale sale)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"INSERT INTO {TABLE} (DATUMPRODEJE, CELKOVACENA, PLATBA_IDPLATBY) VALUES 
                                                            (:saleSaleDate, :saleTotalPrice, :salePaymentId)";

                command.Parameters.Add("saleSaleDate", OracleDbType.Date).Value = sale.SaleDate;
                command.Parameters.Add("saleTotalPrice", OracleDbType.Int32).Value = sale.TotalPrice;
                command.Parameters.Add("salePaymentId", OracleDbType.Int32).Value = sale.PaymentId;

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {TABLE} WHERE IDPRODEJE = :saleId";
                command.Parameters.Add("saleId", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        public void Edit(Sale sale)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                Sale dbSale = GetById(sale.Id);

                if (dbSale == null)
                    return;

                command.Parameters.Clear();

                string query = "";

                if (dbSale.SaleDate != sale.SaleDate)
                {
                    query += "DATUMPRODEJE = :saleSaleDate, ";
                    command.Parameters.Add("saleSaleDate", OracleDbType.Date).Value = sale.SaleDate;
                }

                if (dbSale.TotalPrice != sale.TotalPrice)
                {
                    query += "CELKOVACENA = :saleTotalPrice, ";
                    command.Parameters.Add("saleTotalPrice", OracleDbType.Int32).Value = sale.TotalPrice;
                }
                
                if (dbSale.PaymentId != sale.PaymentId)
                {
                    query += "PLATBA_IDPLATBY = :salePaymentId, ";
                    command.Parameters.Add("salePaymentId", OracleDbType.Int32).Value = sale.PaymentId;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE {TABLE} SET {query} WHERE IDPRODEJE = :saleId";
                    command.Parameters.Add("saleId", OracleDbType.Int32).Value = sale.Id;

                    command.ExecuteNonQuery();
                }

            }
        }


        public Sale GetById(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT IDPRODEJE, DATUMPRODEJE, 
                                        CELKOVACENA, PLATBA_IDPLATBY 
                                        FROM {TABLE} WHERE IDPRODEJE = :saleId";

                command.Parameters.Add("saleId", OracleDbType.Int32).Value = id;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    return CreateSaleFromReader(reader);
                }
            }
        }

        public List<Sale> GetNotUsedSales()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT * FROM {TABLE} p 
                                        WHERE NOT EXISTS 
                                            (SELECT 1 FROM prodane_zbozi pz 
                                              WHERE pz.prodeje_idprodeje = p.idprodeje)";

                List<Sale> sales = new List<Sale>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sales.Add(CreateSaleFromReader(reader));
                    }
                    return sales;
                }
            }
        }

        private Sale CreateSaleFromReader(OracleDataReader reader)
        {
            Sale sale = new()
            {
                Id = int.Parse(reader["IDPRODEJE"].ToString()),
                SaleDate = DateTime.Parse(reader["DATUMPRODEJE"].ToString()),
                TotalPrice = int.Parse(reader["CELKOVACENA"].ToString()),
                PaymentId = int.Parse(reader["PLATBA_IDPLATBY"].ToString())
            };
            sale.Payment = _paymentRepository.GetPayment(sale.PaymentId);
            return sale;
        }
    }
}
