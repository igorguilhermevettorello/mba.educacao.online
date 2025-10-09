using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Application.Events.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.Cursos
{
    public class CriarCursoCommandHandler : IRequestHandler<CriarCursoCommand, bool>
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMediator _mediator;

        public CriarCursoCommandHandler(ICursoRepository cursoRepository, IMediator mediator)
        {
            _cursoRepository = cursoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
        {
            var curso = new Curso(request.Titulo, request.Descricao, request.Instrutor, request.Nivel, request.Valor);

            await _cursoRepository.Adicionar(curso);

            curso.AdicionarEvento(new CriarCursoEvent(curso.Id, request.Titulo, request.Descricao));
            
            // await _mediator.Publish(new CriarCursoEvent(curso.Id, request.Titulo, request.Descricao), cancellationToken);

            return await _cursoRepository.UnitOfWork.Commit();
        }
    }
}


