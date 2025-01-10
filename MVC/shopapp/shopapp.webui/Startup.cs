
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection; //addscored fonksiyonu
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;


using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;


//şimdi shopapp.data içine gidip ilk migration oluştruacağız.
// shopapp.webui startup project başlangıç ekranı olacak.
// migration oluştururken shopapp.webui startup project olarak tanımlayacağız.
//  shopapp.data içine git ve bu kodu yaz.
// Unutma microsoft.entitframeworkCore.design kurulu olsun yoksa hata alırsın.
// --dotnet ef migrations add InitialCreate --startup-project ../shopapp.webui
// "dotnet ef database update --startup-project ../shopapp.webui" startup projeyi tekrar tanımalyarak ile database aktaralım.
namespace shopapp.webui
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Servislerinizi buraya ekleyin, örn: services.AddControllers();

            // mvc
            // razor pages
            // addscored() bu metod generic
            // ilk sanal sınıf değişkeni,ikinci olarak database değişkenini alıyor.
            //services.AddScoped<IProductRepository, EfCoreProductRepository>();

            // Ne zmaan ICategoryRepository çağırırsak "efcorecategoryrepository" bize nesne gelecektir.
            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Şimdi bu kısımda "wwwroot" resim ve style dosyalarını 
            // proje içine yükleyip kullanmak için tanımlama yapacağız.
            app.UseStaticFiles();  //wwwroot

            // bu kısımda bootstrap için node_modules projeye dahil ediyoruz.
            // porjenin ana dosya yolunu alıp üzerine node modules klasörünü ekliyecek
            // node modules dosyasını "modules" olarak çağıralım.


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                    RequestPath = "/modules"
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();


            // localhost:5000    -- abc.com demektir.
            // localhost:5000/home
            // localhost:5000/product/list/2
            // localhost:5000/category/list

            // altta gördüğümüz uzantılarda;
            /*
                // localhost:5000    
                // localhost:5000/home

                // localhost:5000    
                // localhost:5000/home/   her ikisinde de bulunamadı hatası alıyoruz.

                ---localhost:5000  veya localhost:5000/home yani homecontroller kullanıcı geldiği zaman
                bunlara gelince sayfa bulunamadı hastası alıyoruz.Bunlar için varsayılan bir routing 
                yapıyor olmamız lazım.

                ---varsayılan bir controller ve bu controller altında çalışan bir varsayılan "action" ihtiyacımız var.

                pattern: "{controller=Home}/{action}/{id?}"
                Bu kısımda "controller=Home" diyerek controller çağrılmadığı zamanda "Home" controller gidecektir.
                pattern: "{controller=Home}/{action="Index"}/{id?}"
                Bu kısımda "action=Index" action içinde "index" metodunu varsayılan olarak çağırabiliriz.


            */


            app.UseEndpoints(endpoints =>
            {
                // Routing şeması çok önemli
                // haritamızda controller rotamızı kuruyoruz.

                // "{controller}/{action}/{id?}" 3 bölmeli routing var
                // 3.bölme isteğe bağlı

                // Bu şekilde;
                //===================================================
                // localhost:5000          sayfasında home/index
                // localhost:5000/home     sayfasında home/index
                // localhost:5000/product  sayfasında product/index
                //===================================================
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }

}