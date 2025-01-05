

// BU BÖLÜMDE ENTITY FRAMEWORK CORE GÖRECEĞİZ.

/*
ENTITY FRAMEWORK CORE KURULUMU

entity framework core database providers olarak aratalım.
bu kısımda sqlite,mysql,mssql gibi paketlerin nasıl ekleneceğini bize verir.

MYSQL = "dotnet add package MySql.EntityFrameworkCore --version 8.0.8"
SQLITE = "dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.0"
MSSQL = "dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0"
*/

// tool için bir paket daha kurmamız lazım.

// belirli komutları kullanabilmek için gerekli olan kütüphane
// magretion için gerekli olan kütüphanedir.
// "dotnet tool install --global dotnet-ef"
// nuget paket imzası doğrulaması atlanıyor.



// projede oluşturduğumuz classlar magretion yöntemi ile database tarafında oluşturulucaktır.


using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;
using System.Linq;

using System.ComponentModel.DataAnnotations;
// dataannotations [Key] vb. tanımları kapsıyor.
// bu dataannocations namespace içinde yer alan bir özelliktir.


// CODE FIRST YÖNTEMİNİ KULLANACAĞIZ.
// Bu yöntem kodu yazdıktan sonra database oluşturuyor veya ekleyip güncelliyor.
// code first için sqlite kullanıyoruz.
// ilerliyen aşamalarda mysql oluşturup aktaracağız.
// Bu classlar database içinde tablolara karşılık gelicektir.

namespace ConsoleApp
{
    // CONTEXT SINIFININ EKLENMESİ
    public class ShopContext:DbContext
    {
        static readonly string connectionString_Mysql="Server=localhost;port=3306;database=shopdb;user=root;password=Demir492;";
        public DbSet<Product> Products {get; set;}
        public DbSet<Category> Categories {get; set;}
        public DbSet<Order> Orders {get; set;}

        // logging ekliyoruz.
        // bunun ile yaptığımız işlemleri sql sorgularını görürüz.
        // logging projemize yüklememiz lazım.
        // "dotnet add package Microsoft.Extensions.Logging.Console"
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => {builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Bu kısımda 3 farklı sql server kullanabiliyoruz.
            // Bunlardan birini kullanacaksan "migrations" dosyasını silip en atta yorum olarak
            // yazdımğım 2 maddeyi çalıştırmalıyız. O server göre tekrar yeni bir migration kuracağız.
            //optionsBuilder.UseSqlite("Data Source=shop.db");
            optionsBuilder
                    .UseLoggerFactory(MyLoggerFactory)
                    //.UseSqlite("Data Source=shop.db");
                    //.UseMySql("Server=localhost;port=3306;database=shopdb;user=root;password=Demir492;");
                    .UseMySql(connectionString_Mysql,ServerVersion.AutoDetect(connectionString_Mysql));
                    //.UseSqlServer(@"Data Source=DESKTOP-6TD9LPM\SQLEXPRESS;Initial Catalog=shopdb;Integrated Security=SSPI;");
        }
    }



    // ENTITY SINIFLARININ EKLENMESİ
    
    // Burada oluşturduğum classlar magretion denilen yöntem ile database tablolar olarak oluşacaktır.
    // entity classes
    // Product(Id,Name,Price) => Product(Id,Name,Price)
    public class Product
    {
        // Her tablonun mutlaka birincil anahtarı olmalı yani primary key "ıd" diyebiliriz.
        // primary key(Id,<type_name>Id)
        // Id, veya ProductId gibi kullanmıyorsak; üstüne [Key] yazarak tanımlamamız gerekiyor.
        [Key]
        public int Id {get; set;}
        
        // Name değişkeninin maximum alacağı genişliği belli ettik.
        // eğer tanımlamazsak daha yüksek sınırlara sahiptir.
        // required ile girilmesi gerekli diyoruz.(zorunlu hale geliyor.)
        [MaxLength(100)]
        [Required]
        public string Name {get; set;}
        // decimal zorunlu bir alandır zaten
        // "?" işareti koyarsak "null" bir değer alabilir diyoruz.
        // "public decimal? Price {get; set;}"
        public decimal Price {get; set;}

        public int CategoryId {get; set;} // categoryıd ekledik şimdi 2.magration atayalım.
    }

    public class Category
    {
        [Key]
        public int Id {get; set;}
        public decimal Price {get; set;}
    }

    public class Order
    {
        public int Id {get; set;}
        public int ProductId {get; set;}
        public DateTime DateAdded {get; set;}
    }


    class Program
    {
        static void Main(string[] args)
        {
            AddProducts();
            //GetProductByName("samsung");
            //GetProductById(2);
            //UpdateProduct();
            //DeleteProduct(3);
        }

