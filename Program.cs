using Microsoft.EntityFrameworkCore;
using VonnPizzaBackEndService.Data;
using VonnPizzaBackEndService.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VonnPizzaBackEndService.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseMySQL(_connectionString));
builder.Services.AddDbContext<PizzasDbContext>(options =>
    options.UseMySQL(_connectionString));
builder.Services.AddDbContext<OrderDetailsDbContext>(options =>
    options.UseMySQL(_connectionString));

builder.Services.AddDbContext<>(options =>
    options.UseSqlServer("DefaultConnection"));

builder.Services.AddControllers();

// Add service injection (dependency registration) here:
builder.Services.AddScoped<PizzasServices>();
builder.Services.AddScoped<OrdersServices>();
builder.Services.AddScoped<OrderDetailsServices>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    // Configure Swagger to support file uploads
    c.OperationFilter<FileUploadOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Define a custom operation filter to handle file uploads
public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.GetCustomAttributes(typeof(HttpPostAttribute), true).Any())
        {
            // Check if the method has a [HttpPost] attribute
            foreach (var parameter in context.MethodInfo.GetParameters())
            {
                // Iterate through method parameters
                if (parameter.ParameterType == typeof(IFormFile))
                {
                    // If a parameter is of type IFormFile, set it as a file upload parameter
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = parameter.Name,
                        In = ParameterLocation.Header,
                        Schema = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary",
                            Description = "File content"
                        },
                        Required = true // Adjust as needed
                    });
                }
            }
        }
    }
}
