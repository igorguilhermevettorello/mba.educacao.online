using FluentValidation;
using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;

namespace MBA.Educacao.Online.Cursos.Application.Validators
{
    public class DeletarCursoCommandValidator : AbstractValidator<DeletarCursoCommand>
    {
        public DeletarCursoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id do curso é obrigatório.");
        }
    }
}

