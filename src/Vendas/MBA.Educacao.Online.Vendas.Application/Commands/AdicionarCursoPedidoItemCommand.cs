using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Vendas.Application.Validators;

namespace MBA.Educacao.Online.Vendas.Application.Commands
{
    public class AdicionarCursoPedidoItemCommand : Command
    {
        public Guid CursoId { get; set; }
        public decimal Valor { get; set; }

        public AdicionarCursoPedidoItemCommand(Guid cursoId, decimal valor)
        { 
            CursoId = cursoId;
            Valor = valor;
        }

        public override bool IsValid()
        {
            ValidationResult = new AdicionarCursoPedidoItemCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
