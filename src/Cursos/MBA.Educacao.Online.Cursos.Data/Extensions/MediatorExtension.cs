using MBA.Educacao.Online.Core.Domain.Models;
using MBA.Educacao.Online.Cursos.Data.Context;

namespace MBA.Educacao.Online.Cursos.Data.Extensions
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatorHandler mediator, CursoContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.PublicarEvento(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}

