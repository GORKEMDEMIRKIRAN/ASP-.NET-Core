

using System.Collections.Generic;
using System.Linq;
using shopapp.webui.Models;


namespace shopapp.webui.Data
{
    public class ProductRepository
    {
        private static List<Product> _products = null;

        static ProductRepository()
        {
            _products=new List<Product>
            {
                new Product() {ProductId=1,Name="Iphone 7",Price=3500,Description="good phone",IsApproved=false,ImageUrl="1.jpg",CategoryId=1},
                new Product() {ProductId=2,Name="Iphone 8",Price=4500,Description="very good phone",IsApproved=true,ImageUrl="2.jpg",CategoryId=1},
                new Product() {ProductId=3,Name="Iphone 9",Price=5500,Description="very good phone",IsApproved=true,ImageUrl="3.jpg",CategoryId=1},
                new Product() {ProductId=4,Name="Iphone X",Price=3500,Description="good phone",IsApproved=false,ImageUrl="1.jpg",CategoryId=1},
                new Product() {ProductId=5,Name="lenovo 8",Price=4500,Description="very good phone",IsApproved=true,ImageUrl="2.jpg",CategoryId=2},
                new Product() {ProductId=6,Name="lenovo 9",Price=5500,Description="very good phone",IsApproved=true,ImageUrl="3.jpg",CategoryId=2},
                new Product() {ProductId=7,Name="lenovo X",Price=3500,Description="good phone",IsApproved=false,ImageUrl="1.jpg",CategoryId=2},
                new Product() {ProductId=8,Name="lenovo 12",Price=4500,Description="very good phone",IsApproved=true,ImageUrl="2.jpg",CategoryId=2},
                new Product() {ProductId=9,Name="xbox 9",Price=5500,Description="very good phone",IsApproved=true,ImageUrl="3.jpg",CategoryId=3},
                new Product() {ProductId=10,Name="xbox 7",Price=3500,Description="good phone",IsApproved=false,ImageUrl="1.jpg",CategoryId=3},
                new Product() {ProductId=11,Name="xbox 8",Price=4500,Description="very good phone",IsApproved=true,ImageUrl="2.jpg",CategoryId=3},
                new Product() {ProductId=12,Name="xbox 12",Price=5500,Description="very good phone",IsApproved=true,ImageUrl="3.jpg",CategoryId=3}
            };
        }
        public static List<Product> Products
        {
            get{
                return _products;
            }
        }
        // ürün ekleme kısmı
        public static void AddProduct(Product product)
        {
            _products.Add(product);
        }
        // ürün id göre ürünü getirin metot
        public static Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p=>p.ProductId==id);
        }
    }

}