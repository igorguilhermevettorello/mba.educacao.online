using MBA.Educacao.Online.Cursos.Application.Queries;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.Queries
{
    public class ObterAulasPorCursoQueryHandler : IRequestHandler<ObterAulasPorCursoQuery, IEnumerable<Aula>>
    {
        private readonly IAulaRepository _aulaRepository;

        public ObterAulasPorCursoQueryHandler(IAulaRepository aulaRepository)
        {
            _aulaRepository = aulaRepository;
        }

        public async Task<IEnumerable<Aula>> Handle(ObterAulasPorCursoQuery request, CancellationToken cancellationToken)
        {
            var aulas = await _aulaRepository.ObterAtivasPorCursoIdAsync(request.CursoId);
            return aulas;
        }
    }
}

