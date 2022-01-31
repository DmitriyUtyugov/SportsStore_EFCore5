using Microsoft.AspNetCore.Mvc;
using SportsStore_EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore_EFCore.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;

        public HomeController(IRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index() => View(repository.Products);

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            repository.AddProduct(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
