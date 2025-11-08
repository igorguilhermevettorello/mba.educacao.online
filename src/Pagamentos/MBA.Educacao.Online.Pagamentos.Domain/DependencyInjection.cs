using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MBA.Educacao.Online.Pagamentos.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPagamentosApplication(this IServiceCollection services)
        {
            // Registrar MediatR
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}

