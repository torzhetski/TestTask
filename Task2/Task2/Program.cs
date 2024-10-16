using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Task2;
using Task2.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600;
});

string _connectionString = $"Server=localhost;Database=test2db;User Id=root;Password={Environment.GetEnvironmentVariable("SQLPassword")};";
builder.Services.AddDbContext<IApplicationContext, ApplicationContext>(options => options.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapFallbackToPage("/Upload");

app.Run();
