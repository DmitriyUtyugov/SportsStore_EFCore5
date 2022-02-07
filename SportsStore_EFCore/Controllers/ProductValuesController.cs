using Microsoft.AspNetCore.Mvc;
using SportsStore_EFCore.Models;

namespace SportsStore_EFCore.Controllers
{
    [Route("api/products")]
    public class ProductValuesController : Controller
    {
        private IWebServiceRepositroy repositroy;

        public ProductValuesController(IWebServiceRepositroy repositroy) => this.repositroy = repositroy;

        [HttpGet("{id}")]
        public object GetProduct(long id) => repositroy.GetProduct(id) ?? NotFound();

        [HttpGet]
        public object Products(int skip, int take) => repositroy.GetProducts(skip, take);

        [HttpPost]
        public long StoreProduct([FromBody] Product product) => repositroy.StoreProduct(product);

        [HttpPut]
        public void UpdateProduct([FromBody] Product product) => repositroy.UpdateProduct(product);

        [HttpDelete("{id}")]
        public void DeleteProduct(long id) => repositroy.DeleteProduct(id);
    }
}
