using FluentValidation;
using MBA.Educacao.Online.Cursos.Application.Commands.Aulas;

namespace MBA.Educacao.Online.Cursos.Application.Validators
{
    public class AtivarAulaCommandValidator : AbstractValidator<AtivarAulaCommand>
    {
        public AtivarAulaCommandValidator()
        {
            RuleFor(x => x.AulaId)
                .NotEmpty()
                .WithMessage("O ID da aula é obrigatório");
        }
    }
}

