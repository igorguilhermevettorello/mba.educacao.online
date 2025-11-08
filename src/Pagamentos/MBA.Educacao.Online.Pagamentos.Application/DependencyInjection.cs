using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MBA.Educacao.Online.Pagamentos.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPagamentosApplicationHandlers(this IServiceCollection services)
        {
            // Registrar MediatR - isso vai escanear o assembly e encontrar todos os handlers de eventos e comandos
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}

