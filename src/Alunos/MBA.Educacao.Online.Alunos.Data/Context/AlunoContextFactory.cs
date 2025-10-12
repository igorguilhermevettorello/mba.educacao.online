using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MBA.Educacao.Online.Alunos.Data.Context
{
    public class AlunoContextFactory : IDesignTimeDbContextFactory<AlunoContext>
    {
        public AlunoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AlunoContext>();
            
            optionsBuilder.UseSqlite("Data Source=../../../sqlite/dev.db");
            
            return new AlunoContext(optionsBuilder.Options);
        }
    }
}

