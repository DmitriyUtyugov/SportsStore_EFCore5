using Microsoft.AspNetCore.Mvc;
using SportsStore_EFCore.Models;
using SportsStore_EFCore.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore_EFCore.Controllers
{
    public class StoreController : Controller
    {
        private IRepository productRepository;
        private ICategoryRepository categoryRepository;

        public StoreController(IRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index([FromQuery(Name ="options")]
            QueryOptions productOptions,
            QueryOptions categoryOptions,
            long category)
        {
            ViewBag.Categories = categoryRepository.GetCategories(categoryOptions);
            ViewBag.SelectedCategory = category;
            return View(productRepository.GetProducts(productOptions, category));
        }
    }
}
