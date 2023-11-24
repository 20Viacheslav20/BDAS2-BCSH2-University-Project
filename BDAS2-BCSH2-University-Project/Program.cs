using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using BDAS2_BCSH2_University_Project.Repositories;
using Oracle.ManagedDataAccess.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMainRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IMainRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IMainRepository<Shop>, ShopRepository>();
builder.Services.AddScoped<IMainRepository<Position>, PositionRepository>();
builder.Services.AddScoped<IMainRepository<Employer>, EmployerRepository>();

builder.Services.AddScoped<OracleConnection>(provider =>
{
    return new OracleConnection(builder.Configuration.GetConnectionString("DatabaseConnection"));
});


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
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
