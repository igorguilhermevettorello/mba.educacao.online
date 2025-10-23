using MBA.Educacao.Online.Cursos.Application.Commands.Aulas;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.Aulas
{
    public class ObterAulaPorIdCommandHandler : IRequestHandler<ObterAulaPorIdCommand, Aula?>
    {
        private readonly IAulaRepository _aulaRepository;

        public ObterAulaPorIdCommandHandler(IAulaRepository aulaRepository)
        {
            _aulaRepository = aulaRepository;
        }

        public async Task<Aula?> Handle(ObterAulaPorIdCommand request, CancellationToken cancellationToken)
        {
            return await _aulaRepository.BuscarPorIdAsync(request.Id);
        }
    }
}

