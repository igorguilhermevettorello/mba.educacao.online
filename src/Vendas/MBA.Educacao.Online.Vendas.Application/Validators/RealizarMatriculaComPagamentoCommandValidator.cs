using FluentValidation;
using MBA.Educacao.Online.Vendas.Application.Commands;

namespace MBA.Educacao.Online.Vendas.Application.Validators
{
    public class RealizarMatriculaComPagamentoCommandValidator : AbstractValidator<RealizarMatriculaComPagamentoCommand>
    {
        public RealizarMatriculaComPagamentoCommandValidator()
        {
            RuleFor(c => c.AlunoId)
                .NotEmpty()
                .WithMessage("O ID do aluno é inválido")
                .NotEqual(Guid.Empty)
                .WithMessage("O ID do aluno é inválido");

            RuleFor(c => c.CursoIds)
                .NotNull()
                .WithMessage("A lista de cursos é obrigatória")
                .NotEmpty()
                .WithMessage("Deve haver pelo menos um curso selecionado")
                .Must(cursos => cursos != null && cursos.All(id => id != Guid.Empty))
                .WithMessage("Todos os IDs de cursos devem ser válidos");

            RuleFor(c => c.NomeCartao)
                .NotEmpty()
                .WithMessage("O nome no cartão é obrigatório")
                .MinimumLength(3)
                .WithMessage("O nome no cartão deve ter no mínimo 3 caracteres")
                .MaximumLength(100)
                .WithMessage("O nome no cartão deve ter no máximo 100 caracteres");

            RuleFor(c => c.NumeroCartao)
                .NotEmpty()
                .WithMessage("O número do cartão é obrigatório")
                .MinimumLength(13)
                .WithMessage("O número do cartão deve ter no mínimo 13 dígitos")
                .MaximumLength(19)
                .WithMessage("O número do cartão deve ter no máximo 19 dígitos")
                .Matches(@"^\d+$")
                .WithMessage("O número do cartão deve conter apenas dígitos");

            RuleFor(c => c.ExpiracaoCartao)
                .NotEmpty()
                .WithMessage("A data de expiração é obrigatória")
                .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$")
                .WithMessage("A data de expiração deve estar no formato MM/YY");

            RuleFor(c => c.CvvCartao)
                .NotEmpty()
                .WithMessage("O CVV é obrigatório")
                .MinimumLength(3)
                .WithMessage("O CVV deve ter 3 ou 4 dígitos")
                .MaximumLength(4)
                .WithMessage("O CVV deve ter 3 ou 4 dígitos")
                .Matches(@"^\d+$")
                .WithMessage("O CVV deve conter apenas dígitos");
        }
    }
}

