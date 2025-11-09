using MBA.Educacao.Online.Alunos.Data.Repositories;
using MBA.Educacao.Online.Alunos.Data.Services;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Services;
using MBA.Educacao.Online.API.Extensions;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Mediator;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Cursos.Application.Services.Cursos;
using MBA.Educacao.Online.Cursos.Data.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Services;
using MBA.Educacao.Online.Cursos.Domain.Services;
using MBA.Educacao.Online.Pagamentos.AntiCorruption;
using MBA.Educacao.Online.Pagamentos.Data.Repositories;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Payments;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Services;
using MBA.Educacao.Online.Pagamentos.Domain.Services;
using MBA.Educacao.Online.Vendas.Data.Repositories;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;

namespace MBA.Educacao.Online.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterApplicationDependencies(services, configuration);
            RegisterRepositories(services);
            RegisterServices(services);
            return services;
        }

        private static void RegisterApplicationDependencies(IServiceCollection service, IConfiguration configuration)
        {
            //service.AddScoped<IdentityDbContext>();
            //service.AddScoped<CursoContext>();
            //service.AddScoped<PedidoContext>();
            //service.AddScoped<AlunoContext>();
            //service.AddScoped<PagamentoContext>();
            service.AddScoped<INotificador, Notificador>();
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void RegisterRepositories(IServiceCollection service)
        {
            service.AddScoped<ICursoRepository, CursoRepository>();
            service.AddScoped<IAulaRepository, AulaRepository>();
            service.AddScoped<IPedidoRepository, PedidoRepository>();
            service.AddScoped<IAlunoRepository, AlunoRepository>();
            service.AddScoped<IMatriculaRepository, MatriculaRepository>();
            service.AddScoped<ICertificadoRepository, CertificadoRepository>();
            service.AddScoped<IPagamentoRepository, PagamentoRepository>();
        }

        private static void RegisterServices(IServiceCollection service)
        {
            service.AddScoped<IMediatorHandler, MediatorHandler>();
            service.AddScoped<ICursoAppService, CursoAppService>();
            service.AddScoped<ICursoService, CursoService>();
            service.AddScoped<ICertificadoPdfService, CertificadoPdfService>();
            service.AddScoped<IPagamentoService, PagamentoService>();
            service.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
            service.AddScoped<IPayPalGateway, PayPalGateway>();
            service.AddScoped<Pagamentos.Domain.Interfaces.Payments.IConfigurationManager, Pagamentos.AntiCorruption.ConfigurationManager>();
            service.AddScoped<IUser, AspNetUser>();
        }
    }
}