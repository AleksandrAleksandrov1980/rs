using Microsoft.EntityFrameworkCore;
using wmvc.Models;
namespace wmvc.Controllers;

public static class VetvEndpoints
{
    public static void MapVetvEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Vetv", async (RastrwinContext db) =>
        {
            return await db.Vetvs.ToListAsync();
        })
        .WithName("GetAllVetvs")
        .Produces<List<Vetv>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Vetv/{id}", async (int Ip, RastrwinContext db) =>
        {
            return await db.Vetvs.FindAsync(Ip)
                is Vetv model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetVetvById")
        .Produces<Vetv>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Vetv/{id}", async (int Ip, Vetv vetv, RastrwinContext db) =>
        {
            var foundModel = await db.Vetvs.FindAsync(Ip);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(vetv);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateVetv")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Vetv/", async (Vetv vetv, RastrwinContext db) =>
        {
            db.Vetvs.Add(vetv);
            await db.SaveChangesAsync();
            return Results.Created($"/Vetvs/{vetv.Ip}", vetv);
        })
        .WithName("CreateVetv")
        .Produces<Vetv>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Vetv/{id}", async (int Ip, RastrwinContext db) =>
        {
            if (await db.Vetvs.FindAsync(Ip) is Vetv vetv)
            {
                db.Vetvs.Remove(vetv);
                await db.SaveChangesAsync();
                return Results.Ok(vetv);
            }

            return Results.NotFound();
        })
        .WithName("DeleteVetv")
        .Produces<Vetv>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
