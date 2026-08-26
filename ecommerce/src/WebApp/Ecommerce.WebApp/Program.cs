using Ecommerce.WebApp.Components;
using Ecommerce.WebApp.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Add(System.Net.IPAddress.Parse("127.0.0.1"));
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

builder.Services.AddHttpClient<CategoryTreeClient>(client =>
    client.BaseAddress = new Uri(builder.Configuration["Cms:BaseUrl"]
        ?? throw new InvalidOperationException("Missing configuration value 'Cms:BaseUrl'.")));
builder.Services.Configure<CategoryTreeOptions>(builder.Configuration.GetSection("Cms"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Ecommerce.WebApp.Client._Imports).Assembly);

app.Run();
