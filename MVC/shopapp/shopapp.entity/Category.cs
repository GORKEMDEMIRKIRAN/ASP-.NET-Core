
using System.Collections.Generic;

namespace shopapp.entity
{
    public class Category
    {
        public int CategoryId {get;set;}
        public string Name {get;set;}

        public string Description {get;set;}
        public List<ProductCategory> ProductCategory {get;set;}
        
    }
}