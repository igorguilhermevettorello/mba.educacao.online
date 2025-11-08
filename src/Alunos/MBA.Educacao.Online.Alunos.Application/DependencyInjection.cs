using FluentValidation;
using MBA.Educacao.Online.Alunos.Application.Interfaces;
using MBA.Educacao.Online.Alunos.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MBA.Educacao.Online.Alunos.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAlunosApplication(this IServiceCollection services)
        {
            // Registrar MediatR
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Registrar FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Registrar serviços de aplicação
            services.AddScoped<IMatriculaService, MatriculaService>();

            return services;
        }
    }
}
