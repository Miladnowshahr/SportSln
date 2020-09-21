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
        public ViewResult Index(string category,int productPage=1)
        {
            var model = new ProductListViewModel
            {
                Products = repo.Products.Where(p => category == null || p.Category == category)
                            .OrderBy(p => p.ProductId)
                            .Skip((productPage - 1) * pageSize)
                            .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemPerPage = pageSize,
                    TotalItem = category == null ?repo.Products.Count() : repo.Products.Where(e =>e.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
          
    }
}
