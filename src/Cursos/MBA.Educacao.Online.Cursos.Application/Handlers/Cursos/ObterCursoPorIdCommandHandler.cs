using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.Cursos
{
    public class ObterCursoPorIdCommandHandler : IRequestHandler<ObterCursoPorIdCommand, Curso>
    {
        private readonly ICursoRepository _cursoRepository;

        public ObterCursoPorIdCommandHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<Curso> Handle(ObterCursoPorIdCommand request, CancellationToken cancellationToken)
        {
            return await _cursoRepository.BuscarPorIdAsync(request.Id);
        }
    }
}

