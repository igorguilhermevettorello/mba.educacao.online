using FluentValidation;
using MBA.Educacao.Online.Vendas.Application.Commands;

namespace MBA.Educacao.Online.Vendas.Application.Validators
{
    public class AdicionarCursoPedidoItemCommandValidator : AbstractValidator<AdicionarCursoPedidoItemCommand>
    {
        public AdicionarCursoPedidoItemCommandValidator()
        {
            RuleFor(c => c.CursoId)
                .NotEmpty()
                .WithMessage("O código do curso é inválido.")
                .NotEqual(Guid.Empty)
                .WithMessage("O código do curso é inválido.");

            RuleFor(c => c.Valor)
                .GreaterThan(0)
                .WithMessage("O valor do curso precisa ser maior que 0");

        }
    }
}
