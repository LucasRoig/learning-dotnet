using DataFetchingVerticalSlices.Client.Features.Products;
using DataFetchingVerticalSlices.Shared.Features.Products;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
builder.Services.AddScoped<IGetAllProducts.IHandler, GetAllProductsClient>();

await builder.Build().RunAsync();
