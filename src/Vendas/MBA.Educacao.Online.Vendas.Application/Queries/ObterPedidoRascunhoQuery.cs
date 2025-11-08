using MBA.Educacao.Online.Vendas.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Queries
{
    public class ObterPedidoRascunhoQuery : IRequest<Pedido?>
    {
        public Guid AlunoId { get; set; }

        public ObterPedidoRascunhoQuery(Guid alunoId)
        {
            AlunoId = alunoId;
        }
    }
}
