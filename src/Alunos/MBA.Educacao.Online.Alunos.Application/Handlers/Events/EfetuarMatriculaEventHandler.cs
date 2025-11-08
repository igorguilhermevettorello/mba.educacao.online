using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using MediatR;

namespace MBA.Educacao.Online.Alunos.Application.Handlers.Events
{
    public class EfetuarMatriculaEventHandler : INotificationHandler<EfetuarMatriculaEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public EfetuarMatriculaEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(EfetuarMatriculaEvent message, CancellationToken cancellationToken)
        {
            var processarMatriculaCommand = new ProcessarMatriculaCommand(
                message.AlunoId,
                message.PedidoId,
                message.ListaCursos);

            await _mediatorHandler.EnviarComando(processarMatriculaCommand);
        }
    }
}
