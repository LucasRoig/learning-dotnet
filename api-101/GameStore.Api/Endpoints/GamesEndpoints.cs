using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    extension(WebApplication app)
    {
        public void MapGamesEndpoints()
        {
            var group = app.MapGroup("/games");
            group.MapGet("/", async (GameStoreContext dbContext) =>
            {
                return await dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => new GameSummaryDto(
                        game.Id,
                        game.Name,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate
                    ))
                    .AsNoTracking()
                    .ToListAsync();
            });

            group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);
                return game is not null ? Results.Ok(new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )) : Results.NotFound();
            }).WithName(GetGameEndpointName);

            group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                Game game = new()
                {
                    Name = newGame.Name,
                    GenreId = newGame.GenreId,
                    Price = newGame.Price,
                    ReleaseDate = newGame.ReleaseDate,
                };

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                GameDetailsDto gameDetailsDto = new(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                );

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDetailsDto.Id }, gameDetailsDto);
            });

            group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);
                if (game is null)
                {
                    return Results.NotFound();
                }
                game.Name = updatedGame.Name;
                game.GenreId = updatedGame.GenreId;
                game.Price = updatedGame.Price;
                game.ReleaseDate = updatedGame.ReleaseDate;
                
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            });

            group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            });
        }
    }

}