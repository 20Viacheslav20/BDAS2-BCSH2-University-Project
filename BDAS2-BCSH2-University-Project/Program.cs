using Microsoft.AspNetCore.Authentication.Cookies;
using Models.Models;
using Models.Models.Login;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using Repositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Initialization of repositories using dependency injection
builder.Services.AddScoped<IAuthorizationUserRepository, AuthorizationUserRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IMainRepository<Position>, PositionRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ILogsRepository, LogRepository>();
builder.Services.AddScoped<IStorageRepository, StorageRepository>();
builder.Services.AddScoped<ICashDeskRepository, CashDeskRepository>();
builder.Services.AddScoped<IStandRepository, StandRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISystemCatalogRepository, SystemCatalogRepository>();
builder.Services.AddScoped<ISoldProductRepository, SoldProductRepository>();

// Initialization of connection for db
builder.Services.AddScoped<OracleConnection>(provider =>
{
    return new OracleConnection(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

// Adding the authentication service to the application's dependency injection container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    // Setting the path to which the user will be redirected for login.
    options.LoginPath = "/AuthorizationUser/Login";

    // Setting the path to which the user will be redirected if access is denied.
    options.AccessDeniedPath = "/";
});

// Add authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(UserRole.Admin.ToStringValue(), policy => policy.RequireRole(UserRole.Admin.ToStringValue()));
    options.AddPolicy(UserRole.Employee.ToStringValue(), policy => policy.RequireRole(UserRole.Employee.ToStringValue()));
    options.AddPolicy(UserRole.ShiftLeader.ToStringValue(), policy => policy.RequireRole(UserRole.ShiftLeader.ToStringValue()));
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
