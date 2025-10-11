using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Models;
using MBA.Educacao.Online.Vendas.Domain.Enum;

namespace MBA.Educacao.Online.Vendas.Domain.Models
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int Codigo { get; set; }
        public Guid AlunoId { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataCadastro { get; set; }
        public PedidoStatusEnum PedidoStatus { get; set; }
        
        private readonly List<PedidoItem> _pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItens;

        protected Pedido() 
        {
            _pedidoItens = new List<PedidoItem>();
        }

        public Pedido(Guid alunoId)
        {
            AlunoId = alunoId;
            _pedidoItens = new List<PedidoItem>();
        }

        public void CalcularValorPedido()
        { 
            ValorTotal = PedidoItens.Sum(p => p.Valor);
        }

        public bool PedidoItemExistente(PedidoItem item)
        {
            return _pedidoItens.Any(p => p.CursoId == item.CursoId);
        }

        public void AdicionarItem(PedidoItem item)
        { 
            if (item == null) { throw new ArgumentNullException("item"); }

            if (!item.IsValid()) { throw new ArgumentNullException("item"); }

            if (!PedidoItemExistente(item))
            {
                _pedidoItens.Add(item);
            }

            CalcularValorPedido();
        }

        public void RemoverImte(PedidoItem item)
        { 
            if (item == null) { return; } 
            if (!item.IsValid()) { return; }
            _pedidoItens.Remove(item);
            CalcularValorPedido();
        }

        public void AtualizarStatusRascunho()
        {
            PedidoStatus = PedidoStatusEnum.Rascunho;
        }

        public void AtualizarStatusIniciado()
        {
            PedidoStatus = PedidoStatusEnum.Iniciado;
        }

        public void AtualizarStatusPago()
        {
            PedidoStatus = PedidoStatusEnum.Pago;
        }

        public void AtualizarStatusCancelado()
        {
            PedidoStatus = PedidoStatusEnum.Cancelado;
        }

        public static class PedidoFactory
        {
            public static Pedido CriarPedido(Guid alunoId)
            {
                var pedido = new Pedido(alunoId);
                pedido.AtualizarStatusRascunho();
                return pedido;
            }
        }
    }
}