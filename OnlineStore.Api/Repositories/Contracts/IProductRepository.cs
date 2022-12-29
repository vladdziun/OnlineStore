using OnlineStore.Api.Entities;
using OnlineStore.Models.Dtos;

namespace OnlineStore.Api.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<Product> GetProduct(int id);
        Task<Product> CreateProduct(ProductDto productDto);
        Task<Product> UpdateProduct(ProductDto product);
        Task DeleteProduct(int id);
        Task<ProductCategory> GetCategory(int id);

        Task<IEnumerable<Product>> GetItemsByCategory(int id);
    }
}
