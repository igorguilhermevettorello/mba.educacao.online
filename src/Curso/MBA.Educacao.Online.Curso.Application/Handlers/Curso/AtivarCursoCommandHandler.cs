using MBA.Educacao.Online.Curso.Application.Commands.Curso;
using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
using MBA.Educacao.Online.Curso.Application.Common.Models;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Handlers.Curso;

public class AtivarCursoCommandHandler : ICommandHandler<AtivarCursoCommand>
{
    private readonly ICursoRepository _cursoRepository;

    public AtivarCursoCommandHandler(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    public async Task<Result> Handle(AtivarCursoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar o curso
            var curso = await _cursoRepository.GetByIdAsync(request.Id);
            if (curso == null)
            {
                return Result.Failure("Curso não encontrado.");
            }

            // Verificar se já está ativo
            if (curso.Ativo)
            {
                return Result.Failure("O curso já está ativo.");
            }

            // Ativar o curso
            curso.Ativar();

            // Atualizar no repositório
            await _cursoRepository.UpdateAsync(curso);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro interno ao ativar curso: {ex.Message}");
        }
    }
}
