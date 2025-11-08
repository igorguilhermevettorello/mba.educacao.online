using FluentValidation;
using MBA.Educacao.Online.Vendas.Application.Commands;

namespace MBA.Educacao.Online.Vendas.Application.Validators
{
    public class AlterarStatusPedidoRascunhoCommandValidator : AbstractValidator<AlterarStatusPedidoRascunhoCommand>
    {
        public AlterarStatusPedidoRascunhoCommandValidator()
        {
            RuleFor(c => c.PedidoId)
                .NotEmpty()
                .WithMessage("O código do pedido é inválido.")
                .NotEqual(Guid.Empty)
                .WithMessage("O código do pedido é inválido.");
        }
    }
}
