using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Components
{
    public class NavigationMenuViewComponent:ViewComponent
    {
        private IStoreRepository repo;

        public NavigationMenuViewComponent(IStoreRepository repo)
        {
            this.repo = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repo.Products.Select(s => s.Category)
                .Distinct().OrderBy(o => o));
        }
    }
}
