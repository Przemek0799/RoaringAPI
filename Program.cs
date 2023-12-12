﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RoaringAPI.Dashboard;
using RoaringAPI.Interface;
using RoaringAPI.Mapping;
using RoaringAPI.Model;
using RoaringAPI.Search;
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

                builder.Host.UseSerilog((ctx, lc) => lc
                    .WriteTo.Console()
                    .ReadFrom.Configuration(ctx.Configuration));

                ConfigureServices(builder);

                var app = builder.Build();

                ConfigureApp(app);

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

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddEntityFrameworkSqlite().AddDbContext<RoaringDbcontext>();

            // API Services
            builder.Services.AddScoped<RoaringApiService>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var exceptionHandlingService = serviceProvider.GetRequiredService<IExceptionHandlingService>();
                return new RoaringApiService(config, exceptionHandlingService);
            });

            // Exception handling and mappers
            builder.Services.AddScoped<IExceptionHandlingService, ExceptionHandlingService>();
            builder.Services.AddScoped<IFinancialRecordMapperService, FinancialRecordMapperService>();
            builder.Services.AddScoped<IFinancialRatingMapperService, FinancialRatingMapperService>();
            builder.Services.AddScoped<IGroupStructureMapperService, GroupStructureMapperService>();
            builder.Services.AddScoped<ICompanyMapperService, CompanyMapperService>();
            builder.Services.AddScoped<IAddressMapperService, AddressMapperService>();
            builder.Services.AddScoped<ICompanyEmployeeMapperService, CompanyEmployeeMapperService>();

            //broken down controllers
            builder.Services.AddScoped<DashboardResults>();
            builder.Services.AddScoped<GeneralSearchService>();
            builder.Services.AddScoped<FilteredSearchService>();



            builder.Services.AddHttpClient("RoaringAPI", client =>
            {
                client.BaseAddress = new Uri("https://api.roaring.io/");
            });

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Roaring API", Version = "v1" });
            });
        }

        private static void ConfigureApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}