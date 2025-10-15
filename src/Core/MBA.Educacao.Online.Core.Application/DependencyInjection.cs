using FluentValidation;
using MBA.Educacao.Online.Core.Application.Services;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MBA.Educacao.Online.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreApplication(this IServiceCollection services)
        {
            // Registrar MediatR
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Registrar FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Registrar serviços de Identity
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}

