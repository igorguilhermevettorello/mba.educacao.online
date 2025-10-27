using MBA.Educacao.Online.Core.Domain.DTOs;

namespace MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents
{
    public class MatriculaConfirmadaEvent : IntegrationEvent
    {
        public Guid PedidoId { get; private set; }
        public Guid AlunoId { get; private set; }
        public decimal Total { get; private set; }
        public ListaCursosPedidoDto ProdutosPedido { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        public MatriculaConfirmadaEvent(Guid pedidoId, Guid alunoId, decimal total, ListaCursosPedidoDto produtosPedido, string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            AlunoId = alunoId;
            Total = total;
            ProdutosPedido = produtosPedido;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
        }
    }
}