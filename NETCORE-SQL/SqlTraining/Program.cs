

/*
BU BÖLÜMDE MYSQL VE MSSQL İÇİN DRIVER VE PROVIDER BAĞLANTILARINI SAĞLADIK.
BAĞLANTILAR İÇİN ALTTAKİ PAKETLERİ CONSOLA YAZARAK YÜKLEDİK.
BU PAKET BAĞLANTILARIN OLDUĞUNA EMİN OLMAK İÇİN "csproj" içine bakıyoruz.

*/
/*
mysql ve mssql paketlerinin yüklenmesi

dotnet add package System.Data.SqlClient -- mssql
dotnet add package MySql.Data            -- mysql
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
            GetMySqlConnection();
        }

        static void GetMySqlConnection()
        {
            // VERİTABANI BAĞLANTISININ KURULMASI-MYSQL

            // "mysql connector for .net nuget"  internette aratarak nasıl alacağımızı görürüz.
            // view-command palette -mysql yazarız.
            // MySql.Data yazarak mysql ekleriz.
            // "dotnet add package MySql.Data" olarak konsoldan ekleriz.
            string connectionString = @"Server=localhost;port=3306;database=northwind;user=root;password=Demir492;";
            // driver , provider
            using(var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("bağlantı sağlandı.");
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

        static void GetSqlConnection()
        {
            // VERİTABANI BAĞLANTISININ KURULMASI-MSSQL

            //.NET Core Extension Pack  extension yükle
            // .net için visual studio code özelleştireceğiz.
            // NuGet Package Manager yüklüyoruz.
            // internet üzeründen gerekli olan paketleri projemize yüklememizi sağlıyor.
            // .NET Core Extension Pack içinde nuget kurulu olarak geliyor.

            // 1.YOL
            // ctrl+shift+p veya view-commant palette aç
            // buradan projemize paket ekliyoruz.
            // system.data.sqlclient diyerek ekliyoruz.Sonra versiyonu seçiyoruz.

            // csproj gerekli system.data.sqlclient ekliyebiliriz.
            // data sonra "dotnet restore" diyerek güncelleriz.

            // 2.YOL
            // dotnet add package System.Data.SqlClient
        
            //Daha sonra sql server bağlantı sağlayacak program mevcut


            // worker database bağlantısını sağladık.
            string connectionString = @"Data Source=DESKTOP-6TD9LPM\SQLEXPRESS;Initial Catalog=Worker;Integrated Security=SSPI;";
            // driver , provider
            using(var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("bağlantı sağlandı.");
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

    }
}
