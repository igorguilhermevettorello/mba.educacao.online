using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Vendas.Data.Context;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Vendas.Domain.Entities;
using MBA.Educacao.Online.Vendas.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Vendas.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidoContext _context;

        public PedidoRepository(PedidoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public void AdicionarItem(PedidoItem item)
        {
            _context.PedidoItens.Add(item);
        }

        public void Alterar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public void RemoverItem(PedidoItem item)
        {
            _context.PedidoItens.Remove(item);
        }

        public async Task<Pedido?> ObterPedidoRascunhoPorAlunoId(Guid alunoId)
        {
            // Retorna com tracking para permitir modificações
            return await _context.Pedidos
                .Include(p => p.PedidoItens)
                .FirstOrDefaultAsync(p => p.AlunoId == alunoId && p.PedidoStatus == PedidoStatusEnum.Rascunho);
        }

        // Métodos adicionais úteis
        public async Task<Pedido?> ObterPorId(Guid id)
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItens)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pedido?> ObterPorIdComTracking(Guid id)
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItens)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pedido?> ObterPorCodigo(int codigo)
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItens)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public async Task<IEnumerable<Pedido>> ObterPorAlunoId(Guid alunoId)
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItens)
                .AsNoTracking()
                .Where(p => p.AlunoId == alunoId)
                .OrderByDescending(p => p.DataCadastro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosPagos()
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItens)
                .AsNoTracking()
                .Where(p => p.PedidoStatus == PedidoStatusEnum.Pago)
                .OrderByDescending(p => p.DataCadastro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosPorStatus(PedidoStatusEnum status)
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItens)
                .AsNoTracking()
                .Where(p => p.PedidoStatus == status)
                .OrderByDescending(p => p.DataCadastro)
                .ToListAsync();
        }

        public async Task<bool> ExistePedidoRascunhoParaAluno(Guid alunoId)
        {
            return await _context.Pedidos
                .AsNoTracking()
                .AnyAsync(p => p.AlunoId == alunoId && p.PedidoStatus == PedidoStatusEnum.Rascunho);
        }

        public async Task<int> ObterProximoCodigo()
        {
            var ultimoCodigo = await _context.Pedidos
                .AsNoTracking()
                .MaxAsync(p => (int?)p.Codigo) ?? 0;

            return ultimoCodigo + 1;
        }

        public async Task<List<Pedido>> ObterTodos()
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItens)
                .AsNoTracking()
                .OrderByDescending(p => p.DataCadastro)
                .ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
