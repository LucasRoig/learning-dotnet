using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms((context) =>
    {
        context.AddOriginalHost(true);
    });

var app = builder.Build();

app.MapReverseProxy();
app.Run();
