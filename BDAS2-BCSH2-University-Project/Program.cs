using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using BDAS2_BCSH2_University_Project.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Oracle.ManagedDataAccess.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMainRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IMainRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IMainRepository<Shop>, ShopRepository>();
builder.Services.AddScoped<IMainRepository<Position>, PositionRepository>();
builder.Services.AddScoped<IAuthenticateRepository, AuthenticateRepository>();
builder.Services.AddScoped<IMainRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped<IMainRepository<Address>, AddressRepository>();

builder.Services.AddScoped<OracleConnection>(provider =>
{
    return new OracleConnection(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Authenticate/Login";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    // TODO add after creating roles in db
    //options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
    //options.AddPolicy("ShiftLeader", policy => policy.RequireRole("ShiftLeader"));
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");



app.Run();
