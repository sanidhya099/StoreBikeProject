using eBikeManagmentWebApp.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SalesAndReturnSystem.BLL;
using SalesAndReturnSystem.DAL;
using ReceivingSystem.DAL;
using ServicingSystem.DAL;
using ReceivingSystem.BLL;
using ServicingSystem.BLL;
using SalesAndReturnSystem;
using ReceivingSystem;
using ServicingSystem;

var builder = WebApplication.CreateBuilder(args);


var eBikes = builder.Configuration.GetConnectionString("eBike");

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
//builder.Services.AddScoped<SalesService, SalesService>();
//builder.Services.AddScoped<ReceivingService, ReceivingService>();
//builder.Services.AddScoped<ServicingService, ServicingService>();

builder.Services.AddDbContext<SalesAndReturnContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("eBikeManagmentWebApp"));
});
builder.Services.AddDbContext<ReceivingDataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("eBikeManagmentWebApp"));
});
builder.Services.AddDbContext<ServicingDataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("eBikeManagmentWebApp"));
});

builder.Services.AddBackendDependencies(options => options.UseSqlServer(eBikes));

builder.Services.ReceivingDependencies(options => options.UseSqlServer(eBikes));
builder.Services.ServicingDependencies(options => options.UseSqlServer(eBikes));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
