namespace MBA.Educacao.Online.API.DTOs.Pedido
{
    public class PagamentoAdminDto
    {
        public Guid Id { get; set; }
        public Guid PedidoId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string NomeCartao { get; set; } = string.Empty;
        public string NumeroCartaoMascarado { get; set; } = string.Empty;
        public TransacaoDto? Transacao { get; set; }
    }

    public class TransacaoDto
    {
        public Guid Id { get; set; }
        public Guid PedidoId { get; set; }
        public Guid PagamentoId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataTransacao { get; set; }
    }
}

