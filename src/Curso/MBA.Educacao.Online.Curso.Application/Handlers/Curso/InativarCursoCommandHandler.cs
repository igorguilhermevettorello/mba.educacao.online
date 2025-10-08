using MBA.Educacao.Online.Curso.Application.Commands.Curso;
using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
using MBA.Educacao.Online.Curso.Application.Common.Models;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Handlers.Curso;

public class InativarCursoCommandHandler : ICommandHandler<InativarCursoCommand>
{
    private readonly ICursoRepository _cursoRepository;

    public InativarCursoCommandHandler(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    public async Task<Result> Handle(InativarCursoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar o curso
            var curso = await _cursoRepository.GetByIdAsync(request.Id);
            if (curso == null)
            {
                return Result.Failure("Curso não encontrado.");
            }

            // Verificar se já está inativo
            if (!curso.Ativo)
            {
                return Result.Failure("O curso já está inativo.");
            }

            // Inativar o curso
            curso.Inativar();

            // Atualizar no repositório
            await _cursoRepository.UpdateAsync(curso);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro interno ao inativar curso: {ex.Message}");
        }
    }
}
