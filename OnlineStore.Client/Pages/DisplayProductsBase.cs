using Microsoft.AspNetCore.Components;
using OnlineStore.Models.Dtos;

namespace OnlineStore.Client.Pages
{
    public class DisplayProductsBase: ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
