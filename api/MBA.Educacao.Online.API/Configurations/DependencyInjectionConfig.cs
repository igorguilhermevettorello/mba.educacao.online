using MBA.Educacao.Online.Alunos.Data.Context;
using MBA.Educacao.Online.Alunos.Data.Repositories;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.API.Extensions;
using MBA.Educacao.Online.Core.Data.Context;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Mediator;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Cursos.Application.Services.Cursos;
using MBA.Educacao.Online.Cursos.Data.Context;
using MBA.Educacao.Online.Cursos.Data.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Services;
using MBA.Educacao.Online.Cursos.Domain.Services;
using MBA.Educacao.Online.Pagamentos.AntiCorruption;
using MBA.Educacao.Online.Pagamentos.Data.Context;
using MBA.Educacao.Online.Pagamentos.Data.Repositories;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Payments;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Services;
using MBA.Educacao.Online.Pagamentos.Domain.Services;
using MBA.Educacao.Online.Vendas.Data.Context;
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
            service.AddScoped<IdentityDbContext>();
            service.AddScoped<CursoContext>();
            service.AddScoped<PedidoContext>();
            service.AddScoped<AlunoContext>();
            service.AddScoped<PagamentoContext>();
            service.AddScoped<INotificador, Notificador>();
            // service.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // service.AddControllers()
            //    .AddJsonOptions(options =>
            //    {
            //        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            //        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            //    });
        }

        private static void RegisterRepositories(IServiceCollection service)
        {
            service.AddScoped<ICursoRepository, CursoRepository>();
            service.AddScoped<IAulaRepository, AulaRepository>();
            service.AddScoped<IPedidoRepository, PedidoRepository>();
            service.AddScoped<IAlunoRepository, AlunoRepository>();
            service.AddScoped<IPagamentoRepository, PagamentoRepository>();
            // service.AddScoped<ICategoriaRepository, CategoriaRepository>();
            // service.AddScoped<IClienteRepository, ClienteRepository>();
            // service.AddScoped<IProdutoRepository, ProdutoRepository>();
            // service.AddScoped<IVendedorRepository, VendedorRepository>();
            // service.AddScoped<IUserRepository<IdentityUser>, UserRepository>();
            // service.AddScoped<IFavoritoRepository, FavoritoRepository>();
        }

        private static void RegisterServices(IServiceCollection service)
        {
            service.AddScoped<IMediatorHandler, MediatorHandler>();
            service.AddScoped<ICursoAppService, CursoAppService>();
            service.AddScoped<ICursoService, CursoService>();
            service.AddScoped<IPagamentoService, PagamentoService>();
            service.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
            service.AddScoped<IPayPalGateway, PayPalGateway>();
            service.AddScoped<MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Payments.IConfigurationManager, MBA.Educacao.Online.Pagamentos.AntiCorruption.ConfigurationManager>();

            // service.AddScoped<ICategoriaService, CategoriaService>();
            // service.AddScoped<IProdutoService, ProdutoService>();
            // service.AddScoped<IVendedorService, VendedorService>();
            // service.AddScoped<IAccountService, AccountService>();
            service.AddScoped<IUser, AspNetUser>();
            // service.AddScoped<IFavoritoService, FavoritoService>();
            // service.AddScoped<IClienteService, ClienteService>();
        }
    }
}