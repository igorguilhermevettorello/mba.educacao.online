using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MBA.Educacao.Online.Pagamentos.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPagamentosApplication(this IServiceCollection services)
        {
            // Registrar MediatR - isso vai escanear o assembly e encontrar o PagamentoEventHandler
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}

