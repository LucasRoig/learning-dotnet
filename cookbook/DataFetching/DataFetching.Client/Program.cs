using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DataFetching.Client.Services;
using DataFetching.Shared.Features.Products;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
});
builder.Services.AddScoped<IProductService, ClientProductService>();

await builder.Build().RunAsync();
