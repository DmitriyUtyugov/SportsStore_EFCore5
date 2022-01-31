using System.Collections.Generic;

namespace SportsStore_EFCore.Models
{
    public class DataRepository : IRepository
    {
        private List<Product> data = new List<Product>();

        public IEnumerable<Product> Products => data;

        public void AddProduct(Product product)
        {
            this.data.Add(product);
        }
    }
}
