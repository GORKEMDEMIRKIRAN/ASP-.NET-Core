

// REPOSITORY SINIFLARININ EKLENMESİ

using System.Collections.Generic;
using System.Linq;
using shopapp.webui.Models;


namespace shopapp.webui.Data
{
    public class CategoryRepository
    {
        private static List<Category> _categories = null;

        static CategoryRepository()
        {
            _categories = new List<Category>{
                new Category {CategoryId=1,Name="Phone",Description="Phone category"},
                new Category {CategoryId=2,Name="Computer",Description="Computer category"},
                new Category {CategoryId=3,Name="Electronic",Description="Electronic category"}
            };
        }

        public static List<Category> Categories
        {
            get{
                return _categories;
            }
        }

        public static void AddCategory(Category category)
        {
            _categories.Add(category);
        }
        
        public static Category GetCategoryById(int id)
        {
            return _categories.FirstOrDefault(c=>c.CategoryId==id);
        }
    }
}