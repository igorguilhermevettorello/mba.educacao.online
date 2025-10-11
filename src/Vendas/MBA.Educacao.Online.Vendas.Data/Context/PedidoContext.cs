using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Vendas.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace MBA.Educacao.Online.Vendas.Data.Context
{
    public class PedidoContext : DbContext, IUnitOfWork
    {
        public PedidoContext(DbContextOptions<PedidoContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<PedidoItem> PedidoItens { get; set; }

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

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidoContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var success = (await base.SaveChangesAsync()) > 0;
            // if (success) await _mediator.PublicarEventos(this);
            return success;
        }
    }
}
