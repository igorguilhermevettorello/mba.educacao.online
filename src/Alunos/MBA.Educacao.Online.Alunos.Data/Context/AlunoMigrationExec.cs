using MBA.Educacao.Online.Alunos.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MBA.Educacao.Online.Alunos.Data.Context
{
    public static class AlunoMigrationExec
    {
        public static async Task Exec(IServiceProvider serviceProvider, string env)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var alunoContext = scope.ServiceProvider.GetRequiredService<AlunoContext>();
            
            // Garante que o diretório do banco de dados existe
            EnsureDatabaseDirectoryExists(alunoContext);
            
            await alunoContext.Database.MigrateAsync();
            await SeederAplicacao.EnsureSeedAlunos(alunoContext, env);
        }

        private static void EnsureDatabaseDirectoryExists(AlunoContext context)
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

