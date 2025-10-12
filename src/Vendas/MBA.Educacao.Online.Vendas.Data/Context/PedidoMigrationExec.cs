using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MBA.Educacao.Online.Vendas.Data.Context
{
    public static class PedidoMigrationExec
    {
        public static async Task Exec(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var pedidoContext = scope.ServiceProvider.GetRequiredService<PedidoContext>();
            
            // Garante que o diretório do banco de dados existe
            EnsureDatabaseDirectoryExists(pedidoContext);
            
            await pedidoContext.Database.MigrateAsync();
        }

        private static void EnsureDatabaseDirectoryExists(PedidoContext context)
        {
            var connectionString = context.Database.GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                // Extrai o caminho do arquivo da connection string SQLite
                var dataSourceMatch = System.Text.RegularExpressions.Regex.Match(
                    connectionString, 
                    @"Data Source=([^;]+)", 
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                
                if (dataSourceMatch.Success)
                {
                    var dbPath = dataSourceMatch.Groups[1].Value;
                    var directory = Path.GetDirectoryName(dbPath);
                    
                    if (!string.IsNullOrEmpty(directory) && !Path.IsPathRooted(dbPath))
                    {
                        // Para caminhos relativos, resolve baseado no diretório de trabalho atual
                        directory = Path.GetFullPath(directory);
                    }
                    
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }
            }
        }
    }
}
