using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.Cursos
{
    public class InativarCursoCommandHandler : IRequestHandler<InativarCursoCommand, bool>
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMediator _mediator;

        public InativarCursoCommandHandler(ICursoRepository cursoRepository, IMediator mediator)
        {
            _cursoRepository = cursoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(InativarCursoCommand request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.BuscarPorId(request.CursoId);

            if (curso != null)
            {
                curso.Inativar();
                await _cursoRepository.Alterar(curso);
                return await _cursoRepository.UnitOfWork.Commit();
            }

            return false;
        }
    }
}
