using MBA.Educacao.Online.Core.Domain.Models;

namespace MBA.Educacao.Online.Vendas.Domain.Models
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; set; }
        public Guid CursoId { get; set; }
        public string CursoNome { get; set; }
        public decimal Valor { get; set; }
        public Pedido Pedido { get; set; }
    }
}
