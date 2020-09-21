using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Models
{
    public interface IStoreRepository
    {
        //int TotalCount();

        int TotalCount { get; }

        IQueryable<Product> Products { get; }

        IQueryable<Product> GetProducts(string category, int productPage, int pageSize);
    }
}
