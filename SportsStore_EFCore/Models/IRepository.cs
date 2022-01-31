using System.Collections.Generic;

namespace SportsStore_EFCore.Models
{
    public interface IRepository
    {
        IEnumerable<Product> Products { get; }
        void AddProduct(Product product);
    }
}
