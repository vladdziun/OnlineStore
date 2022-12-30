using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OnlineStore.Client.Services;
using OnlineStore.Client.Services.Contracts;
using OnlineStore.Models.Dtos;

namespace OnlineStore.Client.Pages
{
    public class ShoppingCartBase: ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CalculateTotals();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);

            if (cartItemDto != null) 
            {
                // bad perfomance to reload all items, can store all items in the list and remove
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CalculateTotals();
            }
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };

                    var returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateItemDto);
                    UpdateItemTotalPrice(returnedUpdateItemDto);
                    CalculateTotals();
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id); 
                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CalculateTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(i => i.TotalPrice).ToString();
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(i => i.Qty);
        }

        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = ShoppingCartItems.FirstOrDefault(i => i.Id == cartItemDto.Id);

            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }

    }
}
