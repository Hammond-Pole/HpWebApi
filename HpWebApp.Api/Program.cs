using Data.DbContexts;
using Data.Models;
using HpWebApp.Api.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICreateAccessToken, CreateAccessToken>();

builder.Services.AddDbContext<DbSystemContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/users", async (DbSystemContext db) =>
{
    var users = await db.Users.ToListAsync();
    return Results.Ok(users);
})
.WithName("GetUsers")
.WithOpenApi();

app.MapPost("/users", async (User user, DbSystemContext db) =>
{
    var u = db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
})
    .WithName("CreateUser")
    .WithOpenApi();

app.MapPost("/Login", async (User user, DbSystemContext _db, ICreateAccessToken _token) =>
{
    var u = _db.Users.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();

    if (u == null)
    {
        return Results.NotFound();
    }
    else
    {
        // Generate Token
        var token = _token.CreateToken(u);
        return Results.Ok(token);
    }
})
    .WithName("Login")
    .WithOpenApi();

app.Run();
