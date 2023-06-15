using Microsoft.EntityFrameworkCore;
using wmvc.Models;
namespace wmvc.Controllers;

public static class GeneratorEndpoints
{
    public static void MapGeneratorEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Generator", async (RastrwinContext db) =>
        {
            return await db.Generators.ToListAsync();
        })
        .WithName("GetAllGenerators")
        .Produces<List<Generator>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Generator/{id}", async (int Num, RastrwinContext db) =>
        {
            return await db.Generators.FindAsync(Num)
                is Generator model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetGeneratorById")
        .Produces<Generator>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Generator/{id}", async (int Num, Generator generator, RastrwinContext db) =>
        {
            var foundModel = await db.Generators.FindAsync(Num);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(generator);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateGenerator")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Generator/", async (Generator generator, RastrwinContext db) =>
        {
            db.Generators.Add(generator);
            await db.SaveChangesAsync();
            return Results.Created($"/Generators/{generator.Num}", generator);
        })
        .WithName("CreateGenerator")
        .Produces<Generator>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Generator/{id}", async (int Num, RastrwinContext db) =>
        {
            if (await db.Generators.FindAsync(Num) is Generator generator)
            {
                db.Generators.Remove(generator);
                await db.SaveChangesAsync();
                return Results.Ok(generator);
            }

            return Results.NotFound();
        })
        .WithName("DeleteGenerator")
        .Produces<Generator>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
