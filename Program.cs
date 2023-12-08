using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RoaringAPI.Interface;
using RoaringAPI.Mapping;
using RoaringAPI.Model;
using RoaringAPI.Service;
using Serilog;
using System;


namespace RoaringAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Configure Serilog
                builder.Host.UseSerilog((ctx, lc) => lc
                    .WriteTo.Console()
                    .ReadFrom.Configuration(ctx.Configuration));

                // Add services to the container.
                builder.Services.AddEntityFrameworkSqlite().AddDbContext<RoaringDbcontext>();

                builder.Services.AddScoped<RoaringApiService>(serviceProvider =>
                {
                    var config = serviceProvider.GetRequiredService<IConfiguration>();
                    var exceptionHandlingService = serviceProvider.GetRequiredService<IExceptionHandlingService>();

                    return new RoaringApiService(config, exceptionHandlingService);
                });


                // Add scoped services
                builder.Services.AddScoped<IExceptionHandlingService, ExceptionHandlingService>();
                builder.Services.AddScoped<IFinancialRecordMapperService, FinancialRecordMapperService>();
                builder.Services.AddScoped<IFinancialRatingMapperService, FinancialRatingMapperService>();
                builder.Services.AddScoped<IGroupStructureMapperService, GroupStructureMapperService>();
                builder.Services.AddScoped<ICompanyMapperService, CompanyMapperService>();
                builder.Services.AddScoped<IAddressMapperService, AddressMapperService>();
                builder.Services.AddScoped<ICompanyEmployeeMapperService, CompanyEmployeeMapperService>();

                builder.Services.AddHttpClient("RoaringAPI", client =>
                {
                    client.BaseAddress = new Uri("https://api.roaring.io/");
                });

                // Add Controllers and Razor Pages
                builder.Services.AddControllers();
                builder.Services.AddRazorPages(); // Add this line

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Roaring API", Version = "v1" });
                });

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                        // Optionally set the Swagger UI route
                        c.RoutePrefix = "swagger";
                    });
                }
               
                app.UseAuthorization();

                app.MapControllers();
                app.MapRazorPages(); // Add this line

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
