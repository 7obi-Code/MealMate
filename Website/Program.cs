using LobbyLink.Website.OpenIDConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Configure for OpenID
OpenIDConnectUtils oidcConfig = new();
oidcConfig.ConfigureBuild(builder);

builder.Services.AddScoped<APIClient.RestaurantApiClient>(
    _ => new APIClient.RestaurantApiClient(
        builder.Configuration["ApiSettings:RestaurantUrl"]));




var app = builder.Build();



// Configure for OpenID
oidcConfig.ConfigureApp(app);

app.Run();