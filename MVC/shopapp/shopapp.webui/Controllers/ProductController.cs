

using Microsoft.AspNetCore.Mvc;

// Modeli controller tanımladık.
using shopapp.webui.Models;
using shopapp.webui.Data;


namespace shopapp.webui.Controllers
{
    // localhost:5000/product

    public class ProductController : Controller
    {
        // Alttaki satırda "product" dediğimizde direkt "ProductController" getiriyor.
        //"ProductController"  burada "Product" kısmı önemli
        public IActionResult Index()
        {
            //VIEW VERİ AKTARIM YÖNTEMLERİ
            // ViewBag
            // Model
            // ViewData
            // =================================================================================
            //ViewData
            var prd = new Product {Name="Iphone X",Price=6000,Description="Süper telefon"};
            ViewData["Category"]="Telefonlar";
            ViewData["PRD"]=prd;
            // =================================================================================
            //ViewBag
            ViewBag.Category1="Telefonlar";
            ViewBag.prd1=prd;
            // =================================================================================
            //Model
            // direkt prd View içinde verdik.
            // =================================================================================


            return View(prd);
        }

        // localhost:5000/product/list
        public IActionResult List(int? id)
        {
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

            var category = new Category {Name="Telefonlar",Description="Telefonlar katıldı"};

            // ViewBag.Category=category;
            Console.WriteLine(RouteData.Values["controller"]);
            Console.WriteLine(RouteData.Values["action"]);
            Console.WriteLine(RouteData.Values["id"]);

            /*
                {controller}/{action}/{id?}
                product/list/3
                RouteData.Values["controller"] = product
                RouteData.Values["action"] = list
                RouteData.Values["id"] = 3
            */

            /*
            ===================================================================================
                ==int? id eğer değer girmezsek;
                    product/list gelecektir.
                ==int? id eğer değer girersek
                    product/list/1 vs gelirsek gelen kategorideki ürünler listelenicektir.

            // product/list = tüm ürünleri(sayfalama)
            // product/list/2 = 2 nuamralı kategoriyee ait ürünler
            ===================================================================================
            */
            var prd = ProductRepository.Products;
            if(id!=null)
            {
                prd=prd.Where(p=>p.CategoryId==id).ToList();
            }

            var PVM= new ProductViewModel()
            {
                Cty=category,
                // Pds=products,
                // Categories=categories
                // burada "ProductRepository" sınıfı için üstte "data" klasörünü import et.
                //Pds=ProductRepository.Products
                Pds= prd
            };
            return View(PVM);
        }
        
        // localhost:5000/product/details
        public IActionResult Details(int id)
        {
            // Burada bilgileri tek tek taşımak yerine bir model
            // oluşturarak sayfada daha güzel şekilde kullanabiliriz.
            //name: "samsuns s6"
            //price: 3000
            // description: "good phone"

            // Viewbag ile cshtml aktarıyorduk.
            //=====================================
            // ViewBag.Name="samsung s6";
            // ViewBag.Price=3000;
            // ViewBag.Description="good phone";
            //=====================================
            //Bu kısımda model ile aktaracaağız.
            // var product = new Product();
            // product.Name="Samsung";
            // product.Price=3000;
            // product.Description="iyi telefon";
            // return View(product);
            //========================================
            return View(ProductRepository.GetProductById(id));
        }
    }
}