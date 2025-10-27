using MBA.Educacao.Online.Pagamentos.Domain.Models;

namespace MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Payments
{
    public interface IPagamentoCartaoCreditoFacade
    {
        Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento);
    }
}