
using LobbyLink.Website.OpenIDConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Configure for OpenID
OpenIDConnectUtils oidcConfig = new();
oidcConfig.ConfigureBuild(builder);

// Register RestaurantApiClient
builder.Services.AddScoped<APIClient.RestaurantApiClient>(
    _ => new APIClient.RestaurantApiClient(
        builder.Configuration["ApiSettings:RestaurantUrl"]));

// Register CustomerApiClient
builder.Services.AddScoped<APIClient.CustomerApiClient>(
    _ => new APIClient.CustomerApiClient(
        builder.Configuration["ApiSettings:CustomerUrl"]));

var app = builder.Build();

// Configure for OpenID
oidcConfig.ConfigureApp(app);

app.Run();
