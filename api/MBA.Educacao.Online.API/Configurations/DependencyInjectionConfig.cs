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
            // service.AddScoped<ApplicationDbContext>();
            // service.AddScoped<INotificador, Notificador>();
            // service.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            // service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
            // service.AddScoped<ICategoriaRepository, CategoriaRepository>();
            // service.AddScoped<IClienteRepository, ClienteRepository>();
            // service.AddScoped<IProdutoRepository, ProdutoRepository>();
            // service.AddScoped<IVendedorRepository, VendedorRepository>();
            // service.AddScoped<IUserRepository<IdentityUser>, UserRepository>();
            // service.AddScoped<IFavoritoRepository, FavoritoRepository>();
        }

        private static void RegisterServices(IServiceCollection service)
        {
            // service.AddScoped<ICategoriaService, CategoriaService>();
            // service.AddScoped<IProdutoService, ProdutoService>();
            // service.AddScoped<IVendedorService, VendedorService>();
            // service.AddScoped<IAccountService, AccountService>();
            // service.AddScoped<IUser, AspNetUser>();
            // service.AddScoped<IFavoritoService, FavoritoService>();
            // service.AddScoped<IClienteService, ClienteService>();
        }
    }
}