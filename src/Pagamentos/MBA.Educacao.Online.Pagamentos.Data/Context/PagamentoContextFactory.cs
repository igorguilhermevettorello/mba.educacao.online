using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MBA.Educacao.Online.Pagamentos.Data.Context
{
    public class PagamentoContextFactory : IDesignTimeDbContextFactory<PagamentoContext>
    {
        public PagamentoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PagamentoContext>();

            optionsBuilder.UseSqlite("Data Source=../../../sqlite/dev.db");

            return new PagamentoContext(optionsBuilder.Options);
        }
    }
}
