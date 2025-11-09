using MBA.Educacao.Online.Cursos.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Queries
{
    public class ObterAulasPorCursoQuery : IRequest<IEnumerable<Aula>>
    {
        public Guid CursoId { get; set; }

        public ObterAulasPorCursoQuery(Guid cursoId)
        {
            CursoId = cursoId;
        }
    }
}

