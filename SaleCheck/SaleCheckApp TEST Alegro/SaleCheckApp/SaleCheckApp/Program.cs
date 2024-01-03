using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using PogodaApp.Models;
using PogodaApp.Services;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();

var pack = new ConventionPack
{
    new CamelCaseElementNameConvention(),
    new StringIdStoredAsObjectIdConvention()
};
ConventionRegistry.Register("ZPP2_convention", pack, _ => true);


new DriverManager().SetUpDriver(new ChromeConfig());



MongoUrl url = new MongoUrl(builder.Configuration.GetConnectionString("MongoDB"));
MongoClientSettings settings = MongoClientSettings.FromUrl(url);
MongoClient client = new MongoClient(settings);
IMongoDatabase database = client.GetDatabase(url.DatabaseName);

IMongoCollection<ScrapedDataModel> scrapedData = database.GetCollection<ScrapedDataModel>("SaleCheckTest");

builder.Services.AddSingleton(scrapedData);
builder.Services.AddScoped<IWeatherDataService, WeatherDataService>();
builder.Services.AddScoped<Scrapper>(); // Register the Scrapper class

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using(var scope = app.Services.CreateScope())
{
    var scrapper=scope.ServiceProvider.GetRequiredService<Scrapper>();
    await scrapper.Scrap();
}

app.Run();