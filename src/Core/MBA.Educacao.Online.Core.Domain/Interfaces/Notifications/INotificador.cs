using MBA.Educacao.Online.Core.Domain.Notifications;

namespace MBA.Educacao.Online.Core.Domain.Interfaces.Notifications
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}

