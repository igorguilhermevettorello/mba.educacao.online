using FluentValidation;
using MBA.Educacao.Online.Vendas.Application.Commands;

namespace MBA.Educacao.Online.Vendas.Application.Validators
{
    public class AdicionarCursoAoPedidoCommandValidator : AbstractValidator<AdicionarCursoAoPedidoCommand>
    {
        public AdicionarCursoAoPedidoCommandValidator()
        {
            RuleFor(c => c.AlunoId)
                .NotEmpty()
                .WithMessage("O ID do aluno é inválido.")
                .NotEqual(Guid.Empty)
                .WithMessage("O ID do aluno é inválido.");

            RuleFor(c => c.CursoId)
                .NotEmpty()
                .WithMessage("O ID do curso é inválido.")
                .NotEqual(Guid.Empty)
                .WithMessage("O ID do curso é inválido.");
        }
    }
}

