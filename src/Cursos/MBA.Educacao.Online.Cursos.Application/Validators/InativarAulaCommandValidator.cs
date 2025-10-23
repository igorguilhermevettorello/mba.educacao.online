using FluentValidation;
using MBA.Educacao.Online.Cursos.Application.Commands.Aulas;

namespace MBA.Educacao.Online.Cursos.Application.Validators
{
    public class InativarAulaCommandValidator : AbstractValidator<InativarAulaCommand>
    {
        public InativarAulaCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O ID da aula é obrigatório");
        }
    }
}

