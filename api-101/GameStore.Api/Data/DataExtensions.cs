using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    extension(WebApplication app)
    {
        public void MigrateDb()
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            dbContext.Database.Migrate();
        }

        public void AddGameStoreDb()
        {

        }
    }

    extension(WebApplicationBuilder builder)
    {
        public void AddGameStoreDb()
        {
            var connString = builder.Configuration.GetConnectionString("GameStore");
            builder.Services.AddSqlite<GameStoreContext>(
                connString,
                optionsAction: options => options.UseSeeding((context, _) =>
                {
                    if (!context.Set<Genre>().Any())
                    {
                        context.Set<Genre>().AddRange(
                            new Genre { Name = "Fighting" },
                            new Genre { Name = "RPG" },
                            new Genre { Name = "Platformer" },
                            new Genre { Name = "Racing" },
                            new Genre { Name = "Sports" }
                        );
                        context.SaveChanges();
                    }
                })
            );
        }
    }
}