using MBA.Educacao.Online.Core.Domain.DTOs;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Pagamentos.Domain.Entities;
using MBA.Educacao.Online.Pagamentos.Domain.Enums;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Payments;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Services;

namespace MBA.Educacao.Online.Pagamentos.Domain.Services
{
    public class PagamentoService : IPagamentoService
    {
    private readonly IPagamentoCartaoCreditoFacade _pagamentoCartaoCreditoFacade;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly INotificador _notificador;

        public PagamentoService(
            IPagamentoCartaoCreditoFacade pagamentoCartaoCreditoFacade,
            IPagamentoRepository pagamentoRepository, 
            IMediatorHandler mediatorHandler,
            INotificador notificador
        ) {
            _pagamentoCartaoCreditoFacade = pagamentoCartaoCreditoFacade;
            _pagamentoRepository = pagamentoRepository;
            _mediatorHandler = mediatorHandler;
            _notificador = notificador;
        }

        public async Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido)
        {
            var pedido = new Pedido
            {
                Id = pagamentoPedido.PedidoId,
                Valor = pagamentoPedido.Total
            };

            var pagamento = new Pagamento(
                pagamentoPedido.PedidoId,
                pagamentoPedido.Total,
                pagamentoPedido.NomeCartao,
                pagamentoPedido.NumeroCartao,
                pagamentoPedido.ExpiracaoCartao,
                pagamentoPedido.CvvCartao
            );

            var transacao = _pagamentoCartaoCreditoFacade.RealizarPagamento(pedido, pagamento);

            if (transacao.StatusTransacao == StatusTransacao.Pago)
            {
                pagamento.AdicionarEvento(new PagamentoRealizadoEvent(pedido.Id, pagamentoPedido.AlunoId, transacao.PagamentoId, transacao.Id, pedido.Valor));
                _pagamentoRepository.Adicionar(pagamento);
                _pagamentoRepository.AdicionarTransacao(transacao);

                await _pagamentoRepository.UnitOfWork.Commit();

                return transacao;
            }

            _notificador.Handle(new Notificacao { Campo = "pagamento", Mensagem = "A operadora recusou o pagamento" });
            await _mediatorHandler.PublicarEvento(new PagamentoRecusadoEvent(pedido.Id));
            return transacao;
        }
    }   
}