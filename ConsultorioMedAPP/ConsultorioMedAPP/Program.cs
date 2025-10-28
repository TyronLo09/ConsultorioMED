using ConsultorioMedAPP.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar DbContext con la cadena de conexión
builder.Services.AddDbContext<ConsultorioMedDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConsultorioMedDB")));

// Agregar controladores con vistas
builder.Services.AddControllersWithViews();

// Agregar soporte para sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de sesión (30 min)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// Activar uso de sesiones
app.UseSession();

app.UseAuthorization();

// Ruta por defecto: que inicie en el Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
