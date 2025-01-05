


/*
BU BÖLÜMDE İSE MYSQL İLE ÇALIŞIYORUZ.

SQL İÇİNDEKİ PRODUCTS VERİ ALACAĞIZ BUNUN İÇİN PRODUCTS CLASS OLUŞTURACAĞIZ.
*/


using System;
// mssql
using System.Data.SqlClient;
// mysql
using MySql.Data.MySqlClient;

using System.Collections.Generic;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var products = GetAllProducts();
            foreach (var pr in products)
            {
                if(pr.Price>10){
                    Console.WriteLine($"name: {pr.Name} price: {pr.Price}");
                }
            }
            //GetSqlConnection();
            //GetMySqlConnection();
            //GetAllProducts();
        }

        static List<Products> GetAllProducts()
        {
            List<Products> products = null;

            using(var connection =GetMySqlConnection())
            {
                try
                {
                    // Veri tabanından ürün bilgilerinin alınması

                    connection.Open();
                    //Console.WriteLine("bağlantı sağlandı.");

                    string sql = "select * from products";

                    MySqlCommand command = new MySqlCommand(sql,connection);

                    MySqlDataReader reader = command.ExecuteReader();

                    products = new List<Products>();

                    while(reader.Read())
                    {
                        products.Add(
                            new Products
                            {
                                ProductId=int.Parse(reader["id"].ToString()),
                                Name=reader["product_name"].ToString(),
                                Price=double.Parse(reader["list_price"]?.ToString()) // soru işareti null değer olabilir.
                            }
                        );
                    }
                    reader.Close();


                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return products;
        }

        static MySqlConnection GetMySqlConnection()
        {
            // VERİTABANI BAĞLANTISININ KURULMASI-MYSQL
            string connectionString = @"Server=localhost;port=3306;database=northwind;user=root;password=Demir492;";
            // driver , provider
            return new MySqlConnection(connectionString);
        }

        static SqlConnection GetSqlConnection()
        {
            // VERİTABANI BAĞLANTISININ KURULMASI-MSSQL
            string connectionString = @"Data Source=DESKTOP-6TD9LPM\SQLEXPRESS;Initial Catalog=Worker;Integrated Security=SSPI;";
            // driver , provider
            return new SqlConnection(connectionString);
        }

    }
}
