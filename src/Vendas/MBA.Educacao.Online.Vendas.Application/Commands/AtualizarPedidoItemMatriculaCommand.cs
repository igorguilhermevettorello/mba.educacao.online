using MBA.Educacao.Online.Core.Domain.DTOs;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Vendas.Application.Validators;

namespace MBA.Educacao.Online.Vendas.Application.Commands
{
    public class AtualizarPedidoItemMatriculaCommand : Command
    {
        public Guid PedidoId { get; set; }
        public List<MatriculaItemDto> Matriculas { get; set; }

        public AtualizarPedidoItemMatriculaCommand(Guid pedidoId, List<MatriculaItemDto> matriculas)
        {
            PedidoId = pedidoId;
            Matriculas = matriculas ?? new List<MatriculaItemDto>();
        }

        public override bool IsValid()
        {
            ValidationResult = new AtualizarPedidoItemMatriculaCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
