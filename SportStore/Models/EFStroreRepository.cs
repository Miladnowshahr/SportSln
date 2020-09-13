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
        public IQueryable<Product> Products => context.Products;
    }
}
