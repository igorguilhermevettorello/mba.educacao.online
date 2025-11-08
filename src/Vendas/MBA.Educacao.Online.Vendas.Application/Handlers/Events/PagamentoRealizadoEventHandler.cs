using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Events
{
    public class PagamentoRealizadoEventHandler : INotificationHandler<PagamentoRealizadoEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public PagamentoRealizadoEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(PagamentoRealizadoEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.EnviarComando(new EfetuarMatriculaCommand(message.AlunoId));
        }
    }
}
