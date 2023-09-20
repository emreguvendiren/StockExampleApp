using StockExampleApp.Entity;

namespace StockExampleApp.Data.Abstract
{
    public interface IProductRepository
    {
        void GetAllProducts();
        void AddProduct(Product product);
    }
}
