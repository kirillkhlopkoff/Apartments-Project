using ApiApartment.Context;
using ApiApartment.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed data to database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    // Add test data
    if (!dbContext.Apartments.Any())
    {
        dbContext.Apartments.AddRange(new List<Apartment>
        {
            new Apartment
            {
                Address = "123 Main St",
                Price = 100000,
                Rooms = 2,
                Description = "Beautiful apartment with a view"
            },
            new Apartment
            {
                Address = "456 Elm St",
                Price = 200000,
                Rooms = 3,
                Description = "Large apartment in a great location"
            }
        });

        dbContext.SaveChanges();
    }
}

app.Run();