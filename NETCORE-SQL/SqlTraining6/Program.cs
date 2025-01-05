


using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// fluent apı ve data annotations bakalım.
// string olarak tanımladığımız bir değişken null değer alabilir ama biz bunu
// zorunlu kılmak istersek bu iksinden birini kullanırım.
/*
fluent api
-- onmodelcreating metodu içerisinde
-- modelbuilder üzerinden ilgili entity konumlanıyorsunuz
-- .property özelliği ile gerekli değişkene konumlanarak 
-- belli özellikler ekliyebiliyoruz. ( .IsRequired()) zorunlu olma özelliği

data annotations
-- gerekli değişken üzerine gidip [Key],[Required] ekliyerek yapabiliriz.
*/

//!!!!! unutma fluent apı data annotations ezer.





// ilk önce yaptığım değişiklikleri migration oluştur ve update et
// daha sonra dotnet run diyerek eklediğim veya sildiğin değişkenleri aktar.
namespace App
{
    public class ShopContext:DbContext
    {
        static readonly string connectionString_Mysql="Server=localhost;port=3306;database=shop2db;user=root;password=Demir492;";
        public DbSet<Product> Products {get; set;}
        public DbSet<Category> Categories {get; set;}
        public DbSet<User> Users {get; set;}
        public DbSet<Address> Addresses {get; set;}
        public DbSet<Customer> Customers {get; set;}
        public DbSet<Supplier> Suppliers {get; set;}

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => {builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                    .UseLoggerFactory(MyLoggerFactory)
                    //.UseSqlite("Data Source=shop.db");
                    //.UseMySql("Server=localhost;port=3306;database=shopdb;user=root;password=Demir492;");
                    .UseMySql(connectionString_Mysql,ServerVersion.AutoDetect(connectionString_Mysql));
                    //.UseSqlServer(@"Data Source=DESKTOP-6TD9LPM\SQLEXPRESS;Initial Catalog=shopdb;Integrated Security=SSPI;");
        }

        // fluent apı kullanımı
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bu kısımda data annotations ile [required] yapardık.
            // fluent apı ile bu şekilde tanımlarız.
            /*
                modelBuilder.Entity<Customer>()
                        .property(p=>p.IdentityNumber)
                        .HasMaxLength(11)
                        .IsRequired();            

            */
            // username araması yapıyorsak username alanına bir index bırakmak faydalı olacaktır.
            // modelBuilder.Entity<User>()
            //         .HasOne(u=>u.Username)
            //         .IsUnique();
            modelBuilder.Entity<ProductCategory>()
                    .HasKey(t => new{t.ProductId,t.CategoryId});
            modelBuilder.Entity<ProductCategory>()
                    .HasOne(pc => pc.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(pc=>pc.ProductId);
            modelBuilder.Entity<ProductCategory>()
                    .HasOne(pc=>pc.Category)
                    .WithMany(p=>p.ProductCategories)
                    .HasForeignKey(pc=>pc.CategoryId);
        }
    }
    // DATA SEEDING -- TESK VERİLERİNİN OTOMATİK EKLENMESİ
    // Data seeding ile database ilk oluşturduğumuzda test verilerinin atanması 

    // bu kısımda hiç migrations yoksa oluşturduğumuz listelerler database oluşurken
    // bu listeler belirlediğimiz tablolara atanacaktır.
    // database ilk oluşunca database veri eklenmiş olacaktır.
    public static class DataSeeding
    {
        public static void Seed(DbContext context)
        {
            // GetPendingMigrations bize database oluşturduğumuz migrations vericektir.
            if(context.Database.GetPendingMigrations().Count()==0)
            {
                if(context is ShopContext)
                {
                    ShopContext _context = context as ShopContext;
                    if(_context.Products.Count()==0)
                    {
                        // product list ekleyebiliriz.
                        _context.Products.AddRange(products);
                    }
                    if(_context.Categories.Count()==0)
                    {
                        // category listesini ekleyebiliriz.
                        _context.Categories.AddRange(categories);
                    }
                }
                context.SaveChanges();
            }
        }

        private static Product[] products ={
            new Product(){Name="Iphone 14",Price=2200},
            new Product(){Name="Iphone 15",Price=2800},
            new Product(){Name="Iphone 16",Price=3500}
        };
        private static Category[] categories ={
            new Category(){Name="Telefon"},
            new Category(){Name="Elektronik"},
            new Category(){Name="Bilgisayar"}
        };
    }

    // ============= many to many =====================

