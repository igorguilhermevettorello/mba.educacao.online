using MBA.Educacao.Online.Core.Domain.Messages;
using MediatR;

namespace MBA.Educacao.Online.Core.Domain.Interfaces.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarComando<T>(T comando) where T : Command;
        Task<TResponse> EnviarComando<TResponse>(IRequest<TResponse> comando);
        //Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
    }
}

