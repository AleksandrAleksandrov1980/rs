using Microsoft.EntityFrameworkCore;
using wmvc.Models;
namespace wmvc.Controllers;

public static class NodeEndpoints
{
    public static void MapNodeEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Node", async (RastrwinContext db) =>
        {
            return await db.Nodes.ToListAsync();
        })
        .WithName("GetAllNodes")
        .Produces<List<Node>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Node/{id}", async (int Ny, RastrwinContext db) =>
        {
            return await db.Nodes.FindAsync(Ny)
                is Node model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetNodeById")
        .Produces<Node>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Node/{id}", async (int Ny, Node node, RastrwinContext db) =>
        {
            var foundModel = await db.Nodes.FindAsync(Ny);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(node);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateNode")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Node/", async (Node node, RastrwinContext db) =>
        {
            db.Nodes.Add(node);
            await db.SaveChangesAsync();
            return Results.Created($"/Nodes/{node.Ny}", node);
        })
        .WithName("CreateNode")
        .Produces<Node>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Node/{id}", async (int Ny, RastrwinContext db) =>
        {
            if (await db.Nodes.FindAsync(Ny) is Node node)
            {
                db.Nodes.Remove(node);
                await db.SaveChangesAsync();
                return Results.Ok(node);
            }

            return Results.NotFound();
        })
        .WithName("DeleteNode")
        .Produces<Node>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
