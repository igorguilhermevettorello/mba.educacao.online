namespace MBA.Educacao.Online.API.DTOs.Pedido
{
    public class PedidoRascunhoDto
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public decimal ValorTotal { get; set; }
        public int QuantidadeCursos { get; set; }
        public List<PedidoItemDto> Itens { get; set; }

        public PedidoRascunhoDto()
        {
            Itens = new List<PedidoItemDto>();
        }
    }
}

