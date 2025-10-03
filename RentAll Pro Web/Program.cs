using Microsoft.EntityFrameworkCore;
using RentAll_Pro_Web.Data; // <-- Itt a jav�t�s a r�gi 'ToolRental' helyett

var builder = WebApplication.CreateBuilder(args);

// Adatb�zis kapcsolat be�ll�t�sa
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=RentAllPro.db";
builder.Services.AddDbContext<ApplicationDbContext>(options => // <-- Itt a jav�t�s
    options.UseSqlite(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();