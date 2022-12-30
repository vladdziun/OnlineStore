using OnlineStore.Api.Entities;
using OnlineStore.Api.Data;
using OnlineStore.Api.Repositories.Contracts;
using OnlineStore.Models.Dtos;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly OnlineStoreDbContext _onlineStoreDbContext;

        public ShoppingCartRepository(OnlineStoreDbContext onlineStoreDbContext)
        {
            _onlineStoreDbContext = onlineStoreDbContext;
        }

        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            var isCartItemExists = await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId);
            if (!isCartItemExists) 
            {
                var item = _onlineStoreDbContext.Products.Where(p => p.Id == cartItemToAddDto.ProductId).Select(c => new CartItem
                {
                    CartId = cartItemToAddDto.CartId,
                    ProductId = cartItemToAddDto.ProductId,
                    Qty = cartItemToAddDto.Qty,
                }).FirstOrDefault();

                if (item != null)
                {
                    var result = await _onlineStoreDbContext.CartItems.AddAsync(item);
                    await _onlineStoreDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await _onlineStoreDbContext.CartItems.AnyAsync(c => c.CartId == cartId 
                && c.ProductId == productId);
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await _onlineStoreDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                _onlineStoreDbContext.CartItems.Remove(item);
                await _onlineStoreDbContext.SaveChangesAsync();
            }

            return item;
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in _onlineStoreDbContext.Carts
                          join cartItem in _onlineStoreDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await(from cart in _onlineStoreDbContext.Carts
                         join cartItem in _onlineStoreDbContext.CartItems
                         on cart.Id equals cartItem.CartId
                         where cart.UserId == userId
                         select new CartItem
                         {
                             Id = cartItem.Id,
                             ProductId = cartItem.ProductId,
                             Qty = cartItem.Qty,
                             CartId = cartItem.CartId
                         }).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await _onlineStoreDbContext.CartItems.FirstOrDefaultAsync(item => item.Id == id);

            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await _onlineStoreDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}
