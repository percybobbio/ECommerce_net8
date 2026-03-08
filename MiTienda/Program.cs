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
builder.Services.AddScoped<OrdenRepository>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<MarcaService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddHttpContextAccessor(); // Necesario para acceder al contexto HTTP en el CarritoService
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<OrdenService>();
builder.Services.AddScoped<DireccionService>();

//Activar memoria temporal para almacenar el carrito de compras
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
    options.Cookie.HttpOnly = true; // Evitar acceso al cookie desde JavaScript
    options.Cookie.IsEssential = true; // Asegurar que el cookie se envíe incluso si el usuario no acepta cookies
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession(); // Habilitar el uso de sesiones

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
