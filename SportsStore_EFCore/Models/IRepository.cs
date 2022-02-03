using SportsStore_EFCore.Models.Pages;
using System.Collections.Generic;

namespace SportsStore_EFCore.Models
{
    public interface IRepository
    {
        IEnumerable<Product> Products { get; }
        PagedList<Product> GetProducts(QueryOptions options, long category = 0);
        //PagedList<Product> GetProducts(QueryOptions options);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        Product GetProduct(long key);
        void UpdateAll(Product[] products);
        void Delete(Product product);
    }
}
