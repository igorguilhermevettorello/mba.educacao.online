using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Events
{
    public class AtualizarPedidoItemMatriculaEventHandler : INotificationHandler<AtualizarPedidoItemMatriculaEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public AtualizarPedidoItemMatriculaEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(AtualizarPedidoItemMatriculaEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.EnviarComando(new AtualizarPedidoItemMatriculaCommand(message.PedidoId,message.Matriculas));
        }
    }
}
