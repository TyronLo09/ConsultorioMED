using ConsultorioMedAPP.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Registrar DbContext con la cadena de conexi�n
builder.Services.AddDbContext<ConsultorioMedDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConsultorioMedDB")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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
