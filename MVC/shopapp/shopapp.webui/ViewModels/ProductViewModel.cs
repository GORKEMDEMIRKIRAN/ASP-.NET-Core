
using System.Collections.Generic;
// tanımladığımız model içinde bulunan
// category ve product dosyaları geliyor.
using shopapp.webui.Models;

public class ProductViewModel
{
    // public List<Category> Categories {get; set;}
    public Category Cty {get; set;}
    public List<Product> Pds {get; set;}

}

