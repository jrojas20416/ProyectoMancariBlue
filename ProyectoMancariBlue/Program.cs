using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProyectoMancariBlue.Models;
using ProyectoMancariBlue.Models.Clases;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
    });

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Empleado/Index";
        option.AccessDeniedPath = "/Empleado/AccessDenied"; // Cambia la ruta para el acceso denegado por rol
        option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddDbContext<MancariBlueContext>(opciones =>
opciones.UseSqlServer(builder.Configuration.GetConnectionString("MancariBlueContext")));

builder.Services.AddScoped<IEmpleadoModel, EmpleadoModel>();
builder.Services.AddScoped<IAnimalModel, AnimalModel>();
builder.Services.AddScoped<IEmailService, EmailModel>();
builder.Services.AddScoped<IRolModel, RolModel>();
builder.Services.AddScoped<ICategoriaProveedor, CategoriaProveedorModel>();
builder.Services.AddScoped<IProveedor, ProveedorModel>();
builder.Services.AddScoped<IProducto, ProductoModel>();
builder.Services.AddScoped<ICategoriaProducto, CategoriaProductoModel>();
builder.Services.AddHttpContextAccessor();

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
    pattern: "{controller=Empleado}/{action=Index}/{id?}");

app.Run();
