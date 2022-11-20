using Basket.API.Repositories.Interfaces;
using Basket.API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var z = builder.Configuration.GetSection("Basket.API");
                options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}