using MBA.Educacao.Online.Alunos.Data.Context;
using MBA.Educacao.Online.Core.Data.Context;
using MBA.Educacao.Online.Cursos.Data.Context;
using MBA.Educacao.Online.Pagamentos.Data.Context;
using MBA.Educacao.Online.Vendas.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.API.Configurations
{
    public static class DatabaseSelectorExtension
    {
        public static void AddDatabaseSelector(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                var solutionDirectory = Directory.GetParent(builder.Environment.ContentRootPath)?.Parent?.FullName;
                var sqliteDirectory = Path.Combine(solutionDirectory ?? builder.Environment.ContentRootPath, "sqlite");
                var databasePath = Path.Combine(sqliteDirectory, "dev.db");
                
                if (!Directory.Exists(sqliteDirectory))
                {
                    Directory.CreateDirectory(sqliteDirectory);
                }
                
                var connectionString = $"Data Source={databasePath}";
                
                Console.WriteLine($"[DatabaseSelector] Using SQLite database at: {databasePath}");
                
                builder.Services.AddDbContext<CursoContext>(options =>
                    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                        .UseSqlite(connectionString)
                );

                builder.Services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlite(connectionString)
                );

                builder.Services.AddDbContext<PedidoContext>(options =>
                    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                        .UseSqlite(connectionString)
                );

                builder.Services.AddDbContext<AlunoContext>(options =>
                    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                        .UseSqlite(connectionString)
                );

                builder.Services.AddDbContext<PagamentoContext>(options =>
                    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                        .UseSqlite(connectionString)
                );
            }
            else
            {
                builder.Services.AddDbContext<CursoContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

                builder.Services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

                builder.Services.AddDbContext<PedidoContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

                builder.Services.AddDbContext<AlunoContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

                builder.Services.AddDbContext<PagamentoContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );
            }
        }
    }
}

