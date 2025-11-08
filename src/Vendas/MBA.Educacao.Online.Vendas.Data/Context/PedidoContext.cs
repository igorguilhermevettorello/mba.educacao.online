using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Pagamentos.Data;
using MBA.Educacao.Online.Vendas.Data.Extensions;
using MBA.Educacao.Online.Vendas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBA.Educacao.Online.Vendas.Data.Context
{
    public class PedidoContext : DbContext, IUnitOfWork
    {
        private readonly ILogger<PedidoContext>? _logger;
        private readonly IMediatorHandler _mediatorHandler;

        public PedidoContext(DbContextOptions<PedidoContext> options, IMediatorHandler mediatorHandler) : base(options) 
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public PedidoContext(DbContextOptions<PedidoContext> options, IMediatorHandler mediatorHandler, ILogger<PedidoContext> logger) 
            : base(options) 
        {
            _logger = logger;
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

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
            if (success) await _mediatorHandler.PublicarEventos(this);
            return success;
        }
    }
}
