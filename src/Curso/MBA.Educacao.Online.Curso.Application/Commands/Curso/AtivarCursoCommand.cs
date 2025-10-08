using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Commands.Curso;

public record AtivarCursoCommand(Guid Id) : ICommand;
