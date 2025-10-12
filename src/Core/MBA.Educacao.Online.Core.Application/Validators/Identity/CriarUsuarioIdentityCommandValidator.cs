using FluentValidation;
using MBA.Educacao.Online.Core.Application.Commands.Identity;

namespace MBA.Educacao.Online.Core.Application.Validators.Identity
{
    public class CriarUsuarioIdentityCommandValidator : AbstractValidator<CriarUsuarioIdentityCommand>
    {
        public CriarUsuarioIdentityCommandValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório")
                .EmailAddress().WithMessage("O e-mail informado é inválido")
                .MaximumLength(256).WithMessage("O e-mail deve ter no máximo 256 caracteres");

            RuleFor(c => c.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres");

            RuleFor(c => c.ConfirmacaoSenha)
                .NotEmpty().WithMessage("A confirmação de senha é obrigatória")
                .Equal(c => c.Senha).WithMessage("A senha e a confirmação de senha não conferem");
        }
    }
}

