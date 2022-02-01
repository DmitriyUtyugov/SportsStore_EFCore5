using System.Collections.Generic;

namespace SportsStore_EFCore.Models
{
    public interface IRepository
    {
        IEnumerable<Product> Products { get; }
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        Product GetProduct(long key);
        void UpdateAll(Product[] products);
    }
}
