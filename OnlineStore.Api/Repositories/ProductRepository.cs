using Microsoft.EntityFrameworkCore;
using OnlineStore.Api.Data;
using OnlineStore.Api.Entities;
using OnlineStore.Api.Repositories.Contracts;
using OnlineStore.Models.Dtos;

namespace OnlineStore.Api.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly OnlineStoreDbContext _onlineStoreDbContext;

        public ProductRepository(OnlineStoreDbContext OnlineStoreDbContext)
        {
            _onlineStoreDbContext = OnlineStoreDbContext;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await _onlineStoreDbContext.ProductCategories.ToListAsync();

            return categories;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await _onlineStoreDbContext.ProductCategories.FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _onlineStoreDbContext.Products
                                .FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _onlineStoreDbContext.Products.ToListAsync();

            return products;
        }


        public async Task<Product> CreateProduct(ProductDto productDto)
        {
            // needs to be migrated to a service!
            if (productDto != null)
            {
                var product = new Product()
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    ImageURL = productDto.ImageURL,
                    Price = productDto.Price,
                    Qty = productDto.Qty,
                    CategoryId = productDto.CategoryId 
                };
                await _onlineStoreDbContext.Products.AddAsync(product);
                await _onlineStoreDbContext.SaveChangesAsync();
                return product;
            }

            throw new Exception("Error creating product");
        }

        public async Task<Product> UpdateProduct(ProductDto product)
        {
            var productToUpdate = await _onlineStoreDbContext.Products
                    .FirstOrDefaultAsync(p => p.Id == product.Id);

            // needs to be migrated to a service!
            if (productToUpdate != null) 
            {
                productToUpdate.Name = product.Name;
                productToUpdate.Description = product.Description;
                productToUpdate.ImageURL  = product.ImageURL;
                productToUpdate.Price = product.Price;
                productToUpdate.Qty = product.Qty;
            }
            await _onlineStoreDbContext.SaveChangesAsync();

            return productToUpdate;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _onlineStoreDbContext.Products
                    .FirstOrDefaultAsync(p => p.Id == id);

            _onlineStoreDbContext.Remove(product);
            await _onlineStoreDbContext.SaveChangesAsync();
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
