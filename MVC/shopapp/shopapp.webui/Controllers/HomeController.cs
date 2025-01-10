
// Bu controller class başka bir namespace içinde
// Bunu dosyamıza çağırıp aktif etmeliyiz.
// böylelikle aktif ediyoruz.
using Microsoft.AspNetCore.Mvc;
using System;

//using shopapp.webui.Models;
//using shopapp.webui.Data;
using shopapp.entity;
using shopapp.data.Abstract;


// projemiz içindeki controllers alanını temsil ediyor.
namespace shopapp.webui.Controllers
{

    //===========================================================
    // HomeController isminde özel bir sınıf(class)
    // Projeye gelen istekleri karşılayacak olan bir classtır
    // özellik kazandırmamız gerekiyor kalıtım denen bir kavram var.
    // Daha önceden oluşturulmuş olan .net core içerisinde varolan bir class var.
    // Bu bizim adımıza oluşturulmuş yetenekli bir class
    // Bu class "HomeController" aktarıyorum.
    // "HomeController" bir kontroller gibi hareket etsin.
    

    // localhost:5000/home
    // yukarıdaki gibi "home" dersek homecontroller gelir

    // homecontroller 2 tane action metoduna sahip
    public class HomeController : Controller
    {

        //===========================================================
        /* 
        şimdi bu kısımda SHOPAPP.DATA yani dataaccess katmanından repository
        entegre edelim.
        İlk önce csproj dosyasına "shopapp.data" import ettim.
        "using shopapp.data.Abstract;"
        */
        private readonly ICategoryRepository _categoryRepository;  // Dependency Injection
        private readonly IProductRepository _productRepository;

        //public HomeController(ICategoryRepository categoryRepository)
        public HomeController(ICategoryRepository categoryRepository)
        {
            //this._categoryRepository=categoryRepository;
            this._categoryRepository=categoryRepository;
        }

        // BU metotda MVC'de action metodu deniyor.
        // localhost:5000/home/index gelir.

        // localhost:5000/home/index


        public IActionResult Index()
        {
            // Bu kısımdan views'te dinamik veri kullanımını görelim.
            // Index.cshtml içine burada tanımladığımız verileri dinamik olarak aktaracaktır.

            //==============================================================================================
            
            // int userinterface_hour=DateTime.Now.Hour;
            // ViewBag.Greeting = userinterface_hour > 12?"Have a good day":"Good morning";
            // ViewBag.UserName="John";
            // return View();

            //==============================================================================================
            // var products = new List<Product>()
            // {
            //     new Product {Name="Iphone 8",Price=3000,Description="İyi telefon",IsApproved=false},
            //     new Product {Name="Iphone 9",Price=5000,Description="İyi telefon",IsApproved=true},
            //     new Product {Name="Iphone X",Price=7000,Description="İyi telefon",IsApproved=false},
            //     new Product {Name="Iphone 11",Price=9000,Description="İyi telefon",IsApproved=true}
            // };

            // var categories = new List<Category>()
            // {
            //     new Category {Name="Telefonlar",Description="Telefonlar Kategorisi"},
            //     new Category {Name="Bilgisayar",Description="Bilgisayar Kategorisi"},
            //     new Category {Name="Elektronik",Description="Elektronik Kategorisi"}
            // };



            // var category = new Category {Name="Telefonlar", Description="Telefonlar katıldı"};

            // // ViewBag.Category=category;

            // var PVM= new ProductViewModel()
            // {
            //     Cty=category,
            //     //Pds=products,
            //     // Categories=categories
            //     Pds=ProductRepository.Products

            // };

            //return View(PVM);
            return View();
            //==============================================================================================



        }
        
        
        // localhost:5000/home/about
        public IActionResult About() 
        {
            return View();
        }


        public IActionResult Contact()
        {
            // view içine gidip aşırı yüklenmiş metodlara bakarak 3 metotta içine aldığı değer olan sayfayı döndürür.
            return View("MyView");
        }

    }
}