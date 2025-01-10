
using System.Collections.Generic;
using shopapp.data.Abstract;
using shopapp.entity;
using System.Linq;
using System;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository:EfCoreGenericRepository<Category,ShopContext>,ICategoryRepository
    {
        public List<Category> GetPopularCategories()
        {
            using(var context = new ShopContext())
            {
                throw new System.NotImplementedException();
            }
            
        }
        public List<Category> GetTop5Categories()
        {
            using(var context = new ShopContext())
            {
                return context.Categories.Take(5).ToList();
            }
        }

    }
}