using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Cursos.Application.Commands.Aulas;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Handlers.Aulas
{
    public class AtualizarAulaCommandHandler : IRequestHandler<AtualizarAulaCommand, bool>
    {
        private readonly IAulaRepository _aulaRepository;
        private readonly INotificador _notificador;

        public AtualizarAulaCommandHandler(
            IAulaRepository aulaRepository,
            INotificador notificador)
        {
            _aulaRepository = aulaRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(AtualizarAulaCommand request, CancellationToken cancellationToken)
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

            var aula = await _aulaRepository.BuscarPorIdAsync(request.Id);

            if (aula == null)
            {
                _notificador.Handle(new Core.Domain.Notifications.Notificacao
                {
                    Campo = "Id",
                    Mensagem = "Aula não encontrada"
                });
                return false;
            }

            try
            {
                aula.AtualizarTitulo(request.Titulo);
                aula.AtualizarDescricao(request.Descricao);
                aula.AtualizarDuracao(request.DuracaoMinutos);
                aula.AtualizarOrdem(request.Ordem);

                _aulaRepository.Alterar(aula);
                return await _aulaRepository.UnitOfWork.Commit();
            }
            catch (ArgumentException ex)
            {
                _notificador.Handle(new Core.Domain.Notifications.Notificacao
                {
                    Campo = "Aula",
                    Mensagem = ex.Message
                });
                return false;
            }
        }
    }
}

