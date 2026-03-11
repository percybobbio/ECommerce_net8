using Microsoft.AspNetCore.Authentication.Cookies;
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
builder.Services.AddScoped<ClienteService>();

//Activar memoria temporal para almacenar el carrito de compras
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
    options.Cookie.HttpOnly = true; // Evitar acceso al cookie desde JavaScript
    options.Cookie.IsEssential = true; // Asegurar que el cookie se envíe incluso si el usuario no acepta cookies
});

// Configurar la autenticación basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login"; // Ruta a la página de inicio de sesión
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Duración de la cookie de autenticación
        options.LogoutPath = "/Cuenta/Logout"; // Ruta a la página de cierre de sesión
        options.AccessDeniedPath = "/Home/Error"; // Ruta a la página de acceso denegado
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

app.UseAuthentication(); // Habilitar la autenticación

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
