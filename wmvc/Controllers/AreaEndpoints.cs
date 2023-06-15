using Microsoft.EntityFrameworkCore;
using wmvc.Models;
namespace wmvc.Controllers;

public static class AreaEndpoints
{
    public static void MapAreaEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Area", async (RastrwinContext db) =>
        {
            return await db.Areas.ToListAsync();
        })
        .WithName("GetAllAreas")
        .Produces<List<Area>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Area/{id}", async (int Na, RastrwinContext db) =>
        {
            return await db.Areas.FindAsync(Na)
                is Area model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetAreaById")
        .Produces<Area>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Area/{id}", async (int Na, Area area, RastrwinContext db) =>
        {
            var foundModel = await db.Areas.FindAsync(Na);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(area);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateArea")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Area/", async (Area area, RastrwinContext db) =>
        {
            db.Areas.Add(area);
            await db.SaveChangesAsync();
            return Results.Created($"/Areas/{area.Na}", area);
        })
        .WithName("CreateArea")
        .Produces<Area>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Area/{id}", async (int Na, RastrwinContext db) =>
        {
            if (await db.Areas.FindAsync(Na) is Area area)
            {
                db.Areas.Remove(area);
                await db.SaveChangesAsync();
                return Results.Ok(area);
            }

            return Results.NotFound();
        })
        .WithName("DeleteArea")
        .Produces<Area>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
