using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Application.Events.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Entities;
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
            var curso = new Curso(string.Empty, string.Empty, NivelCurso.Avancado);

            _cursoRepository.Adicionar(curso);

            await _mediator.Publish(new CriarCursoEvent(curso.Id, string.Empty, string.Empty), cancellationToken);

            return true;
        }

    }
}

//public class AtivarCursoCommandHandler : ICommandHandler<AtivarCursoCommand>
//{
//    private readonly ICursoRepository _cursoRepository;

//    public AtivarCursoCommandHandler(ICursoRepository cursoRepository)
//    {
//        _cursoRepository = cursoRepository;
//    }

//    public async Task<Result> Handle(AtivarCursoCommand request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            // Buscar o curso
//            var curso = await _cursoRepository.GetByIdAsync(request.Id);
//            if (curso == null)
//            {
//                return Result.Failure("Curso não encontrado.");
//            }

//            // Verificar se já está ativo
//            if (curso.Ativo)
//            {
//                return Result.Failure("O curso já está ativo.");
//            }

//            // Ativar o curso
//            curso.Ativar();

//            // Atualizar no repositório
//            await _cursoRepository.UpdateAsync(curso);

//            return Result.Success();
//        }
//        catch (Exception ex)
//        {
//            return Result.Failure($"Erro interno ao ativar curso: {ex.Message}");
//        }
//    }
//}
