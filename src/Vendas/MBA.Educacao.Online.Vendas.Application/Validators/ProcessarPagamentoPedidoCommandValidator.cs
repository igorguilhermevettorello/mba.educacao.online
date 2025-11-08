using FluentValidation;
using MBA.Educacao.Online.Vendas.Application.Commands;

namespace MBA.Educacao.Online.Vendas.Application.Validators
{
    public class ProcessarPagamentoPedidoCommandValidator : AbstractValidator<ProcessarPagamentoPedidoCommand>
    {
        public ProcessarPagamentoPedidoCommandValidator()
        {
            RuleFor(c => c.AlunoId)
                .NotEmpty()
                .WithMessage("O ID do aluno é inválido.")
                .NotEqual(Guid.Empty)
                .WithMessage("O ID do aluno é inválido.");

            RuleFor(c => c.NomeCartao)
                .NotEmpty()
                .WithMessage("O nome no cartão é obrigatório.")
                .Length(3, 100)
                .WithMessage("O nome no cartão deve ter entre 3 e 100 caracteres.");

            RuleFor(c => c.NumeroCartao)
                .NotEmpty()
                .WithMessage("O número do cartão é obrigatório.")
                .Length(13, 19)
                .WithMessage("O número do cartão deve ter entre 13 e 19 caracteres.")
                .Matches(@"^\d{13,19}$")
                .WithMessage("O número do cartão deve conter apenas dígitos.");

            RuleFor(c => c.ExpiracaoCartao)
                .NotEmpty()
                .WithMessage("A data de expiração é obrigatória.")
                .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$")
                .WithMessage("A data de expiração deve estar no formato MM/YY.");

            RuleFor(c => c.CvvCartao)
                .NotEmpty()
                .WithMessage("O CVV é obrigatório.")
                .Length(3, 4)
                .WithMessage("O CVV deve ter 3 ou 4 dígitos.")
                .Matches(@"^\d{3,4}$")
                .WithMessage("O CVV deve conter apenas dígitos.");
        }
    }
}

