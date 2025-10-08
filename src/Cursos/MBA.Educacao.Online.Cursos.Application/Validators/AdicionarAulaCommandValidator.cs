using FluentValidation;
using MBA.Educacao.Online.Cursos.Application.Commands.Aulas;

namespace MBA.Educacao.Online.Cursos.Application.Validators
{
    public class AdicionarAulaCommandValidator : AbstractValidator<AdicionarAulaCommand>
    {
        public AdicionarAulaCommandValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("Título do curso é obrigatório")
                .MaximumLength(200)
                .WithMessage("Título do curso deve ter no máximo 200 caracteres")
                .MinimumLength(3)
                .WithMessage("Título do curso deve ter no mínimo 3 caracteres");

            RuleFor(x => x.Descricao)
                .NotEmpty()
                .WithMessage("Descrição do curso é obrigatória")
                .MaximumLength(1000)
                .WithMessage("Descrição do curso deve ter no máximo 1000 caracteres")
                .MinimumLength(10)
                .WithMessage("Descrição do curso deve ter no mínimo 10 caracteres");

            RuleFor(x => x.DuracaoMinutos)
                .IsInEnum()
                .WithMessage("Nível do curso deve ser válido");

            RuleFor(x => x.Ordem)
                .IsInEnum()
                .WithMessage("Nível do curso deve ser válido");
        }
    }
}
