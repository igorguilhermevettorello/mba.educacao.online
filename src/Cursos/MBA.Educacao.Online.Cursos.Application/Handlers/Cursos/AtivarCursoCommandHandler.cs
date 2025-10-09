using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.Cursos
{
    public class AtivarCursoCommandHandler : IRequestHandler<AtivarCursoCommand, bool>
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMediator _mediator;

        public AtivarCursoCommandHandler(ICursoRepository cursoRepository, IMediator mediator)
        {
            _cursoRepository = cursoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AtivarCursoCommand request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.BuscarPorId(request.CursoId);

            if (curso != null) 
            {
                curso.Ativar();
                await _cursoRepository.Alterar(curso);
                return await _cursoRepository.UnitOfWork.Commit();
            }

            return false;
        }
    }
}