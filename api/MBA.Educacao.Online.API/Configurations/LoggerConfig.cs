using Elmah.Io.Extensions.Logging;

namespace MBA.Educacao.Online.API.Configurations
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "21774df1db3e4604ad09d19c113f9dd5";
                o.LogId = new Guid("c960945f-c935-46b5-8399-aa2825a92a2a");
            });

            services.AddLogging(builder =>
            {
                builder.AddElmahIo(options =>
                {
                    options.ApiKey = "21774df1db3e4604ad09d19c113f9dd5";
                    options.LogId = new Guid("c960945f-c935-46b5-8399-aa2825a92a2a");
                });
                builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            });

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}