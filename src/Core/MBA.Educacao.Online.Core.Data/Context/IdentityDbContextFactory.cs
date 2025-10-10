using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MBA.Educacao.Online.Core.Data.Context
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            
            // Usar SQLite para desenvolvimento (mesmo caminho do appsettings.json)
            optionsBuilder.UseSqlite("Data Source=../../../sqlite/dev.db");
            
            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}

