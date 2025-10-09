using FluentValidation;
using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;

namespace MBA.Educacao.Online.Cursos.Application.Validators
{
    public class InativarCursoCommandValidator : AbstractValidator<InativarCursoCommand>
    {
        public InativarCursoCommandValidator()
        {
            RuleFor(x => x.CursoId)
                .Empty()
                .WithMessage("Id do curso é obrigatório.");
        }
    }
}
