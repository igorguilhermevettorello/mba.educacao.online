using MBA.Educacao.Online.Core.Domain.DTOs;

namespace MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents
{
    public class AtualizarPedidoItemMatriculaEvent : IntegrationEvent
    {
        public Guid PedidoId { get; private set; }
        public List<MatriculaItemDto> Matriculas { get; private set; }

        public AtualizarPedidoItemMatriculaEvent(Guid pedidoId, List<MatriculaItemDto> matriculas)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            Matriculas = matriculas ?? new List<MatriculaItemDto>();
        }
    }
}
