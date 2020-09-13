using Microsoft.AspNetCore.Mvc;
using SportStore.Models;
using SportStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Controllers
{
    public class HomeController:Controller
    {
        public int pageSize = 4;
        private IStoreRepository repo;
        public HomeController(IStoreRepository repo)
        {
            this.repo = repo;
        }
        public IActionResult Index(int productPage=1)
            => View(new ProductListViewModel
            {
                Products = repo.Products
                .OrderBy(p => p.ProductId)
                .Skip((productPage - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemPerPage = pageSize,
                    TotalItem = repo.Products.Count()
                }
            });
        
    }
}
