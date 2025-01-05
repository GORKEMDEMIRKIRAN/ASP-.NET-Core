
using Microsoft.AspNetCore.Mvc;

namespace shopapp.webui.Models
{
    public class Product
    {
        public int ProductId {get;set;}
        public string Name {get; set;}
        public Double Price {get; set;}
        public string Description {get; set;}

        public string ImageUrl {get;set;} //resimleri saklayacak

        // bu değişken satışta olup olamadığını göstermek için tanımladık.
        public bool IsApproved {get; set;}

        // ürünler içine kategoriId ekleyeceğiz.
        public int CategoryId {get;set;}
    }
}