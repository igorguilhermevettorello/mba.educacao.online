using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MBA.Educacao.Online.Vendas.Data.Context
{
    public class PedidoContextFactory : IDesignTimeDbContextFactory<PedidoContext>
    {
        public PedidoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PedidoContext>();
            
            optionsBuilder.UseSqlite("Data Source=../../../sqlite/dev.db");
            
            return new PedidoContext(optionsBuilder.Options);
        }
    }
}

