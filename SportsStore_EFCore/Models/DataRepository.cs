using System.Collections.Generic;
using System.Linq;

namespace SportsStore_EFCore.Models
{
    public class DataRepository : IRepository
    {
        private DataContext context;

        public DataRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<Product> Products => context.Products.ToArray();

        public void AddProduct(Product product)
        {
            this.context.Products.Add(product);
            this.context.SaveChanges();
        }
    }
}
