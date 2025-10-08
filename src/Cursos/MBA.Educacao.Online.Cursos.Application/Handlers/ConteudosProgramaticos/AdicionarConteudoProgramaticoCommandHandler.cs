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
            var curso = new Curso(request.Titulo, request.Descricao, NivelCurso.Avancado);
            //var curso = new Domain.Entities.Curso(request.Titulo, request.Descricao, NivelCurso.Avancado);

            _cursoRepository.Adicionar(curso);

            await _mediator.Publish(new CriarCursoEvent(curso.Id, request.Titulo, request.Descricao), cancellationToken);

            return true;
        }

    }

}

//public class AdicionarConteudoProgramaticoCommandHandler : ICommandHandler<AdicionarConteudoProgramaticoCommand>
//{
//    private readonly ICursoRepository _cursoRepository;

//    public AdicionarConteudoProgramaticoCommandHandler(ICursoRepository cursoRepository)
//    {
//        _cursoRepository = cursoRepository;
//    }

//    public async Task<Result> Handle(AdicionarConteudoProgramaticoCommand request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            // Buscar o curso
//            var curso = await _cursoRepository.GetByIdAsync(request.CursoId);
//            if (curso == null)
//            {
//                return Result.Failure("Curso não encontrado.");
//            }

//            // Verificar se o curso está ativo
//            if (!curso.Ativo)
//            {
//                return Result.Failure("Não é possível adicionar conteúdo programático a um curso inativo.");
//            }

//            // Criar o conteúdo programático
//            var conteudo = new ConteudoProgramatico(request.Titulo, request.Descricao, request.Ordem);

//            // Adicionar ao curso
//            curso.AdicionarConteudoProgramatico(conteudo);

//            // Atualizar o curso no repositório
//            await _cursoRepository.UpdateAsync(curso);

//            return Result.Success();
//        }
//        catch (ArgumentException ex)
//        {
//            return Result.Failure(ex.Message);
//        }
//        catch (InvalidOperationException ex)
//        {
//            return Result.Failure(ex.Message);
//        }
//        catch (Exception ex)
//        {
//            return Result.Failure($"Erro interno ao adicionar conteúdo programático: {ex.Message}");
//        }
//    }
//}
