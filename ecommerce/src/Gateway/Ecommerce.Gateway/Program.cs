using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms((context) =>
    {
        //Preserving the original host header is important for Umbraco to generate correct URLs in the backoffice and the front-end.
        context.AddOriginalHost(true);
    });

var app = builder.Build();

app.MapReverseProxy();
app.Run();
