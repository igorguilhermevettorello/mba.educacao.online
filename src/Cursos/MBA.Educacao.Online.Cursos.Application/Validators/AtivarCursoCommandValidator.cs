using FluentValidation;
using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;

namespace MBA.Educacao.Online.Cursos.Application.Validators
{
    public class AtivarCursoCommandValidator : AbstractValidator<AtivarCursoCommand>
    {
        public AtivarCursoCommandValidator()
        {
            RuleFor(x => x.CursoId)
                .Empty()
                .WithMessage("Título do curso deve ter no mínimo 3 caracteres");
        }
    }
}
