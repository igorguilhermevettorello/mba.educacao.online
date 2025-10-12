using FluentValidation;
using MBA.Educacao.Online.Alunos.Application.Commands;

namespace MBA.Educacao.Online.Alunos.Application.Validators
{
    public class CriarAlunoCommandValidator : AbstractValidator<CriarAlunoCommand>
    {
        public CriarAlunoCommandValidator()
        {
            RuleFor(c => c.UsuarioId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório")
                .NotEqual(Guid.Empty).WithMessage("O ID do usuário deve ser válido");

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório")
                .EmailAddress().WithMessage("O e-mail informado é inválido")
                .MaximumLength(256).WithMessage("O e-mail deve ter no máximo 256 caracteres");
        }
    }
}

