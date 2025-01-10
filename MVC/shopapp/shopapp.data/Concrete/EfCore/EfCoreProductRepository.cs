
using System.Collections.Generic;
using shopapp.data.Abstract;
using shopapp.entity;
using System.Linq;
using System;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreProductRepository:EfCoreGenericRepository<Product,ShopContext>,IProductRepository
    {
        public List<Product> GetPopularProducts()
        {
            using(var context = new ShopContext())
            {
                throw new System.NotImplementedException();
            }
            
        }
        public List<Product> GetTop5Products()
        {
            using(var context = new ShopContext())
            {
                return context.Products.Take(5).ToList();
            }
            
        }
    }
}