        // KAYIT SİLME METODU
        static void DeleteProduct(int id)
        {
            using(var db = new ShopContext())
            {
                // sorgusuz direkt silme işlemi yapalım.
                var p = new Product(){Id=id};
                //db.Products.Remove(p);
                db.Entry(p).State= EntityState.Deleted;
                db.SaveChanges();

                // Bu kısımda select sorgusu yaparak değiştirme işlemi yapıyoruz.
                /*
                var p = db.Products.FirstOrDefault( i => i.Id==id);
                if(p!=null)
                {
                    db.Products.Remove(p);
                    db.SaveChanges();
                    Console.WriteLine("veri silindi");
                }
                */
            }
        }
        // KAYIT GÜNCELLEME
        static void UpdateProduct()
        {
            using(var db = new ShopContext())
            {
                var p = db.Products.Where( i => i.Id==1).FirstOrDefault();
                if(p!=null)
                {
                    p.Price=6000;
                    db.Products.Update(p);
                    db.SaveChanges();
                }
            }


            // select sorgusu yapmadan direkt güncelliyelim.
            /*
            using(var db = new ShopContext())
            {
                var entity = new Product(){Id=2};
                db.Products.Attach(entity);
                entity.Price=3000;
                db.SaveChanges();
            }
            */

            // ilk önce sorgulayalım sonra silme işlemni yapalım.
            /*
            using(var db = new ShopContext())
            {
                //change tracking
                var p = db
                        .Products
                        //.AsNoTracking()
                        .Where(i => i.Id==1)
                        .FirstOrDefault();
                if(p !=null)
                {
                    p.Price = p.Price * 1.2m;
                    db.SaveChanges();
                    Console.WriteLine("günvelleme yapıldı.");
                }
            }
            */
        }
        // İSMİ VERİLMİŞ ÜRÜNÜ GETİRME
        static void GetProductByName(string name)
        {
            using(var context = new ShopContext())
            {
                // contains parametresi "Name" içinde böyle bir değer varsa "True" döndürücek ve kayıt gelicektir.
                var products = context
                                .Products
                                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                                //.Where(products => p.Price>1000 && p.Name == "samsung")
                                .Select( p =>
                                        new{
                                            p.Name,
                                            p.Price
                                        }
                                ).ToList();

                foreach(var p in products)
                {
                    Console.WriteLine($"name: {p.Name} price: {p.Price}");
                }
            }
        }
        // ID VERİLMİŞ ÜRÜNÜ GETİRME
        static void GetProductById(int id)
        {
            using(var context = new ShopContext())
            {
                var result = context
                                .Products
                                .Where(p => p.Id == id)
                                .Select(p =>
                                        new{
                                            p.Name,
                                            p.Price
                                        }
                                ).FirstOrDefault();
                            
                Console.WriteLine($"name: {result.Name} price: {result.Price}");

            }
        }
        // İRDEN FAZLA ÜRÜN EKLEME
        static void AddProducts()
        {
            using(var db = new ShopContext())
            {
                var products = new List<Product>()
                {
                    new Product {Name="Samsung S6",Price=4000},
                    new Product {Name="Samsung S7",Price=6000},
                    new Product {Name="Samsung S8",Price=8000},
                    new Product {Name="Samsung S9",Price=10000}
                };
                db.Products.AddRange(products);
                db.SaveChanges();
                Console.WriteLine("veriler eklendi.");
            }
        }
        // BİR ÜRÜN EKLEME
        static void AddProduct()
        {
            using(var db= new ShopContext())
            {
                var p = new Product { Name="Samsung S10", Price=8000};
                db.Products.Add(p);
                db.SaveChanges();
                Console.WriteLine("tek veri eklendi.");
            }
        }
        // BÜTÜN ÜRÜNLERİ LİSTELE
        static void GetAllProducts()
        {
            using(var context = new ShopContext())
            {
                var products = context
                .Products
                .Select( p =>
                    new{
                        p.Name,
                        p.Price
                    }
                ).ToList();

                foreach(var p in products)
                {
                    Console.WriteLine($"name: {p.Name} price: {p.Price}");
                }
                
            }
        }


        // one to many
        // one to one
        // many to many

    }
}

// VERİ TABANININ OLUŞTURULMASI


// İlk magretion oluşturacağız.
// Bizim bu yapıyı çalıştırmak için bir magretion oluşturmamız lazım.
// yapılan her değişiklikte yeni bir magretion oluşturmamız gerekiyor.
// oluşturduğumuz yeni magretion database aktarmak için göndermek gerekicektir.


// ENTITY FRAMEWORK CORE aratıp microsoft girelim.

// üst kısımda "create the model" oluşturduk.
// "create the database" kısmına bakalım.

/*
.NET CLI
1-dotnet tool install --global dotnet-ef
2-dotnet add package Microsoft.EntityFrameworkCore.Design

// Bu kısımda yeni bir magration oluşturuyoruz ve ismi "initialcreate"
3-dotnet ef migrations add InitialCreate   // initialcreate bizim verdiğimiz isimdir.
4-dotnet ef database update                // db oluştu.

örneğin bir sqlite kullanıyorsan daha sonra mysql,mssql çevriyorsan
migrations dosyasını silip 3 ve 4 maddeleri tekrar çalıştırman lazım.
*/




/*
Birden fazla migration oluşturma
Şimdi oluşturduğumuz bir database ilk magretion olarak atadık.
1.migration = InitialMigration
2.migration = addColumnProductategoryId   --product categoryId ekledik.
3.migration = addTable                    --order tablosu ekledik.
*/

/*
dotnet ef migrations add InitialMigration
dotnet ef database update      -- bununla database gönderiyoruz.


dotnet ef database update addColumnProductCategoryId
-- bu kod satırı seçtiğimiz migration geri döner ve bundan sonra eklemiş
-- olduğumuz migration gider.
-- database bundan sonra olan migration değişiklikler gider ama proje
-- dosyamızda oluşan migration gelir.


dotnet ef migrations remove        --ile olduğumuz migration kaldırırız.
dotnet ef database update 0        -- bu satır ilk herşeyi başa alır.
dotnet ef database drop --force    -- Bu kod satırı database kaldırır.
*/





