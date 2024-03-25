using Microsoft.EntityFrameworkCore;
using TestWebApplication.Context;
using TestWebApplication.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddTransient<JSONParsingService>();
builder.Services.AddTransient<DatabaseService>();
builder.Services.AddTransient<TextConfigurationParsingService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Configuration}/{action=Index}/{id?}");

app.Run();
