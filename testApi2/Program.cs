using Microsoft.EntityFrameworkCore;
using testApi2.Models;

var builder = WebApplication.CreateBuilder();
string connection = @"Data Source=MSI\EUGEN;Initial Catalog=testApi;Integrated Security=True";
builder.Services.AddDbContext<testModel>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/", async (testModel db) => await db.Table1.ToListAsync());

//app.MapGet("/table1/{id}", async(int id, testModel db) => 
//{ 
//    Table1? table = await db.Table1.FirstOrDefaultAsync(i => i.Id == id);
//    return Results.Json(table);
//});

//получение информации о таблице по id
app.MapGet("/getStationInfo", async (HttpRequest request, testModel db) =>
{
    var id = int.Parse(request.Query["id"]);
    Table1? table = await db.Table1.FirstOrDefaultAsync(i => i.Id == id);
    return Results.Json(table);
});

// post с данными из ссылки
app.MapPost("/table1", async (HttpRequest request, testModel db) =>
{
    var naem = request.Query["name"];
    var age = int.Parse(request.Query["age"]);

    Table1 user = new Table1
    {
        Name = naem,
        Age = age
    };

    await db.Table1.AddAsync(user);
    await db.SaveChangesAsync();
    return user;
});

//post через JSON
app.MapPost("/setStation", async (Table1 user, testModel db) =>
{
    await db.Table1.AddAsync(user);
    await db.SaveChangesAsync();
    return user;
});

app.Run("http://localhost:3000");
