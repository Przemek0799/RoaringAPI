using LazyCache;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RoaringAPI.Dashboard;
using RoaringAPI.Interface;
using RoaringAPI.Mapping;
using RoaringAPI.Model;
using RoaringAPI.Search;
using RoaringAPI.Service;
using Serilog;
using System.Text;

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


            builder.Services.AddSingleton<IAppCache, CachingService>();

            // API Services
            builder.Services.AddScoped<RoaringApiService>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var exceptionHandlingService = serviceProvider.GetRequiredService<IExceptionHandlingService>();
                var cache = serviceProvider.GetRequiredService<IAppCache>();
                var logger = serviceProvider.GetRequiredService<ILogger<RoaringApiService>>(); 

                return new RoaringApiService(config, exceptionHandlingService, cache, logger);
            });



            // Exception handling and mappers
            builder.Services.AddScoped<IExceptionHandlingService, ExceptionHandlingService>();
            builder.Services.AddScoped<IFinancialRecordMapperService, FinancialRecordMapperService>();
            builder.Services.AddScoped<IFinancialRatingMapperService, FinancialRatingMapperService>();
            builder.Services.AddScoped<IGroupStructureMapperService, GroupStructureMapperService>();
            builder.Services.AddScoped<ICompanyMapperService, CompanyMapperService>();
            builder.Services.AddScoped<IAddressMapperService, AddressMapperService>();
            builder.Services.AddScoped<ICompanyEmployeeMapperService, CompanyEmployeeMapperService>();
            builder.Services.AddScoped<IRoaringApiService, RoaringApiService>();

            //broken down controllers
            builder.Services.AddScoped<DashboardResults>();
            builder.Services.AddScoped<GeneralSearchService>();
            builder.Services.AddScoped<FilteredSearchService>();



            // Configure JWT Authentication
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
