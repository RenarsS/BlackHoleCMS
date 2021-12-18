using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlackHoleCMS.Data;
using BlackHoleCMS.Models;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlackHoleCmsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlackHoleCMSContext")));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();

builder.Services.AddAuthentication("UserAuth")
    .AddCookie("UserAuth",options =>
    {
        options.LoginPath = "/User/Login";
    });

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapBlazorHub();
});
app.Run();

//Server=RENARS\\SUSEJSSQL; Database=black_hole; Trusted_Connection=true; MultipleActiveResultSets=true; User=Renars; Password=qwerty123456