using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Cursos.Application.Commands.Aulas;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.Aulas
{
    public class AtivarAulaCommandHandler : IRequestHandler<AtivarAulaCommand, bool>
    {
        private readonly IAulaRepository _aulaRepository;
        private readonly INotificador _notificador;

        public AtivarAulaCommandHandler(
            IAulaRepository aulaRepository,
            INotificador notificador)
        {
            _aulaRepository = aulaRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(AtivarAulaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                foreach (var error in request.ValidationResult.Errors)
                {
                    _notificador.Handle(new Core.Domain.Notifications.Notificacao
                    {
                        Campo = error.PropertyName,
                        Mensagem = error.ErrorMessage
                    });
                }
                return false;
            }

            var aula = await _aulaRepository.BuscarPorIdAsync(request.AulaId);

            if (aula == null)
            {
                _notificador.Handle(new Core.Domain.Notifications.Notificacao
                {
                    Campo = "AulaId",
                    Mensagem = "Aula não encontrada"
                });
                return false;
            }

            aula.Ativar();
            _aulaRepository.Alterar(aula);
            return await _aulaRepository.UnitOfWork.Commit();
        }
    }
}

