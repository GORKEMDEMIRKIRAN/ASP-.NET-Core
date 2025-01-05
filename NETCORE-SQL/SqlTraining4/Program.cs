


/*
BU BÖLÜMDE İSE MYSQL İLE ÇALIŞIYORUZ.

VERİ ERİŞİM SINIFININ HAZIRLANMASINI GÖRELİM.

interface ile;
    -ürün listeleme
    -id göre ürün getirme
    -ürün oluşturma
    -ürün güncelleme
    -ürün silme

mysqlproductdal ile mysql sınıfı oluşturduk.
sqlproductdal ise mssql sınıfı oluşturduk.
productmanager ile mysql veya mssql bağlantısı veririz.

*/



using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace ConsoleApp
{
    // IProductDal bize semayı veriyor.
    public interface IProductDal
    {
        // interface içerisine metotları ekledik.
        List <Product> GetAllProducts();
        // id göre ürünü veriyor.
        Product1 GetProductById(int id);
        // Bu bölümde ürün ismini vererek eşleşen ürünleri getiriyor.
        List<Product> Find(string productName);
        // ürün sayısını alacağız.
        int Count();
        // bu bölümde ürün ekleyeceğiz.
        int Create(Product1 p);
        // bu kısımda ürün güncelliyeceğiz.
        int Update(Product1 p);
        // ürün sileceğiz.
        int Delete(int productId);
    }

    public class MySQLProductDal:IProductDal
    {
        private MySqlConnection GetMySqlConnection()
        {
            string connectionString = @"Server=localhost;port=3306;database=northwind;user=root;password=Demir492;";
            return new MySqlConnection(connectionString);
        }
        // Bu fonksiyonda Kritere göre ürün filtreleme yapıyoruz.
        public Product1 GetProductById(int id)
        {
            Product1 product=null;
            using(var connection = GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    // sql stringte id yerine "productid" değişkenini tanımladık.
                    // sql injection önüne geçmek için yaptık.
                    string sql ="select * from products where id=@productid";
                    MySqlCommand command = new MySqlCommand(sql,connection);
                    // db göndereceğimiz "productid" parametresini dönüştürerek ekledik.
                    command.Parameters.Add("@productid",MySqlDbType.Int32).Value=id;

                    MySqlDataReader reader = command.ExecuteReader();

                    // id gönderip bir kere cevap alacağımız için reader döngü içinde çalıştırmadık.
                    reader.Read();
                    // database veri var mı diye if döngüsü ile sorguluyoruz.
                    if(reader.HasRows)
                    {
                        // product değişkenine Product sınıfını açık "id" sorguladığımız değerleri çektik.
                        product = new Product1()
                        {
                            id=int.Parse(reader["id"].ToString()),
                            productName=reader["product_name"].ToString(),
                            listPrice=double.Parse(reader["list_price"]?.ToString())
                        };                            
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
            return product;
            // bunu manager üzerinde kullanalım.
        }
        // ürün ekleme kısmını tamamlıyalım.
        public int Count()
        {
            int count=0;
            using(var connection = GetMySqlConnection())
            {
                try{
                    connection.Open();
                    string sql="select count(*) from products";
                    MySqlCommand command = new MySqlCommand(sql,connection);
                    object result = command.ExecuteScalar();
                    if(result != null)
                    {
                        //count = int.Parse(result.ToString());
                        count = Convert.ToInt32(result);
                    }
                    else
                    {
                        Console.WriteLine("products tablosu içinde ürün yok.");
                    }
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
            return count;
        }
        public int Create(Product1 p)
        {
            int result = 0;
            //throw new NotImplementedException();
            using(var connection = GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "insert into products (supplier_ids,id,product_code,product_name,description,standard_cost,list_price,reorder_level,target_level,quantity_per_unit,discontinued,minimum_reorder_quantity,category,attachments) VALUES (@suppy_id,@id,@productCode,@productName,@Desc,@stanCode,@listPrice,@reorderLevel,@targetLevel,@perUnit,@Dis,@Minquan,@Cty,@attach)";
                    //string sql = "insert into privileges (id,privilege_name) VALUES (@p_id,@p_name)";
                    MySqlCommand command = new MySqlCommand(sql,connection);

                    //command.Parameters.AddWithValue("@p_id",p.ProductId);
                    //command.Parameters.AddWithValue("@p_name",p.Name);

                    command.Parameters.AddWithValue("@suppy_id",p.SupplyId);
                    command.Parameters.AddWithValue("@id",p.id);
                    command.Parameters.AddWithValue("@productCode",p.productCode);
                    command.Parameters.AddWithValue("@productName",p.productName);
                    command.Parameters.AddWithValue("@Desc",p.description);
                    command.Parameters.AddWithValue("@stanCode",p.standardCost);
                    command.Parameters.AddWithValue("@listPrice",p.listPrice);
                    command.Parameters.AddWithValue("@reorderLevel",p.reorderLevel);
                    command.Parameters.AddWithValue("@targetLevel",p.targetLevel);
                    command.Parameters.AddWithValue("@perUnit",p.quantityUnit);
                    command.Parameters.AddWithValue("@Dis",p.discontinued);
                    command.Parameters.AddWithValue("@Minquan",p.MinumumReorderQuantity);
                    command.Parameters.AddWithValue("@Cty",p.category);
                    command.Parameters.AddWithValue("@attach",p.attachments);

                    result = command.ExecuteNonQuery();
                    Console.WriteLine($"{result} adet kayıt eklendi.");

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
            return result;
        }
        public int Update(Product1 p)
        {
            int result = 0;
            using(var connection = GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "update products set product_name=@p_name where id=@p_id";
                    MySqlCommand  command = new MySqlCommand(sql,connection);

                    command.Parameters.AddWithValue("@p_name",p.productName);
                    command.Parameters.AddWithValue("@p_id",p.id);

                    result = command.ExecuteNonQuery();
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
            return result;
        }
        public int Delete(int productId)
        {
            int result = 0;
            using(var connection = GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "delete from products where id=@productid";
                    MySqlCommand command = new MySqlCommand(sql,connection);
                    command.Parameters.AddWithValue("@productid", productId);
                    result = command.ExecuteNonQuery();
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
            return result;
        }
        public List<Product> GetAllProducts()
        {
            List<Product> products = null;
            using(var connection =GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "select * from products";
                    // Ödemesini kredi kartına iel yapan müşteriler
                    MySqlCommand command = new MySqlCommand(sql,connection);

                    MySqlDataReader reader = command.ExecuteReader();

                    products = new List<Product>();

                    while(reader.Read())
                    {
                        products.Add(
                            new Product
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
        public List<Product> Find(string productName)
        {
            List<Product> product=null;
            using(var connection = GetMySqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "select * from products where product_name LIKE @productName";
                    MySqlCommand command= new MySqlCommand(sql,connection);
                    command.Parameters.Add("@productName",MySqlDbType.String).Value="%"+productName+"%";
                    MySqlDataReader reader = command.ExecuteReader();
                    // geriye bir product listesi döndürüyoruz .Bunu yaparken "ExecuteReader() kullanıyoruz.
                    // geriye bir ürün sayısı istediğimizde "ExecuteScalar() kullanıyoruz.
                    
                    product = new List<Product>();
                    
                    while(reader.Read())
                    {
                        product.Add(
                            new Product
                            {
                                ProductId=int.Parse(reader["id"].ToString()),
                                Name=reader["product_name"].ToString()
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
            return product;
        }

    }




    // IProductDal üzerinden ctrl+"." basarak arabirimi uygula deriz.
    // bu ara birim interface kısmında tanımladığımız metodu sınıf içinde otomatik yazar.
    public class MsSQLProductDal:IProductDal
    {
        private SqlConnection GetSqlConnection()
        {
            string connectionString = @"Data Source=DESKTOP-6TD9LPM\SQLEXPRESS;Initial Catalog=Worker;Integrated Security=SSPI;";
            return new SqlConnection(connectionString);
        }
        public Product1 GetProductById(int id)
        {
            throw new NotImplementedException();
        }
        
        public int Count()
        {
            int count=0;
            using(var connection = GetSqlConnection())
            {
                try{
                    connection.Open();
                    string sql="select count(*) from products";
                    SqlCommand command = new SqlCommand(sql,connection);
                    object result = command.ExecuteScalar();
                    if(result != null)
                    {
                        //count = int.Parse(result.ToString());
                        count = Convert.ToInt32(result);
                    }
                    else
                    {
                        Console.WriteLine("products tablosu içinde ürün yok.");
                    }
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
            return count;
        }

        public int Create(Product1 p)
        {
            throw new NotImplementedException();
        }
        public int Update(Product1 p)
        {
            throw new NotImplementedException();
        }
        public int Delete(int productId)
        {
            throw new NotImplementedException();
        }
        public List<Product> GetAllProducts()
        {
            List<Product> products = null;
            using(var connection =GetSqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "select * from products";
                    // Ödemesini kredi kartına iel yapan müşteriler
                    SqlCommand command = new SqlCommand(sql,connection);

                    SqlDataReader reader = command.ExecuteReader();

                    products = new List<Product>();

                    while(reader.Read())
                    {
                        products.Add(
                            new Product
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
        public List<Product> Find(string productName)
        {
            throw new NotImplementedException();
        }
    }




    public class ProductManager:IProductDal
    {
        IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal=productDal;
        }
        public Product1 GetProductById(int id)
        {
            //throw new NotImplementedException();
            return _productDal.GetProductById(id);
        }
        // ürün sayılarını gören sınıf aktif edelim.
        public int Count()
        {
            return _productDal.Count();
        }
        // Ürün ekleme kısmını yapalım.
        public int Create(Product1 p)
        {
            // throw new NotImplementedException();
            return _productDal.Create(p);
        }
        public int Update(Product1 p)
        {
            //throw new NotImplementedException();
            return _productDal.Update(p);
        }
        public int Delete(int productId)
        {
            return _productDal.Delete(productId);
        }
        public List<Product> GetAllProducts()
        {
            return _productDal.GetAllProducts();
        }
        public List<Product> Find(string productName)
        {
            return _productDal.Find(productName);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //  var productDal = new MySQLProductDal();
            // var productDal = new MsSQLProductDal();
            // mysql veya sql sınfını vererek çalıştırırız.
            var productDal = new ProductManager(new MySQLProductDal());
            //============================================================
            // getallproducts sınıfını çağırıp isimleri bastırmıştık.
            // var products = productDal.GetAllProducts();
            // foreach(var item in products)
            // {
            //     Console.WriteLine($"{item.Name}");
            // }
            //============================================================
            //getproductbyıd ile id verdiğimiz veriyi ağıracağız.
            var productId = productDal.GetProductById(4);
            Console.WriteLine($"{productId.productName}");
            Console.WriteLine("===============================================");
            //============================================================
            // productname vererek product name eşit olan kayıtları getireceğiz.
            var product = productDal.Find("Northwind");
            foreach(var i in product)
            {
                Console.WriteLine($"{i.ProductId} - {i.Name}");
            }
            Console.WriteLine("===============================================");
            //============================================================
            // products tablosunda kaç ürün var onları görelim.
            int count = productDal.Count();
            Console.WriteLine($"Total products: {count}");
            Console.WriteLine("===============================================");
            //============================================================
            // create bölümünde oluşturduğumuz kayıt eklemeyi tamamlıyalım.
            // Bu bölümde ürün ekledik tekrar çalıştırınca böyle kayıt olduğu için aynı
            // kayıttan tekrar eklemez unutma !!
            var p = new Product1()
            {
                SupplyId="221",
                id=134,
                productCode="NBEW-2",
                productName="null",
                description="null",
                standardCost=14.70,
                listPrice=18,
                reorderLevel="23",
                targetLevel="500",
                quantityUnit="45 oz",
                discontinued=0,
                MinumumReorderQuantity="5",
                category="nugget",
                attachments="null"
                // ProductId=32,
                // Name="vuran"

            };
            int cnt = productDal.Create(p);
            Console.WriteLine($"Total products: {cnt}");
            Console.WriteLine("===============================================");
            //============================================================
            // update bölümünde ürün güncelleme işlemini yapalım.
            var u_p = productDal.GetProductById(99);
            u_p.id=100;
            u_p.productName="crole";
            int update_cnt=productDal.Update(u_p);
            Console.WriteLine($"Güncellenen kayıt sayısı: {update_cnt}");
            Console.WriteLine("===============================================");
            //============================================================
            // veridiğimiz ürün id göre products listesinden kayıt siliyoruz.
            // üstte 134 id  sahip kayıt eklemiştik burada o kayıtı sildik.
            // Bu yüzden 134 nolu kayıtı database görmeyeğiz.
            int delete_result = productDal.Delete(134);
            Console.WriteLine($"{delete_result} adet kayıt silindi.");


        }

    }
}
