using MBA.Educacao.Online.Curso.Application.Commands.ConteudoProgramatico;
using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
using MBA.Educacao.Online.Curso.Application.Common.Models;
using MBA.Educacao.Online.Curso.Domain.ValueObjects;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Handlers.ConteudoProgramatico;

public class AdicionarConteudoProgramaticoCommandHandler : ICommandHandler<AdicionarConteudoProgramaticoCommand>
{
    private readonly ICursoRepository _cursoRepository;

    public AdicionarConteudoProgramaticoCommandHandler(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    public async Task<Result> Handle(AdicionarConteudoProgramaticoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar o curso
            var curso = await _cursoRepository.GetByIdAsync(request.CursoId);
            if (curso == null)
            {
                return Result.Failure("Curso não encontrado.");
            }

            // Verificar se o curso está ativo
            if (!curso.Ativo)
            {
                return Result.Failure("Não é possível adicionar conteúdo programático a um curso inativo.");
            }

            // Criar o conteúdo programático
            var conteudo = new ConteudoProgramatico(request.Titulo, request.Descricao, request.Ordem);

            // Adicionar ao curso
            curso.AdicionarConteudoProgramatico(conteudo);

            // Atualizar o curso no repositório
            await _cursoRepository.UpdateAsync(curso);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro interno ao adicionar conteúdo programático: {ex.Message}");
        }
    }
}
