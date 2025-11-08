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
            
            // Garante que o diretório e arquivo do banco de dados existem
            EnsureDatabaseFileExists(pedidoContext);
            
            await pedidoContext.Database.MigrateAsync();
        }

        private static void EnsureDatabaseFileExists(PedidoContext context)
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
                    
                    // Se for caminho relativo, resolve a partir da raiz do projeto
                    if (!Path.IsPathRooted(dbPath))
                    {
                        var projectRoot = GetProjectRoot();
                        dbPath = Path.Combine(projectRoot, dbPath);
                        dbPath = Path.GetFullPath(dbPath);
                        
                        // Atualiza a connection string com o caminho absoluto
                        var newConnectionString = connectionString.Replace(dataSourceMatch.Groups[1].Value, dbPath);
                        context.Database.SetConnectionString(newConnectionString);
                    }
                    
                    var directory = Path.GetDirectoryName(dbPath);
                    
                    // Cria o diretório se não existir
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    
                    // Cria o arquivo do banco de dados se não existir
                    if (!File.Exists(dbPath))
                    {
                        File.Create(dbPath).Dispose();
                    }
                }
            }
        }

        private static string GetProjectRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            
            // Navega para cima até encontrar o arquivo .sln (raiz do projeto)
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            
            return directory?.FullName ?? AppContext.BaseDirectory;
        }
    }
}
