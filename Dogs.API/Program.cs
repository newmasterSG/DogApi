using Dogs.API.Config;
using Dogs.API.Seed;
using Dogs.Application.Interfaces;
using Dogs.Application.Services;
using Dogs.Domain.Entity;
using Dogs.Infrastructure.Context;
using Dogs.Infrastructure.Interfaces;
using Dogs.Infrastructure.Repository;
using Dogs.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dogs.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
            builder.Services.AddDbContext<DogsContext>(options =>
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging());

            builder.Services.AddingOwnDI();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<DogsContext>();

                dbContext.Database.EnsureCreated();

                var dogEntities = Seeding.Seed();

                if (!await dbContext.Dogs.AnyAsync())
                {
                    await dbContext.AddRangeAsync(dogEntities);
                }

                await dbContext.SaveChangesAsync();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/ping", async context =>
                {
                    await context.Response.WriteAsync("Dogshouseservice.Version1.0.1");
                });
                endpoints.MapControllers();
            });


            app.Run();
        }
    }
}