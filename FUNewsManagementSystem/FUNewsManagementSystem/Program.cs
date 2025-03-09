using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Mapper;
using DataAccessLayer;
using FUNewsManagementSystem;
using FUNewsManagementSystem.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(MapperConfiguration));
builder.Services.Configure<AdminAccount>(builder.Configuration.GetSection("AdminAccount"));
builder.Services.AddDbContext<FunewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; 
});
builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(new PageAuthFilter());
});

builder.Services.AddApplicationServices();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.MapGet("/", () => Results.Redirect("/login"));
app.MapGet("/staff", () => Results.Redirect("/staff/category"));
app.MapGet("/admin", () => Results.Redirect("/admin/account"));



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
