using Microsoft.EntityFrameworkCore;
using OnlineStore.Api.Data;
using OnlineStore.Api.Entities;
using OnlineStore.Api.Repositories.Contracts;

namespace OnlineStore.Api.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly OnlineStoreDbContext _OnlineStoreDbContext;

        public ProductRepository(OnlineStoreDbContext OnlineStoreDbContext)
        {
            _OnlineStoreDbContext = OnlineStoreDbContext;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await _OnlineStoreDbContext.ProductCategories.ToListAsync();

            return categories;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await _OnlineStoreDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);

            return category;
        }

        public async Task<Product> GetItem(int id)
        {
            //var product = await _OnlineStoreDbContext.Products
            //                    .Include(p => p.ProductCategory)
            //                    .SingleOrDefaultAsync(p => p.Id == id);
            //return product;
            throw new NotImplementedException();

        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await _OnlineStoreDbContext.Products.ToListAsync();

            return products;

        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            //var products = await _OnlineStoreDbContext.Products
            //                         .Include(p => p.ProductCategory)
            //                         .Where(p => p.CategoryId == id).ToListAsync();
            //return products;
            throw new NotImplementedException();
        }
    }
}
