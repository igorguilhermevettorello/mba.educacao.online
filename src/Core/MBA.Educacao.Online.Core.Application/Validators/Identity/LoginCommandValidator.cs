using FluentValidation;
using MBA.Educacao.Online.Core.Application.Commands.Identity;

namespace MBA.Educacao.Online.Core.Application.Validators.Identity
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório")
                .EmailAddress().WithMessage("O e-mail informado é inválido")
                .MaximumLength(256).WithMessage("O e-mail deve ter no máximo 256 caracteres");

            RuleFor(c => c.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória");
        }
    }
}

