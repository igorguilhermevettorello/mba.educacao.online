using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Vendas.Application.Validators;

namespace MBA.Educacao.Online.Vendas.Application.Commands
{
    public class AlterarStatusPedidoRascunhoCommand : Command
    {
        public Guid PedidoId { get; set; }
        

        public AlterarStatusPedidoRascunhoCommand(Guid pedidoId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
        }

        public override bool IsValid()
        {
            ValidationResult = new AlterarStatusPedidoRascunhoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
