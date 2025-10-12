using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Core.Application.Events.Identity;
using MediatR;

namespace MBA.Educacao.Online.Alunos.Application.EventHandlers
{
    public class UsuarioCriadoEventHandler : INotificationHandler<UsuarioCriadoEvent>
    {
        private readonly IMediator _mediator;

        public UsuarioCriadoEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(UsuarioCriadoEvent notification, CancellationToken cancellationToken)
        {
            // Quando um usuário é criado no Identity, cria o registro correspondente na tabela de Alunos
            var command = new CriarAlunoCommand(
                notification.UsuarioId,
                notification.Nome,
                notification.Email
            );

            await _mediator.Send(command, cancellationToken);
        }
    }
}

