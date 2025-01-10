

// GENERIC BİR SINIF OLUŞTURMA


/*
bu sınıf sayesinde aynı göreve yapacak metodları burada tanımlayarak benzer iş
yapacak alanlarda birden fazla tanımlama yapmamızı engelliyecektir.

EfCoreCategoryRepository içinde dbcontext tanımlayarak tanımaladığımız
değişken üzerinden database bağlanış işlem yapıyoruz.
Bu yapıyı da göz önüne alarak bir  generic sınıf oluşturalım.
*/
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;

// BURADA CONCRETE YAPILARINI OLUŞTURMUŞ OLDUK.
namespace shopapp.data.Concrete.EfCore
{
    //TEntity : category,product hangi entity bağlanıcaksak o gelicek
    //TContext: Context database bağlantı context gelicektir.
    // where sorguları ile gelecek değişken bağlantılarını kısıtlıyoruz ve ne geleceğine emin oluyoruz.
    public class EfCoreGenericRepository<TEntity,TContext>:IRepository<TEntity>
        where TEntity:class
        where TContext:DbContext,new()  
    {
        // id göre ürünü getirir.
        public TEntity GetById(int id)
        {
            using(var context = new TContext())
            {
               return context.Set<TEntity>().Find(id);
            }
        }
        // entity listeler
        public List<TEntity> GetAll()
        {
            using(var context = new TContext())
            {
                return context.Set<TEntity>().ToList();
            }
        }
        // verilen verileri aktarır.
        public void Create(TEntity entity)
        {
            using(var context = new TContext())
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            }
        }
        // güncelleme işlemi yapar.
        public void Update(TEntity entity)
        {
            using(var context = new TContext())
            {
                context.Entry(entity).State=EntityState.Modified;
                context.SaveChanges();
            }
        }
        // silme işlemi yapar.
        public void Delete(TEntity entity)
        {
            using(var context = new TContext())
            {
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}