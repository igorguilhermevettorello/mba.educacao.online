using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Vendas.Domain.Models;

namespace MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        void Adicionar(Pedido pedido);
        void Alterar(Pedido pedido);
        void AdicionarItem(PedidoItem item);
        void RemoverItem(PedidoItem item);
        Task<Pedido> ObterPedidoRascunhoPorAlunoId(Guid alunoId);
        void IDisposable();
    }
}
