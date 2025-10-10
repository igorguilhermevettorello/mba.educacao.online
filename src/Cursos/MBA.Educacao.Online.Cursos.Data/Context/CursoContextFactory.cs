using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MBA.Educacao.Online.Cursos.Data.Context
{
    public class CursoContextFactory : IDesignTimeDbContextFactory<CursoContext>
    {
        public CursoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CursoContext>();
            
            // Usar SQLite para desenvolvimento (mesmo caminho do appsettings.json)
            optionsBuilder.UseSqlite("Data Source=../../../sqlite/dev.db");
            
            // Criar um IMediator fake para design time (migrations)
            var mediatorMock = new NoOpMediator();
            
            return new CursoContext(optionsBuilder.Options, mediatorMock);
        }
    }
    
    // Implementação vazia do IMediator apenas para design time
    internal class NoOpMediator : IMediator
    {
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            return Task.CompletedTask;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            throw new NotImplementedException();
        }

        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

