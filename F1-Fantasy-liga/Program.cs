using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.Enums;
using F1_Fantasy_liga.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<MockDataStore>();
builder.Services.AddSingleton<CircuitMockRepository>();
builder.Services.AddSingleton<ConstructorMockRepository>();
builder.Services.AddSingleton<DriverMockRepository>();
builder.Services.AddSingleton<RaceMockRepository>();
builder.Services.AddSingleton<RaceResultMockRepository>();
builder.Services.AddSingleton<UserMockRepository>();
builder.Services.AddSingleton<FantasyLeagueMockRepository>();
builder.Services.AddSingleton<FantasyTeamMockRepository>();

var app = builder.Build();






// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
