using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContosoPizza.Data;
using ContosoPizza.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Conditional logic for adding DbContext depending on environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<MovieContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("MovieContext") 
            ?? throw new InvalidOperationException("Connection string 'MovieContext' not found.")));
}
else
{
    builder.Services.AddDbContext<MovieContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionMovieContext") 
            ?? throw new InvalidOperationException("Connection string 'ProductionMovieContext' not found.")));
}

// Add other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// builder.Services.AddControllersWithViews();

var app = builder.Build();

// seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseStaticFiles();

// app.UseRouting();

app.UseAuthorization();
app.MapControllers();

app.Run();
