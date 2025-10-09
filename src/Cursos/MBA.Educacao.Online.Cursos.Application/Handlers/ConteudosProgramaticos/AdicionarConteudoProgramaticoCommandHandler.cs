using MBA.Educacao.Online.Cursos.Application.Commands.ConteudosProgramaticos;
using MBA.Educacao.Online.Cursos.Application.Events.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.ConteudosProgramaticos
{
    public class AdicionarConteudoProgramaticoCommandHandler : IRequestHandler<AdicionarConteudoProgramaticoCommand, bool>
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMediator _mediator;

        public AdicionarConteudoProgramaticoCommandHandler(ICursoRepository cursoRepository, IMediator mediator)
        {
            _cursoRepository = cursoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarConteudoProgramaticoCommand request, CancellationToken cancellationToken)
        {
            var curso = _cursoRepository.BuscarPorId(request.CursoId);
                
            // var curso = new Curso(request.Titulo, request.Descricao, NivelCurso.Avancado);
            //var curso = new Domain.Entities.Curso(request.Titulo, request.Descricao, NivelCurso.Avancado);

            // _cursoRepository.Adicionar(curso);
            //
            // await _mediator.Publish(new CriarCursoEvent(curso.Id, request.Titulo, request.Descricao), cancellationToken);

            return true;
        }
    }
}