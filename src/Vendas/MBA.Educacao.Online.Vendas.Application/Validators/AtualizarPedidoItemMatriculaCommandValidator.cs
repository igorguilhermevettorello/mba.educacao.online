using FluentValidation;
using MBA.Educacao.Online.Vendas.Application.Commands;

namespace MBA.Educacao.Online.Vendas.Application.Validators
{
    public class AtualizarPedidoItemMatriculaCommandValidator : AbstractValidator<AtualizarPedidoItemMatriculaCommand>
    {
        public AtualizarPedidoItemMatriculaCommandValidator()
        {
            RuleFor(c => c.PedidoId)
                .NotEmpty()
                .WithMessage("O ID do pedido é inválido.")
                .NotEqual(Guid.Empty)
                .WithMessage("O ID do pedido é inválido.");

            RuleFor(c => c.Matriculas)
                .NotNull()
                .WithMessage("A lista de matrículas é obrigatória.")
                .NotEmpty()
                .WithMessage("Deve haver pelo menos uma matrícula para atualização.");

            RuleForEach(c => c.Matriculas)
                .ChildRules(matricula =>
                {
                    matricula.RuleFor(m => m.CursoId)
                        .NotEmpty()
                        .WithMessage("O ID do curso é inválido.")
                        .NotEqual(Guid.Empty)
                        .WithMessage("O ID do curso é inválido.");

                    matricula.RuleFor(m => m.MatriculaId)
                        .NotEmpty()
                        .WithMessage("O ID da matrícula é inválido.")
                        .NotEqual(Guid.Empty)
                        .WithMessage("O ID da matrícula é inválido.");
                });
        }
    }
}
