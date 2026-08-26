
using Microsoft.AspNetCore.HttpOverrides;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Add(System.Net.IPAddress.Parse("127.0.0.1"));
});

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

// if (builder.Environment.IsDevelopment())
// {
//     builder.Services.Configure<OpenIddictServerAspNetCoreOptions>(options =>
//     {
//         options.DisableTransportSecurityRequirement = true;
//     });
// }

WebApplication app = builder.Build();


await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
    });

await app.RunAsync();
