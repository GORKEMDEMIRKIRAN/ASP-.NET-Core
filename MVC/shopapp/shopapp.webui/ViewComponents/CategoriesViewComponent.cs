

// Bu kısımda View Component oluşturacağız.


/* 
    ViewComponent yapısını oluşturmamızın sebebi bir sınıf içerisinden veri almadan component
    dosyası altında tanımladığımız sql ve ya benzeri veri yapısında veri almasını sağlamak
    için oluşturuyoruz.
    ========================================================================================================================================
    ViewComponent, ASP.NET Core MVC'de kullanılan ve daha modüler, yeniden kullanılabilir 
    ve test edilebilir bir yapı oluşturmaya olanak tanıyan bir özelliktir. ViewComponent,
    klasik Partial View'lerin yerini alabilecek daha güçlü bir alternatiftir.

    ViewComponent, temel olarak belirli bir görevi yerine getiren, bir görünümle birlikte
    kullanılan küçük bir bileşendir. Ancak, Controller'lardan bağımsız olarak çalışır
    ve bir eylem metoduna ihtiyaç duymaz.

    ========================================================================================================================================
    ViewComponent'in özellikleri
    1-Modüler yapı
        Tek bir bileşende hem iş mantığını hem de kullanıcı arayüzünü birleştirir.
    2-Bağımsız çalışma
        Controller'dan bağımsız çalışır. Bu, bileşeninizi farklı sayfalarda veya görünümlerde kolayca kullanabileceğiniz anlamına gelir.
    3-Test Edilebilirlik
        ViewComponent'ler iş mantığınızı izole ettiği için kolayca test edilebilir.
    4-Dinamik Veri Kullanımı
        Gerekli verileri alıp işledikten sonra bir görünümü döndürebilir.
    ========================================================================================================================================
*/
// Bu sayede artık categories için controller bağlı değiliz.
// CategoriesView artık bulunduğu sayfanın mdoeline bir bağımlılığı yok
// Veri tabanından ya da farklı bir dosyadan alıp kendi içeriklerini oluşturabilir.



using Microsoft.AspNetCore.Mvc;
//using shopapp.webui.Models;
using shopapp.entity;

//using shopapp.webui.Data;

namespace shopapp.webui.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // if(RouteData.Values["action"].ToString()=="List")
            // {
            //     ViewBag.SelectedCategory = RouteData?.Values["id"];
            // }
            //return View(CategoryRepository.Categories);
            return View();
        }
    }
}