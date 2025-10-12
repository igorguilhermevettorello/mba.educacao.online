using MBA.Educacao.Online.Alunos.Data.Context;
using MBA.Educacao.Online.Core.Data.Context;
using MBA.Educacao.Online.Cursos.Data.Context;
using MBA.Educacao.Online.Vendas.Data.Context;

namespace MBA.Educacao.Online.API.Configurations
{
    public static class MigrationsAndSeedConfig
    {
        public static void UseMigrationsAndSeedsConfig(this WebApplication app)
        {
            var services = app.Services.CreateScope().ServiceProvider;
            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            string environmentName = env.EnvironmentName;
            IdentityMigrationExec.Exec(services).Wait();
            AlunoMigrationExec.Exec(services, environmentName).Wait();
            CursoMigrationExec.Exec(services, environmentName).Wait();
            PedidoMigrationExec.Exec(services).Wait();
        }
    }
}