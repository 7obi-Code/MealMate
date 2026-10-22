
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<DataAccess.DAO.RestaurantDao>();

builder.Services.AddScoped<APIClient.RestaurantApiClient>(sp =>
    new APIClient.RestaurantApiClient(
        builder.Configuration["ApiSettings:RestaurantUrl"]
        ?? throw new InvalidOperationException(
            "ApiSettings:RestaurantUrl is not configured.")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
