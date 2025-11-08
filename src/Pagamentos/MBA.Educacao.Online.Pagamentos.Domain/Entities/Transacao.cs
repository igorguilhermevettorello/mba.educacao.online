using MBA.Educacao.Online.Core.Domain.Models;
using MBA.Educacao.Online.Pagamentos.Domain.Enums;

namespace MBA.Educacao.Online.Pagamentos.Domain.Entities
{
    public class Transacao : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid PagamentoId { get; private set; }
        public decimal Total { get; private set; }
        public StatusTransacao StatusTransacao { get; private set; }
        public DateTime DataTransacao { get; private set; }

        // EF. Rel.
        public Pagamento Pagamento { get; private set; }

        protected Transacao() { }

        public Transacao(Guid pedidoId, Guid pagamentoId, decimal total, StatusTransacao statusTransacao)
        {
            ValidarPedidoId(pedidoId);
            ValidarPagamentoId(pagamentoId);
            ValidarTotal(total);
            ValidarStatus(statusTransacao);

            PedidoId = pedidoId;
            PagamentoId = pagamentoId;
            Total = total;
            StatusTransacao = statusTransacao;
            DataTransacao = DateTime.UtcNow;
        }

        public void AtualizarStatus(StatusTransacao novoStatus)
        {
            ValidarStatus(novoStatus);

            if (StatusTransacao == StatusTransacao.Pago)
                throw new InvalidOperationException("Não é possível alterar status de transação já paga");

            if (StatusTransacao == StatusTransacao.Cancelado)
                throw new InvalidOperationException("Não é possível alterar status de transação cancelada");

            StatusTransacao = novoStatus;
        }

        public void MarcarComoPago()
        {
            if (StatusTransacao == StatusTransacao.Pago)
                throw new InvalidOperationException("Transação já está marcada como paga");

            if (StatusTransacao == StatusTransacao.Cancelado)
                throw new InvalidOperationException("Não é possível pagar transação cancelada");

            StatusTransacao = StatusTransacao.Pago;
        }

        public void MarcarComoRecusado()
        {
            if (StatusTransacao == StatusTransacao.Pago)
                throw new InvalidOperationException("Não é possível recusar transação já paga");

            StatusTransacao = StatusTransacao.Recusado;
        }

        public void Cancelar()
        {
            if (StatusTransacao == StatusTransacao.Pago)
                throw new InvalidOperationException("Não é possível cancelar transação já paga");

            StatusTransacao = StatusTransacao.Cancelado;
        }

        public bool FoiPago()
        {
            return StatusTransacao == StatusTransacao.Pago;
        }

        public bool FoiRecusado()
        {
            return StatusTransacao == StatusTransacao.Recusado;
        }

        public bool EstaCancelado()
        {
            return StatusTransacao == StatusTransacao.Cancelado;
        }

        public TimeSpan ObterTempoDecorrido()
        {
            return DateTime.UtcNow - DataTransacao;
        }

        // Métodos de validação privados
        private static void ValidarPedidoId(Guid pedidoId)
        {
            if (pedidoId == Guid.Empty)
                throw new ArgumentException("ID do pedido é inválido", nameof(pedidoId));
        }

        private static void ValidarPagamentoId(Guid pagamentoId)
        {
            if (pagamentoId == Guid.Empty)
                throw new ArgumentException("ID do pagamento é inválido", nameof(pagamentoId));
        }

        private static void ValidarTotal(decimal total)
        {
            if (total <= 0)
                throw new ArgumentException("Total da transação deve ser maior que zero", nameof(total));

            if (total > 999999.99m)
                throw new ArgumentException("Total da transação excede o limite máximo permitido", nameof(total));
        }

        private static void ValidarStatus(StatusTransacao status)
        {
            if (!Enum.IsDefined(typeof(StatusTransacao), status))
                throw new ArgumentException("Status da transação inválido", nameof(status));
        }
    }   
}