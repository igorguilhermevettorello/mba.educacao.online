using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Commands.Aula;

public record AdicionarAulaCommand(
    Guid CursoId,
    string Titulo,
    string Descricao,
    int DuracaoMinutos,
    int Ordem
) : ICommand<Guid>;
