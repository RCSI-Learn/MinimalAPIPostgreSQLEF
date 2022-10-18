using Microsoft.EntityFrameworkCore;
using MinimalAPIPostgreSQLEF.Data;
using MinimalAPIPostgreSQLEF.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var ConnectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<OfficeDB>(options => options.UseNpgsql(ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () => {
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//       new WeatherForecast
//       (
//           DateTime.Now.AddDays(index),
//           Random.Shared.Next(-20, 55),
//           summaries[Random.Shared.Next(summaries.Length)]
//       ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");
app.MapPost("/employees/", async (Employee e, OfficeDB db) => {
    db.Employees.Add(e);
    await db.SaveChangesAsync();
    return Results.Created($"/employee/{e.Id}", e);
});

app.MapGet("/employees/{id:long}", async (long id, OfficeDB db) => {
    return await db.Employees.FindAsync(id) is Employee e ? Results.Ok(e) : Results.NotFound();
});

app.MapGet("/employees", async (OfficeDB db) => await db.Employees.ToListAsync());

app.MapPut("/employees/{id:long}", async (long id, Employee e, OfficeDB db) => {
    if (e.Id != id) return Results.BadRequest();
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();
    employee.Name = e.Name;
    employee.LastName = e.LastName;
    employee.Branch = e.Branch;
    employee.Age = e.Age;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/employees/{id:long}", async (long id, OfficeDB db) => {
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();
    db.Employees.Remove(employee);
    await db.SaveChangesAsync();
    return Results.Ok(employee);
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary) {
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}