using MBA.Educacao.Online.Vendas.Application.Queries;
using MBA.Educacao.Online.Vendas.Domain.Entities;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Queries
{
    public class ObterPedidoRascunhoQueryHandler : IRequestHandler<ObterPedidoRascunhoQuery, Pedido?>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public ObterPedidoRascunhoQueryHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<Pedido?> Handle(ObterPedidoRascunhoQuery request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorAlunoId(request.AlunoId);
            return pedido;
        }
    }
}

