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

        public Product GetProduct(long key) => context.Products.Find(key);

        public void UpdateAll(Product[] products)
        {
            Dictionary<long, Product> data = products.ToDictionary(p => p.Id);
            IEnumerable<Product> baseline = context.Products.Where(p => data.Keys.Contains(p.Id));

            foreach (Product databaseProduct in baseline)
            {
                Product requestproduct = data[databaseProduct.Id];
                databaseProduct.Name = requestproduct.Name;
                databaseProduct.Category = requestproduct.Category;
                databaseProduct.PurchasePrice = requestproduct.PurchasePrice;
                databaseProduct.RetailPrice = requestproduct.RetailPrice;
            }
            context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            Product p = GetProduct(product.Id);
            p.Name = product.Name;
            p.Category = product.Category;
            p.PurchasePrice = product.PurchasePrice;
            p.RetailPrice = product.RetailPrice;
            context.SaveChanges();
        }
    }
}
