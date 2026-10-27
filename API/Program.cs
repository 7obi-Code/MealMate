using DataAccess.DAO;
using DataAccess.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Get the database connection string.
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' mangler.");

// Register FoodItemDao.
builder.Services.AddScoped<IFFoodItemDao>(
    _ => new FoodItemDao(connectionString));

// Register MenuDao and inject FoodItemDao.
builder.Services.AddScoped<IFMenuDao>(services =>
    new MenuDao(
        connectionString,
        services.GetRequiredService<IFFoodItemDao>()));

// Register RestaurantDao and inject MenuDao.
builder.Services.AddScoped<IFRestaurantDao>(services =>
    new RestaurantDao(
        connectionString,
        services.GetRequiredService<IFMenuDao>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
