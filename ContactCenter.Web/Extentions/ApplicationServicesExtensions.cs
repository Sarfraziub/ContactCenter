using ContactCenter.Data;
using EDRSM.API.Errors;
using EDRSM.API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDRSM.API.Extentions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
           IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<CCDbContext>(options => options.UseNpgsql(connectionString));

            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IReportingRepository, ReportingRepository>();
            //services.AddScoped<IDashboardStatsRepository, DashboardStatsRepository>();
            //services.AddScoped<IEmailSender, EmailSender>();
            //services.AddScoped<IPhotoService, PhotoService>();

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); ;
                });
            });

            return services;
        }
    }
}
