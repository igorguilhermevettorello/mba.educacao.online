using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Commands
{
    public class AlterarStatusPedidoRascunhoCommandHandler : IRequestHandler<AlterarStatusPedidoRascunhoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly INotificador _notificador;

        public AlterarStatusPedidoRascunhoCommandHandler(
            IPedidoRepository pedidoRepository,
            INotificador notificador)
        {
            _pedidoRepository = pedidoRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(AlterarStatusPedidoRascunhoCommand request, CancellationToken cancellationToken)
        {
            // Valida o comando
            if (!ValidarComando(request)) return false;

            // Busca o pedido com tracking para permitir alterações
            var pedido = await _pedidoRepository.ObterPorIdComTracking(request.PedidoId);

            if (pedido == null)
            {
                Notificar("PedidoId", $"Pedido com ID {request.PedidoId} não encontrado");
                return false;
            }

            // Tenta alterar o status para rascunho
            try
            {
                pedido.AtualizarStatusRascunho();
            }
            catch (InvalidOperationException ex)
            {
                Notificar("Status", ex.Message);
                return false;
            }

            // Atualiza o pedido no repositório
            _pedidoRepository.Alterar(pedido);

            // Salva as alterações
            var resultado = await _pedidoRepository.UnitOfWork.Commit();

            if (!resultado)
            {
                Notificar("Pedido", "Erro ao alterar status do pedido para rascunho");
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

        private void Notificar(string campo, string mensagem)
        {
            _notificador.Handle(new Notificacao
            {
                Campo = campo,
                Mensagem = mensagem
            });
        }
    }
}
