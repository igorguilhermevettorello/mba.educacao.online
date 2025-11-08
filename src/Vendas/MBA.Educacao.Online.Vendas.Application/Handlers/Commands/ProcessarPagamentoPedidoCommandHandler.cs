using MBA.Educacao.Online.Core.Domain.DTOs;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Services;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Commands
{
    public class ProcessarPagamentoPedidoCommandHandler : IRequestHandler<ProcessarPagamentoPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly INotificador _notificador;
        private readonly IPagamentoService _pagamentoService;

        public ProcessarPagamentoPedidoCommandHandler(
            IPedidoRepository pedidoRepository,
            IMediatorHandler mediatorHandler,
            INotificador notificador,
            IPagamentoService pagamentoService)
        {
            _pedidoRepository = pedidoRepository;
            _mediatorHandler = mediatorHandler;
            _notificador = notificador;
            _pagamentoService = pagamentoService;
        }

        public async Task<bool> Handle(ProcessarPagamentoPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorAlunoId(request.AlunoId);
            if (pedido == null)
            {
                Notificar("Pedido", "Pedido não encontrado para este aluno");
                return false;
            }

            if (!pedido.PossuiItens())
            {
                Notificar("Pedido", "Pedido não possui itens para pagamento");
                return false;
            }

            // Atualiza o status do pedido para iniciado
            try
            {
                pedido.AtualizarStatusIniciado();
            }
            catch (InvalidOperationException ex)
            {
                Notificar("Pedido", ex.Message);
                return false;
            }

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedido.Id,
                Itens = pedido.PedidoItens.Select(i => new Item
                {
                    Id = i.CursoId,
                    Descricao = i.CursoNome.GetHashCode(),
                    Valor = i.Valor
                }).ToList()
            };

            pedido.AdicionarEvento(new EfetuarPagamentoEvent(
                pedido.Id,
                request.AlunoId,
                pedido.ValorTotal,
                listaCursos,
                request.NomeCartao,
                request.NumeroCartao,
                request.ExpiracaoCartao,
                request.CvvCartao
            ));

            var resultado = await _pedidoRepository.UnitOfWork.Commit();
            if (!resultado)
            {
                Notificar("Pedido", "Erro ao processar pedido para pagamento");
                return false;
            }

            return true;
        }

        private bool ValidarComando(Command request)
        {
            if (request.IsValid()) return true;

            foreach (var error in request.ValidationResult.Errors)
            {
                Notificar(error.PropertyName, error.ErrorMessage);
            }

            return false;
        }

        private void Notificar(string campo, string mensage)
        {
            _notificador.Handle(new Notificacao
            {
                Campo = campo,
                Mensagem = mensage
            });
        }
    }
}

