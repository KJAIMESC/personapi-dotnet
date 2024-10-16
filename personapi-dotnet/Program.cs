﻿using personapi_dotnet.Interfaces;
using personapi_dotnet.Repositories;
using personapi_dotnet.Models.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // For MVC Controllers (like views)
builder.Services.AddControllers(); // For API Controllers
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger to only document API controllers
builder.Services.AddSwaggerGen(c =>
{
    c.DocInclusionPredicate((_, apiDesc) =>
    {
        // Only include API controllers in Swagger documentation (those with the [ApiController] attribute)
        return apiDesc.ActionDescriptor.EndpointMetadata
            .Any(em => em is Microsoft.AspNetCore.Mvc.ApiControllerAttribute);
    });
});

// Register DbContext with connection string from configuration
builder.Services.AddDbContext<PersonaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IEstudioRepository, EstudioRepository>();
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IProfesionRepository, ProfesionRepository>();
builder.Services.AddScoped<ITelefonoRepository, TelefonoRepository>();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// Use CORS
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

// Optional: Middleware for logging unhandled exceptions
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An error occurred while processing your request.");
    }
});

// Map API controllers (with [ApiController] attributes)
app.MapControllers(); // This grabs API controllers

// Map default MVC route for non-API controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
