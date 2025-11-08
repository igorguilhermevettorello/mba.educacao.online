using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using MBA.Educacao.Online.Pagamentos.Domain.Entities;

namespace MBA.Educacao.Online.Pagamentos.Data.Context
{
    public class PagamentoContext : DbContext, IUnitOfWork
    {
        public PagamentoContext(DbContextOptions<PagamentoContext> options) : base(options) { }
        public DbSet<Pagamento> Pagamentos { get; set; }

        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model
                         .GetEntityTypes()
                         .SelectMany(e => e.GetProperties())
                         .Where(p => p.ClrType == typeof(string)))
            {
                property.SetColumnType("varchar(100)");
            }

            // Ignorar propriedades de domínio que não devem ser persistidas
            modelBuilder.Ignore<Core.Domain.Messages.Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentoContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var success = (await base.SaveChangesAsync()) > 0;
            // if (success) await _mediator.PublicarEventos(this);
            return success;
        }
    }
}