    // product ve category birlikte olduğu bir class yaptık.
    // ortak sınıfı product,category eklemeliyiz.
    // daha sonra bu ortak class context tanımlamıyoruz.
    // Bunların birbirleri arasında bağlantıyı context altında override edeceğiz.
    public class Product
    {
        [Key]
        public int Id {get; set;}

        [MaxLength(100)]
        [Required]
        public string Name {get; set;}
        public decimal Price {get; set;} 

        // ekleme ve son güncelleme tarihlerini oluşturuyoruz.
        // 
        //===================================================
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertedDate {get;set;} = DateTime.Now;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] //son güncelleme tarihini değiştirmemizi sağlar.
        public DateTime LastUpdatedDate {get;set;} = DateTime.Now;
        //===================================================
        public List<ProductCategory> ProductCategories {get;set;}
    }

    public class Category
    {
        [Key]
        public int Id {get; set;}
        public string Name {get;set;}
        public List<ProductCategory> ProductCategories {get;set;}
    }
    // ProductCategory  context olarak tanımlamadık çünkü product ve category içinde kullandık.
    // ama yine de "productcategory" bir tablo olarak geliyor.
    // database tablo olarak geçmesini istemezsek [NotMapped] olarak data annotations olarak tanımlayabiliriz.
    // [Table("tablo ismi")] girerek istediğimiz isimli tablo olarak anılabilir.
    // Bunu fluent apı karşılığı
    /*
       .modelBuilder.Entity<ProductCategory>()
            .ToTable("ürünlerkategoriler");
    */
    // Bunu var olan bir tabloyu farklı isimde uygulama tarafında kullanabiliriz.
    public class ProductCategory
    {
        public int ProductId {get;set;}
        public Product Product {get;set;}
        public int CategoryId {get;set;}
        public Category Category {get;set;}
    }


    // BURADA BAĞLANTI TÜRLERİNİ GÖRELİM.


    // ================ one to many ================

    // Bir kullanıcının birden fazla adresi mevcuttur. one to many bağantısı  var.+
    // bir kullanıcı bir müşteridir one to one bağlantısı yani
    public class User
    {
        // ProductId primary key yani anahtar ve ekleme yapıldıkaça otomatik artar.
        // Bunun id olarak bir ürün kodu girebiliriz ve otomatik artmasını istemeyiz.
        // o zaman bunun otomatik artmaası ve sayı girmesini engellemek için
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  -- bu ise tek bir değer gelicek
        // ve bu değer bir daha değiştirilemeyecektir.Ürünün id bilgisi oluyor.
        public int Id {get; set;}
        public string Username {get; set;}
        [Column(TypeName="varchar(20)")] //database türüne sınır koyduk.
        public string Email {get; set;}
        public Customer Customer {get; set;}  // bir kullanıcının bir müşterisi veya tedarikçi olacak
        public List<Address> Addresses {get; set;}  // navigation property
    }

    // bir adres bir kullanıcıya aittir.
    // kullanıcı ve kullanıcı id tanımlı
    // one to one bağlantısı
    public class Address
    {
        [Column("address_id")] // bu şekilde tanımla 
        // uygulama tarafındaki id bilgisi database address_id karşılık gelir.
        public int Id {get; set;}
        public string Fullname {get; set;}
        public string Title {get; set;}
        public string Body {get ; set;}
        public User User {get; set;}  // navigation property
        public int UserId {get; set;} // int => null, 1,2,3,4
    }

    // ================ one to one ================
    // bir müşteri tek bir kullanıcı ve kullanıcı id yani one to one ilişki
    public class Customer
    {
        public int Id {get;set;}
        public string IdentityNumber {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}

        [NotMapped]  // database tarafında fullname değişkenini göremeyiz.
        public string FullName {get; set;}
        public User User {get; set;}
        public int UserId {get; set;}
    }
    // tedarikçinin id,name,taxnumber var
    public class Supplier
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string TaxNumber {get; set;}
    }


    // many to many analtımı üstte product,category kullandık.
    // convention = user içinde customer ve customer içinde user tanımladık.
    // one to one ilişkisi
    // data annotations  = [Key] ifadeleri bunlar gerekli isim tanımlamarı
    // yapamadığımızda data annotations sayesinde gerekli görevleri atayabiliriz.
    // fluent api = many to many bununla yapmamız lazım başka türlü yapamıyoruz.

    // fluent api en üst olanı yani bununla data annotations ve convertion ezebiliriz.
    // data annotations ile convertion ezebiliriz.

    class Program
    {
        static void Main(string[] args)
        {
            // ========== many to many ==============

            using(var db = new ShopContext())
            {

                // 1. kısım olarak product ve category tablolarını doldurduk.
                // var products = new List<Product>()
                // {
                //     new Product(){Name="Samsung S5",Price=2000},
                //     new Product(){Name="Samsung S6",Price=3000},
                //     new Product(){Name="Samsung S7",Price=4000},
                //     new Product(){Name="Samsung S8",Price=5000}
                // };
                // var categories = new List<Category>()
                // {
                //     new Category(){Name="Telefon"},
                //     new Category(){Name="Elektronik"},
                //     new Category(){Name="Bilgisayar"}
                // };
                // db.Products.AddRange(products);
                // db.Categories.AddRange(categories);
                // db.SaveChanges();

                // 2.kısım olarak productcategory tablosuna 1.numaralı productId 1 ve 2 numaralı categoryId bağlatısı olduğunu ekledik.
                // int[] ids = new int[2]{1,2};
                // var p = db.Products.Find(1);
                // p.ProductCategories = ids.Select(cid=>new ProductCategory(){
                //     CategoryId=cid,
                //     ProductId=p.Id
                // }).ToList();

                // db.SaveChanges();


                // ========================================
                // Bu bölümde products sınıfına ilk ürün ekleme tarihi ve son güncelleme tarihi ekledik.
                // şimdi ilk ürünü çekip değişiklik yapalım ve son güncelleme tarihi değişsin.
                var p = db.Products.FirstOrDefault();
                p.Name="Iphone 14";
                db.SaveChanges();

            }

            // ========== one to many ==============
            //InsertUsers();
            //InsertAddresses();
            // using(var db = new ShopContext())
            // {
            //     var user = db.Users.FirstOrDefault(i => i.Username == "crosliar");
            //     if(user != null)
            //     {
            //         user.Addresses = new List<Address>();
            //         user.Addresses.AddRange(
            //             new List<Address>(){
            //                 new Address(){Fullname="John carter",Title="İş adresi 1",Body="İzmit"},
            //                 new Address(){Fullname="Clara Janet",Title="Ev adresi 1",Body="Paris"},
            //                 new Address(){Fullname="Jack racher",Title="İş adresi 2",Body="İngiltere"}
            //             }
            //         );
            //         db.SaveChanges();
            //     }
            // }

            // ======= one to one ========
            // using(var db = new ShopContext())
            // {
            //     //=============================================
            //     var customer = new Customer()
            //     {
            //         IdentityNumber="23123555",
            //         FirstName="clara",
            //         LastName="diar",
            //         User=db.Users.FirstOrDefault(i=>i.Id==3)
            //     };
            //     db.Customers.Add(customer);
            //     db.SaveChanges();
            //     //=============================================
            //     var user = new User()
            //     {
            //         Username="dnm",
            //         Email="dnm@gmail.com",
            //         Customer=new Customer(){
            //             FirstName="Deneme",
            //             LastName="Deneme",
            //             IdentityNumber="539432" 
            //         };
            //     };
            //     db.Users.Add(user);
            //     db.SaveChanges();
            //     //=============================================
            // }

        }

        // kullanıcının adreslerine liste şeklinde ekleme yapıyoruz.
        static void InsertUsers()
        {
            var users = new List<User>(){
                new User(){Username="CrosLiar",Email="info@crosliar.com"},
                new User(){Username="Clara Janet",Email="info@clarajanet.com"},
                new User(){Username="Jack racher",Email="info@jackracher.com"}
            };

            using(var db = new ShopContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }
        }

        // Bu kısımda adreslere ekleme yapıyoruz.
        static void InsertAddresses()
        {
            var addresses = new List<Address>(){
                new Address(){Fullname="Cros Liar",Title="ev adresi",Body="İstanbul",UserId=1},
                new Address(){Fullname="Cros Liar",Title="iş adresi",Body="İstanbul",UserId=1},
                new Address(){Fullname="Sara crom",Title="iş adresi",Body="danimarka",UserId=3},
                new Address(){Fullname="Sara crom",Title="ev adresi",Body="danimarka",UserId=3},
                new Address(){Fullname="Bilbo gora",Title="iş adresi",Body="fransa",UserId=5},
                new Address(){Fullname="Carla hort",Title="iş adresi",Body="İstanbul",UserId=6}
            };
            using(var db = new ShopContext())
            {
                db.Addresses.AddRange(addresses);
                db.SaveChanges();
            }
        }


    }
}