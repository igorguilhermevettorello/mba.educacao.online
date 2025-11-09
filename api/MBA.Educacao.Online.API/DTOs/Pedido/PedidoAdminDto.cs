using MBA.Educacao.Online.Vendas.Domain.Enum;

namespace MBA.Educacao.Online.API.DTOs.Pedido
{
    public class PedidoAdminDto
    {
        public Guid Id { get; set; }
        public int Codigo { get; set; }
        public Guid AlunoId { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Status { get; set; } = string.Empty;
        public int QuantidadeItens { get; set; }
        public List<PedidoItemAdminDto> Itens { get; set; } = new();
    }

    public class PedidoItemAdminDto
    {
        public Guid Id { get; set; }
        public Guid CursoId { get; set; }
        public string CursoNome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}

