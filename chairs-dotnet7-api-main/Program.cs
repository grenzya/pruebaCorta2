using chairs_dotnet7_api;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");

//TODO: ASIGNACION DE RUTAS A LOS ENDPOINTS
chairs.MapPost("/", CreateChair);
chairs.MapGet("/", GetChairs);
chairs.MapGet("/{name}",GetChairByName);
chairs.MapPut("/{id}", UpdateChair);
chairs.MapPut("/{id}/stock", PutChairStock);
chairs.MapPost("/purchase", PurchaseChair);
chairs.MapDelete("/{id}", DeleteChair);

app.Run();

//TODO: ENDPOINTS SOLICITADOS

//1
static IResult CreateChair(DataContext db, Chair chair)
{
    db.Chairs.Add(chair);
    db.SaveChangesAsync();
    return TypedResults.Created($"/chair/{chair.Id}", chair);
}

//2
static IResult GetChairs(DataContext db)
{
    db.Chairs.ToListAsync();
    return TypedResults.Ok();
}

//3
static IResult GetChairByName(DataContext db)
{
    if (db.FindAsync())
    return TypedResults.NotFound();
}





//7
static IResult DeleteChair(DataContext db, Chair chair)
{
    if (db.Chairs.FindAsync(chair.Id) is chair)
    {
        db.Chairs.Remove(chair);
        db.SaveChangesAsync();
        return Results.NoContent();
    }
    return TypedResults.NotFound();
}