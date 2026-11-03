var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<APIClient.RestaurantApiClient>(_ => new APIClient.RestaurantApiClient(builder.Configuration["ApiSettings:RestaurantUrl"]));
builder.Services.AddScoped<APIClient.CustomerApiClient>(_ =>
    new APIClient.CustomerApiClient(builder.Configuration["ApiSettings:CustomerUrl"]));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run();
