using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MBA.Educacao.Online.Cursos.Data.Context
{
    public class CursoContextFactory : IDesignTimeDbContextFactory<CursoContext>
    {
        public CursoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CursoContext>();
            
            optionsBuilder.UseSqlite("Data Source=../../../sqlite/dev.db");
            
            return new CursoContext(optionsBuilder.Options);
        }
    }
}

