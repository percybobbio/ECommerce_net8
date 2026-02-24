using Microsoft.EntityFrameworkCore;
using MiTienda.Context;
using MiTienda.Repositories;
using MiTienda.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//1. Leer la cadena de conexión desde el archivo appsettings.json
var cadenaSql = builder.Configuration.GetConnectionString("SqlServerString")
    ?? throw new InvalidOperationException("Connection string 'SqlServerString' not found");

//2. Registrar el AppDbContext para que pueda ser inyectado en los controladores o servicios que lo necesiten
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(cadenaSql));

builder.Services.AddScoped(typeof(GenericoRepository<>));
builder.Services.AddScoped<CategoriaService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
