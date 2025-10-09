using Microsoft.AspNetCore.Mvc;

namespace MBA.Educacao.Online.API.Configurations
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    //.AllowCredentials();
                });
            });

            services.AddHealthChecks();

            return services;
        }
    
    }
}

