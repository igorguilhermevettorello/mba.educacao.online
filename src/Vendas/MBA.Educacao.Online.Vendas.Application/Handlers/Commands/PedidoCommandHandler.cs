using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Vendas.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarCursoPedidoItemCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository)
        { 
            _pedidoRepository = pedidoRepository;
        }

        public async Task<bool> Handle(AdicionarCursoPedidoItemCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorAlunoId(request.CursoId);
            var pedidoItem = new PedidoItem();
            if (pedido == null)
            {
                pedido = Pedido.PedidoFactory.CriarPedido(request.CursoId);
                _pedidoRepository.Adicionar(pedido);
            }

            return await _pedidoRepository.UnitOfWork.Commit();            
        }

        private bool ValidarComando(Command request)
        {
            if (request.IsValid()) return true;

            foreach (var error in request.ValidationResult.Errors)
            {

            }

            return false;
        }
    }
}
