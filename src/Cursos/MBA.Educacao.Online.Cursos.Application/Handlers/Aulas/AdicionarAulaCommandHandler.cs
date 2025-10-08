using MBA.Educacao.Online.Cursos.Application.Commands.Aulas;
using MBA.Educacao.Online.Cursos.Application.Events.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.Aulas
{
    public class AdicionarAulaCommandHandler : IRequestHandler<AdicionarAulaCommand, bool>
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMediator _mediator;

        public AdicionarAulaCommandHandler(ICursoRepository cursoRepository, IMediator mediator)
        {
            _cursoRepository = cursoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
        {
            var curso = new Domain.Entities.Curso(request.Titulo, request.Descricao, NivelCurso.Avancado);

            _cursoRepository.Adicionar(curso);

            await _mediator.Publish(new CriarCursoEvent(curso.Id, request.Titulo, request.Descricao), cancellationToken);

            return true;
        }

    }
}


