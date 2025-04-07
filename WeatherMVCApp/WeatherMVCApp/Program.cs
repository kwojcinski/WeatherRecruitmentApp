using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Core.Contexts;
using Microsoft.EntityFrameworkCore;
using Services.Repositories;
using Services.Services;

namespace WeatherMVCApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<WeatherContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddHttpClient<IWeatherApiClient, WeatherApiClient>();
            builder.Services.AddScoped<IWeatherService, WeatherService>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

            builder.Services.AddHostedService<WeatherUpdateService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowAnyOrigin();
                    });
            });

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

            app.UseCors("AllowAllOrigins");

            app.Run();
        }
    }
}