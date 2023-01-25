using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineStore.Client;
using OnlineStore.Client.Services;
using OnlineStore.Client.Services.Contracts;
using Microsoft.AspNetCore.Session;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//builder.Services./*AddSession();*/
builder.Services.AddDistributedMemoryCache();

// point to web api
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7223/") });

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICacheService, CacheService>();


await builder.Build().RunAsync();
