using MBA.Educacao.Online.Core.Domain.Models;
using MBA.Educacao.Online.Pagamentos.Domain.Enums;

namespace MBA.Educacao.Online.Pagamentos.Domain.Models
{
    public class Transacao : Entity
    {
        public Guid PedidoId { get; set; }
        public Guid PagamentoId { get; set; }
        public decimal Total { get; set; }
        public StatusTransacao StatusTransacao { get; set; }

        // EF. Rel.
        public Pagamento Pagamento { get; set; }
    }   
}