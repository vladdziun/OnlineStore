using Microsoft.AspNetCore.Components;
using OnlineStore.Client.Services.Contracts;
using OnlineStore.Models.Dtos;

namespace OnlineStore.Client.Pages
{
    public class ProductsBase: ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetItems();
        }
    }
}
