using ConsultorioMedAPP.Filters;
using ConsultorioMedAPP.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var conexionTyron = builder.Configuration.GetConnectionString("conexionTyron");
var conexionKendall = builder.Configuration.GetConnectionString("conexionKendall");
builder.Services.AddControllersWithViews();


string connectionString;

if (!string.IsNullOrWhiteSpace(conexionTyron) && ProbarConexion(conexionTyron))
{
    connectionString = conexionTyron;
    Console.WriteLine("Conectado a TYRON");
}
else if (!string.IsNullOrWhiteSpace(conexionKendall) && ProbarConexion(conexionKendall))
{
    connectionString = conexionKendall;
    Console.WriteLine("Conectado a KENDALL");
}
else
{
    Console.WriteLine("No se pudo conectar ni a TYRON ni a KENDALL. Verifica los servidores SQL.");
    throw new Exception("No hay servidores SQL disponibles");
}

builder.Services.AddDbContext<ConsultorioMedDBContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure()));

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutorizacionFilter());
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error/500");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");


app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

static bool ProbarConexion(string connectionString)
{
    if (string.IsNullOrWhiteSpace(connectionString))
        return false;

    try
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        Console.WriteLine($"Prueba de conexión exitosa con: {connectionString}");
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al probar conexión: {ex.Message}");
        return false;
    }
}