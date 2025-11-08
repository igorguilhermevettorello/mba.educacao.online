using FluentValidation;
using MBA.Educacao.Online.Vendas.Application.Commands;

namespace MBA.Educacao.Online.Vendas.Application.Validators
{
    public class EfetuarMatriculaCommandValidator : AbstractValidator<EfetuarMatriculaCommand>
    {
        public EfetuarMatriculaCommandValidator()
        {
            RuleFor(c => c.AlunoId)
                .NotEmpty()
                .WithMessage("O código do curso é inválido.")
                .NotEqual(Guid.Empty)
                .WithMessage("O código do curso é inválido.");
        }
    }
}
