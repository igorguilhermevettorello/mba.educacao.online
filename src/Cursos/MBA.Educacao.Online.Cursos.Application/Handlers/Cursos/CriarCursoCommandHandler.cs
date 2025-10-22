using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.ValueObjects;
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
            if (!request.IsValid())
                return false;

            var curso = new Curso(request.Titulo, request.Descricao, request.Instrutor, request.Nivel, request.Valor);
            
            if (request.ConteudoProgramatico != null)
            {
                var conteudoProgramatico = new ConteudoProgramatico(
                    request.ConteudoProgramatico.Ementa,
                    request.ConteudoProgramatico.Objetivo,
                    request.ConteudoProgramatico.Bibliografia,
                    request.ConteudoProgramatico.MaterialUrl
                );

                curso.AdicionarConteudoProgramatico(conteudoProgramatico);
            }

            request.SetAggregateId(curso.Id);
            _cursoRepository.Adicionar(curso);

            // curso.AdicionarEvento(new CriarCursoEvent(curso.Id, request.Titulo, request.Descricao));
            // await _mediator.Publish(new CriarCursoEvent(curso.Id, request.Titulo, request.Descricao), cancellationToken);

            return await _cursoRepository.UnitOfWork.Commit();
        }
    }
}


