using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Commands.ConteudoProgramatico;

public record AdicionarConteudoProgramaticoCommand(
    Guid CursoId,
    string Titulo,
    string Descricao,
    int Ordem
) : ICommand;
