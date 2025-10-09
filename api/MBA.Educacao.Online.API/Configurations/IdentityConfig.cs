using MBA.Educacao.Online.API.Extensions;
using MBA.Educacao.Online.Core.Data.Context;
using Microsoft.AspNetCore.Identity;

namespace MBA.Educacao.Online.API.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddErrorDescriber<IdentityErrorDescriberPtBr>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}