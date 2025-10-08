using MBA.Educacao.Online.Curso.Application.Commands.Aula;
using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
using MBA.Educacao.Online.Curso.Application.Common.Models;
using MBA.Educacao.Online.Curso.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Handlers.Aula;

public class AdicionarAulaCommandHandler : ICommandHandler<AdicionarAulaCommand, Guid>
{
    private readonly ICursoRepository _cursoRepository;

    public AdicionarAulaCommandHandler(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    public async Task<Result<Guid>> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar o curso
            var curso = await _cursoRepository.GetByIdAsync(request.CursoId);
            if (curso == null)
            {
                return Result.Failure<Guid>("Curso não encontrado.");
            }

            // Verificar se o curso está ativo
            if (!curso.Ativo)
            {
                return Result.Failure<Guid>("Não é possível adicionar aulas a um curso inativo.");
            }

            // Criar a aula
            var aula = new Aula(request.Titulo, request.Descricao, request.DuracaoMinutos, request.Ordem);

            // Adicionar ao curso
            curso.AdicionarAula(aula);

            // Atualizar o curso no repositório
            await _cursoRepository.UpdateAsync(curso);

            return Result.Success(aula.Id);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<Guid>(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure<Guid>(ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>($"Erro interno ao adicionar aula: {ex.Message}");
        }
    }
}
