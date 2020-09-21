using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Models
{
    public class EFStroreRepository : IStoreRepository
    {
        private StoreDbContext context;
        
        public EFStroreRepository(StoreDbContext context)
        {
            this.context = context;
        }

        public int TotalCount;

        public IQueryable<Product> Products => context.Products;

        int IStoreRepository.TotalCount => TotalCount;

        public IQueryable<Product> GetProducts(string category, int productPage, int PageSize) {

            var model = Products.Where(w => category == null || w.Category == category);
            TotalCount = model.Count();
            model=model.OrderBy(o => o.ProductId).Skip((productPage - 1) * PageSize).Take(PageSize);
            return model;
        }

    }
}
