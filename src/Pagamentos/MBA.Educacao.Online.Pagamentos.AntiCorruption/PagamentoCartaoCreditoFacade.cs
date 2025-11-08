using MBA.Educacao.Online.Pagamentos.Domain.Enums;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Payments;
using MBA.Educacao.Online.Pagamentos.Domain.Entities;

namespace MBA.Educacao.Online.Pagamentos.AntiCorruption
{
    public class PagamentoCartaoCreditoFacade : IPagamentoCartaoCreditoFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configManager;

        public PagamentoCartaoCreditoFacade(IPayPalGateway payPalGateway, IConfigurationManager configManager)
        {
            _payPalGateway = payPalGateway;
            _configManager = configManager;
        }

        public Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento)
        {
            var apiKey = _configManager.GetValue("apiKey");
            var encriptionKey = _configManager.GetValue("encriptionKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);

            var pagamentoResult = _payPalGateway.CommitTransaction(cardHashKey, pedido.Id.ToString(), pagamento.Valor);

            // Determina o status baseado no resultado do gateway
            var statusTransacao = pagamentoResult ? StatusTransacao.Pago : StatusTransacao.Recusado;

            // Cria a transação usando o construtor com validações
            var transacao = new Transacao(
                pedido.Id,
                pagamento.Id,
                pedido.Valor,
                statusTransacao
            );

            return transacao;
        }
    }
}
