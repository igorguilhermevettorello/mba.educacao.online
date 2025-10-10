using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Domain.Entities;

namespace MBA.Educacao.Online.Cursos.Domain.Events
{
    public class CursoAvisarAlunosEvent : DomainEvent
    {
        private readonly string Aula;
        public CursoAvisarAlunosEvent(Guid aggregateId, string aula) : base(aggregateId)
        {
            Aula = aula;
        }
    }
}

