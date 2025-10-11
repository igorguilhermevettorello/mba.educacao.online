using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Vendas.Domain.Models;

namespace MBA.Educacao.Online.Vendas.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public void Adicionar(Pedido pedido)
        {
            throw new NotImplementedException();
        }

        public void AdicionarItem(PedidoItem item)
        {
            throw new NotImplementedException();
        }

        public void Alterar(Pedido pedido)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void IDisposable()
        {
            throw new NotImplementedException();
        }

        public Task<Pedido> ObterPedidoRascunhoPorAlunoId(Guid alunoId)
        {
            throw new NotImplementedException();
        }

        public void RemoverItem(PedidoItem item)
        {
            throw new NotImplementedException();
        }
    }
}
