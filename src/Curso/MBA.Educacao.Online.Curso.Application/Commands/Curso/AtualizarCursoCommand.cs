using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
using MBA.Educacao.Online.Curso.Domain.Enums;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Commands.Curso;

public record AtualizarCursoCommand(
    Guid Id,
    string Titulo,
    string Descricao,
    NivelCurso Nivel
) : ICommand;
