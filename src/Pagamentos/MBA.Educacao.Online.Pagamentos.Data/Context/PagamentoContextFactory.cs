using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Messages;
using MediatR;
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

            // Usar um mediator fake para design-time (migrations)
            var fakeMediatorHandler = new DesignTimeMediatorHandler();

            return new PagamentoContext(optionsBuilder.Options, fakeMediatorHandler);
        }

        private class DesignTimeMediatorHandler : IMediatorHandler
        {
            public Task<bool> EnviarComando<T>(T comando) where T : Command
            {
                throw new NotImplementedException();
            }

            public Task<TResponse> EnviarComando<TResponse>(IRequest<TResponse> comando)
            {
                throw new NotImplementedException();
            }

            public Task PublicarEvento<T>(T evento) where T : Event
            {
                return Task.CompletedTask;
            }
        }
    }
}
