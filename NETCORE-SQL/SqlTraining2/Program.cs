

/*
BU BÖLÜMDE İSE MYSQL İLE ÇALIŞIYORUZ.
MYSQL İÇİNDEKİ PRODUCTS TABLOSUNA ULAŞIP VERİLERİ GÖRÜNTÜLEDİK.

VERİLERİN NESNE İLE TAŞINMASI

*/


using System;
// mssql
using System.Data.SqlClient;
// mysql
using MySql.Data.MySqlClient;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetSqlConnection();
            //GetMySqlConnection();
            GetAllProducts();
        }

        static void GetAllProducts()
        {
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

                    while(reader.Read())
                    {
                        Console.WriteLine($"name:{reader[3]} price:{reader[6]}");
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


